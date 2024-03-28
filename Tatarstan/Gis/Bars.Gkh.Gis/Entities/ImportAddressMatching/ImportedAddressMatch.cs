using Bars.Gkh.Entities;

namespace Bars.Gkh.Gis.Entities.ImportAddressMatching
{
    using Bars.B4.DataAccess;
    using System;


    /// <summary>
    /// Сопоставление импортированного адреса и адреса из ФИАС
    /// </summary>
    public class ImportedAddressMatch : BaseEntity
    {

        /// <summary>
        /// Сопоставление адресов
        /// </summary>
        public virtual FiasAddressUid FiasAddress { get; set; }

        /// <summary>
        /// Тип импорта
        /// </summary>
        public virtual string ImportType { get; set; }

        /// <summary>
        /// Имя файла импорта
        /// </summary>
        public virtual string ImportFilename { get; set; }
        
        /// <summary>
        /// Код адреса дома в сторонней системе
        /// </summary>
        public virtual string AddressCode { get; set; }
        
        /// <summary>
        /// Адрес дома в сторонней системе - город
        /// </summary>
        public virtual string City { get; set; }
        
        /// <summary>
        /// Адрес дома в сторонней системе - улица
        /// </summary>
        public virtual string Street { get; set; }
        
        /// <summary>
        /// Адрес дома в сторонней системе - дом
        /// </summary>
        public virtual string House { get; set; }
        
        /// <summary>
        /// Дата импорта
        /// </summary>
        public virtual DateTime ImportDate { get; set; }

        /// <summary>
        /// Код дома в сторонней системе
        /// </summary>
        public virtual int HouseCode { get; set; }

        /// <summary>
        /// Код банка в сторонней системе
        /// </summary>
        public virtual int DataBankId { get; set; }

    }
}
