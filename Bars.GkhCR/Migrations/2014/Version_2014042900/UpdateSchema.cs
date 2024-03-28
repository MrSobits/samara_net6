namespace Bars.GkhCr.Migrations.Version_2014042900
{
    using System.Data;
    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2014042900")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.GkhCr.Migrations.Version_2014041500.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.AddColumn("CR_OBJ_PER_ACT_PAYMENT", new Column("DATE_DISPOSAL", DbType.DateTime));
        }

        public override void Down()
        {
            Database.RemoveColumn("CR_OBJ_PER_ACT_PAYMENT", "DATE_DISPOSAL");
        }
    }
}