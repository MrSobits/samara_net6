namespace Bars.Gkh.Regions.Tatarstan.Migrations._2017.Version_2017012800
{
    using System.Data;

    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

    [Migration("2017012800")]
    [MigrationDependsOn(typeof(_2016.Version_2016121500.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        public override void Up()
        {
            this.Database.AddEntityTable("GKH_MAN_ORG_AGR_SERVICE",
                    new RefColumn("CONTRACT_ID", ColumnProperty.NotNull, "MAN_ORG_AGR_SERVICE_CONTRACT_ID", "GKH_MORG_CONTRACT", "ID"),
                    new RefColumn("SERVICE_ID", ColumnProperty.NotNull, "FK_DICT_AGR_SERVICE", "GKH_DICT_AGR_CONTRACT_SERVICE", "ID"),
                    new Column("PAYMENT_SUM", DbType.Decimal, ColumnProperty.Null));

            this.Database.AddEntityTable("GKH_MAN_ORG_ADD_SERVICE",
                    new RefColumn("CONTRACT_ID", ColumnProperty.NotNull, "MAN_ORG_ADD_SERVICE_CONTRACT_ID", "GKH_MORG_CONTRACT", "ID"),
                    new RefColumn("SERVICE_ID", ColumnProperty.NotNull, "FK_DICT_ADD_SERVICE", "GKH_DICT_ADD_CONTRACT_SERVICE", "ID"),
                    new Column("START_DATE", DbType.DateTime, ColumnProperty.Null),
                    new Column("END_DATE", DbType.DateTime, ColumnProperty.Null));

            this.Database.AddEntityTable("GKH_MAN_ORG_COM_SERVICE",
                    new RefColumn("CONTRACT_ID", ColumnProperty.NotNull, "MAN_ORG_COM_SERVICE_CONTRACT_ID", "GKH_MORG_CONTRACT", "ID"),
                    new RefColumn("SERVICE_ID", ColumnProperty.NotNull, "FK_DICT_COM_SERVICE", "GKH_DICT_COM_CONTRACT_SERVICE", "ID"),
                    new Column("START_DATE", DbType.DateTime, ColumnProperty.Null),
                    new Column("END_DATE", DbType.DateTime, ColumnProperty.Null));
        }

        public override void Down()
        {
            this.Database.RemoveTable("GKH_MAN_ORG_AGR_SERVICE");
            this.Database.RemoveTable("GKH_MAN_ORG_ADD_SERVICE");
            this.Database.RemoveTable("GKH_MAN_ORG_COM_SERVICE");
        }
    }
}