namespace Bars.Gkh.Overhaul.Tat.Migration.Version_2013111200
{
    using System.Data;

    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2013111200")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.Overhaul.Tat.Migration.Version_2013110800.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.AddColumn("REAL_EST_TYPE_STRUCT_EL", new Column("EXISTS", DbType.Boolean, true));
        }

        public override void Down()
        {
            Database.RemoveColumn("REAL_EST_TYPE_STRUCT_EL", "EXISTS");
        }
    }
}