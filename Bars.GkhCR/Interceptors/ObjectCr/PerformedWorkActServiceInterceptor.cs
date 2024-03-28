namespace Bars.GkhCr.Interceptors
{
    using Bars.B4;
    using Bars.B4.Modules.States;
    using Bars.B4.Utils;
    using Bars.GkhCr.Entities;

    using System.Linq;

    using Bars.Gkh.ConfigSections.Cr;
    using Bars.Gkh.ConfigSections.Cr.Enums;
    using Bars.Gkh.Utils;

    public class PerformedWorkActServiceInterceptor : EmptyDomainInterceptor<PerformedWorkAct>
    {
        public override IDataResult BeforeCreateAction(IDomainService<PerformedWorkAct> service, PerformedWorkAct entity)
        {
            // Перед сохранением проставляем начальный статус
            var stateProvider = Container.Resolve<IStateProvider>();
            stateProvider.SetDefaultState(entity);

            if (Container.GetGkhConfig<GkhCrConfig>().General.TypeWorkActNumeration == TypeWorkActNumeration.Automatic)
            {
                // Проставляем номер автоматически
                var actNumbers =
                    service.GetAll().Select(x => x.DocumentNum).AsEnumerable().Select(x => x.ToInt()).ToArray();

                entity.DocumentNum = actNumbers.Any() ? (actNumbers.Max() + 1).ToStr() : "1";
            }

            // проверяем суммы и объем по работе
            return CheckSumAndVolume(entity, service);
        }

        public override IDataResult BeforeUpdateAction(IDomainService<PerformedWorkAct> service, PerformedWorkAct entity)
        {
            // проверяем суммы и объем по работе
            return CheckSumAndVolume(entity, service);
        }

        public virtual IDataResult CheckSumAndVolume(PerformedWorkAct act, IDomainService<PerformedWorkAct> service)
        {
            if (act.TypeWorkCr != null)
            {
                var sumVolume =
                    service.GetAll()
                           .Where(x => x.TypeWorkCr.Id == act.TypeWorkCr.Id && (act.Id == 0 || x.Id != act.Id))
                           .Select(x => new { x.Sum, x.Volume })
                           .ToArray();

                var actsTotalSum = sumVolume.Select(x => x.Sum).Sum() + act.Sum;
                var actsTotalVolume = sumVolume.Select(x => x.Volume).Sum() + act.Volume;
                if (!act.OverLimits)
                {
                    if (act.TypeWorkCr.Sum < actsTotalSum || act.TypeWorkCr.Volume < actsTotalVolume)
                    {
                        return Failure("Сумма и/или объем по актам выполненных работ по данному виду работы превышают значения, указанные в паспорте объекта КР. Сохранение отменено.");
                    }
                }
            }

            return Success();
        }

        public override IDataResult BeforeDeleteAction(IDomainService<PerformedWorkAct> service, PerformedWorkAct entity)
        {
            var paymentService = Container.Resolve<IDomainService<PerformedWorkActPayment>>();

            if (paymentService.GetAll().Any(x => x.PerformedWorkAct.Id == entity.Id))
            {
                return Failure("Удаление невозможно! Создано распоряжение на оплату акта.");
            }

            if (Container.Resolve<IDomainService<PerformedWorkActRecord>>().GetAll().Any(x => x.PerformedWorkAct.Id == entity.Id))
            {
                return Failure("Существуют связанные записи в следующих таблицах: Показатели актов выполненных работ;");
            }

            paymentService.GetAll().Where(x => x.PerformedWorkAct.Id == entity.Id).Select(x => x.Id).AsEnumerable().ForEach(x => paymentService.Delete(x));

            return Success();
        }
    }
}