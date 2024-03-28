namespace Bars.GkhGji.Regions.Habarovsk.Interceptors
{
    using Bars.B4;
    using Bars.Gkh.Authentification;
    using Bars.Gkh.Entities;
    using Entities;
    using System;
    using System.Linq;
    using Bars.GkhGji.Regions.BaseChelyabinsk.Enums.SMEV;

    class SMEVSocialHireInterceptor : EmptyDomainInterceptor<SMEVSocialHire>
    {
        public IGkhUserManager UserManager { get; set; }

        public IDomainService<SMEVSocialHireFile> SMEVSocialHireFileDomain { get; set; }

        public IDomainService<Inspector> InspectorDomain { get; set; }

        public override IDataResult BeforeCreateAction(IDomainService<SMEVSocialHire> service, SMEVSocialHire entity)
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
                //entity.RequestId = GuidGenerator.GenerateTimeBasedGuid(DateTime.Now).ToString();
                entity.RequestState = RequestState.NotFormed;

                return Success();
            }
            catch (Exception e)
            {
                return Failure($"Ошибка интерцептора BeforeCreateAction<SMEVSocialHire>: {e.Message}");
            }
        }

        public override IDataResult BeforeUpdateAction(IDomainService<SMEVSocialHire> service, SMEVSocialHire entity)
        {
            try
            {
                entity.ObjectEditDate = DateTime.Now;

                return Success();
            }
            catch (Exception e)
            {
                return Failure($"Ошибка интерцептора BeforeUpdateAction<SMEVSocialHire>: {e.Message}");
            }
        }

        public override IDataResult BeforeDeleteAction(IDomainService<SMEVSocialHire> service, SMEVSocialHire entity)
        {
            try
            {
                SMEVSocialHireFileDomain.GetAll()
               .Where(x => x.SMEVSocialHire.Id == entity.Id)
               .Select(x => x.Id)
               .ToList()
               .ForEach(x => SMEVSocialHireFileDomain.Delete(x));

                return Success();
            }
            catch (Exception e)
            {
                return Failure($"Ошибка интерцептора BeforeDeleteAction<SMEVSocialHire>: {e.Message}");
            }

        }
    }
}
