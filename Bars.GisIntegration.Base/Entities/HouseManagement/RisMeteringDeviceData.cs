namespace Bars.GisIntegration.Base.Entities.HouseManagement
{
    using System;
    using Bars.GisIntegration.Base.Entities;
    using Enums;

    /// <summary>
    /// Данные прибора учета
    /// </summary>
    public class RisMeteringDeviceData : BaseRisEntity
    {
        /// <summary>
        /// Тип ПУ
        /// </summary>
        public virtual MeteringDeviceType MeteringDeviceType { get; set; }

        /// <summary>
        /// Номер ПУ
        /// </summary>
        public virtual string MeteringDeviceNumber { get; set; }

        /// <summary>
        /// Марка ПУ
        /// </summary>
        public virtual string MeteringDeviceStamp { get; set; }

        /// <summary>
        /// Дата установки
        /// </summary>
        public virtual DateTime? InstallationDate { get; set; }

        /// <summary>
        /// Дата ввода в эксплуатацию
        /// </summary>
        public virtual DateTime? CommissioningDate { get; set; }

        /// <summary>
        /// Внесение показаний осуществляется в ручном режиме
        /// </summary>
        public virtual bool? ManualModeMetering { get; set; }

        /// <summary>
        /// Дата первичной поверки
        /// </summary>
        public virtual DateTime? FirstVerificationDate { get; set; }

        /// <summary>
        /// Межповерочный интервал (НСИ 16) - код
        /// </summary>
        public virtual string VerificationInterval { get; set; }

        /// <summary>
        /// Межповерочный интервал (НСИ 16) - Guid
        /// </summary>
        public virtual string VerificationIntervalGuid { get; set; }

        //verification_interval_guid

        /// <summary>
        /// Тип прибора учета
        /// </summary>
        public virtual DeviceType DeviceType { get; set; }

        /// <summary>
        /// Базовое показание T1 Характеристики ПУ 
        /// </summary>
        public virtual decimal BaseValueT1 { get; set; }

        /// <summary>
        /// Базовое показание T2 Характеристики ПУ учета электрической энергии
        /// </summary>
        public virtual decimal? BaseValueT2 { get; set; }

        /// <summary>
        /// Базовое показание T3. В зависимости от количества заданных при создании базовых значений ПУ определяется его тип по количеству тарифов.
        /// Характеристики ПУ учета электрической энергии
        /// </summary>
        public virtual decimal? BaseValueT3 { get; set; }

        /// <summary>
        /// Время и дата снятия показания
        /// </summary>
        public virtual DateTime ReadoutDate { get; set; }

        /// <summary>
        /// Кем внесено
        /// </summary>
        public virtual string ReadingsSource { get; set; }

        /// <summary>
        /// Ссылка на дом
        /// </summary>
        public virtual RisHouse House { get; set; }

        /// <summary>
        /// Ссылка на жилое помещение
        /// </summary>
        public virtual ResidentialPremises ResidentialPremises { get; set; }

        /// <summary>
        /// Ссылка на нежилое помещение
        /// </summary>
        public virtual NonResidentialPremises NonResidentialPremises { get; set; }

        /// <summary>
        /// Коммунальный ресурс_Код записи справочника
        /// </summary>
        public virtual string MunicipalResourceCode { get; set; }

        /// <summary>
        /// Коммунальный ресурс_Идентификатор в ГИС ЖКХ
        /// </summary>
        public virtual string MunicipalResourceGuid { get; set; }

        /// <summary>
        /// Архивация ПУ
        /// </summary>
        public virtual MeteringDeviceArchivation? Archivation { get; set; }

        /// <summary>
        /// Ссылка на НСИ "Причина архивации" (реестровый номер 21)_Код записи справочника
        /// </summary>
        public virtual string ArchivingReasonCode { get; set; }

        /// <summary>
        /// Ссылка на НСИ "Причина архивации" (реестровый номер 21)_Идентификатор в ГИС ЖКХ
        /// </summary>
        public virtual string ArchivingReasonGuid { get; set; }

        /// <summary>
        /// Замена в рамках плановой поверки
        /// </summary>
        public virtual bool? PlannedVerification { get; set; }

        /// <summary>
        /// Показания однотарифного ПУ
        /// </summary>
        public virtual decimal? OneRateDeviceValue { get; set; }

        /// <summary>
        /// Показания по тарифу T1
        /// </summary>
        public virtual decimal? MeteringValueT1 { get; set; }

        /// <summary>
        /// Показания по тарифу T2
        /// </summary>
        public virtual decimal? MeteringValueT2 { get; set; }

        /// <summary>
        /// Показания по тарифу T3
        /// </summary>
        public virtual decimal? MeteringValueT3 { get; set; }

        /// <summary>
        /// Дата поверки
        /// </summary>
        public virtual DateTime? BeginDate { get; set; }

        /// <summary>
        /// Заменить на существующий (идентификатор ПУ в ГИС ЖКХ)
        /// </summary>
        public virtual string ReplacingMeteringDeviceGUID { get; set; }

        /// <summary>
        /// Модель ПУ
        /// </summary>
        public virtual string MeteringDeviceModel { get; set; }

        /// <summary>
        /// Дата опломбиролвания ПУ заводом-изготовителем
        /// </summary>
        public virtual DateTime? FactorySealDate { get; set; }

        /// <summary>
        /// Наличие датчиков температры
        /// </summary>
        public virtual bool? TemperatureSensor { get; set; }

        /// <summary>
        /// Наличие датчиков давления
        /// </summary>
        public virtual bool? PressureSensor { get; set; }

        /// <summary>
        /// Коэффициент трансформации
        /// </summary>
        public virtual decimal? TransformationRatio { get; set; }

        /// <summary>
        /// Архивация ПУ с заменой на другой ПУ- Дата поверки
        /// </summary>
        public virtual DateTime? VerificationDate { get; set; }

        /// <summary>
        /// Дата опломбиролвания ПУ после поверки
        /// </summary>
        public virtual DateTime? SealDate { get; set; }

        /// <summary>
        /// Причина выхода ПУ из строя (НСИ 224) - Код
        /// </summary>
        public virtual string ReasonVerificationCode { get; set; }

        /// <summary>
        /// Причина выхода ПУ из строя (НСИ 224) - Guid
        /// </summary>
        public virtual string ReasonVerificationGuid { get; set; }

        /// <summary>
        /// Информация о наличии возможности дистанционного снятия показаний ПУ
        /// </summary>
        public virtual string ManualModeInformation { get; set; }

        /// <summary>
        /// Информация о наличии датчиков температуры
        /// </summary>
        public virtual string TemperatureSensorInformation { get; set; }

        /// <summary>
        /// Информация о наличии датчиков температуры
        /// </summary>
        public virtual string PressureSensorInformation { get; set; }

        /// <summary>
        /// Архивация ПУ с заменой на другой ПУ - идентификатор версии ПУ в ГИС ЖКХ
        /// </summary>
        public virtual string ReplacingMeteringDeviceVersionGuid { get; set; }
    }
}
