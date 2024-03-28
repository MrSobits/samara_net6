namespace Bars.GkhCr.Migrations._2015.Version_2015111400
{
    using System.Data;
    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2015111400")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.GkhCr.Migrations._2015.Version_2015092800.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.AddColumn("CR_OBJ_TYPE_WORK_REMOVAL", new Column("STRUCT_EL", DbType.String, 500));
            Database.AddColumn("CR_OBJ_TYPE_WORK_HIST", new Column("STRUCT_EL", DbType.String, 500));
        }

        public override void Down()
        {
            Database.RemoveColumn("CR_OBJ_TYPE_WORK_REMOVAL", "STRUCT_EL");
            Database.RemoveColumn("CR_OBJ_TYPE_WORK_HIST", "STRUCT_EL");
        }
    }
}
