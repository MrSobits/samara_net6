namespace Bars.GkhGji.Migration.Version_2014091200
{
    using System.Data;

    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2014091200")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.GkhGji.Migration.Version_2014090900.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.AddColumn("GJI_PRESENTATION", new Column("REQUIR_TEXT", DbType.String, 2000));
            Database.AddColumn("GJI_PRESENTATION", new Column("EXEC_POST", DbType.String, 2000));
        }

        public override void Down()
        {
            Database.RemoveColumn("GJI_PRESENTATION", "REQUIR_TEXT");
            Database.RemoveColumn("GJI_PRESENTATION", "EXEC_POST");

        }
    }
}