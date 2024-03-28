namespace Bars.Gkh.Regions.Msk.Entities
{
    using Bars.B4.DataAccess;

    /// <summary>
    /// Информация о доме для сервиса
    /// </summary>
    public class RealityObjectInfo : BaseEntity
    {
        /// <summary>
        /// Uid
        /// </summary>
        public virtual string Uid { get; set; }

        /// <summary>
        /// Округ
        /// </summary>
        public virtual string Okrug { get; set; }

        /// <summary>
        /// Район
        /// </summary>
        public virtual string Raion { get; set; }

        /// <summary>
        /// Адрес
        /// </summary>
        public virtual string Address { get; set; }

        /// <summary>
        /// код Unom
        /// </summary>
        public virtual string UnomCode { get; set; }

        /// <summary>
        /// MziCode
        /// </summary>
        public virtual string MziCode { get; set; }

        /// <summary>
        /// Серия
        /// </summary>
        public virtual string Serial { get; set; }

        /// <summary>
        /// Год постройки 
        /// </summary>
        public virtual int BuildingYear { get; set; }

        /// <summary>
        /// Общая площадь квартиры 
        /// </summary>
        public virtual decimal TotalArea { get; set; }

        /// <summary>
        /// Жилая площадь квартиры 
        /// </summary>
        public virtual decimal LivingArea { get; set; }

        /// <summary>
        /// Нежилая площадь квартиры 
        /// </summary>
        public virtual decimal NoLivingArea { get; set; }

        /// <summary>
        /// Количество этажей
        /// </summary>
        public virtual int FloorCount { get; set; }

        /// <summary>
        /// Количество подъездов
        /// </summary>
        public virtual int PorchCount { get; set; }

        /// <summary>
        /// Количество квартир
        /// </summary>
        public virtual int FlatCount { get; set; }

        /// <summary>
        /// Просрочка 
        /// </summary>
        public virtual decimal AllDelay { get; set; }

        /// <summary>
        /// Баллы 
        /// </summary>
        public virtual decimal Points { get; set; }

        /// <summary>
        /// Номер в очереди 
        /// </summary>
        public virtual int IndexNumber { get; set; }


        #region periods
        public virtual string EsPeriod { get; set; }
        public virtual string GasPeriod { get; set; }
        public virtual string HvsPeriod { get; set; }
        public virtual string HvsmPeriod { get; set; }
        public virtual string GvsPeriod { get; set; }
        public virtual string GvsmPeriod { get; set; }
        public virtual string KanPeriod { get; set; }
        public virtual string KanmPeriod { get; set; }
        public virtual string OtopPeriod { get; set; }
        public virtual string OtopmPeriod { get; set; }
        public virtual string MusPeriod { get; set; }
        public virtual string PpiaduPeriod { get; set; }
        public virtual string PvPeriod { get; set; }
        public virtual string FasPeriod { get; set; }
        public virtual string KrovPeriod { get; set; }
        public virtual string VdskPeriod { get; set; }
        public virtual string LiftPeriod { get; set; }
        #endregion periods

    }
}
