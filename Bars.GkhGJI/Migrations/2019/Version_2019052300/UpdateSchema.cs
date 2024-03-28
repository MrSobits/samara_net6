namespace Bars.GkhGji.Migrations._2019.Version_2019052300
{
    using System.Data;
    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

    [Migration("2019052300")]
    [MigrationDependsOn(typeof(Bars.GkhGji.Migrations._2019.Version_2019052100.UpdateSchema))]

    public class UpdateSchema : Migration
    {
        public override void Up()
        {
            Database.AddColumn("GJI_APPCIT_STATSUBJ", new Column("EXPORT_CODE", DbType.String));

            Database.AddColumn("GJI_APPEAL_SOURCES", new Column("SSTU_DATE", DbType.DateTime));


        }

        public override void Down()
        {
            Database.RemoveColumn("GJI_APPEAL_SOURCES", "SSTU_DATE");
            Database.RemoveColumn("GJI_APPCIT_STATSUBJ", "EXPORT_CODE");
        }
    }
}