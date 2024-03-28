namespace Bars.GkhGji.Regions.Tomsk.Migrations.Version_2014012900
{
    using System.Data;
    using global::Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2014012900")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.GkhGji.Regions.Tomsk.Migrations.Version_2014012800.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.AddEntityTable(
                "GJI_TOMSK_AC_VERIFRESULT",
                new Column("TYPE_VERIF_RESULT", DbType.Int32),
                new RefColumn("ACT_CHECK_ID", ColumnProperty.NotNull, "ACT_CHECK_VERIFRESULT", "GJI_ACTCHECK", "ID"));

            Database.AddEntityTable(
                "GJI_TOMSK_DISP_VERIFSUBJ",
                new Column("TYPE_VERIF_SUBJ", DbType.Int32),
                new RefColumn("DISPOSAL_ID", ColumnProperty.NotNull, "DISP_VERIFSUBJ", "GJI_DISPOSAL", "ID"));
        }

        public override void Down()
        {
            Database.RemoveTable("GJI_TOMSK_AC_VERIFRESULT");
            Database.RemoveTable("GJI_TOMSK_DISP_VERIFSUBJ");
        }
    }
}
