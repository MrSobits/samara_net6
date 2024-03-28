namespace Bars.GkhGji.Regions.Tatarstan.Interceptors.ActCheck
{
    using System.Linq;

    using Bars.B4;
    using Bars.Gkh.Utils;
    using Bars.B4.IoC;
    using Bars.GkhGji.Regions.Tatarstan.Entities.ActCheckAction.DocRequestAction;

    public class DocRequestActionInterceptor : InheritedActCheckActionBaseInterceptor<DocRequestAction>
    {
        /// <inheritdoc />
        public override IDataResult BeforeCreateAction(IDomainService<DocRequestAction> service, DocRequestAction entity)
        {
            Utils.SaveFiasAddress(this.Container, entity.DocProvidingAddress);
            
            return base.BeforeCreateAction(service, entity);
        }

        /// <inheritdoc />
        protected override void DeleteInheritedActionAdditionalEntities(DocRequestAction entity)
        {
            var docRequestActionRequestInfoService = this.Container.Resolve<IDomainService<DocRequestActionRequestInfo>>();

            using (this.Container.Using(docRequestActionRequestInfoService))
            {
                docRequestActionRequestInfoService
                    .GetAll()
                    .Where(x => x.DocRequestAction.Id == entity.Id)
                    .ToList()
                    .ForEach(x => docRequestActionRequestInfoService.Delete(x.Id));
            }
        }
    }
}