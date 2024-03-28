namespace Bars.Gkh.RegOperator.Utils.OrmChecker
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Text;

    using Bars.B4.Utils;
    using Bars.Gkh.RegOperator.Utils.OrmChecker.Database;
    using Bars.Gkh.RegOperator.Utils.OrmChecker.Entity;

    using Castle.Windsor;
    
    using NHibernate.Cfg;
    using NHibernate.Mapping;

    public class OrmChecker
    {
        private readonly IWindsorContainer container;

        public OrmChecker(IWindsorContainer container)
        {
            this.container = container;
        }

        public string ChechExistColumnAndConstraints()
        {
            var errors = new StringBuilder();

            var mapInfo = GetMapInfo();

            foreach (var entityInfo in mapInfo.SelectMany(x => x.Value))
            {
                foreach (var property in entityInfo.Properties)
                {
                    if (property.IsFormula || property.Type.IsEnumerable())
                    {
                        continue;
                    }
                    
                    var column = property.DbColumn;
                    if (column == null)
                    {
                        errors.AppendLine(string.Format("NotExistsColumn: entity: {0}, property: {1};", entityInfo.Name, property.Name));
                        continue;
                    }

                    /* Отключил проверку на наличие констрейнтов
                    if (!property.Type.Is<string>() && property.Type.IsClass)
                    {
                        var existsConstraint = entityInfo.Table.Constraints.Any(x => x.IsReferences && x.ColumnName == column.ColumnName);
                        if (!existsConstraint)
                        {
                            errors.AppendLine(string.Format("NotExistsConstraint: entity: {0}, property: {1}, table: {2}, column: {3};", entityInfo.Name, property.Name, entityInfo.Table.TableName, column.ColumnName));
                        }
                    }
                    */
                }
            }

            return errors.ToString();
        }

        private Dictionary<Assembly, List<EntityInfo>> GetMapInfo()
        {
            var nhconfig = container.Resolve<Configuration>();
            var result = new Dictionary<Assembly, List<EntityInfo>>();
            var tablesInfo = new NhibernateInfoProvider(container).GetAllTableInfo();

            var subClasses = new Dictionary<EntityInfo, PersistentClass>();

            foreach (var assembly in nhconfig.ClassMappings.GroupBy(x => x.MappedClass.Assembly).Select(x => new { Assembly = x.Key, Mappings = x.ToArray() }))
            {
                var values = new List<EntityInfo>(assembly.Mappings.Length);
                result.Add(assembly.Assembly, values);
                foreach (var classMapping in assembly.Mappings)
                {
                    var persistentObjectType = classMapping.MappedClass;

                    var tableInfo = tablesInfo.ContainsKey(persistentObjectType) ? tablesInfo[persistentObjectType] : null;
                    var entityInfo = new EntityInfo
                    {
                        Name = persistentObjectType.Name,
                        FullName = persistentObjectType.FullName,
                        Type = persistentObjectType,
                        Table = tableInfo,
                        IsJoinedSubClass = classMapping.IsJoinedSubclass
                    };
                    values.Add(entityInfo);

                    // JoinedSubclassMapping
                    if (entityInfo.IsJoinedSubClass)
                    {
                        subClasses.Add(entityInfo, classMapping);
                    }

                    foreach (var property in classMapping.PropertyIterator)
                    {
                        var member = persistentObjectType.GetProperty(property.Name);
                        if (member == null)
                        {
                            continue;
                        }

                        var properyInfo = new EntityPropertyInfo
                        {
                            Name = property.Name,
                            Type = member.PropertyType
                        };

                        entityInfo.Properties.Add(properyInfo);

                        if (property.ColumnIterator.Any())
                        {
                            var mapColumn = property.ColumnIterator.First();
                            if (mapColumn.IsFormula)
                            {
                                properyInfo.IsFormula = true;
                                properyInfo.Formula = mapColumn.Text;
                            }
                            else if (tableInfo != null)
                            {
                                properyInfo.DbColumn = tableInfo.Columns.FirstOrDefault(x => string.Equals(x.ColumnName, mapColumn.Text, StringComparison.CurrentCultureIgnoreCase));
                            }
                        }
                    }
                }
            }

            foreach (var subClass in subClasses)
            {
                subClass.Key.Parent = result.SelectMany(x => x.Value).Single(x => x.Type == subClass.Value.MappedClass);
            }

            return result;
        }
    }
}