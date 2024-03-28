namespace Bars.Gkh.Entities
{
    using System;

    using Bars.B4.Modules.FileStorage;
    using Bars.B4.Modules.States;
    using Bars.Gkh.Entities.Dicts;
    using Bars.Gkh.Enums;

    /// <summary>
    /// Аварийность жилого дома
    /// </summary>
    public class EmergencyObject : BaseGkhEntity, IStatefulEntity
    {
        /// <summary>
        /// Объект недвижимости
        /// </summary>
        public virtual RealityObject RealityObject { get; set; }

        /// <summary>
        /// Дальнейшее использование
        /// </summary>
        public virtual FurtherUse FurtherUse { get; set; }

        /// <summary>
        /// Основание нецелесообразности
        /// </summary>
        public virtual ReasonInexpedient ReasonInexpedient { get; set; }

        /// <summary>
        /// Дата актуальности информации
        /// </summary>
        public virtual DateTime? ActualInfoDate { get; set; }

        /// <summary>
        /// Кадастровый номер
        /// </summary>
        public virtual string CadastralNumber { get; set; }

        /// <summary>
        /// Наименование документа
        /// </summary>
        public virtual string DocumentName { get; set; }

        /// <summary>
        /// Номер документа
        /// </summary>
        public virtual string DocumentNumber { get; set; }

        /// <summary>
        /// Дата документа
        /// </summary>
        public virtual DateTime? DocumentDate { get; set; }

        /// <summary>
        /// Файл
        /// </summary>
        public virtual FileInfo FileInfo { get; set; }

        /// <summary>
        /// Описание
        /// </summary>
        public virtual string Description { get; set; }

        /// <summary>
        /// Наименование документа, подтверждающего признание МКД аварийным
        /// </summary>
        public virtual string EmergencyDocumentName { get; set; }

        /// <summary>
        /// Номер документа, подтверждающего признание МКД аварийным
        /// </summary>
        public virtual string EmergencyDocumentNumber { get; set; }

        /// <summary>
        /// Дата документа, подтверждающего признание МКД аварийным
        /// </summary>
        public virtual DateTime? EmergencyDocumentDate { get; set; }

        /// <summary>
        /// Файл документа, подтверждающего признание МКД аварийным
        /// </summary>
        public virtual FileInfo EmergencyFileInfo { get; set; }

        /// <summary>
        /// Планируемая дата сноса/реконструкции МКД
        /// </summary>
        public virtual DateTime? DemolitionDate { get; set; }

        /// <summary>
        /// Планируемая дата завершения переселения
        /// </summary>
        public virtual DateTime? ResettlementDate { get; set; }

        /// <summary>
        /// Фактическая дата сноса/реконструкции МКД
        /// </summary>
        public virtual DateTime? FactDemolitionDate { get; set; }

        /// <summary>
        /// Фактическая дата завершения переселения
        /// </summary>
        public virtual DateTime? FactResettlementDate { get; set; }
        
        /// <summary>
        /// Площадь земельного участка
        /// </summary>
        public virtual decimal? LandArea { get; set; }

        /// <summary>
        /// Площадь расселяемых жилых помещений
        /// </summary>
        public virtual decimal? ResettlementFlatArea { get; set; }

        /// <summary>
        /// Кол-во расселяемых жилых помещений
        /// </summary>
        public virtual int? ResettlementFlatAmount { get; set; }

        /// <summary>
        /// Число жителей планируемых к переселению
        /// </summary>
        public virtual int? InhabitantNumber { get; set; }

        /// <summary>
        /// Ремонт целесообразен
        /// </summary>
        public virtual bool IsRepairExpedient { get; set; }

        /// <summary>
        /// Состояние дома
        /// </summary>
        public virtual ConditionHouse ConditionHouse { get; set; }

        /// <summary>
        /// Программа переселения
        /// </summary>
        public virtual ResettlementProgram ResettlementProgram { get; set; }

        /// <summary>
        /// Статус
        /// </summary>
        public virtual State State { get; set; }

        /// <summary>
        /// Основание изъятия
        /// </summary>
        public virtual string ExemptionsBasis { get; set; }

        /// <summary>
        /// Полный адрес
        /// </summary>
        public virtual string AddressName
        {
            get
            {
                return RealityObject != null ? RealityObject.FiasAddress.AddressName : string.Empty;
            }
        }
    }
}
