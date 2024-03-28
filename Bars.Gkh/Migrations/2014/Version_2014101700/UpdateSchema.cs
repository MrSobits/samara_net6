namespace Bars.Gkh.Migrations.Version_2014101700
{
    using System.Data;
    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2014101700")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.Migrations.Version_2014101200.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.AddColumn("GKH_DICT_WORK", new Column("IS_ADDITIONAL_WORK", DbType.Boolean, ColumnProperty.NotNull, false));
        }

        public override void Down()
        {
            Database.RemoveColumn("GKH_DICT_WORK", "IS_ADDITIONAL_WORK");
        }
    }
}