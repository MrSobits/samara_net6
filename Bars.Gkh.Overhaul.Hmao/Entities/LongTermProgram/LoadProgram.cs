namespace Bars.Gkh.Overhaul.Hmao.Entities
{
    using Bars.Gkh.Entities;

    /// <summary>
    /// Сущность записи загруженной программы
    /// </summary>
    public class LoadProgram : BaseImportableEntity
    {
        /// <summary>
        /// Порядковый номер
        /// </summary>
        public virtual int IndexNumber { get; set; }

        /// <summary>
        /// Населенный пункт
        /// </summary>
        public virtual string Locality { get; set; }

        /// <summary>
        /// Улица
        /// </summary>
        public virtual string Street { get; set; }

        /// <summary>
        /// Дом
        /// </summary>
        public virtual string House { get; set; }

        /// <summary>
        /// Корпус
        /// </summary>
        public virtual string Housing { get; set; }

        /// <summary>
        /// Адрес
        /// </summary>
        public virtual string Address { get; set; }

        /// <summary>
        /// Год ввода в эксплуатацию
        /// </summary>
        public virtual int CommissioningYear { get; set; }

        /// <summary>
        /// Объект общего имущества
        /// </summary>
        public virtual string CommonEstateobject { get; set; }

        /// <summary>
        /// Износ
        /// </summary>
        public virtual string Wear { get; set; }

        /// <summary>
        /// Дата последнего капитального ремонта
        /// </summary>
        public virtual int LastOverhaulYear { get; set; }

        /// <summary>
        /// Плановый год проведения капитального ремонта
        /// </summary>
        public virtual int PlanOverhaulYear { get; set; } 
    }
}
