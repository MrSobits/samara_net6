namespace Bars.GkhCr.Entities
{
    using System;
    using Bars.B4.Modules.FileStorage;
    using Bars.B4.Modules.States;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Enums;

    /// <summary>
    /// Акт выполненных работ
    /// </summary>
    public class SpecialPerformedWorkAct : BaseGkhEntity, IStatefulEntity
    {
        /// <summary>
        /// Объект капитального ремонта
        /// </summary>
        public virtual SpecialObjectCr ObjectCr { get; set; }

        /// <summary>
        /// Работа
        /// </summary>
        public virtual SpecialTypeWorkCr TypeWorkCr { get; set; }

        /// <summary>
        /// Номер акта
        /// </summary>
        public virtual string DocumentNum { get; set; }

        /// <summary>
        /// Объем
        /// </summary>
        public virtual decimal? Volume { get; set; }

        /// <summary>
        /// Сумма
        /// </summary>
        public virtual decimal? Sum { get; set; }

        /// <summary>
        /// Дата от
        /// </summary>
        public virtual DateTime? DateFrom { get; set; }

        /// <summary>
        /// Статус
        /// </summary>
        public virtual State State { get; set; }

        /// <summary>
        /// Справка о стоимости выполненных работ и затрат
        /// </summary>
        public virtual FileInfo CostFile { get; set; }

        /// <summary>
        /// Документ акта
        /// </summary>
        public virtual FileInfo DocumentFile { get; set; }

        /// <summary>
        /// Приложение к акту
        /// </summary>
        public virtual FileInfo AdditionFile { get; set; }

        /// <summary>
        /// Выводить документ на портал
        /// </summary>
        public virtual YesNo UsedInExport { get; set; }

        /// <summary>
        /// Акт подписан представителем собственников
        /// </summary>
        public virtual YesNo RepresentativeSigned { get; set; }

        /// <summary>
        /// Фамилия представителя
        /// </summary>
        public virtual string RepresentativeSurname { get; set; }

        /// <summary>
        /// Имя представителя
        /// </summary>
        public virtual string RepresentativeName { get; set; }

        /// <summary>
        /// Отчество представителя
        /// </summary>
        public virtual string RepresentativePatronymic { get; set; }

        /// <summary>
        /// Принята в эксплуатацию
        /// </summary>
        public virtual YesNo ExploitationAccepted { get; set; }

        /// <summary>
        /// Дата начала гарантийного срока
        /// </summary>
        public virtual DateTime? WarrantyStartDate { get; set; }

        /// <summary>
        /// Дата окончания гарантийного срока
        /// </summary>
        public virtual DateTime? WarrantyEndDate { get; set; }
    }
}
