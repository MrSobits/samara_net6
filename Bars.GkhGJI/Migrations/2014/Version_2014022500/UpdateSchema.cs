namespace Bars.GkhGji.Migrations.Version_2014022500
{
    using System.Data;

    using global::Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2014022500")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.GkhGji.Migrations.Version_2014022400.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.AddEntityTable("GJI_ACT_TSJ_MEMBER",
                new Column("YEAR", DbType.Int32),
                new RefColumn("STATE_ID", "GJI_ACT_TSJ_MMB_ST", "B4_STATE", "ID"),
                new Column("FILE_ID", DbType.Int64, 22),                
                new Column("ACTIVITY_TSJ_ID", DbType.Int64, 22, ColumnProperty.NotNull),
                new Column("EXTERNAL_ID", DbType.String, 36));

            Database.AddIndex("IND_GJI_ACT_TSJ_MMB_AC", false, "GJI_ACT_TSJ_MEMBER", "ACTIVITY_TSJ_ID");
            Database.AddIndex("IND_GJI_ACT_TSJ_MMB_FL", false, "GJI_ACT_TSJ_MEMBER", "FILE_ID");
            Database.AddForeignKey("FK_GJI_ACT_TSJ_MMB_AC", "GJI_ACT_TSJ_MEMBER", "ACTIVITY_TSJ_ID", "GJI_ACTIVITY_TSJ", "ID");
            Database.AddForeignKey("FK_GJI_ACT_TSJ_MMB_FL", "GJI_ACT_TSJ_MEMBER", "FILE_ID", "B4_FILE_INFO", "ID");
        }

        public override void Down()
        {
            Database.RemoveColumn("GJI_ACT_TSJ_MEMBER", "STATE_ID");
            Database.RemoveTable("GJI_ACT_TSJ_MEMBER");

            Database.RemoveConstraint("GJI_ACT_TSJ_MEMBER", "FK_GJI_ACT_TSJ_MMB_AC");
            Database.RemoveConstraint("GJI_ACT_TSJ_MEMBER", "FK_GJI_ACT_TSJ_MMB_FL");
        }
    }
}