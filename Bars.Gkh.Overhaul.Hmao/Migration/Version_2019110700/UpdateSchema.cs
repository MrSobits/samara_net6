namespace Bars.Gkh.Overhaul.Hmao.Migration.Version_2019110700
{
    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using System.Data;

    [Migration("2019110700")]
    [MigrationDependsOn(typeof(Version_2019091201.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        public override void Up()
        {           
           Database.AddEntityTable("OVRHL_ECON_FEASIBILITY_CALC_RESULT",                
                new RefColumn("RO_ID", ColumnProperty.NotNull, "FK_GKH_REALITY_OBJECT_ID", "GKH_REALITY_OBJECT", "ID"),
                new RefColumn("SQUARE_COST_ID", ColumnProperty.NotNull, "FK_SQUARE_COST_ID_ID", "GKH_DICT_LIVING_SQUARE_COST", "ID"),
                new Column("YEAR_START", DbType.Int32),
                new Column("YEAR_END", DbType.Int32),
                new Column("TOTAL_REPAIR_SUMM",DbType.Decimal),
                new Column("TOTAL_SQUARE_COST", DbType.Decimal),
                new Column("PERCENT", DbType.Decimal),
                new Column("DECISION",DbType.Int16)
                );

            Database.AddEntityTable("OVRHL_ECON_FEASIBILITY_WORK",
                new RefColumn("RECORD_ID", ColumnProperty.NotNull, "FK_OVRHL_ECON_FEASIBILITY_CALC_RESULT_ID", "OVRHL_ECON_FEASIBILITY_CALC_RESULT", "ID"),
                new RefColumn("REC_WORK_ID", ColumnProperty.NotNull, "FK_OVRHL_VERSION_REC_ID", "OVRHL_VERSION_REC", "ID")
                );
        }
                       
        public override void Down()
        {
            Database.RemoveTable("OVRHL_ECON_FEASIBILITY_WORK");
            Database.RemoveTable("OVRHL_ECON_FEASIBILITY_CALC_RESULT");  
        }
    }
}