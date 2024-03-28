namespace Bars.Gkh.Regions.Tatarstan.Migrations._2017.Version_2017092100
{
    using System.Data;

    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

    [Migration("2017092100")]
    [MigrationDependsOn(typeof(Version_2017070400.UpdateSchema))]
    [MigrationDependsOn(typeof(Gkh.Migrations._2017.Version_2017070300.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        public override void Up()
        {
            this.Database.ExecuteNonQuery(@"
                CREATE TABLE IF NOT EXISTS TEMP_GKH_MAN_ORG_ADD_SERVICE AS TABLE GKH_MAN_ORG_ADD_SERVICE;
                CREATE TABLE IF NOT EXISTS TEMP_GKH_MAN_ORG_AGR_SERVICE AS TABLE GKH_MAN_ORG_AGR_SERVICE;
                CREATE TABLE IF NOT EXISTS TEMP_GKH_MAN_ORG_COM_SERVICE AS TABLE GKH_MAN_ORG_COM_SERVICE;
            ");

            this.Database.AddEntityTable("GKH_MAN_ORG_CONTRACT_SERVICE",
                new Column("SERVICE_TYPE", DbType.Int32, ColumnProperty.NotNull),
                new Column("START_DATE", DbType.DateTime, ColumnProperty.Null),
                new Column("END_DATE", DbType.DateTime, ColumnProperty.Null),
                new RefColumn("SERVICE_ID", ColumnProperty.NotNull, "GKH_MAN_ORG_CONTRACT_SERVICE_DICT_MAN_CONTRACT_SERVICE", "GKH_DICT_MAN_CONTRACT_SERVICE", "ID"),
                new RefColumn("CONTRACT_ID", ColumnProperty.NotNull, "GKH_MAN_ORG_CONTRACT_SERVICE_MORG_CONTRACT", "GKH_MORG_CONTRACT", "ID")
            );

            this.Database.RemoveTable("GKH_MAN_ORG_ADD_SERVICE");
            this.Database.RemoveTable("GKH_MAN_ORG_AGR_SERVICE");
            this.Database.RemoveTable("GKH_MAN_ORG_COM_SERVICE");

            this.Database.AddJoinedSubclassTable("GKH_MAN_ORG_AGR_SERVICE",
                "GKH_MAN_ORG_CONTRACT_SERVICE",
                "GKH_MAN_ORG_AGR_SERVICE_BASE_ID",
                new Column("PAYMENT_SUM", DbType.Decimal, ColumnProperty.Null)
            );
            this.Database.AddJoinedSubclassTable("GKH_MAN_ORG_ADD_SERVICE",
                "GKH_MAN_ORG_CONTRACT_SERVICE",
                "GKH_MAN_ORG_ADD_SERVICE_BASE_ID"
            );
            this.Database.AddJoinedSubclassTable("GKH_MAN_ORG_COM_SERVICE",
                "GKH_MAN_ORG_CONTRACT_SERVICE",
                "GKH_MAN_ORG_COM_SERVICE_BASE_ID"
            );

            this.Database.ExecuteNonQuery(@"
                INSERT INTO GKH_MAN_ORG_CONTRACT_SERVICE (
                    id, object_version, object_create_date, object_edit_date, contract_id, service_id, service_type, start_date, end_date
                )
                SELECT * FROM (
                    SELECT
                        export_id, object_version, object_create_date, object_edit_date, contract_id, service_id, 1, start_date, end_date
                    FROM TEMP_GKH_MAN_ORG_ADD_SERVICE
                    UNION
                    SELECT
                        export_id, object_version, object_create_date, object_edit_date, contract_id, service_id, 2, null, null
                    FROM TEMP_GKH_MAN_ORG_AGR_SERVICE
                    UNION
                    SELECT
                        export_id, object_version, object_create_date, object_edit_date, contract_id, service_id, 0, start_date, end_date
                    FROM TEMP_GKH_MAN_ORG_COM_SERVICE
                    ) uni ORDER BY 1;

                INSERT INTO GKH_MAN_ORG_AGR_SERVICE (ID, PAYMENT_SUM)
                    SELECT EXPORT_ID, PAYMENT_SUM FROM TEMP_GKH_MAN_ORG_AGR_SERVICE;

                INSERT INTO GKH_MAN_ORG_ADD_SERVICE (ID)
                    SELECT ID FROM GKH_MAN_ORG_CONTRACT_SERVICE WHERE SERVICE_TYPE = 1;

                INSERT INTO GKH_MAN_ORG_COM_SERVICE (ID)
                    SELECT ID FROM GKH_MAN_ORG_CONTRACT_SERVICE WHERE SERVICE_TYPE = 0;

                SELECT setval('gkh_man_org_contract_service_id_seq', (SELECT max(id) FROM GKH_MAN_ORG_CONTRACT_SERVICE));
            ");
        }

        public override void Down()
        {
        }
    }
}