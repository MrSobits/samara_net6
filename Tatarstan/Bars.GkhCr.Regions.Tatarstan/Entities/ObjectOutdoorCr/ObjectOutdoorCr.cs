namespace Bars.GkhCr.Regions.Tatarstan.Entities.ObjectOutdoorCr
{
    using System;

    using Bars.B4.DataAccess;
    using Bars.B4.Modules.States;
    using Bars.Gkh.Entities.RealityObj;
    using Bars.GkhCr.Regions.Tatarstan.Entities.Dict.RealityObjectOutdoorProgram;

    /// <summary>
    /// Объект программы благоустройства дворов.
    /// </summary>
    public class ObjectOutdoorCr : BaseEntity, IStatefulEntity
    {
        /// <summary>
        /// Двор.
        /// </summary>
        public virtual RealityObjectOutdoor RealityObjectOutdoor { get; set; }

        /// <summary>
        /// программа благоустройства.
        /// </summary>
        public virtual RealityObjectOutdoorProgram RealityObjectOutdoorProgram { get; set; }

        /// <summary>
        /// Программа, на которую ссылался объект до удаления.
        /// </summary>
        public virtual RealityObjectOutdoorProgram BeforeDeleteRealityObjectOutdoorProgram { get; set; }
        
        /// <summary>
        /// Дата завершения работ подрядчиком.
        /// </summary>
        public virtual DateTime? DateEndBuilder { get; set; }

        /// <summary>
        /// Дата начала работ.
        /// </summary>
        public virtual DateTime? DateStartWork { get; set; }

        /// <summary>
        /// Дата окончания работ.
        /// </summary>
        public virtual DateTime? DateEndWork { get; set; }

        /// <summary>
        /// Дата принятия на регистрацию.
        /// </summary>
        public virtual DateTime? СommissioningDate { get; set; }

        /// <summary>
        /// Сумма на разработку экспертизы ПСД
        /// </summary>
        public virtual decimal? SumDevolopmentPsd { get; set; }

        /// <summary>
        /// Сумма на СМР.
        /// </summary>
        public virtual decimal? SumSmr { get; set; }

        /// <summary>
        /// Утвержденная сумма.
        /// </summary>
        public virtual decimal? SumSmrApproved { get; set; }
        
        /// <summary>
        /// Статус.
        /// </summary>
        public virtual State State { get; set; }

        /// <summary>
        /// Примечание.
        /// </summary>
        public virtual string Description { get; set; }

        /// <summary>
        /// Предельная сумма.
        /// </summary>
        public virtual decimal? MaxAmount { get; set; }

        /// <summary>
        /// Фактически освоенная сумма.
        /// </summary>
        public virtual decimal? FactAmountSpent { get; set; }

        /// <summary>
        /// Фактическая дата начала работ.
        /// </summary>
        public virtual DateTime? FactStartDate { get; set; }

        /// <summary>
        /// Фактическая дата окончания работ.
        /// </summary>
        public virtual DateTime? FactEndDate { get; set; }

        /// <summary>
        /// Дата окончания гарантийных обязательств.
        /// </summary>
        public virtual DateTime? WarrantyEndDate { get; set; }

        /// <summary>
        /// Номер ГЖИ
        /// </summary>
        public virtual string GjiNum { get; set; }

        /// <summary>
        /// Дата принятия ГЖИ.
        /// </summary>
        public virtual DateTime? DateAcceptGji { get; set; }

        /// <summary>
        /// Дата регистрации ГЖИ.
        /// </summary>
        public virtual DateTime? DateGjiReg { get; set; }

        /// <summary>
        /// Дата остановки работ ГЖИ.
        /// </summary>
        public virtual DateTime? DateStopWorkGji { get; set; }
    }
}
