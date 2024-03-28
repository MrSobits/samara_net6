namespace Bars.GkhGji.Regions.Habarovsk.Interceptors
{
    using Bars.B4;
    using Bars.Gkh.Authentification;
    using Bars.Gkh.Entities;
    using Bars.GkhGji.Regions.Habarovsk.Entities.SMEVOwnershipProperty;
    using Bars.GkhGji.Regions.BaseChelyabinsk.Enums.SMEV;
    using Entities;
    using System;
    using System.Linq;

    class SMEVOwnershipPropertyInterceptor : EmptyDomainInterceptor<SMEVOwnershipProperty>
    {
        public IGkhUserManager UserManager { get; set; }

        public IDomainService<SMEVOwnershipPropertyFile> SMEVOwnershipPropertyFileDomain { get; set; }

        public IDomainService<Inspector> InspectorDomain { get; set; }

        public override IDataResult BeforeCreateAction(IDomainService<SMEVOwnershipProperty> service, SMEVOwnershipProperty entity)
        {
            try
            {
          
                if (entity.Inspector == null)
                {
                    Operator thisOperator = UserManager.GetActiveOperator();
               
                    if (thisOperator?.Inspector == null)
                    {
                        return Failure("Обмен информацией с ГИС ГМП доступен только сотрудникам ГЖИ");
                    }
                    else
                    {
                        entity.Inspector = thisOperator.Inspector;
                    }                                                   
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
                return Failure($"Ошибка интерцептора BeforeCreateAction<SMEVOwnershipProperty>: {e.Message}");
            }
        }

        public override IDataResult BeforeUpdateAction(IDomainService<SMEVOwnershipProperty> service, SMEVOwnershipProperty entity)
        {
            try
            {
                entity.ObjectEditDate = DateTime.Now;

                return Success();
            }
            catch (Exception e)
            {
                return Failure($"Ошибка интерцептора BeforeUpdateAction<SMEVOwnershipProperty>: {e.Message}");
            }
        }

        public override IDataResult BeforeDeleteAction(IDomainService<SMEVOwnershipProperty> service, SMEVOwnershipProperty entity)
        {
            try
            {
                SMEVOwnershipPropertyFileDomain.GetAll()
               .Where(x => x.SMEVOwnershipProperty.Id == entity.Id)
               .Select(x => x.Id)
               .ToList()
               .ForEach(x => SMEVOwnershipPropertyFileDomain.Delete(x));

                return Success();
            }
            catch (Exception e)
            {
                return Failure($"Ошибка интерцептора BeforeDeleteAction<SMEVOwnershipProperty>: {e.Message}");
            }

        }
    }
}
