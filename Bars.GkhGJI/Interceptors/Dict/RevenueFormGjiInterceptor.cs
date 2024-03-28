namespace Bars.GkhGji.Interceptors
{
    using System.Linq;

    using Bars.B4;
    using Bars.GkhGji.Entities;

    public class RevenueFormGjiInterceptor : EmptyDomainInterceptor<RevenueFormGji>
    {
        public override IDataResult BeforeDeleteAction(IDomainService<RevenueFormGji> service, RevenueFormGji entity)
        {
            if (Container.Resolve<IDomainService<AppealCitsSource>>().GetAll().Any(x => x.RevenueForm.Id == entity.Id))
            {
                return Failure("Существуют связанные записи в следующих таблицах: Форма поступления реестра обращений;");
            }

            return this.Success();
        }
    }
}