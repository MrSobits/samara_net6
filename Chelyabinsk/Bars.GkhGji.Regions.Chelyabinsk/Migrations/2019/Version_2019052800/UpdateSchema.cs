namespace Bars.GkhGji.Regions.Chelyabinsk.Migrations.Version_2019052800
{
    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using System.Data;

    [Migration("2019052800")]
    [MigrationDependsOn(typeof(Version_2019032200.UpdateSchema))]

    public class UpdateSchema : Migration
    {
        public override void Up()
        {
            Database.AddEntityTable("GJI_SSTU_EXPORT_TASK",
                   new Column("SSTU_EXPORT_STATE", DbType.Int32, 4, ColumnProperty.NotNull, 10),
                   new Column("SSTU_SOURCE", DbType.Int32, 4, ColumnProperty.NotNull, 10),
                   new Column("EXPORTEXPORTED", DbType.Boolean, ColumnProperty.NotNull, false),
                   new RefColumn("OPERATOR_ID", ColumnProperty.NotNull, "GJI_SSTU_EXPORT_TASK_OPER", "GKH_OPERATOR", "ID"),
                   new RefColumn("FILE_ID", ColumnProperty.None, "SSTU_EXPORT_TASK_FILE", "B4_FILE_INFO", "ID"));

            Database.AddEntityTable("GJI_SSTU_EXPORT_TASK_APPEAL",
                   new RefColumn("GJI_SSTU_EXPORT_TASK_ID", ColumnProperty.NotNull, "TASK_APPEAL_TASK", "GJI_SSTU_EXPORT_TASK", "ID"),
                   new RefColumn("GJI_APPEAL_ID", ColumnProperty.NotNull, "TASK_APPEAL_APPEAL", "GJI_APPEAL_CITIZENS", "ID"));

            Database.AddEntityTable("GJI_EXPORTED_APPEAL",
                 new Column("EXPORT_DATE", DbType.DateTime),
                new RefColumn("GJI_APPEAL_ID", ColumnProperty.NotNull, "EXPORTED_APPEAL_APPEAL", "GJI_APPEAL_CITIZENS", "ID"));
        }

        public override void Down()
        {
            Database.RemoveTable("GJI_SSTU_EXPORT_TASK_APPEAL");
            Database.RemoveTable("GJI_SSTU_EXPORT_TASK");
            Database.RemoveTable("GJI_EXPORTED_APPEAL");
        }
    }
}


