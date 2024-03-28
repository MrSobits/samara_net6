namespace Bars.GkhGji.Regions.Tatarstan.Entities
{
    using System;

    using Bars.B4.DataModels;
    using Bars.B4.Modules.States;

    /// <summary>
    /// Сущность тип которой возвращает ResolProsAndResolutionService
    /// </summary>
    public class ResolProsAndResolution : IHaveId
    {
        /// <summary>
        /// Идентификатор
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// Статус
        /// </summary>
        public State State { get; set; }

        /// <summary>
        /// Номер документа
        /// </summary>
        public string DocumentNumber { get; set; }

        /// <summary>
        /// Дата документа
        /// </summary>
        public DateTime? DocumentDate { get; set; }

        /// <summary>
        /// Муниципальное образование
        /// </summary>
        public string Municipality { get; set; }

        /// <summary>
        /// Тип контрагента
        /// </summary>
        public string Executant { get; set; }

        /// <summary>
        /// Юридическое лицо
        /// </summary>
        public string Contragent { get; set; }

        /// <summary>
        /// Физическое лицо
        /// </summary>
        public string PhysicalPerson { get; set; }

        /// <summary>
        /// Должностное лицо
        /// </summary>
        public string Inspector { get; set; }

        /// <summary>
        /// Сумма штрафа
        /// </summary>
        public decimal? PenaltyAmount { get; set; }

        /// <summary>
        /// УИН
        /// </summary>
        public string Uin { get; set; }

        /// <summary>
        /// Идентификатор проверки ГЖИ (пердача на фронт для возможности брать записи при помощи метода GetMenu InspectionMenuService)
        /// </summary>
        public long InspectionId { get; set; }
    }
}