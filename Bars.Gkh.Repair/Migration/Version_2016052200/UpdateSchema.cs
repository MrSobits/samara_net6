namespace Bars.Gkh.Repair.Migration.Version_2016052200
{
    using System.Data;
    using B4.Modules.Ecm7.Framework;
    using B4.Modules.NH.Migrations.DatabaseExtensions;

    [Migration("2016052200")]
    [MigrationDependsOn(typeof(Version_2015121500.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        public override void Up()
        {
            this.Database.AddEntityTable(
                "RP_PERFORMED_REPAIR_WORK_ACT",
                new RefColumn("RP_WORK_ID", ColumnProperty.NotNull, "PERFRWORKACT_RP_WORK_ID", "RP_TYPE_WORK", "ID"),
                new Column("OBJ_PHOTO_DESC", DbType.String),
                new Column("ACT_NUMBER", DbType.String),
                new Column("WORK_VOLUME", DbType.Decimal, ColumnProperty.NotNull),
                new Column("ACT_DATE", DbType.DateTime),
                new Column("ACT_SUM", DbType.Decimal, ColumnProperty.NotNull),
                new Column("ACT_DESC", DbType.String),
                new RefColumn("OBJECT_PHOTO_ID", ColumnProperty.Null, "PERFRWORKACT_OBJ_PHOTO_ID", "B4_FILE_INFO", "ID"),
                new RefColumn("ACT_FILE_ID", ColumnProperty.Null, "PERFRWORKACT_ACT_FILE_ID", "B4_FILE_INFO", "ID"));
        }

        public override void Down()
        {
            this.Database.RemoveTable("RP_PERFORMED_REPAIR_WORK_ACT");
        }
    }
}
