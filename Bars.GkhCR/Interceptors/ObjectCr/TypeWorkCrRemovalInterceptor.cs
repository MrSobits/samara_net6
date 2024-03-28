namespace Bars.GkhCr.Interceptors
{
    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.GkhCr.DomainService;
    using Bars.GkhCr.Entities;
    using Bars.GkhCr.Enums;

    using Castle.Windsor;

    public class TypeWorkCrRemovalInterceptor : EmptyDomainInterceptor<TypeWorkCrRemoval>
    {
        public IWindsorContainer Container { get; set; }

        public override IDataResult BeforeCreateAction(IDomainService<TypeWorkCrRemoval> service, TypeWorkCrRemoval entity)
        {
            if (entity.TypeReason == TypeWorkCrReason.NewYear)
            {
                if (!entity.NewYearRepair.HasValue)
                {
                    return Failure("Новый год выполнения не может быть пустым");
                }

				if (entity.YearRepair.HasValue && entity.NewYearRepair.Value <= entity.YearRepair.Value)
                {
                    return Failure("Новый год выполнения работы не может быть меньше года выполнения по долгосрочной программе");
                }
            }

            return this.Success();
        }

        public override IDataResult AfterCreateAction(IDomainService<TypeWorkCrRemoval> service, TypeWorkCrRemoval entity)
        {
            var historyService = Container.Resolve<ITypeWorkCrHistoryService>();
            var twDomain = Container.Resolve<IRepository<TypeWorkCr>>(); // Ни вкоем случае не меняйте здесь на IDomainService, а то при Update вызовуться не нужные методы Интерцепторов
            
            try
            {
                if (entity.TypeReason != TypeWorkCrReason.NotSet) 
                { 
                    // Сначала делаем не активной Вид работы ОКР чтобы он непоказывался в списке
                    var tw = twDomain.Load(entity.TypeWorkCr.Id);
                    tw.IsActive = false;

                    twDomain.Update(tw);

                    // Теперь запускаем создание истории для удаления вида работ
                    historyService.HistoryAfterRemove(entity);
                }

                return this.Success();
            }
            finally 
            {
                Container.Release(historyService);
                Container.Release(twDomain);
            }
        }
    }
}