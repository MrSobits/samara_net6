namespace Bars.Gkh.Migrations._2023.Version_2023050103
{
    using System.Data;
    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using Bars.Gkh.Utils;

    [Migration("2023050103")]
    
    [MigrationDependsOn(typeof(Version_2023050102.UpdateSchema))]

    /// Является Version_2018030600 из ядра
    public class UpdateSchema : Migration
    {
        public override void Up()
        {
            this.Database.AddColumn("GKH_MORG_CONTRACT", new Column("IS_LAST_DAY_MDV_BEGIN_DATE", DbType.Boolean, ColumnProperty.NotNull, false));
            this.Database.AddColumn("GKH_MORG_CONTRACT", new Column("IS_LAST_DAY_MDV_END_DATE", DbType.Boolean, ColumnProperty.NotNull, false));
            this.Database.AddColumn("GKH_MORG_CONTRACT", new Column("IS_LAST_DAY_DRAWING_PD_DATE", DbType.Boolean, ColumnProperty.NotNull, false));
            this.Database.AddColumn("GKH_MORG_CONTRACT", new Column("IS_LAST_DAY_PAYMENT_SERV_DATE", DbType.Boolean, ColumnProperty.NotNull, false));

            this.Database.AddColumn("GKH_MORG_CONTRACT_OWNERS",
                new FileColumn("OWNERS_SIGNED_CONTRACT_FILE", "MORG_CONTRACT_OWNERS_SIGNED_CONTRACT_FILE"));

            this.Database.ExecuteNonQuery(
                "INSERT INTO GKH_FIELD_REQUIREMENT (object_version, object_create_date, object_edit_date, requirementid) VALUES " +
                "(0, current_date, current_date, 'Gkh.Orgs.Contragent.Manorg.HouseManaging.TsjJsk.Field.ProtocolNumber_Rqrd'), " +
                "(0, current_date, current_date, 'Gkh.Orgs.Contragent.Manorg.HouseManaging.TsjJsk.Field.ProtocolFileInfo_Rqrd'), " +
                "(0, current_date, current_date, 'Gkh.Orgs.Contragent.Manorg.HouseManaging.TsjJsk.Field.FileInfo_Rqrd'), " +
                "(0, current_date, current_date, 'Gkh.Orgs.Contragent.Manorg.HouseManaging.TsjJsk.Field.InputMeteringDeviceValuesBeginDate_Rqrd'), " +
                "(0, current_date, current_date, 'Gkh.Orgs.Contragent.Manorg.HouseManaging.TsjJsk.Field.InputMeteringDeviceValuesEndDate_Rqrd'), " +
                "(0, current_date, current_date, 'Gkh.Orgs.Contragent.Manorg.HouseManaging.TsjJsk.Field.DrawingPaymentDocumentDate_Rqrd'), " +
                "(0, current_date, current_date, 'Gkh.Orgs.Contragent.Manorg.HouseManaging.TsjJsk.Field.PaymentServicePeriodDate_Rqrd'), " +
                "(0, current_date, current_date, 'Gkh.Orgs.Contragent.Manorg.HouseManaging.UK.Field.OwnersSignedContractFile_Rqrd'), " +
                "(0, current_date, current_date, 'Gkh.Orgs.Contragent.Manorg.HouseManaging.UK.Field.FileInfo_Rqrd'), " +
                "(0, current_date, current_date, 'Gkh.Orgs.Contragent.Manorg.HouseManaging.UK.Field.PlannedEndDate_Rqrd'), " +
                "(0, current_date, current_date, 'Gkh.Orgs.Contragent.Manorg.HouseManaging.UK.Field.Addition_Service_Rqrd'), " +
                "(0, current_date, current_date, 'Gkh.Orgs.Contragent.Manorg.HouseManaging.UK.Field.Addition_StartDate_Rqrd'), " +
                "(0, current_date, current_date, 'Gkh.Orgs.Contragent.Manorg.HouseManaging.UK.Field.Communal_Service_Rqrd'), " +
                "(0, current_date, current_date, 'Gkh.Orgs.Contragent.Manorg.HouseManaging.UK.Field.Communal_StartDate_Rqrd'), " +
                "(0, current_date, current_date, 'Gkh.Orgs.Contragent.Manorg.HouseManaging.UK.Field.Communal_StartDate_Rqrd'), " +
                "(0, current_date, current_date, 'Gkh.Orgs.Contragent.Manorg.HouseManaging.UK.Field.InputMeteringDeviceValuesBeginDate_Rqrd'), " +
                "(0, current_date, current_date, 'Gkh.Orgs.Contragent.Manorg.HouseManaging.UK.Field.InputMeteringDeviceValuesEndDate_Rqrd'), " +
                "(0, current_date, current_date, 'Gkh.Orgs.Contragent.Manorg.HouseManaging.UK.Field.DrawingPaymentDocumentDate_Rqrd'), " +
                "(0, current_date, current_date, 'Gkh.Orgs.Contragent.Manorg.HouseManaging.UK.Field.PaymentServicePeriodDate_Rqrd'), " +
                "(0, current_date, current_date, 'Gkh.Orgs.Contragent.Manorg.HouseManaging.UK.Field.WorkServiceStartDate_Rqrd')");
        }

        /// <inheritdoc />
        public override void Down()
        {
            this.Database.RemoveColumn("GKH_MORG_CONTRACT", "IS_LAST_DAY_MDV_BEGIN_DATE");
            this.Database.RemoveColumn("GKH_MORG_CONTRACT", "IS_LAST_DAY_MDV_END_DATE");
            this.Database.RemoveColumn("GKH_MORG_CONTRACT", "IS_LAST_DAY_DRAWING_PD_DATE");
            this.Database.RemoveColumn("GKH_MORG_CONTRACT", "IS_LAST_DAY_PAYMENT_SERV_DATE");
            this.Database.RemoveColumn("GKH_MORG_CONTRACT_OWNERS", "OWNERS_SIGNED_CONTRACT_FILE");
        }
    }
}