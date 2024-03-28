namespace Bars.Gkh.Migrations._2015.Version_2015100900
{
    using System.Data;
    using global::Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2015100900")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.Migrations._2015.Version_2015100500.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.AddEntityTable(
                "GKH_DICT_ANNEX_APPEAL_LIC_ISS",
                new Column("NAME", DbType.String, 500),
                new Column("CODE", DbType.String, 300));

            Database.AddRefColumn("GKH_MANORG_REQ_ANNEX", new RefColumn("DOCUMENT_TYPE_ID", "GKH_MANORG_REQ_ANNEX_DT", "GKH_DICT_ANNEX_APPEAL_LIC_ISS", "ID"));
        }

        public override void Down()
        {
            Database.RemoveColumn("GKH_MANORG_REQ_ANNEX", "DOCUMENT_TYPE_ID");

            Database.RemoveTable("GKH_DICT_ANNEX_APPEAL_LIC_ISS");
        }
    }
}