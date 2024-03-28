namespace Bars.GkhGji.Regions.Habarovsk.Interceptors
{
    using Bars.B4;
    using Bars.B4.Config;
    using Bars.Gkh.Authentification;
    using Bars.Gkh.Entities;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Enums;
    using Bars.GkhGji.Regions.BaseChelyabinsk.Entities;
    using Bars.GkhGji.Regions.BaseChelyabinsk.Entities.Protocol197;
    using Entities;
    using Bars.GkhGji.Regions.BaseChelyabinsk.Enums;
    //using Enums;
    using System;
    using System.Linq;
    using Bars.GkhGji.Regions.BaseChelyabinsk.Enums.SMEV;

    class SMEVFNSLicRequestInterceptor : EmptyDomainInterceptor<SMEVFNSLicRequest>
    {
        public IGkhUserManager UserManager { get; set; }

        public IDomainService<SMEVFNSLicRequestFile> SMEVFNSLicRequestFileDomain { get; set; }

        public IDomainService<SMEVFNSLicRequest> SMEVFNSLicRequestDomain { get; set; }

        //public IDomainService<FLDocType> FLDocTypeDomain { get; set; }

        public override IDataResult BeforeCreateAction(IDomainService<SMEVFNSLicRequest> service, SMEVFNSLicRequest entity)
        {
            try
            {
//#if DEBUG
               //entity.Inspector = InspectorDomain.GetAll().First();
//#else
                Operator thisOperator = UserManager.GetActiveOperator();
                    if (thisOperator?.Inspector == null)
                        return Failure("Обмен информацией с ГИС ГМП доступен только сотрудникам ГЖИ");

                    entity.Inspector = thisOperator.Inspector;
//#endif
                entity.CalcDate = DateTime.Now;
                entity.ObjectCreateDate = DateTime.Now;
                entity.ObjectEditDate = DateTime.Now;
                entity.ObjectVersion = 1;
                entity.IdDoc = GuidGenerator.GenerateTimeBasedGuid(DateTime.Now).ToString();
                entity.RequestState = RequestState.Formed;
                return Success();
            }
            catch (Exception e)
            {
                return Failure($"Ошибка интерцептора BeforeCreateAction<SMEVFNSLicRequest>: {e.Message}");
            }
        }
    }
}
