namespace Bars.GkhGji.Interceptors
{
    using System;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.Modules.States;
    using Bars.B4.Utils;
    using Bars.GkhGji.Entities;

    public class BusinessActivityServiceInterceptor : EmptyDomainInterceptor<BusinessActivity>
    {
        public override IDataResult BeforeCreateAction(IDomainService<BusinessActivity> service, BusinessActivity entity)
        {
            // проверяем, есть ли уже уведомление с таким контрагентом
            var notif = service.GetAll()
                             .Where(x => x.Contragent.Id == entity.Contragent.Id && x.Id != entity.Id)
                             .ToList();

            if (notif.Any())
            {
                return this.Failure("У данного контрагента уже существует уведомление о начале деятельности");
            }

            // Перед сохранением проставляем начальный статус
            var stateProvider = this.Container.Resolve<IStateProvider>();
            stateProvider.SetDefaultState(entity);

            return this.Success();
        }

        public override IDataResult BeforeDeleteAction(IDomainService<BusinessActivity> service, BusinessActivity entity)
        {
            var servJurGjiServ = Container.Resolve<IDomainService<ServiceJuridicalGji>>();

            try
            {
                servJurGjiServ.GetAll().Where(x => x.BusinessActivityNotif.Id == entity.Id)
                    .Select(x => x.Id).ForEach(x => servJurGjiServ.Delete(x));

                return Success();
            }
            catch (Exception)
            {
                return Failure("Не удалось удалить связанные записи");
            }
            finally
            {
                Container.Release(servJurGjiServ);
            }
        }
    }
}