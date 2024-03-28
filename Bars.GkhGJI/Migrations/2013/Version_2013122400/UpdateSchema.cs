namespace Bars.GkhGji.Migrations.Version_2013122400
{
    using System.Data;
    using global::Bars.B4.Modules.Ecm7.Framework;
    using global::Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2013122400")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.GkhGji.Migrations.Version_2013122300.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.AddEntityTable(
                "GJI_DICT_VIOLACTIONREMOV",
                new Column("VIOLATION_ID", DbType.Int64, 22, ColumnProperty.NotNull),
                new Column("ACTIONSREMOVVIOL_ID", DbType.Int64, 22, ColumnProperty.NotNull));
            Database.AddIndex("IND_GJI_VIOLACTREM_VIOL", false, "GJI_DICT_VIOLACTIONREMOV", "VIOLATION_ID");
            Database.AddIndex("IND_GJI_VIOLACTREM_ACT", false, "GJI_DICT_VIOLACTIONREMOV", "ACTIONSREMOVVIOL_ID");
            Database.AddForeignKey("FK_GJI_VIOLACTREM_VIOL", "GJI_DICT_VIOLACTIONREMOV", "VIOLATION_ID", "GJI_DICT_VIOLATION", "ID");
            Database.AddForeignKey("FK_GJI_VIOLACTREM_ACT", "GJI_DICT_VIOLACTIONREMOV", "ACTIONSREMOVVIOL_ID", "GJI_DICT_ACTREMOVVIOL", "ID");
        }

        public override void Down()
        {
            Database.RemoveTable("GJI_DICT_VIOLACTIONREMOV");
            Database.RemoveConstraint("GJI_DICT_VIOLACTIONREMOV", "FK_GJI_VIOLACTREM_VIOL");
            Database.RemoveConstraint("GJI_DICT_VIOLACTIONREMOV", "FK_GJI_VIOLACTREM_ACT");
        }
    }
}