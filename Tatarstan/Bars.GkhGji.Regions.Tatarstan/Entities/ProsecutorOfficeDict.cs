namespace Bars.GkhGji.Regions.Tatarstan.Entities
{
    using Bars.B4.DataAccess;
    using Bars.GkhGji.Regions.Tatarstan.Enums;

    /// <summary>
    /// Справочник прокуратур
    /// </summary>
    public class ProsecutorOfficeDict : BaseEntity
    {
        /// <summary>
        /// Тип прокуратуры
        /// </summary>
        public virtual ProsecutorOfficeType? Type { get; set; }

        /// <summary>
        /// Код прокуратуры
        /// </summary>
        public virtual string Code { get; set; }

        /// <summary>
        /// Код ЕРКНМ
        /// </summary>
        public virtual string ErknmCode { get; set; }

        /// <summary>
        /// Наименование прокуратуры
        /// </summary>
        public virtual string Name { get; set; }
        
        /// <summary>
        /// Код федерального округа
        /// </summary>
        public virtual int FederalDistrictCode { get; set; }
        
        /// <summary>
        /// Наименование федерального округа
        /// </summary>
        public virtual string FederalDistrictName { get; set; }
        
        /// <summary>
        /// Код федерального центра
        /// </summary>
        public virtual int FederalCenterCode { get; set; }
        
        /// <summary>
        /// Наименование федерального центра
        /// </summary>
        public virtual string FederalCenterName { get; set; }
        
        /// <summary>
        /// Код района по версии ЕРП
        /// </summary>
        public virtual int? DistrictCode { get; set; }
        
        /// <summary>
        /// Идентификатор родительской записи
        /// </summary>
        public virtual ProsecutorOfficeDict Parent { get; set; }

        /// <summary>
        /// Код региона (ОКАТО)
        /// </summary>
        public virtual string OkatoTer { get; set; }

        /// <summary>
        /// Код района (ОКАТО)
        /// </summary>
        public virtual string OkatoKod1 { get; set; }

        /// <summary>
        /// Код рабочего поселка/сельсовета (ОКАТО)	
        /// </summary>
        public virtual string OkatoKod2 { get; set; }

        /// <summary>
        /// Код населенного пункта (ОКАТО)
        /// </summary>
        public virtual string OkatoKod3 { get; set; }

        /// <summary>
        /// Использовать по умолчанию
        /// </summary>
        public virtual bool UseDefault { get; set; }
    }
}