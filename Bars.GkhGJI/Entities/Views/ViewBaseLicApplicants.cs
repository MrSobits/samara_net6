namespace Bars.GkhGji.Entities
{
    using Bars.B4.DataAccess;
    using Bars.B4.Modules.States;
    using Bars.Gkh.Enums;
    using Bars.GkhGji.Enums;

    /*
     * Данная вьюха прденазначена для реестра проверок соискателей
     * наличие распоряжения,
     * муниципальное образование первого дома
    */

    public class ViewBaseLicApplicants : PersistentObject
    {
        /// <summary>
        /// Наличие распоряжения
        /// </summary>
        public virtual bool IsDisposal { get; set; }

        /// <summary>
        /// Наименования муниципальных образований жилых домов
        /// </summary>
        public virtual string MunicipalityNames { get; set; }

        /// <summary>
        /// Наименования муниципальных образований жилых домов
        /// </summary>
        public virtual string MoNames { get; set; }

        /// <summary>
        /// Наименования населенных пунктов жилых домов
        /// </summary>
        public virtual string PlaceNames { get; set; }

        /// <summary>
        /// Муниципальное образование первого жилого дома
        /// </summary>
        public virtual long? MunicipalityId { get; set; }

        /// <summary>
        /// Контрагент (в отношении)
        /// </summary>
        public virtual string ContragentName { get; set; }

        /// <summary>
        /// Объект проверки
        /// </summary>
        public virtual PersonInspection PersonInspection { get; set; }

        /// <summary>
        /// Тип контрагента
        /// </summary>
        public virtual TypeJurPerson? TypeJurPerson { get; set; }

        /// <summary>
        /// Номер проверки
        /// </summary>
        public virtual string InspectionNumber { get; set; }

        /// <summary>
        /// Адреса домов
        /// </summary>
        public virtual string RealObjAddresses { get; set; }

        /// <summary>
        /// Статус
        /// </summary>
        public virtual State State { get; set; }

        /// <summary>
        /// Номер обращения
        /// </summary>
        public virtual string ReqNumber { get; set; }

        /// <summary>
        /// Id юр. лица
        /// </summary>
        public virtual long? ContragentId { get; set; }
    }
}