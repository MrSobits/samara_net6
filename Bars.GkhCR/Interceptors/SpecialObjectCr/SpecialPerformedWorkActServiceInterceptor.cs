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

    public class SpecialPerformedWorkActServiceInterceptor : EmptyDomainInterceptor<SpecialPerformedWorkAct>
    {
        public override IDataResult BeforeCreateAction(IDomainService<SpecialPerformedWorkAct> service, SpecialPerformedWorkAct entity)
        {
            // Перед сохранением проставляем начальный статус
            var stateProvider = this.Container.Resolve<IStateProvider>();
            stateProvider.SetDefaultState(entity);

            if (this.Container.GetGkhConfig<GkhCrConfig>().General.TypeWorkActNumeration == TypeWorkActNumeration.Automatic)
            {
                // Проставляем номер автоматически
                var actNumbers =
                    service.GetAll().Select(x => x.DocumentNum).AsEnumerable().Select(x => x.ToInt()).ToArray();

                entity.DocumentNum = actNumbers.Any() ? (actNumbers.Max() + 1).ToStr() : "1";
            }

            // проверяем суммы и объем по работе
            return this.CheckSumAndVolume(entity, service);
        }

        public override IDataResult BeforeUpdateAction(IDomainService<SpecialPerformedWorkAct> service, SpecialPerformedWorkAct entity)
        {
            // проверяем суммы и объем по работе
            return this.CheckSumAndVolume(entity, service);
        }

        public virtual IDataResult CheckSumAndVolume(SpecialPerformedWorkAct act, IDomainService<SpecialPerformedWorkAct> service)
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

                if (act.TypeWorkCr.Sum < actsTotalSum || act.TypeWorkCr.Volume < actsTotalVolume)
                {
                    return this.Failure("Сумма и/или объем по актам выполненных работ по данному виду работы превышают значения, указанные в паспорте объекта КР. Сохранение отменено.");
                }
            }

            return this.Success();
        }

        public override IDataResult BeforeDeleteAction(IDomainService<SpecialPerformedWorkAct> service, SpecialPerformedWorkAct entity)
        {
            var paymentService = this.Container.Resolve<IDomainService<SpecialPerformedWorkActPayment>>();

            if (paymentService.GetAll().Any(x => x.PerformedWorkAct.Id == entity.Id))
            {
                return this.Failure("Удаление невозможно! Создано распоряжение на оплату акта.");
            }

            if (this.Container.Resolve<IDomainService<SpecialPerformedWorkActRecord>>().GetAll().Any(x => x.PerformedWorkAct.Id == entity.Id))
            {
                return this.Failure("Существуют связанные записи в следующих таблицах: Показатели актов выполненных работ;");
            }

            paymentService.GetAll().Where(x => x.PerformedWorkAct.Id == entity.Id).Select(x => x.Id).AsEnumerable().ForEach(x => paymentService.Delete(x));

            return this.Success();
        }
    }
}