namespace Bars.Gkh.Regions.Voronezh.Migrations._2018.Version_2018071000
{
    using System;
    using System.Data;

    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2018071000")]
    [MigrationDependsOn(typeof(Version_2018051400.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        SchemaQualifiedObjectName tableEncumbrance = new SchemaQualifiedObjectName { Name = "ROSREGEXTRACTENCUMBRANCE", Schema = "IMPORT" };
        SchemaQualifiedObjectName tableEncOwner = new SchemaQualifiedObjectName { Name = "ROSREGEXTRACTENCOWNER", Schema = "IMPORT" };
        SchemaQualifiedObjectName tablePers = new SchemaQualifiedObjectName { Name = "ROSREGEXTRACTPERS", Schema = "IMPORT" };
        SchemaQualifiedObjectName tableTmpId = new SchemaQualifiedObjectName { Name = "tmp_clw_id", Schema = "IMPORT" };
        SchemaQualifiedObjectName tableCmp = new SchemaQualifiedObjectName { Name = "GKH_VR_ACCOUNT_COMPARSION", Schema = "PUBLIC" };
        public override void Up()
        {
            this.Database.AddColumn(tablePers, new Column("PERS_SNILS", DbType.String));
            this.Database.RemoveTable(tableTmpId);
            this.Database.RemoveTable(tableEncOwner);
            this.Database.RemoveTable(tableEncumbrance);
            this.Database.RemoveTable(tableCmp);
        }

        public override void Down()
        {
            throw new NotImplementedException();
        }
    }
}