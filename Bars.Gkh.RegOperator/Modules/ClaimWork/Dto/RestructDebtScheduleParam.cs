namespace Bars.Gkh.ClaimWork.Dto
{
    using System;
    using System.Linq;
    using System.Reflection;

    using Bars.B4.DataAccess;
    using Bars.Gkh.ClaimWork.Entities;
    using Bars.Gkh.RegOperator.Entities;

    /// <summary>
    /// Параметры для создания графика реструктуризации
    /// </summary>
    public class RestructDebtScheduleParam : PersistentObject
    {
        public long? RestructDebt { get; set; }
        public long? PersonalAccount { get; set; }
        public decimal? TotalDebtSum { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public int? PaymentDay { get; set; }

        public RestructDebt GetRestructDebt()
        {
            return this.GetEntity<RestructDebt>(this.RestructDebt);
        }

        public BasePersonalAccount GetPersonalAccount()
        {
            return this.GetEntity<BasePersonalAccount>(this.PersonalAccount);
        }

        /// <summary>
        /// Проверка на заполненность всех полей
        /// </summary>
        /// <returns></returns>
        public bool IsValid()
        {
            return this.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.SetProperty)
                .All(x => x.GetValue(this) != null);
        }

        private T GetEntity<T>(long? instance)
            where T : PersistentObject, new()
        {
            if (instance.HasValue)
            {
                return new T { Id = instance.Value };
            }

            return null;
        }
    }
}