namespace Bars.Gkh.Gis.Entities.Register.HouseServiceRegister
{
    using System;
    using B4.DataAccess;

    using Bars.Gkh.Entities.Dicts;

    using Enum;
    using HouseRegister;
    using LoadedFileRegister;

    /// <summary>
    /// Услуги дома
    /// </summary>
    public class HouseServiceRegister : BaseEntity
    {
        /// <summary>
        /// Дом
        /// </summary>
        public virtual HouseRegister House { get; set; }
        public virtual long HouseId { get; set; }

        /// <summary>
        /// Услуга
        /// </summary>
        public virtual ServiceDictionary Service { get; set; }
        public virtual int ServiceId { get; set; }
        public virtual int SupplierId { get; set; }

        /// <summary>
        /// Адрес
        /// </summary>
        public virtual string HouseAddress { get; set; }

        /// <summary>
        /// Период
        /// </summary>
        public virtual DateTime CalculationDate { get; set; }

        /// <summary>
        /// Начислено
        /// </summary>
        public virtual decimal? Charge { get; set; }

        /// <summary>
        /// Оплачено
        /// </summary>
        public virtual decimal? Payment { get; set; }

        /// <summary>
        /// Управляющая организация
        /// </summary>
        public virtual string ManOrgs { get; set; }

        /// <summary>
        /// Объем ИПУ
        /// </summary>
        public virtual decimal? VolumeIndividualCounter { get; set; }

        /// <summary>
        /// Объем по нармативу
        /// </summary>
        public virtual decimal? VolumeNormative { get; set; }

        /// <summary>
        /// Коэффициент ОДН
        /// </summary>
        public virtual decimal? CoefOdn { get; set; }

        /// <summary>
        /// Распределенный объем
        /// </summary>
        public virtual decimal? VolumeDistributed { get; set; }

        /// <summary>
        /// Нераспределенный объем
        /// </summary>
        public virtual decimal? VolumeNotDistributed { get; set; }

        /// <summary>
        /// Объем ОДН приборов учета
        /// </summary>
        public virtual decimal? VolumeOdnIndividualCounter { get; set; }

        /// <summary>
        /// Объем ОДН норматив
        /// </summary>
        public virtual decimal? VolumeOdnNormative { get; set; }

        /// <summary>
        /// Тариф
        /// </summary>
        public virtual decimal? Tariff { get; set; }

        /// <summary>
        /// Период тарифа
        /// </summary>
        public virtual DateTime? TariffDate { get; set; }

        /// <summary>
        /// РСО
        /// </summary>
        public virtual string Rso { get; set; }

        /// <summary>
        /// РСО ИНН
        /// </summary>
        public virtual long? RsoInn { get; set; }

        /// <summary>
        /// Общий объем
        /// </summary>
        public virtual decimal? TotalVolume { get; set; }


        /// <summary>
        /// Признак того, что запись опубликована
        /// </summary>
        public virtual bool IsPublished { get; set; }

        /// <summary>
        /// Внутренний идентифкатор записи
        /// </summary>
        public virtual long InternalId { get; set; }

        /// <summary>
        /// Загруженный файл
        /// </summary>
        public virtual LoadedFileRegister LoadedFile { get; set; }

        /// <summary>
        /// Тип помещения
        /// </summary>
        public virtual TypeArea AreaType { get; set; }

        /// <summary>
        /// Тип договора
        /// </summary>
        public virtual TypeContract ContractType { get; set; }

        /// <summary>
        /// Стоимость по услуге
        /// </summary>
        public virtual decimal ServiceSum { get; set; }

        /// <summary>
        /// В т.ч. НДС
        /// </summary>
        public virtual decimal ServiceNds { get; set; }

        /// <summary>
        /// В т.ч. перерсчет
        /// </summary>
        public virtual decimal ServiceRecalculation { get; set; }
    }
}
  