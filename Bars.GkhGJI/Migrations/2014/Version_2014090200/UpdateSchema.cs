namespace Bars.GkhGji.Migration.Version_2014090200
{
    using System.Data;

    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2014090200")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.GkhGji.Migration.Version_2014071700.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.AddColumn("GJI_RESOLUTION", new Column("GIS_UIN", DbType.String, 50));
            Database.AddColumn("GJI_RESOLUTION", new Column("FINEMUNICIPALITY_ID", DbType.Int64, 22));

            Database.AddIndex("IND_GJI_RESOLUTION_FMCP", false, "GJI_RESOLUTION", "FINEMUNICIPALITY_ID");
            Database.AddForeignKey("FK_GJI_RESOLUTION_FMCP", "GJI_RESOLUTION", "FINEMUNICIPALITY_ID", "GKH_DICT_MUNICIPALITY", "ID");

            Database.AddColumn("GJI_RESOLUTION_PAYFINE", new Column("GIS_UIP", DbType.String, 50));
        }

        public override void Down()
        {
            Database.RemoveConstraint("GJI_RESOLUTION", "FK_GJI_RESOLUTION_FMCP");
            Database.RemoveColumn("GJI_RESOLUTION", "FINEMUNICIPALITY_ID");
            Database.RemoveColumn("GJI_RESOLUTION", "GIS_UIN");
            Database.RemoveColumn("GJI_RESOLUTION_PAYFINE", "GIS_UIP");
        }
    }
}