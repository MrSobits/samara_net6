namespace Bars.Gkh.Overhaul.Hmao.Migration.Version_2014060600
{
    using System.Data;

    using global::Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2014060600")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.Overhaul.Hmao.Migration.Version_2014060502.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.RemoveTable("OVRHL_REALEST_WORKPRICE");

            Database.AddTable(
                "HMAO_OVRHL_DICT_WORKPRICE",
                new Column("ID", DbType.Int64, 22, ColumnProperty.NotNull | ColumnProperty.Unique),
                new Column("REAL_ESTATE_ID", DbType.Int64, 22));
            Database.AddForeignKey("FK_HMAO_OVRHL_DICT_WORKPRICE_W", "HMAO_OVRHL_DICT_WORKPRICE", "ID", "OVRHL_DICT_WORK_PRICE", "ID");

            Database.AddIndex("IND_HMAOOVRHL_WORKPRICE_RE", false, "HMAO_OVRHL_DICT_WORKPRICE", "REAL_ESTATE_ID");
            Database.AddForeignKey("FK_HMAOOVRHL_DICT_WORKPRICE_RE", "HMAO_OVRHL_DICT_WORKPRICE", "REAL_ESTATE_ID", "OVRHL_REAL_ESTATE_TYPE", "ID");

            Database.ExecuteNonQuery(@"insert into HMAO_OVRHL_DICT_WORKPRICE (id)
                                     select id from ovrhl_dict_work_price");
        }

        public override void Down()
        {
            Database.AddEntityTable("OVRHL_REALEST_WORKPRICE",
                new RefColumn("REAL_ESTATE_ID", ColumnProperty.NotNull, "OVRHL_REALEST_WORKPRICE_RE", "OVRHL_REAL_ESTATE_TYPE", "ID"),
                new RefColumn("WORK_PRICE_ID", ColumnProperty.NotNull, "OVRHL_REALEST_WORKPRICE_WP", "OVRHL_DICT_WORK_PRICE", "ID"));

            Database.RemoveConstraint("HMAO_OVRHL_DICT_WORKPRICE", "FK_HMAO_OVRHL_DICT_WORKPRICE_W");
            Database.RemoveConstraint("HMAO_OVRHL_DICT_WORKPRICE", "FK_HMAOOVRHL_DICT_WORKPRICE_RE");
            Database.RemoveTable("HMAO_OVRHL_DICT_WORKPRICE");
        }
    }
}