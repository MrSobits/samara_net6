namespace Bars.GkhGji.Regions.Habarovsk.Interceptors
{
    using Bars.B4;
    using Bars.Gkh.Authentification;
    using Bars.Gkh.Entities;
    using Entities;
    using System;
    using System.Linq;
    using Bars.GkhGji.Regions.BaseChelyabinsk.Enums.SMEV;

    class SMEVNDFLInterceptor : EmptyDomainInterceptor<SMEVNDFL>
    {
        public IGkhUserManager UserManager { get; set; }

        public IDomainService<SMEVNDFLFile> SMEVNDFLFileDomain { get; set; }

        public IDomainService<Inspector> InspectorDomain { get; set; }

        public override IDataResult BeforeCreateAction(IDomainService<SMEVNDFL> service, SMEVNDFL entity)
        {
            try
            {
          
                if (entity.Inspector == null)
                {
                    Operator thisOperator = UserManager.GetActiveOperator();
               
                    if (thisOperator?.Inspector == null)
                    {
                        return Failure("Обмен информацией доступен только сотрудникам ГЖИ");
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
                entity.RequestId = GuidGenerator.GenerateTimeBasedGuid(DateTime.Now).ToString();
                entity.RequestState = RequestState.NotFormed;

                return Success();
            }
            catch (Exception e)
            {
                return Failure($"Ошибка интерцептора BeforeCreateAction<SMEVNDFL>: {e.Message}");
            }
        }

        public override IDataResult BeforeUpdateAction(IDomainService<SMEVNDFL> service, SMEVNDFL entity)
        {
            try
            {
                entity.ObjectEditDate = DateTime.Now;

                return Success();
            }
            catch (Exception e)
            {
                return Failure($"Ошибка интерцептора BeforeUpdateAction<SMEVNDFL>: {e.Message}");
            }
        }

        public override IDataResult BeforeDeleteAction(IDomainService<SMEVNDFL> service, SMEVNDFL entity)
        {
            try
            {
                SMEVNDFLFileDomain.GetAll()
               .Where(x => x.SMEVNDFL.Id == entity.Id)
               .Select(x => x.Id)
               .ToList()
               .ForEach(x => SMEVNDFLFileDomain.Delete(x));

                return Success();
            }
            catch (Exception e)
            {
                return Failure($"Ошибка интерцептора BeforeDeleteAction<SMEVNDFL>: {e.Message}");
            }

        }
    }
}
