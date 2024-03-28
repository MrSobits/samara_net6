namespace Bars.Gkh.RegOperator.Interceptors
{
    using System.Linq;

    using Bars.B4;
    using Bars.Gkh.RegOperator.Entities;
    using Bars.Gkh.RegOperator.Entities.Dict;
    using Bars.Gkh.RegOperator.Entities.PersonalAccount;
    using Bars.Gkh.RegOperator.Enums;

    /// <summary>
    /// Интерцептор настройки расчета пени с отсрочкой
    /// </summary>
    public class PenaltiesWithDeferredInterceptor : EmptyDomainInterceptor<PenaltiesWithDeferred>
    {
        /// <summary>
        /// Домен-сервис Трассировка параметров расчетов ЛС
        /// </summary>
        public IDomainService<CalculationParameterTrace> CalcParamTraceDomainService { get; set; }

        /// <summary>
        /// Домен-сервис начислений ЛС
        /// </summary>
        public IDomainService<PersonalAccountCharge> AccountChargeDomainService { get; set; }

        /// <inheritdoc />
        public override IDataResult BeforeCreateAction(IDomainService<PenaltiesWithDeferred> service, PenaltiesWithDeferred entity)
        {
            if (service.GetAll()
                .Any(x => x.DateEndCalc <= entity.DateStartCalc))
            {
                return this.Failure("Внимание! Дата начала действия нового значения не может быть раньше уже действующего параметра!");
            }

            if (service.GetAll()
                .Any(x => x.DateEndCalc <= entity.DateStartCalc))
            {
                return this.Failure("Внимание! Дата начала действия нового значения не может быть раньше уже действующего параметра!");
            }

            return this.Success();
        }

        /// <inheritdoc />
        public override IDataResult BeforeUpdateAction(IDomainService<PenaltiesWithDeferred> service, PenaltiesWithDeferred entity)
        {
            if (entity.DateStartCalc > entity.DateEndCalc)
            {
                return this.Failure("Внимание! Дата начала действия не может быть позже даты окончания!");
            }

            if (service.GetAll()
                .Where(x => x.Id != entity.Id)
                .Any(x => x.DateEndCalc <= entity.DateStartCalc))
            {
                return this.Failure("Внимание! Дата начала действия нового значения не может быть раньше уже действующего параметра!");
            }

            if (this.CheckPenaltytrace(entity))
            {
                return this.Success();
            }

            return this.Failure("Запрещено изменять параметры, по которым производился расчет!");
        }

        /// <inheritdoc />
        public override IDataResult BeforeDeleteAction(IDomainService<PenaltiesWithDeferred> service, PenaltiesWithDeferred entity)
        {
            if (this.CheckPenaltytrace(entity))
            {
                return this.Success();
            }

            return this.Failure("Запрещено удалять параметры, по которым производился расчет!");
        }

        private bool CheckPenaltytrace(PenaltiesWithDeferred entity)
        {
            return !this.CalcParamTraceDomainService
               .GetAll()
               .Where(
                   y => this.AccountChargeDomainService.GetAll()
                       .Where(x => x.ChargeDate >= entity.DateStartCalc)
                       .Where(x => x.ChargeDate <= entity.DateEndCalc)
                       .Any(x => x.Guid == y.CalculationGuid && x.IsActive)).Any(x => x.CalculationType == CalculationTraceType.DelayPenaltyRecalc);
        }
    }
}