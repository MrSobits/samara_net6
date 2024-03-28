namespace Bars.Gkh.Repair.Migrations.Version_2014030600
{
    using System.Data;
    using global::Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2014030600")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.Repair.Migrations.Version_2014022800.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.RemoveColumn("RP_TYPE_WORK", "BUILDER_ID");
            Database.AddColumn("RP_TYPE_WORK", new Column("BUILDER", DbType.String, 255));
        }

        public override void Down()
        {
            Database.RemoveColumn("RP_TYPE_WORK", "BUILDER");
            Database.AddRefColumn("RP_TYPE_WORK", new RefColumn("BUILDER_ID", ColumnProperty.Null, "RP_TYPE_WORK_BUILDER", "GKH_BUILDER", "ID"));
        }
    }
}