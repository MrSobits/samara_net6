namespace Bars.Gkh.Overhaul.Hmao.Migration.Version_2018041900
{
    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using System.Data;

    [Migration("2018041900")]
    [MigrationDependsOn(typeof(Version_2018041700.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        public override void Up()
        {
            Database.AddEntityTable(
               "OVRHL_SUBPROGRAM_CRITERIAS",
               new Column("NAME", DbType.String, ColumnProperty.NotNull),
               new RefColumn("ORERATOR_ID", ColumnProperty.NotNull, "OVRHL_SUBPROGRAM_CRITERIAS_GKH_OPERATOR_ORERATOR_ID_ID", "GKH_OPERATOR", "ID"),
               //
               new Column("IS_STATE_USED", DbType.Boolean, ColumnProperty.NotNull),
               new RefColumn("STATE_ID", ColumnProperty.Null, "OVRHL_SUBPROGRAM_CRITERIAS_B4_STATE_STATE_ID_ID", "B4_STATE", "ID"),
               //
               new Column("IS_TYPE_HOUSE_USED", DbType.Boolean, ColumnProperty.NotNull),
               new Column("TYPE_HOUSE", DbType.Byte, ColumnProperty.NotNull),
               //
               new Column("IS_CONDITION_HOUSE_USED", DbType.Boolean, ColumnProperty.NotNull),
               new Column("CONDITION_HOUSE", DbType.Byte, ColumnProperty.NotNull),
               //
               new Column("IS_NUMBER_APARTMENTS_USED", DbType.Boolean, ColumnProperty.NotNull),
               new Column("NUMBER_APARTMENTS_CONDITION", DbType.Byte, ColumnProperty.NotNull),
               new Column("NUMBER_APARTMENTS", DbType.Int32, ColumnProperty.NotNull),
               //
               new Column("IS_YEAR_REPAIR_USED", DbType.Boolean, ColumnProperty.NotNull),
               new Column("YEAR_REPAIR_CONDITION", DbType.Byte, ColumnProperty.NotNull),
               new Column("YEAR_REPAIR", DbType.Int16, ColumnProperty.NotNull),
               //
               new Column("IS_REPAIR_NOT_ADVISABLE_USED", DbType.Boolean, ColumnProperty.NotNull),
               new Column("REPAIR_NOT_ADVISABLE", DbType.Boolean, ColumnProperty.NotNull),
               //
               new Column("IS_NOT_INVOLVED_CR_USED", DbType.Boolean, ColumnProperty.NotNull),
               new Column("NOT_INVOLVED_CR", DbType.Boolean, ColumnProperty.NotNull),
               //
               new Column("IS_STRUCT_EL_COUNT_USED", DbType.Boolean, ColumnProperty.NotNull),
               new Column("STRUCT_EL_COUNT_CONDITION", DbType.Byte, ColumnProperty.NotNull),
               new Column("STRUCT_EL_COUNT", DbType.Int32, ColumnProperty.NotNull),
               //
               new Column("IS_FLOOR_COUNT_USED", DbType.Boolean, ColumnProperty.NotNull),
               new Column("FLOOR_COUNT_CONDITION", DbType.Byte, ColumnProperty.NotNull),
               new Column("FLOOR_COUNT", DbType.Int32, ColumnProperty.NotNull),
               //
               new Column("IS_LIFETIME_USED", DbType.Boolean, ColumnProperty.NotNull),
               new Column("LIFETIME_CONDITION", DbType.Byte, ColumnProperty.NotNull),
               new Column("LIFETIME", DbType.Int16, ColumnProperty.NotNull)      
               );            
        }

        public override void Down()
        {
            Database.RemoveTable("OVRHL_SUBPROGRAM_CRITERIAS");
        }
    }
}