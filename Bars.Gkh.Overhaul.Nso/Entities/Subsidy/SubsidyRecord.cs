namespace Bars.Gkh.Overhaul.Nso.Entities
{
    using Bars.B4.DataAccess;

    /// <summary>
    /// Запись субсидии
    /// В этой записи будут содержатся значения которые не зависят от версии (Но которые либо расчитываются либо забиваются в ручную) 
    /// </summary>
    public class SubsidyRecord : BaseEntity
    {
        /// <summary>
        /// Год
        /// </summary>
        public virtual int SubsidyYear { get; set; }

        /// <summary>
        /// Бюджет региона
        /// </summary>
        public virtual decimal BudgetRegion { get; set; }

        /// <summary>
        /// Бюджет МО
        /// </summary>
        public virtual decimal BudgetMunicipality { get; set; }

        /// <summary>
        /// Бюджет ФСР
        /// </summary>
        public virtual decimal BudgetFcr { get; set; }

        /// <summary>
        /// Бюджет других источников
        /// </summary>
        public virtual decimal BudgetOtherSource { get; set; }

        /// <summary>
        /// Плановая собираемость (То есть сколько средств по плану соберут собственники)
        /// </summary>
        public virtual decimal PlanOwnerCollection { get; set; }

        /// <summary>
        /// (% Забивается в ручную) Процент собираемости (То есть ест ьвероятность того что несоберут нужную сумма)
        /// и поэтому заводят процент который покажет Нормальную сумму средств собственников
        /// </summary>
        public virtual decimal PlanOwnerPercent { get; set; }

        /// <summary>
        /// (% Забивается в ручную) Не снижаемый размер регионального фонда
        /// </summary>
        public virtual decimal NotReduceSizePercent { get; set; }

        /// <summary>
        /// Средства собственников на кап. ремонт
        /// </summary>
        public virtual decimal OwnerSumForCr { get; set; }

        /// <summary>
        /// Не хранимое поле Имеющиеся средства (То есть Плановые средства * ПроцентРиска)
        /// </summary>
        public virtual decimal Available { get; set; }

        /// <summary>
        /// Не хранимое поле Резерв (или Остаток Которое используется только в момент расчета Когда необходимо получит ьОстаок за предыдущий год)
        /// </summary>
        public virtual decimal Reserve { get; set; }

        /// <summary>
        /// Не хранимое поле Бюджет на КР (Привязываю к версии посколку все это расчитывается на суммы именно той версии)
        /// </summary>
        public virtual decimal BudgetCr { get; set; }

        /// <summary>
        /// Не хранимое поле Потребность в финансировании 
        /// </summary>
        public virtual decimal CorrectionFinance { get; set; }

        /// <summary>
        /// Не хранимое поле. Остаток  средств после проведения КР на конец года
        /// </summary>
        public virtual decimal BalanceAfterCr { get; set; }
    }
}