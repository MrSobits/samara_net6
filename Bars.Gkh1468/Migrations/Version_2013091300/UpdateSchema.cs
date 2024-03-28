namespace Bars.Gkh1468.Migrations.Version_2013091300
{
    using System.Data;

    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2013091300")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh1468.Migrations.Version_2013091200.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.AddColumn("GKH_PSTRUCT_META_ATTR", new Column("FILLER_CODE", DbType.String, 100, ColumnProperty.Null));
        }

        public override void Down()
        {
            Database.RemoveColumn("GKH_PSTRUCT_META_ATTR", "FILLER_CODE");
        }
    }
}