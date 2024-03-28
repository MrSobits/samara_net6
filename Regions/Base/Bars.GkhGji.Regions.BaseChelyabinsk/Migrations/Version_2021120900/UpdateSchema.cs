namespace Bars.GkhGji.Regions.BaseChelyabinsk.Migrations.Version_2021120900
{
    using B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using System.Data;

    [Migration("2021120900")]
    [MigrationDependsOn(typeof(Version_2021120600.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        public override void Up()
        {
            this.Database.AddEntityTable("GJI_DICTIONARY_ERKNM",
                   new Column("DICTIONARY_ERKNM_GUID", DbType.String, 36, ColumnProperty.NotNull),
                   new Column("NAME", DbType.String, 50),
                   new Column("TYPE", DbType.String, 50),
                   new Column("DESCRIPTION", DbType.String, 100),
                   new Column("ORDER", DbType.Int32),
                   new Column("REQUIRED", DbType.Boolean, ColumnProperty.NotNull,false),
                   new Column("DATE_LAST_UPDATE", DbType.DateTime),
                   new Column("ENTITY_NAME", DbType.String, 50),
                   new Column("ENTITY_ID", DbType.Int64));

            this.Database.AddEntityTable("GJI_DICTIONARY_ERKNM_RECORD",
                        new Column("REC_ID", DbType.Int64, ColumnProperty.NotNull),
                        new Column("NAME", DbType.String, 50),
                        new Column("NAME1", DbType.String, 50),
                        new Column("NAME2", DbType.String, 50),
                        new Column("ENTITY_NAME", DbType.String, 50),
                        new Column("ENTITY_ID", DbType.Int64));
        }
                       
        public override void Down()
        {
            this.Database.RemoveTable("GJI_DICTIONARY_ERKNM_RECORD");
            this.Database.RemoveTable("GJI_DICTIONARY_ERKNM");
        }

    }
}