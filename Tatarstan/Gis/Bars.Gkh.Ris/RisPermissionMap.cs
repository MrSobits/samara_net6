namespace Bars.Gkh.Ris
{
    using B4;

    public class RisPermissionMap : PermissionMap
    {
        public RisPermissionMap()
        {
            // экспортеры данных
            this.Namespace("Administration.OutsideSystemIntegrations.Gis.Exporters", "Экспортеры данных");
            this.Permission("Administration.OutsideSystemIntegrations.Gis.Exporters.AcknowledgmentExporter", "Экспорт сведений о квитировании");
            this.Permission("Administration.OutsideSystemIntegrations.Gis.Exporters.ContractDataExporter", "Экспорт договоров управления");
            this.Permission("Administration.OutsideSystemIntegrations.Gis.Exporters.CharterDataExporter", "Экспорт уставов");
            this.Permission("Administration.OutsideSystemIntegrations.Gis.Exporters.MeteringDeviceDataExporter", "Экспорт данных о приборах учёта");
            this.Permission("Administration.OutsideSystemIntegrations.Gis.Exporters.HouseUODataExporter", "Экспорт сведений о доме для управляющих организаций");
            this.Permission("Administration.OutsideSystemIntegrations.Gis.Exporters.HouseOMSDataExporter", "Экспорт сведений о доме для органов местного самоуправления");
            this.Permission("Administration.OutsideSystemIntegrations.Gis.Exporters.HouseRSODataExporter", "Экспорт сведений о доме для ресурсоснабжающих организаций");
            this.Permission("Administration.OutsideSystemIntegrations.Gis.Exporters.AdditionalServicesExporter", "Экспорт данных справочника \"Дополнительные услуги\"");
            this.Permission("Administration.OutsideSystemIntegrations.Gis.Exporters.MunicipalServicesExporter", "Экспорт данных справочника \"Коммунальные услуги\"");
            this.Permission("Administration.OutsideSystemIntegrations.Gis.Exporters.OrganizationWorksExporter", "Экспорт данных справочника \"Работы и услуги\"");
            this.Permission("Administration.OutsideSystemIntegrations.Gis.Exporters.SubsidiaryExporter", "Экспорт сведений об обособленных подразделениях");
            this.Permission("Administration.OutsideSystemIntegrations.Gis.Exporters.NotificationDataExporter", "Экспорт новостей для информирования граждан");
            this.Permission("Administration.OutsideSystemIntegrations.Gis.Exporters.AccountDataExporter", "Экспорт сведений о лицевых счетах");
            this.Permission("Administration.OutsideSystemIntegrations.Gis.Exporters.PublicPropertyContractExporter", "Импорт договора на пользование общим имуществом");
            this.Permission("Administration.OutsideSystemIntegrations.Gis.Exporters.VotingProtocolExporter", "Экспорт протоколов голосования");
            this.Permission("Administration.OutsideSystemIntegrations.Gis.Exporters.WorkingPlanExporter", "Экспорт актуальных планов по перечню работ/услуг");
            this.Permission("Administration.OutsideSystemIntegrations.Gis.Exporters.WorkingListExporter", "Экспорт перечня работ и услуг на период");
            this.Permission("Administration.OutsideSystemIntegrations.Gis.Exporters.RisCompletedWorkExporter", "Экспорт сведений о выполненных работах/услугах");
            this.Permission("Administration.OutsideSystemIntegrations.Gis.Exporters.RkiDataExporter", "Управление ОКИ в РКИ");
            this.Permission("Administration.OutsideSystemIntegrations.Gis.Exporters.MeteringDeviceValuesExporter", "Экспорт сведений о показаниях приборов учета");
            this.Permission("Administration.OutsideSystemIntegrations.Gis.Exporters.NotificationsOfOrderExecutionExporter", "Экспорт документов «Извещение о принятии к исполнению распоряжения»");
            this.Permission("Administration.OutsideSystemIntegrations.Gis.Exporters.SupplierNotificationsOfOrderExecutionExporter", "Экспорт документов «Извещение о принятии к исполнению распоряжения, размещаемых исполнителем»");
            this.Permission("Administration.OutsideSystemIntegrations.Gis.Exporters.SupplyResourceContractExporter", "Экспорт договора ресурсоснабжения");
            this.Permission("Administration.OutsideSystemIntegrations.Gis.Exporters.PaymentDocumentDataExporter", "Экспорт сведений о платежных документах");
            this.Permission("Administration.OutsideSystemIntegrations.Gis.Exporters.NotificationsOfOrderExecutionCancellationExporter", "Экспорт документов «Аннулирование извещения о принятии к исполнению распоряжения»");
            
        }
    }
}