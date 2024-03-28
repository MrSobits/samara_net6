namespace Bars.Gkh.RegOperator.Dto
{
    using System;

    using Bars.Gkh.Entities;
    using Bars.Gkh.RegOperator.Entities.PersonalAccount;
    using Bars.Gkh.RegOperator.Enums;

    /// <summary>
    /// История перерасчета
    /// </summary>
    public class RecalcHistoryDto
    {
        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="source"></param>
        public RecalcHistoryDto(RecalcHistory source)
        {
            this.ObjectCreateDate = source.ObjectCreateDate;
            this.ObjectEditDate = source.ObjectEditDate;
            this.ObjectVersion = source.ObjectVersion;
            this.PersonalAccountId = source.PersonalAccount.Id;
            this.CalcPeriod = source.CalcPeriod;
            this.CalcPeriodId = source.CalcPeriod.Id;
            this.RecalcPeriod = source.RecalcPeriod;
            this.RecalcPeriodId = source.RecalcPeriod.Id;
            this.RecalcSum = source.RecalcSum;
            this.UnacceptedChargeGuid = source.UnacceptedChargeGuid;
            this.RecalcType = source.RecalcType;
            this.RecalcTypeId = (long)source.RecalcType;
        }

        /// <summary>
        /// Пустой конструктор для dapper
        /// </summary>
        public RecalcHistoryDto()
        {

        }

        /// <summary>
        /// Дата создания
        /// </summary>
        public DateTime ObjectCreateDate { get; set; }

        /// <summary>
        /// Дата последнего редактирования
        /// </summary>
        public DateTime ObjectEditDate { get; set; }

        /// <summary>
        /// Версия объекта
        /// </summary>
        public int ObjectVersion { get; set; }

        /// <summary>
        /// ЛС
        /// </summary>
        public long PersonalAccountId { get; set; }

        /// <summary>
        /// Период, в котором считается
        /// </summary>
        public ChargePeriod CalcPeriod { get; set; }

        /// <summary>
        /// Период, для которого происходит перерасчет 
        /// </summary>
        public ChargePeriod RecalcPeriod { get; set; }

        /// <summary>
        /// Период, в котором считается
        /// </summary>
        public long CalcPeriodId { get; set; }

        /// <summary>
        /// Период, для которого происходит перерасчет 
        /// </summary>
        public long RecalcPeriodId { get; set; }

        /// <summary>
        /// Сумма перерасчета (дельта)
        /// </summary>
        public decimal RecalcSum { get; set; }

        /// <summary>
        /// Гуид связи с неподтвержденным начислением и боевым 
        /// </summary>
        public string UnacceptedChargeGuid { get; set; }

        /// <summary>
        /// Тип перерасчета
        /// </summary>
        public RecalcType RecalcType { get; set; }

        /// <summary>
        /// Тип перерасчета
        /// </summary>
        public long RecalcTypeId { get; set; }
    }
}