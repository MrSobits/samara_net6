namespace Bars.Gkh.RegOperator.Migrations._2014.Version_2014020902
{
    using System.Data;
    using global::Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2014020902")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.RegOperator.Migrations._2014.Version_2014020901.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.RemoveTable("REGOP_LOC_CODE");

            Database.AddEntityTable(
                "REGOP_LOC_CODE",
                new Column("CODE_L1", DbType.String, 10, ColumnProperty.NotNull),
                new Column("CODE_L2", DbType.String, 10, ColumnProperty.NotNull),
                new Column("CODE_L3", DbType.String, 10, ColumnProperty.NotNull),
                new RefColumn("FIAS_ID_L1", ColumnProperty.NotNull, "LOC_CODE_MU1", "GKH_DICT_MUNICIPALITY", "ID"),
                new RefColumn("FIAS_ID_L2", ColumnProperty.NotNull, "LOC_CODE_MU2", "GKH_DICT_MUNICIPALITY", "ID"),
                new RefColumn("FIAS_ID_L3", ColumnProperty.NotNull, "LOC_CODE_FIAS3", "B4_FIAS", "ID"));
        }

        public override void Down()
        {
            Database.RemoveTable("REGOP_LOC_CODE");

            Database.AddEntityTable("REGOP_LOC_CODE",
               new Column("CODE_L1", DbType.String, 10, ColumnProperty.NotNull),
               new Column("CODE_L2", DbType.String, 10, ColumnProperty.NotNull),
               new Column("CODE_L3", DbType.String, 10, ColumnProperty.NotNull),
               new RefColumn("FIAS_ID_L1", ColumnProperty.NotNull, "LOC_CODE_FIAS1", "B4_FIAS", "ID"),
               new RefColumn("FIAS_ID_L2", ColumnProperty.NotNull, "LOC_CODE_FIAS2", "B4_FIAS", "ID"),
               new RefColumn("FIAS_ID_L3", ColumnProperty.NotNull, "LOC_CODE_FIAS3", "B4_FIAS", "ID"));
        }
    }
}
