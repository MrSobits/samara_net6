namespace Bars.GkhGji.Interceptors
{
    using System.Linq;
    using Bars.B4.Utils;
    using Bars.B4;
    using Bars.GkhGji.Entities;

    public class KindWorkNotifGjiInterceptor : EmptyDomainInterceptor<KindWorkNotifGji>
    {
        public override IDataResult BeforeCreateAction(IDomainService<KindWorkNotifGji> service, KindWorkNotifGji entity)
        {
            return CheckForm(entity);
        }

        public override IDataResult BeforeUpdateAction(IDomainService<KindWorkNotifGji> service, KindWorkNotifGji entity)
        {
            return CheckForm(entity);
        }

        public override IDataResult BeforeDeleteAction(IDomainService<KindWorkNotifGji> service, KindWorkNotifGji entity)
        {
            if (Container.Resolve<IDomainService<ServiceJuridicalGji>>().GetAll().Any(x => x.KindWorkNotif.Id == entity.Id))
            {
                return Failure("Существуют связанные записи в следующих таблицах: Услуги уведомления о начале предпринимательской деятельности;");
            }

            return this.Success();
        }

        private IDataResult CheckForm(KindWorkNotifGji entity)
        {
            if (entity.Code.IsNotEmpty() && entity.Code.Length > 50)
            {
                return Failure("Количество знаков в поле Код не должно превышать 50 символов");
            }

            if (entity.Name.IsNotEmpty() && entity.Name.Length > 300)
            {
                return Failure("Количество знаков в поле Наименование не должно превышать 300 символов");
            }

            return Success();
        }
    }
}