namespace Bars.GkhCr.Entities
{
    using B4.DataAccess;

    /// <summary>
    /// Вьюха на реест квалификационного отбора
    /// </summary>
    public class ViewQualification : PersistentObject
    {
        /// <summary>
        /// Программа КР
        /// </summary>
        public virtual string ProgrammName { get; set; }

        /// <summary>
        /// Муниципальное образование
        /// </summary>
        public virtual string MunicipalityName { get; set; }

        /// <summary>
        /// Сумма
        /// </summary>
        public virtual decimal? Sum { get; set; }

        /// <summary>
        /// Подрядчик
        /// </summary>
        public virtual string BuilderName { get; set; }

        /// <summary>
        /// Рейтинг
        /// </summary>
        public virtual string Rating { get; set; }
        
        /// <summary>
        /// Рейтинг
        /// </summary>
        public virtual string Address { get; set; }

        /// <summary>
        /// Кол-во участников квал  отбора
        /// </summary>
        public virtual string QualMemberCount { get; set; }
    }
}