namespace Bars.Gkh.Ris.Migrations.Version_2016070401
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4.Application;
    using Bars.B4.DataAccess;
    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.GisIntegration.Base.Entities;

    using NHibernate;
    using NHibernate.Engine;
    using NHibernate.Persister.Entity;

    [Migration("2016070401")]
    [MigrationDependsOn(typeof(Version_2016070400.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        public override void Up()
        {
            //var refs = this.GetContragentRefs();
            //var fks = this.Database.GetForeignKeys("GI_CONTRAGENT");

            //foreach (var @ref in refs)
            //{
            //    if (this.Database.ColumnExists(@ref.TableName, "RIS_CONTRAGENT_ID"))
            //    {
            //        this.Database.RenameColumn(@ref.TableName, "RIS_CONTRAGENT_ID", "GI_CONTRAGENT_ID");
            //    }

            //    if (fks.Any(x => string.Equals(x.TableName, @ref.TableName, StringComparison.OrdinalIgnoreCase)))
            //    {
            //        continue;
            //    }

            //    this.Database.AddForeignKey(this.CreateConstraintName(@ref.TableName), @ref.TableName, @ref.ColumnName, "GI_CONTRAGENT", "ID");
            //}
        }

        //private string CreateConstraintName(string tableName)
        //{
        //    var parts = tableName.Split('_');
        //    var len = parts.Sum(x => x.Length) + parts.Length - 1;
        //    var i = 0;

        //    var hasChanges = false;
        //    var useDelimiters = true;
        //    var partMinLength = 6;
        //    var maxLength = 15;
        //    while (len > maxLength)
        //    {
        //        if (parts[i].Length > partMinLength)
        //        {
        //            parts[i] = parts[i].Substring(0, parts[i].Length - 1);
        //            hasChanges = true;
        //            len--;
        //        }

        //        i++;
        //        if (i != parts.Length)
        //        {
        //            continue;
        //        }

        //        if (!hasChanges)
        //        {
        //            if (partMinLength > 3)
        //            {
        //                partMinLength--;
        //            }
        //            else if (useDelimiters)
        //            {
        //                useDelimiters = false;
        //                len -= parts.Length - 1;
        //            }
        //            else if (partMinLength > 2)
        //            {
        //                partMinLength--;
        //            }
        //            else
        //            {
        //                maxLength++;
        //            }
        //        }

        //        hasChanges = false;
        //        i = 0;
        //    }

        //    return $"FK_{string.Join(useDelimiters ? "_" : string.Empty, parts)}_CTRG";
        //}

        //private List<Ref> GetContragentRefs()
        //{
        //    var sessionFactory = ApplicationContext.Current.Container.Resolve<ISessionFactory>();
        //    var sessionFactoryImpl = (ISessionFactoryImplementor)sessionFactory;
        //    var sessionProvider = ApplicationContext.Current.Container.Resolve<ISessionProvider>();
        //    var session = sessionProvider.GetCurrentSession();
        //    var allClassMetadata = sessionFactory.GetAllClassMetadata();

        //    var contragentRefs = new List<Ref>();

        //    foreach (var meta in allClassMetadata.Values)
        //    {
        //        var type = meta.GetMappedClass(session.ActiveEntityMode);
        //        if (!typeof(BaseRisEntity).IsAssignableFrom(type))
        //        {
        //            continue;
        //        }

        //        var persister = (AbstractEntityPersister)sessionFactoryImpl.GetEntityPersister(meta.EntityName);
        //        var propertyName =
        //            persister.PropertyTypes.Select((x, i) => new Tuple<string, Type>(persister.PropertyNames[i], x.ReturnedClass))
        //                     .Where(x => x.Item2 == typeof(RisContragent))
        //                     .Select(x => x.Item1)
        //                     .FirstOrDefault();

        //        if (string.IsNullOrEmpty(propertyName))
        //        {
        //            continue;
        //        }

        //        var columns = persister.GetPropertyColumnNames(propertyName);
        //        if (columns == null || columns.Length != 1)
        //        {
        //            continue;
        //        }

        //        contragentRefs.Add(new Ref { TableName = persister.TableName, ColumnName = columns[0] });
        //    }

        //    return contragentRefs;
        //}

        //private class Ref
        //{
        //    public string TableName { get; set; }

        //    public string ColumnName { get; set; }
        //}
    }
}