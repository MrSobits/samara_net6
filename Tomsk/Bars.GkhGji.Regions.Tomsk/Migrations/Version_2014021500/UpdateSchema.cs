namespace Bars.GkhGji.Regions.Tomsk.Migrations.Version_2014021500
{
    using System.Data;
    using global::Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2014021500")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.GkhGji.Regions.Tomsk.Migrations.Version_2014021400.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            //-----Определение постановления прокуратуры
            Database.AddEntityTable(
                "GJI_RESOL_PROS_DEFINITION",
                new Column("RESOL_PROS_ID", DbType.Int64, 22, ColumnProperty.NotNull),
                new Column("ISSUED_DEFINITION_ID", DbType.Int64, 22),
                new Column("EXECUTION_DATE", DbType.DateTime),
                new Column("DOCUMENT_DATE", DbType.DateTime),
                new Column("DESCRIPTION", DbType.String, 500),
                new Column("RESOL_INIT_VIOLATION", DbType.String, 500),
                new Column("RETURN_REASON", DbType.String, 500),
                new Column("REQUEST_NEED", DbType.String, 500),
                new Column("TYPE_RES_PROS_DEFINITION", DbType.Int32, 4, ColumnProperty.NotNull, 10));
            Database.AddIndex("IND_RES_PRS_DEF_DOC", false, "GJI_RESOL_PROS_DEFINITION", "RESOL_PROS_ID");
            Database.AddIndex("IND_RES_PRS_DEF_ISD", false, "GJI_RESOL_PROS_DEFINITION", "ISSUED_DEFINITION_ID");
            Database.AddForeignKey("FK_RES_PRS_DEF_ISD", "GJI_RESOL_PROS_DEFINITION", "ISSUED_DEFINITION_ID", "GKH_DICT_INSPECTOR", "ID");
            Database.AddForeignKey("FK_RES_PRS_DEF_DOC", "GJI_RESOL_PROS_DEFINITION", "RESOL_PROS_ID", "GJI_RESOLPROS", "ID");
        }

        public override void Down()
        {
            Database.RemoveTable("GJI_RESOL_PROS_DEFINITION");
        }
    }
}
