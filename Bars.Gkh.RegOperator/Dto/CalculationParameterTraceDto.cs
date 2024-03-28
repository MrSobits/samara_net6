namespace Bars.Gkh.RegOperator.Dto
{
    using System;
    using System.Collections.Generic;
    
    using Bars.Gkh.DataAccess;
    using Bars.Gkh.Entities;
    using Bars.Gkh.RegOperator.Entities.PersonalAccount;
    using Bars.Gkh.RegOperator.Enums;

    using Newtonsoft.Json;

    /// <summary>
    /// Трассировка параметров расчетов ЛС
    /// </summary>
    public class CalculationParameterTraceDto
    {

        /// <summary>
        /// Конструктор принимающий протокол расчета
        /// </summary>
        /// <param name="source"></param>
        public CalculationParameterTraceDto(CalculationParameterTrace source)
        {
            this.ObjectCreateDate = source.ObjectCreateDate;
            this.ObjectEditDate = source.ObjectEditDate;
            this.ObjectVersion = source.ObjectVersion;
            this.CalculationType = source.CalculationType;
            this.CalculationTypeId = (long)source.CalculationType;
            this.ParameterValuesString = new JsonParameterValuesSerializer().Init().SerializeObject(source.ParameterValues);
            this.ParameterValues = source.ParameterValues;
            this.DateStart = source.DateStart;
            this.DateEnd = source.DateEnd;
            this.CalculationGuid = source.CalculationGuid;
            this.ChargePeriod = source.ChargePeriod;
            this.ChargePeriodId = source.ChargePeriod.Id;
        }

        /// <summary>
        /// Пустой конструктор для dapper
        /// </summary>
        public CalculationParameterTraceDto()
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
        /// Тип расчета
        /// </summary>
        public CalculationTraceType CalculationType { get; set; }
        /// <summary>
        /// Тип расчета
        /// </summary>
        public long CalculationTypeId { get; set; }

        /// <summary>
        /// Словарь значений параметров на шаг расчета
        /// </summary>
        public Dictionary<string, object> ParameterValues { get; set; }

        /// <summary>
        /// Словарь значений параметров на шаг расчета
        /// </summary>
        public string ParameterValuesString { get; set; }

        /// <summary>
        /// Дата начала действия параметров
        /// </summary>
        public DateTime DateStart { get; set; }

        /// <summary>
        /// Дата окончания действия параметров
        /// </summary>
        public DateTime? DateEnd { get; set; }

        /// <summary>
        /// Гуид связи с неподтвержденным начислением и боевым 
        /// </summary>
        public string CalculationGuid { get; set; }

        /// <summary>
        /// Период расчета
        /// </summary>
        public ChargePeriod ChargePeriod { get; set; }

        /// <summary>
        /// Идентификатор периода расчета
        /// </summary>
        public long ChargePeriodId { get; set; }

        private class JsonParameterValuesSerializer : ImprovedJsonSerializedType<Dictionary<string, object>>
        {
            public JsonParameterValuesSerializer Init()
            {
                var settings = new JsonSerializerSettings
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                    Formatting = Formatting.None,
                    PreserveReferencesHandling = PreserveReferencesHandling.Objects,
                    TypeNameHandling = TypeNameHandling.Auto
                };
                settings.Converters.Clear();

                this.SetSerializationSettings(settings);

                return this;
            }

            public string SerializeObject(object value)
            {
                return base.Serialize(value);
            }
        }
    }
}