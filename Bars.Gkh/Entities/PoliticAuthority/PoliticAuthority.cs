namespace Bars.Gkh.Entities
{
    using Bars.Gkh.Enums;

    /// <summary>
    /// Органы государственной власти
    /// </summary>
    public class PoliticAuthority : BaseGkhEntity
    {
        /// <summary>
        /// Контрагент
        /// </summary>
        public virtual Contragent Contragent { get; set; }

        /// <summary>
        /// Описание
        /// </summary>
        public virtual string Description { get; set; }

        /// <summary>
        /// Статус
        /// </summary>
        public virtual OrgStateRole OrgStateRole { get; set; }

        /// <summary>
        /// Наименование Контрагента (не хранимое)
        /// </summary>
        public virtual string ContragentName { get; set; }

        /// <summary>
        /// Наименование МО (не хранимое)
        /// </summary>
        public virtual string MunicipalityName { get; set; }

        /// <summary>
        /// Email
        /// </summary>
        public virtual string Email { get; set; }

        /// <summary>
        /// Наименование подразделения ответсвенного за ЖКХ
        /// </summary>
        public virtual string NameDepartamentGkh { get; set; }

        /// <summary>
        /// Официальный сайт подразделения 
        /// </summary>
        public virtual string OfficialSite { get; set; }

        /// <summary>
        /// Телефон
        /// </summary>
        public virtual string Phone { get; set; }
    }
}
