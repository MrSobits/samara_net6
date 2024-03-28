namespace Bars.GkhGji.Regions.Voronezh.Interceptors
{
    using Bars.B4;
    using Bars.B4.Config;
    using Bars.Gkh.Authentification;
    using Bars.Gkh.Entities;
    using Bars.GkhGji.Regions.BaseChelyabinsk.Enums.SMEV;
    using Entities;
    using Enums;
    using System;
    using System.Linq;

    class SMEVValidPassportInterceptor : EmptyDomainInterceptor<SMEVValidPassport>
    {
        public IGkhUserManager UserManager { get; set; }
        public IDomainService<SMEVValidPassportFile> SMEVValidPassportFileDomain { get; set; }
        public IDomainService<Inspector> InspectorDomain { get; set; }

        public override IDataResult BeforeCreateAction(IDomainService<SMEVValidPassport> service, SMEVValidPassport entity)
        {
            entity.CalcDate = DateTime.Now;
            try
            {

                Operator thisOperator = UserManager.GetActiveOperator();
                if (thisOperator?.Inspector == null)
                    return Failure("Обмен информацией со СМЭВ2 доступен только сотрудникам ГЖИ");

                entity.Inspector = thisOperator.Inspector;

                //
                entity.RequestState = RequestState.NotFormed;
                return Success();
            }
            catch (Exception e)
            {
                return Failure($"Ошибка интерцептора BeforeCreateAction<SMEVValidPassportFileDomain>: {e.Message}");
               

            }
        }     

        public override IDataResult BeforeDeleteAction(IDomainService<SMEVValidPassport> service, SMEVValidPassport entity)
        {
            try
            {
                //чистка приаттаченных файлов
                SMEVValidPassportFileDomain.GetAll()
               .Where(x => x.SMEVValidPassport.Id == entity.Id)
               .Select(x => x.Id)
               .ToList()
               .ForEach(x => SMEVValidPassportFileDomain.Delete(x));

             
                return Success();
            }
            catch (Exception e)
            {
                return Failure($"Ошибка интерцептора BeforeDeleteAction<SMEVValidPassportFileDomain>: {e.ToString()}");
            }
        }
 
       
    }
}
