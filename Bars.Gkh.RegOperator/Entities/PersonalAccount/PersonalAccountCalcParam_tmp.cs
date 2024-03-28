namespace Bars.Gkh.RegOperator.Entities.PersonalAccount
{
    using B4.DataAccess;
    using Gkh.Entities;

    /// <summary>
    /// Временный объект для хранения использованных параметров.
    /// Создается при каждом вычислении тарифа и т.д.
    /// При закрытии периода используется как источник параметров для фиксирования, затем удаляется
    /// </summary>
    public class PersonalAccountCalcParam_tmp : BaseEntity
    {
        public virtual BasePersonalAccount PersonalAccount { get; set; }

        public virtual EntityLogLight LoggedEntity { get; set; }
    }
}