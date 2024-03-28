namespace Bars.Gkh.RegOperator.Entities
{
    using Bars.Gkh.Entities;

    /// <summary>
    /// Максимальное значение номера лицевого счета (для генерации короткого номера лс)
    /// </summary>
    public class PersonalAccountNumberHiValue : BaseImportableEntity
    {
        /// <summary>
        /// Значение
        /// </summary>
        public virtual int Value { get; set; }
    }
}