namespace Bars.GkhDi.Entities
{
    using B4.Modules.FileStorage;

    using Bars.Gkh.Entities;

    /// <summary>
    /// Документы сведений об УО
    /// </summary>
    public class Documents : BaseGkhEntity
    {
        /// <summary>
        /// Сведения об УО
        /// </summary>
        public virtual DisclosureInfo DisclosureInfo { get; set; }

        #region Проект договора управлений с собственником помещений

        /// <summary>
        /// Файл проект договора
        /// </summary>
        public virtual FileInfo FileProjectContract { get; set; }

        /// <summary>
        /// Не имеется проект договора
        /// </summary>
        public virtual bool NotAvailable { get; set; }

        /// <summary>
        /// Описание проект договора
        /// </summary>
        public virtual string DescriptionProjectContract { get; set; }
        
        #endregion

        #region Перечень и качество коммунальных услуг

        /// <summary>
        /// Файл коммунальных услуг
        /// </summary>
        public virtual FileInfo FileCommunalService { get; set; }

        /// <summary>
        /// Описание стоимости коммунальных услуг
        /// </summary>
        public virtual string DescriptionCommunalCost { get; set; }

        /// <summary>
        /// Описание тарифов коммунальных услуг
        /// </summary>
        public virtual string DescriptionCommunalTariff { get; set; }

        #endregion

        #region Базовый перечень показателей качества содержания, эксплуатации и технического обслуживания жилых зданий

        /// <summary>
        /// Файл обслуживания жилых зданий
        /// </summary>
        public virtual FileInfo FileServiceApartment { get; set; }

        #endregion
    }
}
