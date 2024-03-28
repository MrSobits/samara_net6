namespace Bars.GkhCr.Migrations.Version_2014100600
{
    using System.Data;
    
    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2014100600")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.GkhCr.Migrations.Version_2014100500.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.AddColumn("CR_OBJ_TYPE_WORK_HIST", new Column("USER_NAME", DbType.String, 300));
        }

        public override void Down()
        {
            Database.RemoveColumn("CR_OBJ_TYPE_WORK_HIST", "USER_NAME");
        }
    }
}