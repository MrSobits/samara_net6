namespace Bars.Gkh.Repair.Migrations.Version_2014022601
{
    using global::Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2014022601")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.Repair.Migrations.Version_1.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            //-----Объект текущего ремонта
            Database.AddEntityTable(
                "RP_OBJECT",
                new RefColumn("PROGRAM_ID", ColumnProperty.NotNull, "PR_OBJECT_PR", "RP_DICT_PROGRAM", "ID"),
                new RefColumn("REALITY_OBJECT_ID", ColumnProperty.NotNull, "PR_OBJECT_RO", "GKH_REALITY_OBJECT", "ID"),
                new RefColumn("STATE_ID", ColumnProperty.NotNull, "PR_OBJECT_STATE", "B4_STATE", "ID"));
            //-----
        }

        public override void Down()
        {
            Database.RemoveColumn("RP_OBJECT", "PROGRAM_ID");
            Database.RemoveColumn("RP_OBJECT", "REALITY_OBJECT_ID");
            Database.RemoveColumn("RP_OBJECT", "STATE_ID");

            Database.RemoveTable("RP_OBJECT");
        }
    }
}