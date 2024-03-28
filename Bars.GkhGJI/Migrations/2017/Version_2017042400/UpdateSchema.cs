namespace Bars.GkhGji.Migrations._2017.Version_2017042400
{
    using System.Data;

    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

    [Migration("2017042400")]
    [MigrationDependsOn(typeof(Version_2017042200.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        public override void Up()
        {
            this.Database.AddEntityTable(
                "GJI_DICT_INSPECTION_BASE_TYPE",
                new Column("CODE", DbType.Int32, ColumnProperty.NotNull),
                new Column("NAME", DbType.String, ColumnProperty.NotNull));

            this.Database.AddColumn("GJI_ACTCHECK", "ACQUAINT_STATE", DbType.Int32);
            this.Database.AddColumn("GJI_ACTCHECK", "REFUSED_TO_ACQUAINT_PERSON", DbType.String);
            this.Database.AddColumn("GJI_ACTCHECK", "ACQUAINTED_PERSON", DbType.String);
            this.Database.AddColumn("GJI_ACTCHECK", "ACQUAINTED_DATE", DbType.DateTime);
        }

        public override void Down()
        {
            this.Database.RemoveTable("GJI_DICT_INSPECTION_BASE_TYPE");

            this.Database.RemoveColumn("GJI_ACTCHECK", "ACQUAINT_STATE");
            this.Database.RemoveColumn("GJI_ACTCHECK", "REFUSED_TO_ACQUAINT_PERSON");
            this.Database.RemoveColumn("GJI_ACTCHECK", "ACQUAINTED_PERSON");
            this.Database.RemoveColumn("GJI_ACTCHECK", "ACQUAINTED_DATE");
        }
    }
}