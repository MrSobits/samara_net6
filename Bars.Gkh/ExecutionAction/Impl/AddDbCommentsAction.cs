namespace Bars.Gkh.ExecutionAction.Impl
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using System.Text;
    using System.Xml.Linq;
    using System.Xml.Serialization;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.Utils;

    using Microsoft.Extensions.Logging;

    using NHibernate;
    using NHibernate.Persister.Entity;

    public class SchemaObject
    {
        [XmlAttribute]
        public string Name;

        [XmlAttribute]
        public string Type;

        [XmlAttribute]
        public string Comment;
    }

    public class XmlRoot
    {
        public List<SchemaObject> Objects;
    }

    /// <summary>
    /// Функция/процедура использована из соображений производительности - чтобы избежать тысяч запросов к базе.
    /// </summary>
    public class AddDbCommentsAction : BaseExecutionAction
    {
        private const string PostgreFunctionSql = @"
CREATE OR REPLACE FUNCTION gkhupdateschemacomments(objects json) RETURNS void AS
$BODY$
DECLARE

object_ VARCHAR;
type_ VARCHAR;
comment_ VARCHAR;

objectsCursor CURSOR IS SELECT * FROM TMP_OBJECTS;

BEGIN
    DROP TABLE IF EXISTS TMP_OBJECTS;
    CREATE TEMP TABLE TMP_OBJECTS (object_ VARCHAR, type_ VARCHAR, comment_ VARCHAR);

    INSERT INTO TMP_OBJECTS
    SELECT * FROM json_populate_recordset(CAST(NULL AS TMP_OBJECTS), objects);

    OPEN objectsCursor;
    LOOP
    FETCH objectsCursor INTO object_, type_, comment_;
        EXIT WHEN not FOUND;

        BEGIN
            EXECUTE 'COMMENT ON ' || type_ || ' ' || object_ || ' IS ''' || comment_ || '''';
            EXCEPTION WHEN OTHERS THEN
        END;
    END LOOP;
    CLOSE objectsCursor;

END;
$BODY$
LANGUAGE plpgsql VOLATILE
";

        private const string OracleFunctionSql = @"
CREATE OR REPLACE PROCEDURE gkhupdateschemacomments(objects XMLTYPE) AS
pragma autonomous_transaction;
CURSOR objectsCursor IS
SELECT
Extract(COLUMN_VALUE, '/SchemaObject/@Name').getStringVal() AS name_,
Extract(COLUMN_VALUE, '/SchemaObject/@Type').getStringVal() AS type_,
Extract(COLUMN_VALUE, '/SchemaObject/@Comment').getStringVal() AS comment_
FROM TABLE (XMLSEQUENCE (objects.EXTRACT ('/XmlRoot/Objects/SchemaObject') ) );
BEGIN
    FOR obj IN objectsCursor
    LOOP
    BEGIN
        EXECUTE IMMEDIATE 'COMMENT ON ' || obj.type_ || ' ' || obj.name_ || ' IS ''' || obj.comment_ || ''''; 
        EXCEPTION WHEN OTHERS THEN NULL;
    END;
    END LOOP;
END;
";

        private readonly Dictionary<string, XElement> xdocCache = new Dictionary<string, XElement>();
        private readonly List<string> successfullObjects = new List<string>();
        private readonly List<string> unsuccessfullObjects = new List<string>();

        public ILogger Logger { get; set; }

        public ISessionProvider SessionProvider { get; set; }

        public override string Description => "Внести русскоязычные комментарии с описанием к таблицам и колонкам БД.";

        public override string Name => "Внести русскоязычные комментарии с описанием к таблицам и колонкам БД.";

        public override Func<IDataResult> Action => this.PerformAction;

        private BaseDataResult PerformAction()
        {
            try
            {
                using (this.SessionProvider.GetCurrentSession())
                {
                    var comments = this.LoadComments();

                    if (this.SessionProvider.CurrentSession.Connection.GetType().Name.ToLower().Contains("oracle"))
                    {
                        this.AddCommentsOracle(comments);
                    }
                    else
                    {
                        this.AddCommentsPostgre(comments);
                    }

                    this.SessionProvider.CurrentSession.Flush();
                    this.SessionProvider.CurrentSession.Clear();
                }

                this.LogActionSummary();

                var message = "Документирование БД завершено. {0} объектов документированы, {1} объектов не содержат документации. Детали в системном логе."
                    .FormatUsing(this.successfullObjects.Count, this.unsuccessfullObjects.Count);
                return new BaseDataResult(true, message);
            }
            catch (Exception e)
            {
                this.Logger.LogError(e, "Ошибка при обновлении комментариев к объектам БД.");
                return new BaseDataResult(false, "Ошибка при обновлении комментариев к объектам БД. " + e.Message);
            }
        }

        private List<SchemaObject> LoadComments()
        {
            var result = new List<SchemaObject>();

            var metadata = this.SessionProvider.CurrentSession.SessionFactory.GetAllClassMetadata();

            foreach (var entityMeta in metadata.Values.Cast<AbstractEntityPersister>())
            {
                var tableName = entityMeta.TableName;
                var entityType = entityMeta.MappedClass;

                var entityComment = this.GetEntityComment(entityType);
                if (entityComment == null)
                {
                    this.unsuccessfullObjects.Add(tableName);
                }
                else
                {
                    result.Add(
                        new SchemaObject
                        {
                            Name = tableName,
                            Type = tableName.StartsWith("view_", StringComparison.OrdinalIgnoreCase) ? "VIEW" : "TABLE",
                            Comment = entityComment
                        });
                    this.successfullObjects.Add(tableName);
                }

                foreach (var propertyName in entityMeta.PropertyNames)
                {
                    MemberInfo property = entityType.GetProperty(propertyName, BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);
                    property = property ?? entityType.GetField(propertyName, BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);
                    var propertyComment = this.GetPropertyComment(property);

                    if (propertyComment == null)
                    {
                        this.unsuccessfullObjects.Add(entityMeta.EntityName + "." + propertyName);
                    }
                    else
                    {
                        var columns = entityMeta.GetPropertyColumnNames(propertyName);
                        foreach (var columnName in columns)
                        {
                            result.Add(
                                new SchemaObject
                                {
                                    Name = tableName + "." + columnName,
                                    Type = "COLUMN",
                                    Comment = propertyComment
                                });
                        }
                        this.successfullObjects.Add(entityMeta.EntityName + "." + propertyName);
                    }
                }
            }

            return result;
        }

        private string GetEntityComment(Type entityType)
        {
            var name = "T:" + entityType.FullName;

            return this.GetMemberComment(entityType.Assembly.GetName().Name, name);
        }

        private string GetPropertyComment(MemberInfo property)
        {
            var name = (property is FieldInfo ? "F:" : "P:") + property.DeclaringType.FullName + "." + property.Name;

            return this.GetMemberComment(property.DeclaringType.Assembly.GetName().Name, name);
        }

        private string GetMemberComment(string assemblyName, string memberName)
        {
            var comments = this.GetAssemblyComments(assemblyName);
            var member = comments.Descendants("member").FirstOrDefault(e => e.Attribute("name").Value == memberName);

            return member.With(m => m.Descendants("summary").FirstOrDefault().With(s => s.Value.Trim()));
        }

        /// <summary>
        /// Загружает XML с комментариями для указанной сборки.
        /// </summary>
        private XElement GetAssemblyComments(string assemblyName)
        {
            var xmlName = Path.Combine(AppDomain.CurrentDomain.RelativeSearchPath, assemblyName + ".xml");

            XElement result;
            if (!this.xdocCache.TryGetValue(assemblyName, out result))
            {
                try
                {
                    result = XElement.Load(xmlName);
                }
                catch (Exception e)
                {
                    this.Logger.LogError(e, "Комментарии для сборки " + assemblyName + " не найдены.");
                    result = new XElement("_");
                }

                this.xdocCache.Add(assemblyName, result);
            }

            return result;
        }

        private void AddCommentsPostgre(List<SchemaObject> comments)
        {
            this.SessionProvider.CurrentSession.CreateSQLQuery(AddDbCommentsAction.PostgreFunctionSql).ExecuteUpdate();

            var dto = comments.Select(
                c => new
                {
                    object_ = c.Name,
                    type_ = c.Type,
                    comment_ = c.Comment.Replace("'", "''")
                });
            var json = JsonNetConvert.SerializeObject(this.Container, dto);
            this.SessionProvider.CurrentSession.CreateSQLQuery("SELECT gkhupdateschemacomments(CAST(:json AS JSON));")
                .SetString("json", json)
                .ExecuteUpdate();

            this.SessionProvider.CurrentSession.CreateSQLQuery("DROP FUNCTION IF EXISTS gkhupdateschemacomments (json);")
                .ExecuteUpdate();
        }

        private void AddCommentsOracle(List<SchemaObject> comments)
        {
            this.SessionProvider.CurrentSession.CreateSQLQuery(AddDbCommentsAction.OracleFunctionSql).ExecuteUpdate();

            int take = 50; //Примерно, чтобы уложиться в ограничение длины строкового литерала

            //Oracle не может принять XML больше 4КБ (возможно, 32КБ, если постараться, но это все равно мало)
            for (var skip = 0; skip < comments.Count; skip += take)
            {
                var xmlRoot = new XmlRoot();
                xmlRoot.Objects = comments.Skip(skip).Take(take).ToList();

                foreach (var obj in xmlRoot.Objects)
                {
                    obj.Comment = obj.Comment.Replace("'", "''");
                    if (obj.Type.Equals("VIEW", StringComparison.OrdinalIgnoreCase))
                    {
                        obj.Type = "TABLE";
                    }
                }

                var serializer = new XmlSerializer(xmlRoot.GetType());
                var writer = new StringWriter();
                serializer.Serialize(writer, xmlRoot);
                var xml = writer.ToString();

                //Использование параметров приводило к ORA-01460, даже если XML был < 4КБ
                this.SessionProvider.CurrentSession.CreateSQLQuery("BEGIN gkhupdateschemacomments(XMLTYPE ('" + xml.Replace("'", "''") + "')); END;")
                    .ExecuteUpdate();
            }

            this.SessionProvider.CurrentSession.CreateSQLQuery("DROP PROCEDURE gkhupdateschemacomments")
                .ExecuteUpdate();
        }

        private void LogActionSummary()
        {
            var log = new StringBuilder();
            log.AppendLine("Обновление комментариев к объектам БД завершено.");

            log.AppendFormat("Успешно документированы объекты {0}: ", this.successfullObjects.Count);
            foreach (var obj in this.successfullObjects.OrderBy(s => s))
            {
                log.Append(obj).Append(", ");
            }
            log.AppendLine();

            log.AppendFormat("Не документированы оъекты {0}: ", this.unsuccessfullObjects.Count);
            foreach (var obj in this.unsuccessfullObjects.OrderBy(s => s))
            {
                log.Append(obj).Append(", ");
            }

            var result = log.ToString();
            this.Logger.LogInformation(result);
        }
    }
}