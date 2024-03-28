namespace Bars.GkhGji.Regions.Voronezh.Entities
{
    using Bars.B4.DataAccess;
    using Bars.Gkh.Entities;
    using System;
    using Enums;
    using Bars.GkhGji.Regions.BaseChelyabinsk.Enums.SMEV;
    using Bars.B4.Modules.FileStorage;

    public class SMEVEGRIP : BaseEntity
    {
        /// <summary>
        /// Инициатор запроса
        /// </summary>
        public virtual Inspector Inspector { get; set; }

        /// <summary>
        /// дата запроса
        /// </summary>
        public virtual DateTime CalcDate { get; set; }

        /// <summary>
        /// ИНН в запросе
        /// </summary>
        public virtual string INNReq { get; set; }

        // остальное для ответа
        /// <summary>
        /// НаимВидИП 
        /// </summary>
        public virtual string IPType { get; set; }

        /// <summary>
        /// ОГРНИП
        /// </summary>
        public virtual string OGRN { get; set; }

        /// <summary>
        /// НаимСтран гражданство
        /// </summary>
        public virtual string Citizenship { get; set; }

        /// <summary>
        /// ТипРегион 
        /// </summary>
        public virtual string RegionType { get; set; }

        /// <summary>
        /// НаимРегион 
        /// </summary>
        public virtual string RegionName { get; set; }

        /// <summary>
        /// ТипНаселП 
        /// </summary>
        public virtual string CityType { get; set; }

        /// <summary>
        /// НаимНаселП 
        /// </summary>
        public virtual string CityName { get; set; }

        /// <summary>
        /// ДатаОГРНИП
        /// </summary>
        public virtual DateTime? OGRNDate { get; set; }

        /// <summary>
        /// ДатаВып Дата формирования выписки
        /// </summary>
        public virtual DateTime?  ResponceDate { get; set; }

      
        /// <summary>
        /// НаимВидЗап Способ образования 
        /// </summary>
        public virtual string CreateWayName { get; set; }

        /// <summary>
        /// НаимНО Наименование регистрирующего органа
        /// </summary>
        public virtual string RegOrgName { get; set; }

        /// <summary>
        /// АдрРО Адрес регистрирующего органа
        /// </summary>
        public virtual string AddressRegOrg { get; set; }

        /// <summary>
        /// КодНО Код регистрирующего органа
        /// </summary>
        public virtual string CodeRegOrg { get; set; }

        /// <summary>
        /// СвФл ФИО должностного лица
        /// </summary>
        public virtual string FIO { get; set; }

        /// <summary>
        /// НаимОКВЭД Наименование вмдов деятельности
        /// </summary>
        public virtual string OKVEDNames { get; set; }

        /// <summary>
        /// КодыОКВЭД
        /// </summary>
        public virtual string OKVEDCodes { get; set; }

        /// <summary>
        /// Ответ
        /// </summary>
        public virtual string Answer { get; set; }

        /// <summary>
        /// Статус запроса
        /// </summary>
        public virtual RequestState RequestState { get; set; }

        /// <summary>
        /// MessageId
        /// </summary>
        public virtual string MessageId { get; set; }

        public virtual InnOgrn InnOgrn { get; set; }
        
        /// <summary>
        /// XML выписки
        /// </summary>
        public virtual FileInfo XmlFile { get; set; }
    }
}
