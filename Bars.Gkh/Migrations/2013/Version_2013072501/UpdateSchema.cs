namespace Bars.Gkh.Migrations.Version_2013072501
{
    using System.Data;

    using global::Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2013072501")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.Migrations.Version_2013072500.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
             Database.AddColumn("GKH_REALITY_OBJECT", new Column("TOTAL_BUILD_VOL", DbType.Decimal));
             Database.AddColumn("GKH_REALITY_OBJECT", new Column("AREA_OWNED", DbType.Decimal));
             Database.AddColumn("GKH_REALITY_OBJECT", new Column("AREA_MUNICIPAL_OWNED", DbType.Decimal));
             Database.AddColumn("GKH_REALITY_OBJECT", new Column("AREA_GOVERNMENT_OWNED", DbType.Decimal));
             Database.AddColumn("GKH_REALITY_OBJECT", new Column("AREA_NOT_LIV_FUNCT", DbType.Decimal));
             Database.AddColumn("GKH_REALITY_OBJECT", new Column("CADASTRE_NUMBER", DbType.String, 300));
             Database.AddColumn("GKH_REALITY_OBJECT", new Column("NECESSARY_CONDUCT_CR", DbType.Int32, 4, ColumnProperty.NotNull, 30));
             Database.AddColumn("GKH_REALITY_OBJECT", new Column("FLOOR_HEIGHT", DbType.Decimal));
             Database.AddColumn("GKH_REALITY_OBJECT", new Column("PERCENT_DEBT", DbType.Decimal));

              Database.AddEntityTable(
               "GKH_REALOBJ_ELEMENT_OBJ",
               new RefColumn("REALITY_OBJECT_ID", "GKH_REALEL_OBJ_RO", "GKH_REALITY_OBJECT", "ID"),
               new RefColumn("ELEMENT_OBJECT_ID", "GKH_REALEL_OBJ_EO", "GKH_DICT_GROUP_ELEM_OBJ", "ID"),
               new Column("IS_EXIST", DbType.Int32, 4, ColumnProperty.NotNull, 30),
               new Column("YEAR_LAST_CR", DbType.Int32));
        }

        public override void Down()
        {
            Database.RemoveColumn("GKH_REALITY_OBJECT", "TOTAL_BUILD_VOL");
            Database.RemoveColumn("GKH_REALITY_OBJECT", "AREA_OWNED");
            Database.RemoveColumn("GKH_REALITY_OBJECT", "AREA_MUNICIPAL_OWNED");
            Database.RemoveColumn("GKH_REALITY_OBJECT", "AREA_GOVERNMENT_OWNED");
            Database.RemoveColumn("GKH_REALITY_OBJECT", "AREA_NOT_LIV_FUNCT");
            Database.RemoveColumn("GKH_REALITY_OBJECT", "CADASTRE_NUMBER");
            Database.RemoveColumn("GKH_REALITY_OBJECT", "NECESSARY_CONDUCT_CR");
            Database.RemoveColumn("GKH_REALITY_OBJECT", "FLOOR_HEIGHT");
            Database.RemoveColumn("GKH_REALITY_OBJECT", "PERCENT_DEBT");

            Database.RemoveTable("GKH_REALOBJ_ELEMENT_OBJ");
        }
    }
}          