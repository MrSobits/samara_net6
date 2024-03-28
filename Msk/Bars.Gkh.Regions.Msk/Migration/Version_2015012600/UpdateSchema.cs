namespace Bars.Gkh.Regions.Msk.Migration.Version_2015012600
{
    using System.Data;
    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2015012600")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.Regions.Msk.Migration.Version_2015012100.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.AddColumn("MSK_DPKR_INFO", new Column("POINT", DbType.Int32, ColumnProperty.NotNull, 0));
        }

        public override void Down()
        {
            Database.RemoveColumn("MSK_DPKR_INFO", "POINT");
        }
    }
}