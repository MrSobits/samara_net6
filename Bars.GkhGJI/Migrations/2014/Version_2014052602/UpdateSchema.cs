namespace Bars.GkhGji.Migration.Version_2014052602
{
    using System.Data;

    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2014052602")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.GkhGji.Migration.Version_2014052601.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.AddColumn("GJI_PRESCRIPTION", new Column("IS_FAMILIAR", DbType.Int16, ColumnProperty.NotNull, 20));
        }

        public override void Down()
        {
            Database.RemoveColumn("GJI_PRESCRIPTION", "IS_FAMILIAR");
        }
    }
}