namespace Bars.Gkh.Repair.Migrations.Version_2014022800
{
    using System.Data;
    using global::Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2014022800")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.Repair.Migrations.Version_2014022702.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.AddColumn("RP_TYPE_WORK", new Column("ADD_DATE_END", DbType.Date));

        }

        public override void Down()
        {
            Database.RemoveColumn("RP_TYPE_WORK", "ADD_DATE_END");
        }
    }
}