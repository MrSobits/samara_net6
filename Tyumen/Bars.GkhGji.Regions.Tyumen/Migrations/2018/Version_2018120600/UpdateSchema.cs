namespace Bars.GkhGji.Regions.Tyumen.Migrations.Version_2018120600
{
    using Bars.B4.Modules.Ecm7.Framework;
    using global::Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using System.Data;

    [Migration("2018120600")]
    [MigrationDependsOn(typeof(Bars.GkhGji.Regions.Tyumen.Migrations.Version_2015120600.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.AddEntityTable("GKH_LIC_PRESCRIPTION",
                new Column("CODE", DbType.String,  ColumnProperty.Null),
                new Column("ACTUAL_DATE", DbType.DateTime, ColumnProperty.Null),
                new Column("DOCUMENT_DATE", DbType.DateTime, ColumnProperty.NotNull),
                new Column("DOCUMENT_NUMBER", DbType.String, ColumnProperty.Null),
                new Column("PENALTY", DbType.Decimal, ColumnProperty.Null),
                new Column("IS_CANCELED", DbType.Int32, 4, ColumnProperty.NotNull, 30),
                new RefColumn("ART_LAW_ID", ColumnProperty.NotNull, "FK_LIC_PR_ARTLAW", "GJI_DICT_ARTICLELAW", "ID"),
                new RefColumn("SANCTION_ID", ColumnProperty.NotNull, "FK_LIC_PR_SANCTION", "GJI_DICT_SANCTION", "ID"),
                new RefColumn("FILE_INFO_ID", ColumnProperty.NotNull, "FK_LIC_PR_FILE", "B4_FILE_INFO", "ID"),
                new RefColumn("MC_ID", ColumnProperty.NotNull, "FK_LIC_PR_MC", "GKH_MORG_CONTRACT_REALOBJ", "ID"));

        }

        public override void Down()
        {
            Database.RemoveTable("GKH_LIC_PRESCRIPTION");
        }
    }
}