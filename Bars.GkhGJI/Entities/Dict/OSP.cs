namespace Bars.GkhGji.Entities
{
    using System;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Overhaul.Entities;
    using Bars.B4.Modules.FIAS;
    using Bars.Gkh.Enums;
    using Bars.Gkh.Utils;

    /// <summary>
    /// Справочник "Отделы судебных приставов"
    /// </summary>
    public class OSP : BaseGkhEntity
    {
        private string shortName;

        /// <summary>
        /// Муниципальный район
        /// </summary>
        public virtual Municipality Municipality { get; set; }

        /// <summary>
        /// Наименование
        /// </summary>
        public virtual string Name { get; set; }

        /// <summary>
        /// Краткое наименование
        /// </summary>
        public virtual string ShortName
        {
            get
            {
                return string.IsNullOrWhiteSpace(this.shortName) ? this.Name : this.shortName;
            }

            set
            {
                this.shortName = value;
            }
        }


        /// <summary>
        /// Улица
        /// </summary>
        public virtual string Street { get; set; }


        /// <summary>
        /// Город
        /// </summary>
        public virtual string Town { get; set; }

        /// <summary>
        /// Рассчетный счет получателя
        /// </summary>
        public virtual string BankAccount { get; set; }

        /// <summary>
        /// Банк
        /// </summary>
        public virtual CreditOrg CreditOrg { get; set; }

        /// <summary>
        /// КБК
        /// </summary>
        public virtual string KBK { get; set; }

    }
}