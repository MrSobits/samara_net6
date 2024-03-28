namespace Bars.Gkh.Entities
{
    using System;
    using Bars.Gkh.Entities.Dicts;
    using Bars.Gkh.Enums;

    /// <summary>
    /// Лифт дома
    /// </summary>
    public class RealityObjectLift : BaseImportableEntity
    {
        /// <summary>
        /// Дом
        /// </summary>
        public virtual RealityObject RealityObject { get; set; }

        /// <summary>
        /// Номер подъезда
        /// </summary>
        public virtual string PorchNum { get; set; }

        /// <summary>
        /// Не хранимое поле для отображения в карточках доп информации
        /// </summary>
        public virtual string Info { get; set; }

        /// <summary>
        /// Номер лифта
        /// </summary>
        public virtual string LiftNum { get; set; }

        /// <summary>
        /// Заводской номер
        /// </summary>
        public virtual string FactoryNum { get; set; }

        /// <summary>
        /// Регистрационный номер
        /// </summary>
        public virtual string RegNum { get; set; }

        /// <summary>
        /// Период замены
        /// </summary>
        public virtual string ReplacementPeriod { get; set; }

        /// <summary>
        /// Год установки
        /// </summary>
        public virtual int? YearInstallation { get; set; }

        /// <summary>
        /// Год последней модернизации/восстановительного ремонта
        /// </summary>
        public virtual int? YearLastUpgradeRepair { get; set; }

        /// <summary>
        /// Год проведения экспертной диагностики
        /// </summary>
        public virtual int? YearEstimate { get; set; }

        /// <summary>
        /// Плановый год замены
        /// </summary>
        public virtual int? YearPlannedReplacement { get; set; }

        /// <summary>
        /// Количество остановок
        /// </summary>
        public virtual int StopCount { get; set; }

        /// <summary>
        /// Грузоподъемность
        /// </summary>
        public virtual decimal Capacity { get; set; }

        /// <summary>
        /// Стоимость работ по замене/ ремонту
        /// </summary>
        public virtual decimal Cost { get; set; }

        /// <summary>
        /// Стоимость оценки
        /// </summary>
        public virtual decimal CostEstimate { get; set; }

        /// <summary>
        /// Скорость подьема
        /// </summary>
        public virtual decimal SpeedRise { get; set; }

        /// <summary>
        /// Предельный срок эксплуатации
        /// </summary>
        public virtual DateTime? LifeTime { get; set; }

        /// <summary>
        /// Тип шахты
        /// </summary>
        public virtual TypeLiftShaft TypeLiftShaft { get; set; }

        /// <summary>
        /// Расположение машинного помещения
        /// </summary>
        public virtual TypeLiftMashineRoom TypeLiftMashineRoom { get; set; }

        /// <summary>
        /// Привод передней кабины
        /// </summary>
        public virtual TypeLiftDriveDoors TypeLiftDriveDoors { get; set; }

        /// <summary>
        /// Тип лифта
        /// </summary>
        public virtual TypeLift TypeLift { get; set; }

        /// <summary>
        /// Модель лифта
        /// </summary>
        public virtual ModelLift ModelLift { get; set; }

        /// <summary>
        /// Обслуживающая организация
        /// </summary>
        public virtual Contragent Contragent { get; set; }

        /// <summary>
        /// Наличие устройства
        /// </summary>
        public virtual LiftAvailabilityDevices AvailabilityDevices { get; set; }

        /// <summary>
        /// Год ввода в эксплуатацию
        /// </summary>
        public virtual int? YearExploitation { get; set; }

        /// <summary>
        /// Этажность
        /// </summary>
        public virtual int? NumberOfStoreys { get; set; }

        /// <summary>
        /// Глубина шахты
        /// </summary>
        public virtual decimal? DepthLiftShaft { get; set; }

        /// <summary>
        /// Ширина шахты
        /// </summary>
        public virtual decimal? WidthLiftShaft { get; set; }

        /// <summary>
        /// Высота шахты
        /// </summary>
        public virtual decimal? HeightLiftShaft { get; set; }

        /// <summary>
        /// Глубина кабины
        /// </summary>
        public virtual decimal? DepthCabin { get; set; }

        /// <summary>
        /// Ширина кабины
        /// </summary>
        public virtual decimal? WidthCabin { get; set; }

        /// <summary>
        /// Высота кабины
        /// </summary>
        public virtual decimal? HeightCabin { get; set; }

        /// <summary>
        /// Ширина проема в свету кабины
        /// </summary>
        public virtual decimal? WidthOpeningCabin { get; set; }

        /// <summary>
        /// Владелец лифтового оборудования
        /// </summary>
        public virtual string OwnerLift { get; set; }

        /// <summary>
        /// Кабина лифта
        /// </summary>
        public virtual CabinLift CabinLift { get; set; }

        /// <summary>
        /// Дата ввода в эксплуатацию
        /// </summary>
        public virtual DateTime? ComissioningDate { get; set; }

        /// <summary>
        /// Дата вывода из эксплуатации
        /// </summary>
        public virtual DateTime? DecommissioningDate { get; set; }

        /// <summary>
        /// Планируемая Дата вывода из эксплуатации
        /// </summary>
        public virtual DateTime? PlanDecommissioningDate { get; set; }
    }
}