namespace Bars.GkhGji.Regions.BaseChelyabinsk.Interceptors
{
    using Bars.B4;
    using Bars.Gkh.Authentification;
    using Bars.Gkh.Entities;
    using Entities;
    using Bars.GkhGji.Regions.BaseChelyabinsk.Enums.SMEV;
    using System;
    using System.Linq;

    class SMEVERULReqNumberInterceptor : EmptyDomainInterceptor<SMEVERULReqNumber>
    {
        public IGkhUserManager UserManager { get; set; }

        public IDomainService<SMEVERULReqNumberFile> SMEVERULReqNumberFileDomain { get; set; }

        public IDomainService<Inspector> InspectorDomain { get; set; }

        public override IDataResult BeforeCreateAction(IDomainService<SMEVERULReqNumber> service, SMEVERULReqNumber entity)
        {
            try
            {

                Operator thisOperator = UserManager.GetActiveOperator();
                if (thisOperator.Inspector != null)
                {
                    entity.Inspector = thisOperator.Inspector;
                    entity.CalcDate = DateTime.Now;
                    entity.ObjectCreateDate = DateTime.Now;
                    entity.ObjectEditDate = DateTime.Now;
                    entity.ObjectVersion = 1;
                    entity.RequestState = RequestState.NotFormed;
                }
                else
                    return Failure("Обмен информацией со СМЭВ доступен только сотрудникам ГЖИ");

                return Success();
            }
            catch (Exception e)
            {
                return Failure("Не удалось создать запрос");
            }
        }    

        public override IDataResult BeforeDeleteAction(IDomainService<SMEVERULReqNumber> service, SMEVERULReqNumber entity)
        {
            try
            {
                SMEVERULReqNumberFileDomain.GetAll()
               .Where(x => x.SMEVERULReqNumber.Id == entity.Id)
               .Select(x => x.Id)
               .ToList()
               .ForEach(x => SMEVERULReqNumberFileDomain.Delete(x));

                return Success();
            }
            catch (Exception e)
            {
                return Failure($"Ошибка интерцептора BeforeDeleteAction<SMEVERULReqNumber>: {e.Message}");
            }

        }
    }
}
