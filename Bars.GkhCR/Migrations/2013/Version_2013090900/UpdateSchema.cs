namespace Bars.GkhCr.Migrations.Version_2013090900
{
    using System.Data;

    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2013090900")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.GkhCr.Migrations.Version_2013090600.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.AddColumn("CR_OBJ_TYPE_WORK", new Column("ADD_DATE_END", DbType.Date));

        }

        public override void Down()
        {
            Database.RemoveColumn("CR_OBJ_TYPE_WORK", "ADD_DATE_END");
        }
    }
}