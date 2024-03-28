namespace Bars.GkhCr.Migrations._2015.Version_2015121500
{
    using System.Data;
    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2015121500")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.GkhCr.Migrations._2015.Version_2015121400.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.AddColumn("CR_OBJ_CONTRACT", new Column("DATE_START_WORK", DbType.Date));
            Database.AddColumn("CR_OBJ_CONTRACT", new Column("DATE_END_WORK", DbType.Date));
        }

        public override void Down()
        {
            Database.RemoveColumn("CR_OBJ_CONTRACT", "DATE_START_WORK");
            Database.RemoveColumn("CR_OBJ_CONTRACT", "DATE_START_WORK");
        }
    }
}
