namespace Bars.Gkh.Migrations._2015.Version_2015122500
{
    using System.Data;

    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2015122500")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.Migrations._2015.Version_2015121700.UpdateSchema))]
    public class UpdateSchema: global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.AddColumn("GKH_DICT_WORK", new Column("WORK_ASSIGNMENT", DbType.Int32, 4));

            Database.AddEntityTable(
                "GKH_DICT_CONTENT_REPAIR_MKD_WORK",
                new RefColumn("WORK_ID", "GKH_DICT_CONTENT_REPAIR_MKD_WORK_WORK", "GKH_DICT_WORK", "ID"),
                new Column("NAME", DbType.String, 300, ColumnProperty.NotNull),
                new Column("DESCRIPTION", DbType.String, 500),
                new Column("CODE", DbType.String, 10));
            Database.AddIndex("IND_GKH_DICT_CONTENT_REPAIR_MKD_WORK_NAME", false, "GKH_DICT_CONTENT_REPAIR_MKD_WORK", "NAME");
            Database.AddIndex("IND_GKH_DICT_CONTENT_REPAIR_MKD_WORK_CODE", false, "GKH_DICT_CONTENT_REPAIR_MKD_WORK", "CODE");

            Database.AddColumn("GKH_MAN_ORG_SERVICE", new RefColumn("WORK_ID", "GKH_MAN_ORG_SERVICE_WORK", "GKH_DICT_WORK", "ID"));
        }

        public override void Down()
        {
            Database.RemoveColumn("GKH_DICT_WORK", "WORK_ASSIGNMENT");

            Database.RemoveTable("GKH_DICT_CONTENT_REPAIR_MKD_WORK");

            Database.RemoveColumn("GKH_MAN_ORG_SERVICE", "WORK_ID");
        }
    }
}
