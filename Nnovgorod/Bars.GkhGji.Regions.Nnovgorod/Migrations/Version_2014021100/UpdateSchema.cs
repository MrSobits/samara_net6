namespace Bars.GkhGji.Regions.Nnovgorod.Migrations.Version_2014021100
{
    using System.Data;

    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2014021100")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.GkhGji.Regions.Nnovgorod.Migrations.Version_2014020700.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.ChangeColumn("GJI_NNOV_INSP_VIOL_WORD", new Column("WORDING", DbType.String, 2000));
        }

        public override void Down()
        {
        }
    }
}