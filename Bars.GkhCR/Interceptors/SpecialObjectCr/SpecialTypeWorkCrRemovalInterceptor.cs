namespace Bars.GkhCr.Interceptors
{
    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.IoC;
    using Bars.GkhCr.DomainService;
    using Bars.GkhCr.Entities;
    using Bars.GkhCr.Enums;

    using Castle.Windsor;

    /// <summary>
    /// Интерцептор для <see cref="SpecialTypeWorkCrRemoval"/>
    /// </summary>
    public class SpecialTypeWorkCrRemovalInterceptor : EmptyDomainInterceptor<SpecialTypeWorkCrRemoval>
    {
        public IWindsorContainer Container { get; set; }

        public override IDataResult AfterCreateAction(IDomainService<SpecialTypeWorkCrRemoval> service, SpecialTypeWorkCrRemoval entity)
        {
            var historyService = this.Container.Resolve<ISpecialTypeWorkCrHistoryService>();
            var twDomain = this.Container.Resolve<IRepository<SpecialTypeWorkCr>>(); // Ни в коем случае не меняйте здесь на IDomainService, а то при Update вызовутся ненужные методы Интерцепторов

            using (this.Container.Using(historyService, twDomain))
            {
                if (entity.TypeReason != TypeWorkCrReason.NotSet)
                {
                    // Сначала делаем неактивной Вид работы ОКР чтобы он не показывался в списке
                    var tw = twDomain.Load(entity.TypeWorkCr.Id);
                    tw.IsActive = false;

                    twDomain.Update(tw);

                    // Теперь запускаем создание истории для удаления вида работ
                    historyService.HistoryAfterRemove(entity);
                }

                return this.Success();
            }
        }
    }
}