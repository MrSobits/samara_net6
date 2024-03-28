namespace Bars.GkhGji.Regions.Chelyabinsk.Interceptors
{
    using Entities;
    using System;
    using System.Collections.Generic;

    using Bars.B4;
    using Bars.B4.Utils;
    using Bars.Gkh.Authentification;
    using Bars.Gkh.Entities;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Regions.BaseChelyabinsk.Enums.SMEV;
    using System.Linq;

    class SMEVEGRIPInterceptor : EmptyDomainInterceptor<SMEVEGRIP>
    {
        public IGkhUserManager UserManager { get; set; }
        public IDomainService<SMEVEGRIPFile> SMEVEGRIPFileDomain { get; set; }

        public IDomainService<Inspector> InspectorDomain { get; set; }

        public override IDataResult BeforeCreateAction(IDomainService<SMEVEGRIP> service, SMEVEGRIP entity)
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
                        return Failure("Обмен информацией с ГИС ГМП доступен только сотрудникам ГЖИ");
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
                          
                return Success();
            }
            catch (Exception e)
            {
                return Failure($"Ошибка интерцептора BeforeCreateAction<SMEVEGRIP>: {e.Message}");
            }
            
        }

        public override IDataResult BeforeUpdateAction(IDomainService<SMEVEGRIP> service, SMEVEGRIP entity)
        {
            try
            {
                entity.ObjectEditDate = DateTime.Now;
                return Success();
            }
            catch (Exception e)
            {
                return Failure($"Ошибка интерцептора BeforeUpdateAction<SMEVEGRIP>: {e.Message}");
            }
        }

        public override IDataResult BeforeDeleteAction(IDomainService<SMEVEGRIP> service, SMEVEGRIP entity)
        {
            try
            {
                var reportRow = SMEVEGRIPFileDomain.GetAll()
               .Where(x => x.SMEVEGRIP.Id == entity.Id)
               .Select(x => x.Id).ToList();
                foreach (var id in reportRow)
                {
                    SMEVEGRIPFileDomain.Delete(id);
                }
                return Success();
            }
            catch (Exception e)
            {
                return Failure(e.ToString());
            }
           
        }
    }
}
