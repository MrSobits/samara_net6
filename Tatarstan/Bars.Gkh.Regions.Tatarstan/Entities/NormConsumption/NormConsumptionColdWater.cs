namespace Bars.Gkh.Regions.Tatarstan.Entities.NormConsumption
{
    using Bars.Gkh.Enums;

    /// <summary>
    /// Сведения для норматива потребления ХВС
    /// </summary>
    public class NormConsumptionColdWater : NormConsumptionRecord
    {
        /// <summary>
        /// Наличие отдельного ипу у нежилых помещений (да/нет)
        /// </summary>
        public virtual YesNo IsIpuNotLivingPermises { get; set; }

        /// <summary>
        /// Площадь нежилых помещений при наличии отдельного ипу у нежилых помещений, кв.м 
        /// </summary>
        public virtual decimal? AreaIpuNotLivingPermises { get; set; }

        /// <summary>
        /// Объем ГВС по нежилым помещениям при наличии ипу, куб.м.
        /// </summary>
        public virtual decimal? VolumeColdWaterNotLivingIsIpu { get; set; }

        /// <summary>
        /// Для домов с ОПУ Объем воды по общедомовому прибору за отопительный период 2014-2015 
        /// (без показаний потребления тепловой энергии нежилыми помещениями - магазинами и т.д.) куб.м
        /// </summary>
        public virtual decimal? VolumeWaterOpuOnPeriod { get; set; }

        /// <summary>
        /// Отопительный период, дни
        /// </summary>
        public virtual int? HeatingPeriod { get; set; }

        /// <summary>
        /// МКД и жилые дома с централ. ХВС и ГВС, водоотведением, оборудованные унитазами,
        /// раковинами, мойками, ваннами сидячими длиной 1200 мм с душем
        /// </summary>
        public virtual YesNo IsBath1200 { get; set; }

        /// <summary>
        /// МКД и жилые дома с централ. ХВС и ГВС, водоотведением, оборудованные унитазами,
        /// раковинами, мойками, ваннами длиной 1500 - 1550 мм с душем
        /// </summary>
        public virtual YesNo IsBath1500With1550 { get; set; }

        /// <summary>
        /// МКД и жилые дома с централ. ХВС и ГВС, водоотведением, оборудованные унитазами,
        /// раковинами, мойками, ваннами длиной 1650 - 1700 мм с душем
        /// </summary>
        public virtual YesNo IsBath1650With1700 { get; set; }

        /// <summary>
        /// МКД и жилые дома с централ. ХВС и ГВС, водоотведением, оборудованные унитазами,
        /// раковинами, мойками, ваннами без душа
        /// </summary>
        public virtual YesNo IsBathNotShower { get; set; }

        /// <summary>
        /// МКД и жилые дома с централ. ХВС и ГВС, водоотведением, оборудованные унитазами, раковинами, мойками, душем
        /// </summary>
        public virtual YesNo IsShower { get; set; }

        /// <summary>
        /// МКД и жилые дома с централ. ХВС, водонагревателями, водоотведением, оборудованные унитазами,
        /// раковинами, мойками, душами и ваннами сидячими длиной 1200 мм с душем
        /// </summary>
        public virtual YesNo HvsIsBath1200 { get; set; }

        /// <summary>
        /// МКД и жилые дома с централ. ХВС, водонагревателями, водоотведением, оборудованные унитазами, 
        /// раковинами, мойками, душами и ваннами длиной 1500 - 1550 мм с душем
        /// </summary>
        public virtual YesNo HvsIsBath1500With1550 { get; set; }

        /// <summary>
        /// МКД и жилые дома с централ. ХВС, водонагревателями, водоотведением, 
        /// оборудованные унитазами, раковинами, мойками, душами и ваннами без душа
        /// </summary>
        public virtual YesNo HvsIsBathNotShower { get; set; }

        /// <summary>
        /// МКД и жилые дома с централ. ХВС, водонагревателями, водоотведением, 
        /// оборудованные унитазами, раковинами, мойками, душами
        /// </summary>
        public virtual YesNo HvsIsShower { get; set; }

        /// <summary>
        /// МКД и жилые дома без водонагревателей с водопроводом и канализацией, 
        /// оборудованные раковинами, мойками и унитазами
        /// </summary>
        public virtual YesNo IsNotBoiler { get; set; }

        /// <summary>
        /// МКД и жилые дома без водонагревателей с централ. ХВС 
        /// и водоотведением, оборудованные раковинами и мойками
        /// </summary>
        public virtual YesNo HvsIsNotBoiler { get; set; }

        /// <summary>
        /// МКД и жилые дома с централ. ХВС, без централизованного водоотведения, 
        /// оборудованные умывальниками, мойками, унитазами, ваннами, душами 
        /// </summary>
        public virtual YesNo IsHvsBathIsNotCentralSewage { get; set; }

        /// <summary>
        /// МКД и жилые дома с централ. ХВС, без централизованного водоотведения, 
        /// оборудованные умывальниками, мойками, унитазами
        /// </summary>
        public virtual YesNo IsHvsIsNotCentralSewage { get; set; }

        /// <summary>
        /// МКД и жилые дома с водоразборной колонкой
        /// </summary>
        public virtual YesNo IsStandpipes { get; set; }

        /// <summary>
        /// Дома, использующиеся в качестве общежитий без душевых
        /// </summary>
        public virtual YesNo IsHostelNoShower { get; set; }

        /// <summary>
        /// Дома, использующиеся в качестве общежитий с общими душевыми
        /// </summary>
        public virtual YesNo IsHostelSharedShower { get; set; }

        /// <summary>
        /// Дома, использующиеся в качестве общежитий с душами при всех жилых комнатах
        /// </summary>
        public virtual YesNo IsHostelShowerAllLivPermises { get; set; }

        /// <summary>
        /// Дома, использующиеся в качестве общежитий с общими кухнями и 
        /// блоками душевых на этажах при жилых комнатах в каждой секции здания
        /// </summary>
        public virtual YesNo ShowerInHostelInSection { get; set; }
    }
}