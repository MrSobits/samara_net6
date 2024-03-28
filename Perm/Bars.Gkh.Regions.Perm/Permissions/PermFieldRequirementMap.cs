namespace Bars.Gkh.Regions.Perm.Permissions
{
    using Bars.Gkh.DomainService;

    public class PermFieldRequirementMap : FieldRequirementMap
    {
        public PermFieldRequirementMap()
        {
            this.Namespace("GkhGji.ManOrgLicenseRequest", "Заявление на выдачу лицензии");
            this.Namespace("GkhGji.ManOrgLicenseRequest.Field", "Поля");
            this.Requirement("GkhGji.ManOrgLicenseRequest.Field.DateRequest", "Дата заявления");
            this.Requirement("GkhGji.ManOrgLicenseRequest.Field.RegisterNum", "Регистрационный номер");
            this.Requirement("GkhGji.ManOrgLicenseRequest.Field.ConfirmationOfDuty", "Документ, подтверждающий уплату гос. пошлины");
            this.Requirement("GkhGji.ManOrgLicenseRequest.Field.TaxSum", "Сумма пошлины");
            this.Requirement("GkhGji.ManOrgLicenseRequest.Field.File", "Файл");
            this.Requirement("GkhGji.ManOrgLicenseRequest.Field.LicenseRegistrationReason", "Причины переоформления лицензии");
            this.Requirement("GkhGji.ManOrgLicenseRequest.Field.LicenseRejectReason", "Причина отказа");
            this.Requirement("GkhGji.ManOrgLicenseRequest.Field.Note", "Примечание");
        }

           
    }
}