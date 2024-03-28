using System;

namespace Bars.B4.Modules.FIAS
{
    using Bars.B4.Modules.FIAS.Enums;
    using DataAccess;

    /// <summary>
    /// Запись сформированного Адреса на основе составляющих элементов ФИАС
    /// </summary>
    public class FiasAddress : BaseEntity
    {

        /// <summary>
        /// Все гуиды адреса разделенные по уровням
        /// </summary>
        public virtual string AddressGuid { get; set; }

        /// <summary>
        /// Полный адрес Населенный пункт+Улица+Дом+Корпус.... 
        /// </summary>
        public virtual string AddressName { get; set; }

        /// <summary>
        /// Почтовый индекс
        /// </summary>
        public virtual string PostCode { get; set; }

        /// <summary>
        /// Код населенного пункта
        /// </summary>
        public virtual string PlaceCode { get; set; }

        /// <summary>
        /// Гуид записи ФИАСа населенного пункта
        /// </summary>
        public virtual string PlaceGuidId { get; set; }

        /// <summary>
        /// Наименование населенного пункта
        /// </summary>
        public virtual string PlaceName { get; set; }

        /// <summary>
        /// Адрес до Населенного пункта ( тоесть Улица, Дом корпус не включены)
        /// </summary>
        public virtual string PlaceAddressName { get; set; }

        /// <summary>
        /// Код улицы
        /// </summary>
        public virtual string StreetCode { get; set; }

        /// <summary>
        /// Гуид записи ФИАС улицы
        /// </summary>
        public virtual string StreetGuidId { get; set; }

        /// <summary>
        /// Наименование улицы
        /// </summary>
        public virtual string StreetName { get; set; }

        /// <summary>
        /// Дом
        /// </summary>
        public virtual string House { get; set; }

        /// <summary>
        /// Литер
        /// </summary>
        public virtual string Letter { get; set; }
        
        /// <summary>
        /// Корпус
        /// </summary>
        public virtual string Housing { get; set; }

        /// <summary>
        /// Секция
        /// </summary>
        public virtual string Building { get; set; }

        /// <summary>
        /// Квартира
        /// </summary>
        public virtual string Flat { get; set; }

        /// <summary>
        /// Координаты
        /// </summary>
        public virtual string Coordinate { get; set; }

		/// <summary>
		/// Глобальный уникальный идентификатор дома 
		/// </summary>
		public virtual Guid? HouseGuid { get; set; }

        /// <summary>
        /// Признак владения
        /// </summary>
        public virtual FiasEstimateStatusEnum EstimateStatus { get; set; }

        /// <summary>
        /// Метод проверяет изменились ли поля
        /// </summary>

        public virtual bool IsModified(FiasAddress newValue)
        {
            if(this.AddressName != newValue.AddressName ||
                this.PostCode != newValue.PostCode ||
                this.PlaceCode != newValue.PlaceCode ||
                this.PlaceGuidId != newValue.PlaceGuidId ||
                this.PlaceName != newValue.PlaceName ||
                this.PlaceAddressName != newValue.PlaceAddressName ||
                this.StreetCode != newValue.StreetCode ||
                this.StreetGuidId != newValue.StreetGuidId ||
                this.StreetName != newValue.StreetName ||
                this.House != newValue.House ||
                this.Housing != newValue.Housing ||
                this.Building != newValue.Building ||
                this.Flat != newValue.Flat ||
                this.Coordinate != newValue.Coordinate ||
                this.HouseGuid != newValue.HouseGuid ||
				this.EstimateStatus != newValue.EstimateStatus)
            {
                return true;
            }
            return false;
        }

    }
}
