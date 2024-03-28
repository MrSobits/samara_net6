namespace Bars.GkhGji.Regions.Chelyabinsk.Interceptors
{
    using Bars.B4;
    using Bars.B4.Config;
    using Bars.Gkh.Authentification;
    using Bars.Gkh.Entities;
    using Entities;
    using Bars.GkhGji.Regions.BaseChelyabinsk.Enums.SMEV;
    using Enums;
    using System;
    using System.Linq;

    class PayRegInterceptor : EmptyDomainInterceptor<PayRegRequests>
    {
        public IGkhUserManager UserManager { get; set; }

        public IDomainService<PayRegFile> PayRegFileDomain { get; set; }

        public IDomainService<Inspector> InspectorDomain { get; set; }

        public override IDataResult BeforeCreateAction(IDomainService<PayRegRequests> service, PayRegRequests entity)
        {
            try
            {
                if (entity.Inspector == null)
                {
#if DEBUG
                    entity.Inspector = InspectorDomain.GetAll().FirstOrDefault();
#else
                    Operator thisOperator = UserManager.GetActiveOperator();
               
                    if (thisOperator?.Inspector == null)
                    {
                        return Failure("Обмен информацией со ГИС ГМП доступен только сотрудникам ГЖИ");
                    }
                    else
                    {
                        entity.Inspector = thisOperator.Inspector;
                    }                                                   
#endif
                }
                entity.CalcDate = DateTime.Now;
                entity.ObjectCreateDate = DateTime.Now;
                entity.ObjectEditDate = DateTime.Now;
                entity.ObjectVersion = 1;
                entity.RequestState = RequestState.NotFormed;
                
                entity.RequestState = RequestState.Formed;
                return Success();
            }
            catch (Exception e)
            {
                return Failure($"Ошибка интерцептора BeforeCreateAction<PayReg>: {e.Message}");
            }
        }

        public override IDataResult BeforeUpdateAction(IDomainService<PayRegRequests> service, PayRegRequests entity)
        {
            try
            {
//#if DEBUG
//                entity.Inspector = InspectorDomain.GetAll().First();
//#else
//                Operator thisOperator = UserManager.GetActiveOperator();
//                if (thisOperator?.Inspector == null)
//                {

//                }
//                else
//                {
//                  entity.Inspector = thisOperator.Inspector;
//                }

                    
//#endif
                entity.ObjectEditDate = DateTime.Now;
               
                return Success();
            }
            catch (Exception e)
            {
                return Failure($"Ошибка интерцептора BeforeUpdateAction<PayReg>: {e.Message}");
            }
        }

        public override IDataResult BeforeDeleteAction(IDomainService<PayRegRequests> service, PayRegRequests entity)
        {
            try
            {
                //чистка приаттаченных файлов
                PayRegFileDomain.GetAll()
               .Where(x => x.PayRegRequests.Id == entity.Id)
               .Select(x => x.Id)
               .ToList()
               .ForEach(x => PayRegFileDomain.Delete(x));

                return Success();
            }
            catch (Exception e)
            {
                return Failure($"Ошибка интерцептора BeforeDeleteAction<PayReg>: {e.ToString()}");
            }
        }

        string cachedId = null;

        private string GetOriginatorIdFromConfig()
        {
            if (cachedId != null)
                return cachedId;

            var configProvider = Container.Resolve<IConfigProvider>();
            var config = configProvider.GetConfig().GetModuleConfig("Bars.GkhGji.Regions.Chelyabinsk");
            string id = config.GetAs("OriginatorId", (string)null, true);

            if (id == null)
                throw new Exception("Не определен OriginatorId в user.config в секции Bars.GkhGji.Regions.Chelyabinsk");

            cachedId = id;
            return cachedId;
        }
                
    }
}
