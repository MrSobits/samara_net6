namespace Bars.Gkh.Overhaul.Nso.Entities
{
    using Bars.B4.DataAccess;

    /// <summary>
    /// Запись Опубликованной программы
    /// </summary>
    public class PublishedProgramRecord : BaseEntity
    {
        /// <summary>
        /// Ссылка на версию программы
        /// </summary>
        public virtual PublishedProgram PublishedProgram { get; set; }

        /// <summary>
        /// Ссылка на версию
        /// </summary>
        public virtual VersionRecordStage2 Stage2 { get; set; }

        // Далее идут поля, которые подписываются ЭЦП 

        /// <summary>
        /// Порядковый номер
        /// </summary>
        public virtual decimal Sum { get; set; }

        /// <summary>
        /// Стоимость
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
        public virtual decimal Wear { get; set; }

        /// <summary>
        /// Дата последнего капитального ремонта
        /// </summary>
        public virtual int LastOverhaulYear { get; set; }

        /// <summary>
        /// Плановый год проведения капитального ремонта
        /// </summary>
        public virtual int PublishedYear { get; set; } 
    }
}