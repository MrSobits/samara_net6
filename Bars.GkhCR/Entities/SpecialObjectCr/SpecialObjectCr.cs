namespace Bars.GkhCr.Entities
{
    using System;

    using Bars.B4.Modules.States;
    using Bars.Gkh.Entities;

    /// <summary>
    /// Объект КР для владельцев специальных счетов
    /// </summary>
    public class SpecialObjectCr : BaseGkhEntity, IStatefulEntity
    {
        /// <summary>
        /// Конструктор
        /// </summary>
        [Obsolete("Хватит создавать объекты просто так!!!", true)]
        public SpecialObjectCr()
        {
        }

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="realty">Жилой дом</param>
        /// <param name="programCr">Программа КР</param>
        public SpecialObjectCr(RealityObject realty, ProgramCr programCr)
        {
            this.ProgramCr = programCr;
            this.RealityObject = realty;
        }

        /// <summary>
        /// Жилой дом
        /// </summary>
        public virtual RealityObject RealityObject { get; set; }

        /// <summary>
        /// Программа
        /// </summary>
        public virtual ProgramCr ProgramCr { get; set; }

        /// <summary>
        /// Программа, на которую ссылался объект до удаления
        /// </summary>
        public virtual ProgramCr BeforeDeleteProgramCr { get; set; }
        
        /// <summary>
        /// Номер ГЖИ
        /// </summary>
        public virtual string GjiNum { get; set; }

        /// <summary>
        /// Номер по программе
        /// </summary>
        public virtual string ProgramNum { get; set; }

        /// <summary>
        /// Дата завершения работ подрядчиком
        /// </summary>
        public virtual DateTime? DateEndBuilder { get; set; }

        /// <summary>
        /// Дата начала работ
        /// </summary>
        public virtual DateTime? DateStartWork { get; set; }

        /// <summary>
        /// Дата окончания работ
        /// </summary>
        public virtual DateTime? DateEndWork { get; set; }

        /// <summary>
        /// Дата остановки работ ГЖИ
        /// </summary>
        public virtual DateTime? DateStopWorkGji { get; set; }

        /// <summary>
        /// Дата отклонения от регистрации
        /// </summary>
        public virtual DateTime? DateCancelReg { get; set; }

        /// <summary>
        /// Дата принятия КР ГЖИ
        /// </summary>
        public virtual DateTime? DateAcceptCrGji { get; set; }

        /// <summary>
        /// Дата принятия на регистрацию
        /// </summary>
        public virtual DateTime? DateAcceptReg { get; set; }

        /// <summary>
        /// Дата регистрации ГЖИ
        /// </summary>
        public virtual DateTime? DateGjiReg { get; set; }

        /// <summary>
        /// Сумма на разработку экспертизы ПСД
        /// </summary>
        public virtual decimal? SumDevolopmentPsd { get; set; }

        /// <summary>
        /// Сумма на СМР
        /// </summary>
        public virtual decimal? SumSmr { get; set; }

        /// <summary>
        /// Утвержденная сумма
        /// </summary>
        public virtual decimal? SumSmrApproved { get; set; }

        /// <summary>
        /// Сумма на технадзор
        /// </summary>
        public virtual decimal? SumTehInspection { get; set; }

        /// <summary>
        /// Федеральный номер
        /// </summary>
        public virtual string FederalNumber { get; set; }

        /// <summary>
        /// Статус
        /// </summary>
        public virtual State State { get; set; }

        /// <summary>
        /// Примечание
        /// </summary>
        public virtual string Description { get; set; }

        /// <summary>
        /// Разрешение на повторное согласование
        /// </summary>
        public virtual bool AllowReneg { get; set; }
        
        /// <summary>
        /// Предельная сумма из КПКР
        /// </summary>
        public virtual decimal? MaxKpkrAmount { get; set; }

        /// <summary>
        /// Фактически освоенная сумма
        /// </summary>
        public virtual decimal? FactAmountSpent { get; set; }

        /// <summary>
        /// Фактическая дата начала работ
        /// </summary>
        public virtual DateTime? FactStartDate { get; set; }

        /// <summary>
        /// Фактическая дата окончания работ
        /// </summary>
        public virtual DateTime? FactEndDate { get; set; }

        /// <summary>
        /// Дата окончания гарантийных обязательств
        /// </summary>
        public virtual DateTime? WarrantyEndDate { get; set; }
    }
}