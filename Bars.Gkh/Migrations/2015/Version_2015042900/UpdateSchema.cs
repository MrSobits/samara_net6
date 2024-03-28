namespace Bars.Gkh.Migration.Version_2015042900
{
    using System.Data;
    using global::Bars.B4.Modules.Ecm7.Framework;
    using global::Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2015042900")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.Migrations._2015.Version_2015042400.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.AddRefColumn("CLW_CLAIM_WORK", new RefColumn("REAL_OBJ_ID", "CLW_RO_FK", "GKH_REALITY_OBJECT", "ID"));
            Database.AddColumn("CLW_CLAIM_WORK", new Column("BASE_INFO", DbType.String, 200));
        }

        public override void Down()
        {
            Database.RemoveColumn("CLW_CLAIM_WORK", "BASE_INFO");
            Database.RemoveColumn("CLW_CLAIM_WORK", "REAL_OBJ_ID");
        }
    }
}