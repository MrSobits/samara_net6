namespace Bars.Gkh.RegOperator.Interceptors
{
    using System.Linq;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.Utils;
    using Bars.Gkh.RegOperator.Entities.Dict;

    /// <summary>
    /// Interceptor для Настрйка фиксированная периода расчета пени
    /// </summary>
    public class FixedPeriodCalcPenaltiesInterceptor : EmptyDomainInterceptor<FixedPeriodCalcPenalties>
    {
        public override IDataResult BeforeCreateAction(IDomainService<FixedPeriodCalcPenalties> service, FixedPeriodCalcPenalties entity)
        {
            if (service.GetAll()
                .Any(x => x.DateStart >= entity.DateStart))
            {
                return this.Failure("Внимание! Дата начала действия нового значения не может быть раньше уже действующего параметра!");
            }

            if (entity.StartDay > 31 || entity.EndDay > 31)
            {
                return this.Failure("Количество дней не может быть больше 31 дня. Укажите другое значение");
            }

            this.SetDateEnd(service, entity);

            return this.Success();
        }

        public override IDataResult BeforeUpdateAction(IDomainService<FixedPeriodCalcPenalties> service, FixedPeriodCalcPenalties entity)
        {
            if (entity.DateStart > entity.DateEnd)
            {
                return this.Failure("Внимание! Дата начала действия не может быть позже даты окончания!");
            }

            // Проверка на наличие "поглощения" других периодов
            var coversAnotherPeriod = service.GetAll()
                .Where(x => x.Id != entity.Id)
                .WhereIf(entity.DateEnd.HasValue, x => x.DateStart <= entity.DateEnd)
                .Any(x => x.DateStart >= entity.DateStart);

            if (coversAnotherPeriod)
            {
                return this.Failure("Внимание! Дата начала действия значения должна быть позже даты начала предыдущего!");
            }

            if (entity.StartDay > 31 || entity.EndDay > 31)
            {
                return this.Failure("Количество дней не может быть больше 31 дня. Укажите другое значение");
            }

            this.SetDateEnd(service, entity);
            return this.Success();
        }

        private void SetDateEnd(IDomainService<FixedPeriodCalcPenalties> service, FixedPeriodCalcPenalties entity)
        {
            var lastItem = service.GetAll()
                .Where(x => x.DateStart < entity.DateStart)
                .OrderByDescending(x => x.DateStart)
                .ThenByDescending(x => x.DateEnd)
                .FirstOrDefault();

            if (lastItem != null)
            {
                lastItem.DateEnd = entity.DateStart.AddDays(-1);

                this.Container.Resolve<IRepository<FixedPeriodCalcPenalties>>().Update(lastItem);
            }
        }
    }
}