namespace Bars.GkhGji.Migrations.Version_2013122300
{
    using System.Data;
    using global::Bars.B4.Modules.Ecm7.Framework;
    using global::Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2013122300")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.GkhGji.Migrations.Version_2013121800.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.AddEntityTable("GJI_DICT_ACTREMOVVIOL",
                new Column("NAME", DbType.String, 500, ColumnProperty.NotNull),
                new Column("CODE", DbType.String, 300));
            Database.AddIndex("IND_ACTREMOVVIOL_NAME", false, "GJI_DICT_ACTREMOVVIOL", "NAME");
            Database.AddIndex("IND_ACTREMOVVIOL_CODE", false, "GJI_DICT_ACTREMOVVIOL", "CODE");
        }

        public override void Down()
        {
            Database.RemoveTable("GJI_DICT_ACTREMOVVIOL");
        }
    }
}