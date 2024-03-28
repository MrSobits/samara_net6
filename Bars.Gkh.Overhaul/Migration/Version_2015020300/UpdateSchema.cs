namespace Bars.Gkh.Overhaul.Migration.Version_2015020300
{
    using System.Data;
    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2015020300")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.Overhaul.Migration.Version_2014080401.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.AddColumn("OVRHL_RO_STRUCT_EL", new Column("CONDITION", DbType.Int32, ColumnProperty.NotNull, 10));
        }

        public override void Down()
        {
            Database.RemoveColumn("OVRHL_RO_STRUCT_EL", "CONDITION");
        }
    }
}