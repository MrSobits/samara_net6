namespace Bars.Gkh.Decisions.Nso.Entities.Decisions
{
    using System;

    using Bars.B4.Modules.FileStorage;
    using Bars.B4.Modules.States;
    using Bars.Gkh.Decisions.Nso.Domain;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Entities.Dicts;
    using Bars.Gkh.Enums;
    using Bars.Gkh.FormatDataExport.ExportableEntities;

    /// <summary>
    ///     Протокол решения органа государственной власти
    /// </summary>
    public class GovDecision : BaseImportableEntity, IStatefulEntity, IDecisionProtocol, IHaveExportId
    {
        /// <inheritdoc />
        public virtual long ExportId { get; set; }

        /// <summary>
        ///     МКД
        /// </summary>
        public virtual RealityObject RealityObject { get; set; }

        /// <summary>
        ///     Номер протокола
        /// </summary>
        public virtual string ProtocolNumber { get; set; }

        /// <summary>
        ///     Дата портокола
        /// </summary>
        public virtual DateTime ProtocolDate { get; set; }

        /// <summary>
        /// Дата вступления в силу
        /// </summary>
        public virtual DateTime DateStart { get; set; }

        /// <summary>
        ///     Уполномоченное лицо
        /// </summary>
        public virtual string AuthorizedPerson { get; set; }

        /// <summary>
        ///     Управление домом
        /// </summary>
        public virtual string RealtyManagement { get; set; }

        /// <summary>
        ///     Телефон уполномоченного лица
        /// </summary>
        public virtual string AuthorizedPersonPhone { get; set; }

        /// <summary>
        ///     Файл протокола
        /// </summary>
        public virtual FileInfo ProtocolFile { get; set; }

        /// <summary>
        ///     Способ формирования фонда на счету регионального оператора
        /// </summary>
        public virtual bool FundFormationByRegop { get; set; }

        /// <summary>
        ///     Снос МКД
        /// </summary>
        public virtual bool Destroy { get; set; }

        /// <summary>
        ///     Дата сноса МКД
        /// </summary>
        public virtual DateTime? DestroyDate { get; set; }

        /// <summary>
        ///     Реконструкция МКД
        /// </summary>
        public virtual bool Reconstruction { get; set; }

        /// <summary>
        ///     Дата начала реконструкции
        /// </summary>
        public virtual DateTime? ReconstructionStart { get; set; }

        /// <summary>
        ///     Дата окончания реконструкции
        /// </summary>
        public virtual DateTime? ReconstructionEnd { get; set; }

        /// <summary>
        ///     Изъятие для государственных или муниципальных нужд зумельного участка, на котором расположен МКД
        /// </summary>
        public virtual bool TakeLandForGov { get; set; }

        /// <summary>
        ///     Дата изъятия земельного участка
        /// </summary>
        public virtual DateTime? TakeLandForGovDate { get; set; }

        /// <summary>
        ///     Изъятие каждого жилого помещения в доме
        /// </summary>
        public virtual bool TakeApartsForGov { get; set; }

        /// <summary>
        ///     Дата изъятия жилых помещений
        /// </summary>
        public virtual DateTime? TakeApartsForGovDate { get; set; }

        /// <summary>
        /// Максимальный размер фонда
        /// </summary>
        public virtual decimal MaxFund { get; set; }

        /// <summary>
        ///     Состояние
        /// </summary>
        public virtual State State { get; set; }

        /// <summary>
        /// Номер входящего письма
        /// </summary>
        public virtual string LetterNumber { get; set; }

        /// <summary>
        /// Дата входящего письма
        /// </summary>
        public virtual DateTime? LetterDate { get; set; }

        /// <summary>
        /// ГИС ЖКХ GUID
        /// </summary>
        public virtual string GisGkhGuid { get; set; }

        /// <summary>
        /// ГИС ЖКХ Transport GUID
        /// </summary>
        public virtual string GisGkhTransportGuid { get; set; }

        /// <summary>
        /// ГИС ЖКХ GUID вложения
        /// </summary>
        public virtual string GisGkhAttachmentGuid { get; set; }
        
        /// <summary>
        /// Наименование
        /// </summary>
        public virtual string NpaName { get; set; }

        /// <summary>
        /// Дата принятия документа
        /// </summary>
        public virtual DateTime? NpaDate { get; set; }

        /// <summary>
        /// Номер
        /// </summary>
        public virtual string NpaNumber { get; set; }

        /// <summary>
        /// Тип информации в НПА
        /// </summary>
        public virtual TypeInformationNpa TypeInformationNpa { get; set; }

        /// <summary>
        /// Тип НПА
        /// </summary>
        public virtual TypeNpa TypeNpa { get; set; }

        /// <summary>
        /// Вид нормативного акта
        /// </summary>
        public virtual TypeNormativeAct TypeNormativeAct { get; set; }

        /// <summary>
        /// Орган, принявший НПА
        /// </summary>
        public virtual Contragent NpaContragent { get; set; }

        /// <summary>
        /// Файл
        /// </summary>
        public virtual FileInfo NpaFile { get; set; }

        /// <summary>
        /// Статус
        /// </summary>
        public virtual NpaStatus? NpaStatus { get; set; }

        /// <summary>
        /// Причина аннулирования
        /// </summary>
        public virtual string NpaCancellationReason { get; set; }
    }
}