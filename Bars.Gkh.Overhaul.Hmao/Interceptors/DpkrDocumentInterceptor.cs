namespace Bars.Gkh.Overhaul.Hmao.Interceptors
{
    using System.Linq;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.IoC;
    using Bars.B4.Modules.States;
    using Bars.Gkh.Overhaul.Hmao.Entities;

    using NHibernate.Linq;

    public class DpkrDocumentInterceptor : EmptyDomainInterceptor<DpkrDocument>
    {
        public override IDataResult BeforeCreateAction(IDomainService<DpkrDocument> service, DpkrDocument entity)
        {
            // Перед сохранением проставляем начальный статус
            var stateProvider = Container.Resolve<IStateProvider>();
            try
            {
                stateProvider.SetDefaultState(entity);
                return Success();
            }
            finally
            {
                Container.Release(stateProvider);
            }
        }

        /// <inheritdoc />
        public override IDataResult BeforeDeleteAction(IDomainService<DpkrDocument> service, DpkrDocument entity)
        {
            var dpkrDocumentProgramVersionDomain = this.Container.ResolveDomain<DpkrDocumentProgramVersion>();
            var dpkrDocumentRealityObjectDomain = this.Container.ResolveDomain<DpkrDocumentRealityObject>();

            using (this.Container.Using(dpkrDocumentProgramVersionDomain, dpkrDocumentRealityObjectDomain))
            {
                dpkrDocumentProgramVersionDomain.GetAll()
                    .Where(x => x.DpkrDocument.Id == entity.Id)
                    .Delete();

                dpkrDocumentRealityObjectDomain.GetAll()
                    .Where(x => x.DpkrDocument.Id == entity.Id)
                    .Delete();
            }

            return base.BeforeDeleteAction(service, entity);
        }
    }
}
