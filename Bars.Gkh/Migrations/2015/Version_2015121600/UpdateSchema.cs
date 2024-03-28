namespace Bars.Gkh.Migrations._2015.Version_2015121600
{
    using System.Data;
    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2015121600")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.Migrations._2015.Version_2015121500.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.AddColumn("GKH_MORG_CONTRACT_OWNERS", new Column("DOCUMENT_NUMBER_ON_REGISTRY", DbType.String));
            Database.AddColumn("GKH_MORG_CONTRACT_OWNERS", new Column("DOCUMENT_DATE_ON_REGISTRY", DbType.DateTime));
            Database.AddColumn("GKH_MORG_CONTRACT_OWNERS", new Column("DOCUMENT_NUMBER_OFF_REGISTRY", DbType.String));
            Database.AddColumn("GKH_MORG_CONTRACT_OWNERS", new Column("DOCUMENT_DATE_OFF_REGISTRY", DbType.DateTime));
        }

        public override void Down()
        {
            Database.RemoveColumn("GKH_MORG_CONTRACT_OWNERS", "DOCUMENT_NUMBER_ON_REGISTRY");
            Database.RemoveColumn("GKH_MORG_CONTRACT_OWNERS", "DOCUMENT_DATE_ON_REGISTRY");
            Database.RemoveColumn("GKH_MORG_CONTRACT_OWNERS", "DOCUMENT_NUMBER_OFF_REGISTRY");
            Database.RemoveColumn("GKH_MORG_CONTRACT_OWNERS", "DOCUMENT_DATE_OFF_REGISTRY");
        }
    }
}
