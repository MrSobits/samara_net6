namespace Bars.GkhGji.Migrations._2020.Version_2020072900
{
    using System.Data;
    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

    [Migration("2020072900")]
    [MigrationDependsOn(typeof(Version_2020072100.UpdateSchema))]

    public class UpdateSchema : Migration
    {
        public override void Up()
        {
            Database.AddColumn("GJI_DISPOSAL", new Column("FIO_DOC_APPROVE", DbType.String, 300));
            Database.AddColumn("GJI_DISPOSAL", new Column("POSITION_DOC_APPROVE", DbType.String, 300));
        }

        public override void Down()
        {
            Database.RemoveColumn("GJI_DISPOSAL", "POSITION_DOC_APPROVE");
            Database.RemoveColumn("GJI_DISPOSAL", "FIO_DOC_APPROVE");
        }
    }
}