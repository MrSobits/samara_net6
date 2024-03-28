namespace Bars.GkhGji.Regions.BaseChelyabinsk.Migrations.Version_2020101900
{
    using B4.Modules.Ecm7.Framework;
    using System.Data;

    [Migration("2020101900")]
    [MigrationDependsOn(typeof(Version_2020071500.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        public override void Up()
        {
            Database.AddColumn("GJI_DISP_CON_MEASURE", new Column("DATE_START", DbType.DateTime, ColumnProperty.Null));
            Database.AddColumn("GJI_DISP_CON_MEASURE", new Column("DATE_END", DbType.DateTime, ColumnProperty.Null));
            Database.AddColumn("GJI_DISP_CON_MEASURE", new Column("DESCRIPTION", DbType.String, 1500));
            this.Database.ExecuteNonQuery(@"update GJI_DISP_CON_MEASURE cm set DESCRIPTION = ca.name from GJI_DICT_CON_ACTIVITY ca where cm.control_measures_id = ca.id");
        }
        public override void Down()
        {
            Database.RemoveColumn("GJI_DISP_CON_MEASURE", "DESCRIPTION");
            Database.RemoveColumn("GJI_DISP_CON_MEASURE", "DATE_END");
            Database.RemoveColumn("GJI_DISP_CON_MEASURE", "DATE_START");
        }
    }
}