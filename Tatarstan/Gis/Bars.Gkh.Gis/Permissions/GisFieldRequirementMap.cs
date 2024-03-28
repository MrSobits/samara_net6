namespace Bars.Gkh.Gis.Permissions
{
    using Bars.Gkh.DomainService;

    /// <summary>
    /// Обязательность полей ГИС
    /// </summary>
    public class GisFieldRequirementMap : FieldRequirementMap
    {
        /// <summary>
        /// .ctor
        /// </summary>
        public GisFieldRequirementMap()
        {
            // Управляющие организации
            // УК
            this.Requirement("Gkh.Orgs.Contragent.Manorg.HouseManaging.UK.Field.WorkServiceName_Rqrd", "Работы / услуги - Наименование");
            this.Requirement("Gkh.Orgs.Contragent.Manorg.HouseManaging.UK.Field.WorkServiceType_Rqrd", "Работы / услуги - Тип работ");
            this.Requirement("Gkh.Orgs.Contragent.Manorg.HouseManaging.UK.Field.WorkServicPaymentAmount_Rqrd", "Работы / услуги - Размер платы (цена) за услуги, работы по управлению домом");

            // ТСЖ/ЖСК
            this.Requirement("Gkh.Orgs.Contragent.Manorg.HouseManaging.TsjJsk.Field.WorkServiceName_Rqrd", "Работы / услуги - Наименование");
            this.Requirement("Gkh.Orgs.Contragent.Manorg.HouseManaging.TsjJsk.Field.WorkServiceType_Rqrd", "Работы / услуги - Тип работ");
            this.Requirement("Gkh.Orgs.Contragent.Manorg.HouseManaging.TsjJsk.Field.WorkServicPaymentAmount_Rqrd", "Работы / услуги - Размер платы (цена) за услуги, работы по управлению домом");
        }
    }
}