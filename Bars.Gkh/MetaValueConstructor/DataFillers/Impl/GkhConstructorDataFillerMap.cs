namespace Bars.Gkh.MetaValueConstructor.DataFillers
{
    using Bars.Gkh.MetaValueConstructor.Enums;

    /// <summary>
    /// Маппинг источников данных Gkh
    /// </summary>
    public class GkhConstructorDataFillerMap : ConstructorDataFillerMap
    {
        /// <summary>
        /// Тип конструктора
        /// </summary>
        public override DataMetaObjectType DataMetaObjectType => DataMetaObjectType.EfficientcyRating;

        /// <summary>
        /// Зарегистрировать все сущности
        /// </summary>
        public override void Map()
        {
            this.Namespace("GkhGji", "Жилищная инспекция");
            this.Namespace("GkhGji.License", "Лицензирование");
            this.Namespace("GkhGji.License.RegistryOfficials", "Реестр должностных лиц");
            this.CollectorImpl<CertifiedCompanyStaffDataFiller>("GkhGji.License.RegistryOfficials.CertifiedCompanyStaff", "Сертифицированный персонал компании");

            this.Namespace("GkhContragent", "Участники процесса");
            this.Namespace("GkhContragent.Roles", "Роли контрагента");
            this.Namespace("GkhContragent.Roles.ManagingOrganizations", "Управляющие организации");
            this.Namespace("GkhContragent.Roles.ManagingOrganizations.UK", "УК");
            this.Namespace("GkhContragent.Roles.ManagingOrganizations.UK.GeneralInfo", "Общие сведения");
            this.CollectorImpl<NumberEmployeesDataFiller>("GkhContragent.Roles.ManagingOrganizations.UK.GeneralInfo.NumberEmployees", "Общая численность сотрудников");
        }
    }
}