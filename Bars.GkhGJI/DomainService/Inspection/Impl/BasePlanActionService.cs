namespace Bars.GkhGji.DomainService
{
    using System;
    using System.Linq;

    using B4;

    using Bars.Gkh.Entities;

    using Entities;

    using Castle.Windsor;

    public class BasePlanActionService : IBasePlanActionService
    {
        public IWindsorContainer Container { get; set; }

        public IDataResult GetContragentInfo(BaseParams baseParams)
        {

            var contragentService = Container.Resolve<IDomainService<Contragent>>(); 
            
            try
            {
                var contragentInn = string.Empty;
                var contragentOgrn = string.Empty;
                var contragentAddress = string.Empty;

                var contragentId = baseParams.Params.GetAs("contragentId", (long)0);

                var obj = contragentService.GetAll().FirstOrDefault(x => x.Id == contragentId);
                
                if (obj != null)
                {
                    contragentInn = obj.Inn;
                    contragentOgrn = obj.Ogrn;
                    contragentAddress = obj.FactAddress;
                }

                return new BaseDataResult(new { contragentInn, contragentOgrn, contragentAddress });
            }
            catch (ValidationException e)
            {
                return new BaseDataResult { Success = false, Message = e.Message };
            }
            finally
            {
                Container.Release(contragentService);
            }
        }

        public IDataResult GetStartFilters()
        {

            var planActionService = Container.Resolve<IDomainService<PlanActionGji>>();

            try
            {
                var plan = planActionService.GetAll()
                                     .Where(x => x.DateEnd >= DateTime.Now && x.DateStart <= DateTime.Now)
                                     .OrderByDescending(x => x.ObjectCreateDate)
                                     .FirstOrDefault();

                return plan != null ? new BaseDataResult(new { planId = plan.Id, planName = plan.Name, dateStart = plan.DateStart }) : new BaseDataResult();
            }
            finally
            {
                Container.Release(planActionService);
            }
        }
    }
}