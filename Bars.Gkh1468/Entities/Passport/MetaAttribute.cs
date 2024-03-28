namespace Bars.Gkh1468.Entities
{
    using System.Linq;
    using Bars.B4.Utils;
    using Bars.Gkh.Entities;
    using Bars.Gkh1468.Enums;

    using Newtonsoft.Json;

    /// <summary>
    /// Атрибут структуры паспорта
    /// </summary>
    public class MetaAttribute : BaseGkhEntity
    {
        /// <summary>
        /// Номер для сортировки, берется порядок атрибута, если он больше нуля, иначе последнее целое число из свойства Code
        /// </summary>
        /// <remarks>
        /// Порядок атрибута изменяется при перетаскивании атрибутов мышкой
        /// </remarks>
        [JsonIgnore]
        public virtual int SortNumber
        {
            get
            {
                if (this.OrderNum > 0)
                {
                    return this.OrderNum;
                }

                return string.IsNullOrEmpty(this.Code) ? 0 : this.Code.Replace(',', '.').TrimEnd('.').Split('.').Last().ToInt();
            }
        }

        /// <summary>
        /// Код атрибута
        /// </summary>
        public virtual string Code { get; set; }

        /// <summary>
        /// Порядок атрибута
        /// </summary>
        public virtual int OrderNum { get; set; }

        /// <summary>
        /// Наименование
        /// </summary>
        public virtual string Name { get; set; }

        /// <summary>
        /// Тип атрибута
        /// </summary>
        public virtual MetaAttributeType Type { get; set; }

        /// <summary>
        /// Тип хранимого значения
        /// </summary>
        public virtual ValueType ValueType { get; set; }
        
        /// <summary>
        /// Тип хранимого значения
        /// </summary>
        public virtual string DictCode { get; set; }

        /// <summary>
        /// Родительский раздел
        /// </summary>
        public virtual Part ParentPart { get; set; }

        /// <summary>
        /// Родительский атрибут
        /// </summary>
        public virtual MetaAttribute Parent { get; set; }

        /// <summary>
        /// Проверять соответсвие суммы значений дочерних атрибутов (для групповых со значением)
        /// </summary>
        public virtual bool ValidateChilds { get; set; }

        /// <summary>
        /// Текст используемый при группировке (для групповых)
        /// </summary>
        public virtual string GroupText { get; set; }

        /// <summary>
        /// Единица измерения
        /// </summary>
        public virtual UnitMeasure UnitMeasure { get; set; }

        /// <summary>
        /// Код интеграции
        /// </summary>
        public virtual string IntegrationCode { get; set; }

        /// <summary>
        /// Код заполнителя значения по-умолчанию
        /// </summary>
        public virtual string DataFillerCode { get; set; }

        /// <summary>
        /// Учитывать поле при расчете процента заполнения
        /// </summary>
        public virtual bool UseInPercentCalculation { get; set; }

        /// <summary>
        /// Поля используемые при валидации
        /// </summary>
        public virtual int MaxLength { get; set; }
        public virtual int MinLength { get; set; }
        public virtual string Pattern { get; set; }
        public virtual int Exp { get; set; }
        public virtual bool Required { get; set; }
        public virtual bool AllowNegative { get; set; }

        public override string ToString()
        {
            return string.Format("Name: {0}, ParentName: {1}, GroupText: {2}", Name,
                Parent.Return(x => x.Name, string.Empty), GroupText);
        }

        public virtual MetaAttribute Clone()
        {
            return MemberwiseClone() as MetaAttribute;
        }
    }
}
