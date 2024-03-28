namespace Bars.Gkh.RegOperator.Entities.Import.Ches
{
    using Bars.B4.DataAccess;
    using Bars.Gkh.RegOperator.Enums;

    /// <summary>
    /// Сопоставленный абонент ЧЭС и ЖКХ.Комплекс
    /// </summary>
    public abstract class ChesMatchAccountOwner : PersistentObject
    {
        /// <summary>
        /// Сопоставленный владелец
        /// </summary>
        public virtual PersonalAccountOwner AccountOwner { get; set; }

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
    }
}