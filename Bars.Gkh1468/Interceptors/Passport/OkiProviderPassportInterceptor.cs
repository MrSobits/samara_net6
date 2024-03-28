namespace Bars.Gkh1468.Interceptors.Passport
{
    using System.Linq;

    using B4;
    using B4.Utils;
    using Entities;

    public class OkiProviderPassportInterceptor : EmptyDomainInterceptor<OkiProviderPassport>
    {
        public IDomainService<OkiProviderPassportRow> rowDomain { get; set; }

        public override IDataResult BeforeDeleteAction(IDomainService<OkiProviderPassport> service, OkiProviderPassport entity)
        {
            rowDomain.GetAll().Where(x => x.ProviderPassport.Id == entity.Id).Select(x => x.Id).ForEach(x => rowDomain.Delete(x));
            
            return base.BeforeDeleteAction(service, entity);
        }

        public override IDataResult BeforeUpdateAction(IDomainService<OkiProviderPassport> service, OkiProviderPassport entity)
        {
            RecalculateHousePassportPercent(service, entity);

            return base.BeforeUpdateAction(service, entity);
        }

        public override IDataResult AfterDeleteAction(IDomainService<OkiProviderPassport> service, OkiProviderPassport entity)
        {
            RecalculateHousePassportPercent(service, entity);

            return base.AfterDeleteAction(service, entity);
        }

        private void RecalculateHousePassportPercent(IDomainService<OkiProviderPassport> service, OkiProviderPassport entity)
        {
            var providerFillCount = service.GetAll()
                .Count(x => x.OkiPassport.Id == entity.OkiPassport.Id && x.Percent == 100);

            var providerCount = service.GetAll()
                .Count(x => x.OkiPassport.Id == entity.OkiPassport.Id);


            entity.OkiPassport.Percent = providerCount != 0 ? ((decimal)providerFillCount * 100 / providerCount) : 0;
        }
    }
}