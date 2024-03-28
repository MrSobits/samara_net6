namespace Bars.Gkh.Migrations._2019.Version_2019071000
{
    using System.Data;
    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

    [Migration("2019071000")]
    [MigrationDependsOn(typeof(Version_2019060300.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        public override void Up()
        {
            Database.AddEntityTable("EXTERNAL_EXCHANGE_TESTING_FILES",
                new Column("ENTITY_ID", DbType.Int64, ColumnProperty.None),
                new Column("CCLASS_NAME", DbType.String, 1000, ColumnProperty.None),
                new Column("CCLASS_DESC", DbType.String, 1000, ColumnProperty.None),
                new Column("CDATE_APPLIED", DbType.DateTime, ColumnProperty.None),
                new RefColumn("FILE_ID", "FK_EXT_EXCH_TEST_FILE", "B4_FILE_INFO", "ID"),                
                new Column("CUSER_NAME", DbType.String, 300, ColumnProperty.Null)
                );
        }

        public override void Down()
        {
            Database.RemoveTable("EXTERNAL_EXCHANGE_TESTING_FILES");
        }
    }
}