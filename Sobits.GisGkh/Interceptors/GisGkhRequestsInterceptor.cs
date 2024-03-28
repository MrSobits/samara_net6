namespace Sobits.GisGkh.Interceptors
{
    using Bars.B4;
    using Bars.B4.Config;
    using Bars.Gkh.Authentification;
    using Bars.Gkh.Entities;
    using Entities;
    using Enums;
    using System;
    using System.Linq;

    class GisGkhRequestsInterceptor : EmptyDomainInterceptor<GisGkhRequests>
    {
        public IGkhUserManager UserManager { get; set; }

        //public IDomainService<PayRegFile> PayRegFileDomain { get; set; }

        public IDomainService<Operator> OperatorDomain { get; set; }

        public override IDataResult BeforeCreateAction(IDomainService<GisGkhRequests> service, GisGkhRequests entity)
        {
            if(this.OperatorDomain == null)
            this.OperatorDomain = Container.Resolve<IDomainService<Operator>>();
            if(this.UserManager == null)
            this.UserManager = Container.Resolve<IGkhUserManager>();
            //if (entity != null && entity.TypeRequest != GisGkhTypeRequest.importDebtRequests)
            //{
            try
                {
                    if (entity.Operator == null)
                    {
                        Operator thisOperator = UserManager.GetActiveOperator();
                        if (thisOperator == null)
                        {
                            thisOperator = OperatorDomain.GetAll().FirstOrDefault();
                        }
                        entity.Operator = thisOperator;

                    }
                    //entity.ReqDate = DateTime.Now;
                    entity.ObjectCreateDate = DateTime.Now;
                    entity.ObjectEditDate = DateTime.Now;
                    entity.ObjectVersion = 1;
                    //entity.RequestState = RequestState.NotFormed;

                    //entity.RequestState = RequestState.Formed;
                    //if (entity.TypeRequest == GisGkhTypeRequest.exportBriefApartmentHouse ||
                    //    entity.TypeRequest == GisGkhTypeRequest.exportHouseData ||
                    //    entity.TypeRequest == GisGkhTypeRequest.exportNsiItems ||
                    //    entity.TypeRequest == GisGkhTypeRequest.exportNsiList ||
                    //    entity.TypeRequest == GisGkhTypeRequest.exportNsiRaoList ||
                    //    entity.TypeRequest == GisGkhTypeRequest.exportRegionalProgram ||
                    //    entity.TypeRequest == GisGkhTypeRequest.exportRegionalProgramWork ||
                    //    entity.TypeRequest == GisGkhTypeRequest.exportOrgRegistry ||
                    //    entity.TypeRequest == GisGkhTypeRequest.exportAccountData ||
                    //    entity.TypeRequest == GisGkhTypeRequest.exportPlan)
                    //{
                    //    entity.IsExport = Bars.Gkh.Enums.YesNo.Yes;
                    //}
                    //else
                    //{
                    //    entity.IsExport = Bars.Gkh.Enums.YesNo.No;
                    //}
                    return Success();
                }
                catch (Exception e)
                {
                    // return Success();
                    return Failure($"Ошибка интерцептора BeforeCreateAction<GisGkhRequests>: {e.Message} {e.StackTrace}");
                }
            //}
            //else
            //    return Success();
        }

        public override IDataResult BeforeUpdateAction(IDomainService<GisGkhRequests> service, GisGkhRequests entity)
        {
            try
            {
                entity.ObjectEditDate = DateTime.Now;
               
                return Success();
            }
            catch (Exception e)
            {
                return Success();
                //  return Failure($"Ошибка интерцептора BeforeUpdateAction<PayReg>: {e.Message}");
            }
        }

        public override IDataResult BeforeDeleteAction(IDomainService<GisGkhRequests> service, GisGkhRequests entity)
        {
            try
            {
                //чистка приаттаченных файлов
               // PayRegFileDomain.GetAll()
               //.Where(x => x.PayRegRequests.Id == entity.Id)
               //.Select(x => x.Id)
               //.ToList()
               //.ForEach(x => PayRegFileDomain.Delete(x));

                return Success();
            }
            catch (Exception e)
            {
                return Failure($"Ошибка интерцептора BeforeDeleteAction<GisGkhRequests>: {e.ToString()}");
            }
        }     
    }
}
