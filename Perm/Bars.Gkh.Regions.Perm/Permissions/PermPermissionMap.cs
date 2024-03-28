namespace Bars.Gkh.Regions.Perm.Permissions
{
    using Bars.B4;

    /// <summary>
    /// Права доступа
    /// </summary>
    public class PermPermissionMap : PermissionMap
    {
        /// <summary>
        /// .ctor
        /// </summary>
        public PermPermissionMap()
        {
            this.Permission("Reports.GKH.UnderstandingHomeReport", "Общие сведения по домам");

            this.Permission("Gkh.RealityObject.Field.Edit.AreaCleaning_Edit", "Уборочная площадь (кв.м.)");
            this.Permission("Gkh.RealityObject.Field.View.AreaCleaning_View", "Уборочная площадь (кв.м.)");

            this.Namespace("GkhGji.Licensing", "Лицензирование");
            this.Namespace("GkhGji.Licensing.FormGovService", "Форма 1-ГУ");
            this.CRUDandViewPermissions("GkhGji.Licensing.FormGovService");
            this.Permission("GkhGji.Licensing.FormGovService.Export", "Экспорт");

            this.Namespace("Gkh.Dictionaries.LicenseRegistrationReason", "Причины переоформления лицензии");

            this.Namespace("GkhGji.HeatSeason.FuelInfoPeriod", "Сведения о наличии и расходе топлива ЖКХ");
            this.CRUDandViewPermissions("GkhGji.HeatSeason.FuelInfoPeriod");

            this.Namespace("Gkh.HousingFundMonitoringPeriod", "Мониторинг жилищного фонда");
            this.CRUDandViewPermissions("Gkh.HousingFundMonitoringPeriod");
            this.CRUDandViewPermissions("Gkh.Dictionaries.LicenseRegistrationReason");

            this.Namespace("Gkh.Dictionaries.LicenseRejectReason", "Причины отказа");
            this.CRUDandViewPermissions("Gkh.Dictionaries.LicenseRejectReason");

            this.Permission("Gkh.ManOrgLicense.Request.Field.DateRequest_View", "Дата заявления - Просмотр");
            this.Permission("Gkh.ManOrgLicense.Request.Field.DateRequest_Edit", "Дата заявления - Редактирование");
            this.Permission("Gkh.ManOrgLicense.Request.Field.RegisterNum_View", "Регистрационный номер - Просмотр");
            this.Permission("Gkh.ManOrgLicense.Request.Field.ConfirmationOfDuty_View", "Документ, подтверждающий уплату гос. пошлины - Просмотр");
            this.Permission("Gkh.ManOrgLicense.Request.Field.ConfirmationOfDuty_Edit", "Документ, подтверждающий уплату гос. пошлины - Редактирование");
            this.Permission("Gkh.ManOrgLicense.Request.Field.TaxSum_View", "Сумма пошлины - Просмотр");
            this.Permission("Gkh.ManOrgLicense.Request.Field.TaxSum_Edit", "Сумма пошлины - Редактирование");
            this.Permission("Gkh.ManOrgLicense.Request.Field.File_View", "Файл - Просмотр");
            this.Permission("Gkh.ManOrgLicense.Request.Field.File_Edit", "Файл - Редактирование");
            this.Permission("Gkh.ManOrgLicense.Request.Field.LicenseRegistrationReason_View", "Причины переоформления лицензии - Просмотр");
            this.Permission("Gkh.ManOrgLicense.Request.Field.LicenseRegistrationReason_Edit", "Причины переоформления лицензии - Редактирование");
            this.Permission("Gkh.ManOrgLicense.Request.Field.LicenseRejectReason_View", "Причина отказа - Просмотр");
            this.Permission("Gkh.ManOrgLicense.Request.Field.LicenseRejectReason_Edit", "Причина отказа - Редактирование");
            this.Permission("Gkh.ManOrgLicense.Request.Field.Note_View", "Примечание - Просмотр");
            this.Permission("Gkh.ManOrgLicense.Request.Field.Note_Edit", "Примечание - Редактирование");
            this.Permission("Gkh.ManOrgLicense.Request.Field.NoticeAcceptanceDate_View", "Дата уведомления о принятии документов к рассмотрению - Просмотр");
            this.Permission("Gkh.ManOrgLicense.Request.Field.NoticeAcceptanceDate_Edit", "Дата уведомления о принятии документов к рассмотрению - Редактирование");
            this.Permission("Gkh.ManOrgLicense.Request.Field.NoticeViolationDate_View", "Дата уведомления об устранении нарушений - Просмотр");
            this.Permission("Gkh.ManOrgLicense.Request.Field.NoticeViolationDate_Edit", "Дата уведомления об устранении нарушений - Редактирование");
            this.Permission("Gkh.ManOrgLicense.Request.Field.ReviewDate_View", "Дата рассмотрения документов - Просмотр");
            this.Permission("Gkh.ManOrgLicense.Request.Field.ReviewDate_Edit", "Дата рассмотрения документов - Редактирование");
            this.Permission("Gkh.ManOrgLicense.Request.Field.NoticeReturnDate_View", "Дата уведомления о возврате документов - Просмотр");
            this.Permission("Gkh.ManOrgLicense.Request.Field.NoticeReturnDate_Edit", "Дата уведомления о возврате документов - Редактирование");
            this.Permission("Gkh.ManOrgLicense.Request.Field.ReviewDateLk_View", "Дата рассмотрения документов ЛК - Просмотр");
            this.Permission("Gkh.ManOrgLicense.Request.Field.ReviewDateLk_Edit", "Дата рассмотрения документов ЛК - Редактирование");
            this.Permission("Gkh.ManOrgLicense.Request.Field.PreparationOfferDate_View", "Дата подготовки мотивированного предложения - Просмотр");
            this.Permission("Gkh.ManOrgLicense.Request.Field.PreparationOfferDate_Edit", "Дата подготовки мотивированного предложения - Редактирование");
            this.Permission("Gkh.ManOrgLicense.Request.Field.SendResultDate_View", "Дата отправки результата - Просмотр");
            this.Permission("Gkh.ManOrgLicense.Request.Field.SendResultDate_Edit", "Дата отправки результата - Редактирование");
            this.Permission("Gkh.ManOrgLicense.Request.Field.SendMethod_View", "Способ отправки - Просмотр");
            this.Permission("Gkh.ManOrgLicense.Request.Field.SendMethod_Edit", "Способ отправки - Редактирование");
        }
    }
}