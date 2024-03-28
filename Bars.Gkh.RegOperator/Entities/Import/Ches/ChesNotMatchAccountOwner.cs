namespace Bars.Gkh.RegOperator.Entities.Import.Ches
{
    using Bars.B4.DataAccess;
    using Bars.Gkh.Entities;
    using Bars.Gkh.RegOperator.Enums;

    /// <summary>
    /// Несопоставленный в периоде абонент
    /// </summary>
    public abstract class ChesNotMatchAccountOwner : PersistentObject
    {
        /// <summary>
        /// Наименование абонента
        /// </summary>
        public virtual string Name { get; set; }

        /// <summary>
        /// Номер ЛС из файла
        /// </summary>
        public virtual string PersonalAccountNumber { get; set; }

        /// <summary>
        /// Тип абонента
        /// </summary>
        public virtual PersonalAccountOwnerType OwnerType { get; set; }

        /// <summary>
        /// Период
        /// </summary>
        public virtual ChargePeriod Period { get; set; }
    }
}