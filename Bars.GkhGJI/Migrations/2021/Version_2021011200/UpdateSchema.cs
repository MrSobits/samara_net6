namespace Bars.GkhGji.Migrations._2021.Version_2021011200
{
    using System.Data;
    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

    [Migration("2021011200")]
    [MigrationDependsOn(typeof(_2020.Version_2020121800.UpdateSchema))]

    public class UpdateSchema : Migration
    {
        public override void Up()
        {
            Database.AddEntityTable(
              "GJI_MKD_LIC_STATEMENT_INSPECTOR",
              new RefColumn("REQUEST_ID", ColumnProperty.NotNull, "GJI_MKD_LIC_STATEMENT_INSP_STMNT", "GJI_MKD_LIC_STATEMENT", "ID"),
              new RefColumn("INSPECTOR_ID", ColumnProperty.NotNull, "GJI_MKD_LIC_STATEMENT_INSP_INSP", "GKH_DICT_INSPECTOR", "ID"));

            Database.AddRefColumn("GJI_APPCIT_ANSWER", new RefColumn("SIGNER_ID", "GJI_ANSW_SIGNER_ID", "GKH_DICT_INSPECTOR", "ID"));
        }

        public override void Down()
        {
            Database.RemoveTable("GJI_MKD_LIC_STATEMENT_INSPECTOR");
        }
    }
}