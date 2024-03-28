namespace Bars.Gkh.Regions.Nso
{
    using System;

    using Bars.Gkh.Entities;

    /// <summary>
    /// Органы местного самоуправления - Данная сущнсоть расширяет базовый класс дополнителньыми полями
    /// </summary>
    public class NsoLocalGovernment : LocalGovernment
    {

        /// <summary>
        /// ФИО
        /// </summary>
        public virtual string Fio { get; set; }

        /// <summary>
        /// Регистрационный номер уведомления
        /// </summary>
        public virtual string RegNumNotice { get; set; }

        /// <summary>
        /// дата регистрации уведомления
        /// </summary>
        public virtual DateTime? RegDateNotice { get; set; }

        /// <summary>
        /// № НПА органа МО
        /// </summary>
        public virtual string NumNpa { get; set; }

        /// <summary>
        /// Дата НПА органа МО
        /// </summary>
        public virtual DateTime? DateNpa { get; set; }

        /// <summary>
        /// Наименование НПА, утвержденного регламентом
        /// </summary>
        public virtual string NameNpa { get; set; }

        /// <summary>
        /// Регламенты взаимодействия - номер МО
        /// </summary>
        public virtual string InteractionMuNum { get; set; }

        /// <summary>
        /// Регламенты взаимодействия - дата МО
        /// </summary>
        public virtual DateTime? InteractionMuDate { get; set; }

        /// <summary>
        /// Регламенты взаимодействия - номер ГЖИ
        /// </summary>
        public virtual string InteractionGjiNum { get; set; }

        /// <summary>
        /// Регламенты взаимодействия - дата ГЖИ
        /// </summary>
        public virtual DateTime? InteractionGjiDate { get; set; }
        
    }
}
