namespace Bars.Gkh.Overhaul.Nso.Migration.Version_2014060600
{
    using System.Data;

    using global::Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2014060600")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.Overhaul.Nso.Migration.Version_2014050500.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            
            Database.AddTable(
                "NSO_OVRHL_DICT_WORKPRICE",
                new Column("ID", DbType.Int64, 22, ColumnProperty.NotNull | ColumnProperty.Unique),
                new Column("REAL_ESTATE_ID", DbType.Int64, 22));
            Database.AddForeignKey("FK_NSO_O_D_WP_W", "NSO_OVRHL_DICT_WORKPRICE", "ID", "OVRHL_DICT_WORK_PRICE", "ID");

            Database.AddIndex("IND_NSO_O_D_WP_RE", false, "NSO_OVRHL_DICT_WORKPRICE", "REAL_ESTATE_ID");
            Database.AddForeignKey("FK_NSO_O_D_WP_RE", "NSO_OVRHL_DICT_WORKPRICE", "REAL_ESTATE_ID", "OVRHL_REAL_ESTATE_TYPE", "ID");

            Database.ExecuteNonQuery(@"insert into NSO_OVRHL_DICT_WORKPRICE (id)
                                     select id from ovrhl_dict_work_price");
        }

        public override void Down()
        {
            Database.RemoveConstraint("NSO_OVRHL_DICT_WORKPRICE", "FK_NSO_O_D_WP_RE");
            Database.RemoveConstraint("NSO_OVRHL_DICT_WORKPRICE", "FK_NSO_O_D_WP_W");
            Database.RemoveTable("NSO_OVRHL_DICT_WORKPRICE");
        }
    }
}