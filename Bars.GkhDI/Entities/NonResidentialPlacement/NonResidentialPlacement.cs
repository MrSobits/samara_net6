namespace Bars.GkhDi.Entities
{
    using System;
    using Bars.Gkh.Entities;
    using Enums;

    /// <summary>
    /// Сведения об использование нежилых помещений
    /// </summary>
    public class NonResidentialPlacement : BaseGkhEntity
    {
        #warning Оставить ссылку только на дом
        /// <summary>
        /// Объект в управление
        /// </summary>
        public virtual DisclosureInfoRealityObj DisclosureInfoRealityObj { get; set; }

        /// <summary>
        /// Тип контрагента
        /// </summary>
        public virtual TypeContragentDi TypeContragentDi { get; set; }

        /// <summary>
        /// Наименование контрагента
        /// </summary>
        public virtual string ContragentName { get; set; }

        /// <summary>
        /// Площадь помещения
        /// </summary>
        public virtual decimal? Area { get; set; }

        /// <summary>
        /// Дата начала
        /// </summary>
        public virtual DateTime? DateStart { get; set; }

        /// <summary>
        /// Дата окончания
        /// </summary>
        public virtual DateTime? DateEnd { get; set; }

        /// <summary>
        /// Номер документа жилищная услуга
        /// </summary>
        public virtual string DocumentNumApartment { get; set; }

        /// <summary>
        /// Дата документа жилищная услуга
        /// </summary>
        public virtual DateTime? DocumentDateApartment { get; set; }

        /// <summary>
        /// Документ жилищная услуга
        /// </summary>
        public virtual string DocumentNameApartment { get; set; }

        /// <summary>
        /// Номер документа коммунальная услуга
        /// </summary>
        public virtual string DocumentNumCommunal { get; set; }

        /// <summary>
        /// Дата документа коммунальная услуга
        /// </summary>
        public virtual DateTime? DocumentDateCommunal { get; set; }

        /// <summary>
        /// Документ коммунальная услуга
        /// </summary>
        public virtual string DocumentNameCommunal { get; set; }
    }
}
