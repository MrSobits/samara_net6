namespace Bars.Gkh.Overhaul.Tat.Migration.Version_2013110701
{
    using System.Data;

    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2013110701")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.Overhaul.Tat.Migration.Version_2013110700.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.AddColumn("OVRHL_VERSION_REC", new Column("CRITERIA", DbType.String, 2000, ColumnProperty.Null));
        }

        public override void Down()
        {
            Database.RemoveColumn("OVRHL_VERSION_REC", "CRITERIA");
        }
    }
}