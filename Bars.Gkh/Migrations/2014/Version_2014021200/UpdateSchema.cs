namespace Bars.Gkh.Migrations.Version_2014021200
{
    using System.Data;
    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2014021200")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.Migrations.Version_2014021100.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            //перенесено в модуль Decisions

            //Database.AddColumn("GKH_OBJ_D_PROTOCOL", new Column("AUTHORIZED_PERSON", DbType.String, 200, ColumnProperty.Null));
        }

        public override void Down()
        {
            //Database.RemoveColumn("GKH_OBJ_D_PROTOCOL", "AUTHORIZED_PERSON");
        }
    }
}