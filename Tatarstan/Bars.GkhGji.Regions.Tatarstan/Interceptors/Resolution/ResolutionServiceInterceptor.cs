namespace Bars.GkhGji.Regions.Tatarstan.Interceptors.Resolution
{
    using System.Linq;

    using Bars.B4;
    using Bars.GkhGji.Entities.Dict;
    using Bars.GkhGji.Enums;
    using Bars.GkhGji.Interceptors;
    using Bars.GkhGji.Regions.Tatarstan.Entities;
    using Bars.GkhGji.Regions.Tatarstan.Entities.Resolution;

    public class ResolutionServiceInterceptor : ResolutionServiceInterceptor<TatarstanResolution>
    {
        public override IDataResult BeforeCreateAction(IDomainService<TatarstanResolution> service, TatarstanResolution entity)
        {
            var citizenshipDomain = this.Container.Resolve<IDomainService<Citizenship>>();
            try
            {
                if (entity.CitizenshipType == CitizenshipType.RussianFederation && entity.Citizenship == null)
                {
                    var citizenship = citizenshipDomain.GetAll().FirstOrDefault(x => x.OksmCode == 643);

                    entity.Citizenship = citizenship;
                }
            }
            finally
            {
                this.Container.Release(citizenshipDomain);
            }

            return base.BeforeCreateAction(service, entity);
        }

        public override IDataResult BeforeUpdateAction(IDomainService<TatarstanResolution> service, TatarstanResolution entity)
        {
            var citizenshipDomain = this.Container.Resolve<IDomainService<Citizenship>>();
            try
            {
                if (entity.CitizenshipType == CitizenshipType.RussianFederation && entity.Citizenship == null)
                {
                    var citizenship = citizenshipDomain.GetAll().FirstOrDefault(x => x.OksmCode == 643);

                    entity.Citizenship = citizenship;
                }
            }
            finally
            {
                this.Container.Release(citizenshipDomain);
            }

            return base.BeforeUpdateAction(service, entity);
        }
        
        public override IDataResult BeforeDeleteAction(IDomainService<TatarstanResolution> service, TatarstanResolution entity)
        {
            var gisChargeToSendDomain = this.Container.Resolve<IDomainService<GisChargeToSend>>();
            try
            {
                var gisChargeList = gisChargeToSendDomain.GetAll().Where(x => x.Document.Id == entity.Id);

                // Удаление записей реестра отправки начислений в ГИС ГМП
                foreach (var gisCharge in gisChargeList)
                {
                    gisChargeToSendDomain.Delete(gisCharge.Id);
                }
                
                return base.BeforeDeleteAction(service, entity);
            }
            finally
            {
                this.Container.Release(gisChargeToSendDomain);
            }
        }
    }
}