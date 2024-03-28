namespace Bars.GkhGji.Regions.Habarovsk.Interceptors
{
    using Bars.B4;
    using Bars.Gkh.Authentification;
    using Bars.Gkh.Entities;
    using Bars.GkhGji.Regions.Habarovsk.Entities.SMEVEmergencyHouse;
    using Bars.GkhGji.Regions.BaseChelyabinsk.Enums.SMEV;
    using Entities;
    using System;
    using System.Linq;

    class SMEVEmergencyHouseInterceptor : EmptyDomainInterceptor<SMEVEmergencyHouse>
    {
        public IGkhUserManager UserManager { get; set; }

        public IDomainService<SMEVEmergencyHouseFile> SMEVEmergencyHouseFileDomain { get; set; }

        public IDomainService<Inspector> InspectorDomain { get; set; }

        public override IDataResult BeforeCreateAction(IDomainService<SMEVEmergencyHouse> service, SMEVEmergencyHouse entity)
        {
            try
            {
          
                if (entity.Inspector == null)
                {
                    Operator thisOperator = UserManager.GetActiveOperator();
               
                    if (thisOperator?.Inspector == null)
                    {
                        return Failure("Обмен информацией с ГИС доступен только сотрудникам ГЖИ");
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
                return Failure($"Ошибка интерцептора BeforeCreateAction<SMEVEmergencyHouse>: {e.Message}");
            }
        }

        public override IDataResult BeforeUpdateAction(IDomainService<SMEVEmergencyHouse> service, SMEVEmergencyHouse entity)
        {
            try
            {
                entity.ObjectEditDate = DateTime.Now;

                return Success();
            }
            catch (Exception e)
            {
                return Failure($"Ошибка интерцептора BeforeUpdateAction<SMEVEmergencyHouse>: {e.Message}");
            }
        }

        public override IDataResult BeforeDeleteAction(IDomainService<SMEVEmergencyHouse> service, SMEVEmergencyHouse entity)
        {
            try
            {
                SMEVEmergencyHouseFileDomain.GetAll()
               .Where(x => x.SMEVEmergencyHouse.Id == entity.Id)
               .Select(x => x.Id)
               .ToList()
               .ForEach(x => SMEVEmergencyHouseFileDomain.Delete(x));

                return Success();
            }
            catch (Exception e)
            {
                return Failure($"Ошибка интерцептора BeforeDeleteAction<SMEVEmergencyHouse>: {e.Message}");
            }

        }
    }
}
