namespace Bars.Gkh.InspectorMobile.Api.Version1.Models.Decision
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Bars.B4.Utils;
    using Bars.Gkh.Domain;
    using Bars.Gkh.InspectorMobile.Api.Version1.Models.Common;
    using Bars.GkhGji.Enums;

    using Newtonsoft.Json;

    /// <summary>
    /// Модель данных для <see cref="Decision"/>
    /// </summary>
    public class DocumentDecision : BaseDocument
    {
        private TypeBase _typeBase;
        
        /// <summary>
        /// Уникальный идентификатор проверки, к которой относится документ
        /// </summary>
        [JsonIgnore]
        public long InspectionId { get; set; }

        /// <summary>
        /// Основание обследования
        /// </summary>
        public string BasisSurvey {
            get
            {
                return this._typeBase.GetDisplayName();
            }
            set
            {
                this._typeBase = (TypeBase)Enum.Parse(typeof(TypeBase), value);
            } 
        }

        /// <summary>
        /// Строковый массив родительских документов - Предписаний
        /// </summary>
        [JsonIgnore]
        public string ParentIdsStr { get; set; }

        /// <summary>
        /// Коллекция Идентификаторов родительских документов - Предписаний
        /// </summary>
        public IEnumerable<long> ParentIds =>
            ParentIdsStr?.Split(',')
            .Select(x => long.TryParse(x, out var pId) ? pId : 0)
            .Where(x => x != 0);

        /// <summary>
        /// Документ основание
        /// </summary>
        public string DocumentBase { get; set; }
        
        /// <summary>
        /// Период обследования с
        /// </summary>
        public DateTime StartDate { get; set; }
        
        /// <summary>
        /// Период обследования по
        /// </summary>
        public DateTime EndDate { get; set; }
        
        /// <summary>
        /// Вид проверки
        /// </summary>
        public int TypeCheckId { get; set; }
        
        /// <summary>
        /// Вид контроля
        /// </summary>
        public long TypeControlId { get; set; }
        
        /// <summary>
        /// Юридическое лицо
        /// </summary>
        public long? OrganizationId { get; set; }
        
        /// <summary>
        /// ФИО
        /// </summary>
        public string Individual { get; set; }
        
        /// <summary>
        /// ИНН
        /// </summary>
        public string Inn { get; set; }

        /// <summary>
        /// Список домов в карточке проверки (ФИАС GUID)(строка)
        /// </summary>
        [JsonIgnore]
        public string AddressesString
        {
            set
            {
                if (this.Addresses == null && !string.IsNullOrEmpty(value))
                {
                    this.Addresses = value.ToLongArray();
                }
            }
        }

        /// <summary>
        /// Список домов в карточке проверки (ФИАС GUID)
        /// </summary>
        public long[] Addresses { get; set; }

        /// <summary>
        /// ДЛ, вынесшее решение
        /// </summary>
        public string Executive { get; set; }

        /// <summary>
        /// Список инспекторов(строка)
        /// </summary>
        [JsonIgnore]
        public string InspectorIdsString
        {
            set
            {
                if (this.InspectorIds == null && !string.IsNullOrEmpty(value))
                {
                    this.InspectorIds = value.ToLongArray();
                }
            }
        }

        /// <summary>
        /// Список инспекторов
        /// </summary>
        public long[] InspectorIds { get; set; }

        /// <summary>
        /// У решения присутствуют дочерние документы не в конечном статусе
        /// </summary>
        public bool RelatedDocuments { get; set; }

        /// <summary>
        /// Проверка
        /// </summary>
        public InspectionInfo Inspection { get; set; }
    }
}