namespace Bars.Gkh.Entities
{
    using System;

    using Bars.B4.DataAccess;
    using Bars.Gkh.Enums;

    /// <summary>
    /// Представление "Договора с жилым домом"
    /// </summary>
    /*
     * вьюха договоров жилого дома и управляющих организаций
     * для фильтрации в реестрах, отличных от реестра жилых домов
     */
    public class ViewRealityObjectManOrgContract : PersistentObject
    {
        /// <summary>
        /// Муниципальное образование
        /// </summary>
        public virtual RealityObject RealityObject { get; set; }

        /// <summary>
        /// Тип договора с упр орг
        /// </summary>
        public virtual TypeContractManOrg TypeContractManOrgRealObj { get; set; }

        /// <summary>
        /// Управляющая организация
        /// </summary>
        public virtual ManagingOrganization ManagingOrganization { get; set; }

        /// <summary>
        /// Наименование упр орг-ии (контрагента)
        /// </summary>
        public virtual string ManagingOrgname { get; set; }

        /// <summary>
        /// Дата начала договора
        /// </summary>
        public virtual DateTime? StartDate { get; set; }

        /// <summary>
        /// Дата окончания договора
        /// </summary>
        public virtual DateTime? EndDate { get; set; }
    }
}