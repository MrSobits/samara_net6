namespace Bars.GkhCr.Migrations.Version_2015062900
{
    using System.Data;

    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2015062900")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.GkhCr.Migration.Version_2015050700.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.AddColumn("CR_OBJ_PROTOCOL", "OWNER_NAME", DbType.String, 300, ColumnProperty.Null);
        }

        public override void Down()
        {
            Database.RemoveColumn("CR_OBJ_PROTOCOL", "OWNER_NAME");
        }
    }
}