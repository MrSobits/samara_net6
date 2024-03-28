namespace Bars.GkhGji.Interceptors
{
    using B4.Modules.States;
    using Bars.B4;
    using Bars.B4.Config;
    using Bars.B4.DataAccess;
    using Bars.Gkh.Authentification;
    using Bars.Gkh.Domain.CollectionExtensions;
    using Bars.Gkh.Entities;
    using Bars.GkhGji.Entities;
    using Entities;
    using Enums;
    using System;
    using System.Linq;

    public class MKDLicRequestInterceptor : EmptyDomainInterceptor<MKDLicRequest>
    {
        public IGkhUserManager UserManager { get; set; }

        public override IDataResult BeforeCreateAction(IDomainService<MKDLicRequest> service, MKDLicRequest entity)
        {
            try
            { 
                var servStateProvider = Container.Resolve<IStateProvider>();

                try
                {
                    servStateProvider.SetDefaultState(entity);                  
                }
                finally
                {
                    Container.Release(servStateProvider);
                }
                return Success();
            }
            catch (Exception e)
            {
                return Failure($"Ошибка интерцептора BeforeCreateAction<MKDLicRequest>: {e.Message}");
            }
        }

        public override IDataResult BeforeDeleteAction(IDomainService<MKDLicRequest> service, MKDLicRequest entity)
        {
            var annexService = this.Container.Resolve<IDomainService<MKDLicRequestFile>>();
            var inspectorService = this.Container.Resolve<IDomainService<MKDLicRequestInspector>>();
            var queryService = this.Container.Resolve<IDomainService<MKDLicRequestQuery>>();
            var queryAnswerService = this.Container.Resolve<IDomainService<MKDLicRequestQueryAnswer>>();
            var roService = this.Container.Resolve<IDomainService<MKDLicRequestRealityObject>>();

            try
            {

                var annexIds = annexService.GetAll().Where(x => x.MKDLicRequest.Id == entity.Id).Select(x => x.Id).ToList();
                foreach (var value in annexIds)
                {
                    annexService.Delete(value);
                }
                var inspectorIds = inspectorService.GetAll().Where(x => x.MKDLicRequest.Id == entity.Id).Select(x => x.Id).ToList();
                foreach (var value in inspectorIds)
                {
                    inspectorService.Delete(value);
                }
                var queryIds = queryService.GetAll().Where(x => x.MKDLicRequest.Id == entity.Id).Select(x => x.Id).ToList();
                foreach (var value in queryIds)
                {
                    var queryAnswerIds = queryAnswerService.GetAll().Where(x => x.MKDLicRequestQuery.Id == value).Select(x => x.Id).ToList();
                    foreach (var value2 in queryAnswerIds)
                    {
                        queryAnswerService.Delete(value2);
                    }
                    queryService.Delete(value);
                }
                var roIds = roService.GetAll().Where(x => x.MKDLicRequest.Id == entity.Id).Select(x => x.Id).ToList();
                foreach (var value in roIds)
                {
                    roService.Delete(value);
                }

                return Success();
            }
            finally
            {
                Container.Release(annexService);              
            }
        }

        public override IDataResult BeforeUpdateAction(IDomainService<MKDLicRequest> service, MKDLicRequest entity)
        {
            var queryAnswerService = this.Container.Resolve<IDomainService<MKDLicRequestQueryAnswer>>();
            queryAnswerService.GetAll().Where(x => x.Id > 0);

            return base.BeforeUpdateAction(service, entity);
        }
    }
}
