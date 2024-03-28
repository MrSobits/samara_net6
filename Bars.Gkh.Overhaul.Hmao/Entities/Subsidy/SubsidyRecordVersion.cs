

namespace Bars.Gkh.Overhaul.Hmao.Entities
{
    using Bars.Gkh.Entities;
    using System;

    public class SubsidyRecordVersion : BaseImportableEntity
    {
        /// <summary>
        /// Версия ДПКР
        /// </summary>
        public virtual ProgramVersion Version { get; set; }

        /// <summary>
        /// Ссылка на строку субсидирования
        /// </summary>
        public virtual SubsidyRecord SubsidyRecord { get; set; }

        /// <summary>
        /// Бюджет на КР (Привязываю к версии посколку все это расчитывается на суммы именно той версии)
        /// </summary>
        public virtual decimal BudgetCr { get; set; }

        /// <summary>
        /// Потребность в финансировании 
        /// </summary>
        public virtual decimal CorrectionFinance { get; set; }

        /// <summary>
        /// Остаток  средств после проведения КР на конец года
        /// </summary>
        public virtual decimal BalanceAfterCr { get; set; }


        /// <summary>
        /// Дополнительные расходы
        /// </summary>
        public virtual decimal AdditionalExpences { get; set; }

        /// <summary>
        /// Год
        /// </summary>
        public virtual int SubsidyYear { get; set; }

        /// <summary>
        /// Сальдо нарастающим итогом
        /// </summary>
        public virtual decimal SaldoBallance { get; set; }


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
        /// Дата расчета собираемости
        /// </summary>
        public virtual DateTime? DateCalcOwnerCollection { get; set; }

        /// <summary>
        /// Не хранимое поле Имеющиеся средства (То есть Плановые средства * ПроцентРиска)
        /// </summary>
        public virtual decimal Available { get; set; }

        /// <summary>
        /// Не хранимое поле Резерв (или Остаток Которое используется только в момент расчета Когда необходимо получит ьОстаок за предыдущий год)
        /// </summary>
        public virtual decimal Reserve { get; set; }
    }
}