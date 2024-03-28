using Bars.Gkh.Enums.Decisions;

namespace Bars.Gkh.RegOperator.Interceptors
{
    using System.Linq;

    using B4;
    using B4.DataAccess;
    using B4.IoC;
    using B4.Utils;

    using Bars.Gkh.RegOperator.Enums;

    using Entities.Dict;
    using Entities.PersonalAccount;

    public class PaymentPenaltiesInterceptor : EmptyDomainInterceptor<PaymentPenalties>
    {
        public override IDataResult BeforeCreateAction(IDomainService<PaymentPenalties> service, PaymentPenalties entity)
        {
            if (entity.DecisionType == CrFundFormationDecisionType.Unknown)
            {
                return Failure("Не заполнены обязательные поля: Способ формирования");
            }

            if (service.GetAll()
                .Where(x => x.DecisionType == entity.DecisionType)
                .Any(x => x.DateStart >= entity.DateStart))
            {
                return Failure("Внимание! Дата начала действия нового значения не может быть раньше уже действующего параметра!");
            }

            if (entity.Days > 31)
            {
                return Failure("Количество дней просрочки не может быть больше 31 дня. Укажите другое значение");
            }

            SetDateEnd(service, entity);

            var recalcEvent = new PersonalAccountRecalcEvent
            {
                EventDate = entity.DateStart,
                RecalcProvider = "Изменение справочника параметров пени",
                RecalcType = PersonalAccountRecalcEvent.PenaltyType,
                PersonalAccount = null,
                RecalcEventType = RecalcEventType.ChangePenaltyParam
            };

            var eventDomain = Container.ResolveDomain<PersonalAccountRecalcEvent>();

            using (Container.Using(eventDomain))
            {
                eventDomain.Save(recalcEvent);
            }

            return Success();
        }

        public override IDataResult BeforeUpdateAction(IDomainService<PaymentPenalties> service, PaymentPenalties entity)
        {
            if (entity.DateStart > entity.DateEnd)
            {
                return Failure("Внимание! Дата начала действия не может быть позже даты окончания!");
            }

            // Проверка на наличие "поглощения" других периодов
            var coversAnotherPeriod = service.GetAll()
                .Where(x => x.DecisionType == entity.DecisionType)
                .Where(x => x.Id != entity.Id)
                .WhereIf(entity.DateEnd.HasValue, x => x.DateStart <= entity.DateEnd)
                .Any(x => x.DateStart >= entity.DateStart);

            if (coversAnotherPeriod)
            {
                return Failure("Внимание! Дата начала действия значения должна быть позже даты начала предыдущего!");
            }

            if (entity.Days > 31)
            {
                return Failure("Количество дней просрочки не может быть больше 31 дня. Укажите другое значение");
            }

            SetDateEnd(service, entity);
            return Success();
        }

        private void SetDateEnd(IDomainService<PaymentPenalties> service, PaymentPenalties entity)
        {
            var lastItem = service.GetAll()
                .Where(x => x.DateStart < entity.DateStart)
                .Where(x => x.DecisionType == entity.DecisionType)
                .OrderByDescending(x => x.DateStart)
                .ThenByDescending(x => x.DateEnd)
                .FirstOrDefault();

            if (lastItem != null)
            {
                lastItem.DateEnd = entity.DateStart.AddDays(-1);

                Container.Resolve<IRepository<PaymentPenalties>>().Update(lastItem);
            }
        }
    }
}