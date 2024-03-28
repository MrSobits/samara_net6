namespace Bars.Gkh.Migrations.Version_2013080100
{
    using System.Data;

    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2013080100")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.Migrations.Version_2013073100.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.RemoveColumn("GKH_REALOBJ_ELEMENT_OBJ", "IS_EXIST");
        }

        public override void Down()
        {
            Database.AddColumn("GKH_REALOBJ_ELEMENT_OBJ", new Column("IS_EXIST", DbType.Int32, 4, ColumnProperty.NotNull, 30));
        }
    }
}