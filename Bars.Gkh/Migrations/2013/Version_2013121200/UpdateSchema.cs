namespace Bars.Gkh.Migrations.Version_2013121200
{
    using System.Data;

    using global::Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2013121200")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.Migrations.Version_2013121101.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.AddColumn("GKH_OBJ_DIRECT_MANAG_CNRT", new Column("SERV_START_DATE", DbType.DateTime));
            Database.AddColumn("GKH_OBJ_DIRECT_MANAG_CNRT", new Column("SERV_END_DATE", DbType.DateTime));
            Database.AddColumn("GKH_OBJ_DIRECT_MANAG_CNRT", new Column("IS_SERV_CONTR", DbType.Boolean, ColumnProperty.NotNull, false));
            Database.AddColumn("GKH_OBJ_DIRECT_MANAG_CNRT", new RefColumn("SERV_FILE_ID", "CONTR_DIR_MANAG_FILE", "B4_FILE_INFO", "ID"));
        }
        
        public override void Down()
        {
            Database.RemoveColumn("GKH_OBJ_DIRECT_MANAG_CNRT", "SERV_FILE_ID");
            Database.RemoveColumn("GKH_OBJ_DIRECT_MANAG_CNRT", "SERV_START_DATE");
            Database.RemoveColumn("GKH_OBJ_DIRECT_MANAG_CNRT", "SERV_END_DATE");
            Database.RemoveColumn("GKH_OBJ_DIRECT_MANAG_CNRT", "IS_SERV_CONTR");
        }
    }
}