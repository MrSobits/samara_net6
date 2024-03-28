namespace Bars.GkhGji.Migration.Version_2014061001
{
    using System.Data;

    using global::Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2014061001")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.GkhGji.Migrations.Version_2014060600.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            //-----Компетентная организация
            Database.AddEntityTable(
                "GJI_DICT_CON_ACTIVITY",
                new Column("CODE", DbType.String, 300),
                new Column("NAME", DbType.String, 300),
                new Column("EXTERNAL_ID", DbType.String, 36));
            Database.AddIndex("IND_DICT_CON_ACT_NAME", false, "GJI_DICT_CON_ACTIVITY", "NAME");
            Database.AddIndex("IND_DICT_CON_ACT_CODE", false, "GJI_DICT_CON_ACTIVITY", "CODE");
        }

        public override void Down()
        {
            Database.RemoveTable("GJI_DICT_CON_ACTIVITY");
        }
    }
}