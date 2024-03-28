namespace Bars.Gkh.Overhaul.Tat.Migration.Version_2013121100
{
    using System.Data;

    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2013121100")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.Overhaul.Tat.Migration.Version_2013121002.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.AddColumn("OVRHL_DEC_SPEC_ACC_NOTICE", new Column("NOTICE_NUM", DbType.Int32));
        }

        public override void Down()
        {
            Database.RemoveColumn("OVRHL_DEC_SPEC_ACC_NOTICE", "NOTICE_NUM");
        }
    }
}