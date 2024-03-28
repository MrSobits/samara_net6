namespace Bars.GkhGji.Regions.Tatarstan.TorIntegration.Controllers
{
	using System;
	using System.Collections;
	using System.Linq;
	using Microsoft.AspNetCore.Mvc;

	using Bars.B4;
	using Bars.B4.DataAccess;
	using Bars.B4.IoC;
	// TODO : Расскоментировать после реализации GisIntegration.Tor
	//using Bars.GisIntegration.Tor.Service.SendData;
	using Bars.Gkh.Domain;
    using Bars.Gkh.Entities;
	using Bars.Gkh.Utils;
	using Bars.GkhGji.Regions.Tatarstan.Entities;
    using Bars.GkhGji.Regions.Tatarstan.Entities.Dict;

    public class TorIntegrationController : BaseController
	{
		/*
		/// <summary>
		/// Отправить КНМ в ТОР КНД
		/// </summary>
		public ActionResult SendKnmToTor(BaseParams baseParams)
		{
			// TODO : Расскоментировать после реализации GisIntegration.Tor
			var service = this.Container.Resolve<ISendDataService<TatarstanDisposal>>();
			using (this.Container.Using(service))
			{
				return service.PrepareData(baseParams).ToJsonResult();
			}
		}

        /// <summary>
		/// Отправить Обязательные требования в ТОР КНД
		/// </summary>
		public ActionResult SendMandatoryReqsToTor(BaseParams baseParams)
        {
            var service = this.Container.Resolve<ISendDataService<MandatoryReqs>>();
            using (this.Container.Using(service))
            {
                return service.PrepareData(baseParams).ToJsonResult();
            }
        }
        
        /// <summary>
        /// Получить все субъекты из ТОР
        /// </summary>
        public ActionResult GetAllSubjectsFromTor(BaseParams baseParams)
        {
           var service = this.Container.Resolve<ISendDataService<Contragent>>();
            using (this.Container.Using(service))
            {
                return service.GetData(baseParams).ToJsonResult();
            }
        }
        
        /// <summary>
        /// Отправить показатели эффективности и результативности в ТОР
        /// </summary>
        public ActionResult SendEpIndexToTor(BaseParams baseParams)
        {
           var service = this.Container.Resolve<ISendDataService<EffectivenessAndPerformanceIndexValue>>();
            using (this.Container.Using(service))
            {
                return service.PrepareData(baseParams).ToJsonResult();
            }
        }

        /// <summary>
        /// Получить показатели эффективности и результативности в ТОР
        /// </summary>
        public ActionResult GetEpIndexFromTor(BaseParams baseParams)
        {
            var service = this.Container.Resolve<ISendDataService<EffectivenessAndPerformanceIndexValue>>();
            using (this.Container.Using(service))
            {
                var result = service.GetData(baseParams).ToJsonResult();

                return result;
            }
        }
        */

        /// <summary>
        /// Получает отправленные в ТОР КНД объекты
        /// </summary>
        public ActionResult GetObjects(BaseParams baseParams)
        {
	        var roDomain = this.Container.ResolveDomain<RealityObject>();
	        using (this.Container.Using(roDomain))
	        {
		        var result = roDomain.GetAll()
			        .Where(x => x.TorId != null && x.TorId != Guid.Empty)
			        .Select(x => new
			        {
				        x.Id,
				        x.TorId,
				        Municipality = x.Municipality.Name,
				        x.Address,
				        FiasAddress = x.FiasAddress.HouseGuid
			        })
			        .ToListDataResult(baseParams.GetLoadParam(), this.Container);

		        return new JsonListResult((IList) result.Data, result.TotalCount);
	        }
        }

        /// <summary>
        /// Получает отправленные в ТОР КНД субъекты
        /// </summary>
        public ActionResult GetSubjects(BaseParams baseParams)
        {
	        var contragentDomain = this.Container.ResolveDomain<Contragent>();
	        using (this.Container.Using(contragentDomain))
	        {
		        var result = contragentDomain.GetAll()
			        .Where(x => x.TorId != null && x.TorId != Guid.Empty)
			        .Select(x => new
			        {
				        x.Id,
				        x.TorId,
				        x.Name,
				        x.JuridicalAddress,
				        x.Inn,
				        x.Kpp,
				        x.Ogrn
			        })
			        .ToListDataResult(baseParams.GetLoadParam(), this.Container);

		        return new JsonListResult((IList) result.Data, result.TotalCount);
	        }
        }
    }
}