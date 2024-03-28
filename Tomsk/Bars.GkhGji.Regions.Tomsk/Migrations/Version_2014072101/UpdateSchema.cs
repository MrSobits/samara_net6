namespace Bars.GkhGji.Regions.Tomsk.Migrations.Version_2014072101
{
    using System.Data;

    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2014072101")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.GkhGji.Regions.Tomsk.Migrations.Version_2014072100.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.AddTable(
                "TOMSK_GJI_ARTICLELAW",
                new Column("ID", DbType.Int64, 22, ColumnProperty.NotNull | ColumnProperty.Unique),
                new Column("PHYS_PENALTY", DbType.Decimal),
                new Column("JUR_PENALTY", DbType.Decimal));

            Database.AddForeignKey("FK_TOMSK_GJI_ARTICLELAW", "TOMSK_GJI_ARTICLELAW", "ID", "GJI_DICT_ARTICLELAW", "ID");

            Database.ExecuteNonQuery(@"insert into TOMSK_GJI_ARTICLELAW (id)
                                     select id from GJI_DICT_ARTICLELAW");
        }

        public override void Down()
        {
            Database.RemoveTable("TOMSK_GJI_ARTICLELAW");
        }
    }
}