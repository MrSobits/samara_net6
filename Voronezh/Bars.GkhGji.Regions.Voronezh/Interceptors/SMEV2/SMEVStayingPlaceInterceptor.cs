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

    class SMEVStayingPlaceInterceptor : EmptyDomainInterceptor<SMEVStayingPlace>
    {
        public IGkhUserManager UserManager { get; set; }
        public IDomainService<SMEVStayingPlaceFile> SMEVStayingPlaceFileDomain { get; set; }
        public IDomainService<Inspector> InspectorDomain { get; set; }

        public override IDataResult BeforeCreateAction(IDomainService<SMEVStayingPlace> service, SMEVStayingPlace entity)
        {
            entity.CalcDate = DateTime.Now;
            try
            {

                //  entity.Inspector = InspectorDomain.GetAll().First();

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
                return Failure($"Ошибка интерцептора BeforeCreateAction<SMEVStayingPlaceFileDomain>: {e.Message}");
               

            }
        }     

        public override IDataResult BeforeDeleteAction(IDomainService<SMEVStayingPlace> service, SMEVStayingPlace entity)
        {
            try
            {
                //чистка приаттаченных файлов
                SMEVStayingPlaceFileDomain.GetAll()
               .Where(x => x.SMEVStayingPlace.Id == entity.Id)
               .Select(x => x.Id)
               .ToList()
               .ForEach(x => SMEVStayingPlaceFileDomain.Delete(x));

             
                return Success();
            }
            catch (Exception e)
            {
                return Failure($"Ошибка интерцептора BeforeDeleteAction<SMEVStayingPlaceFileDomain>: {e.ToString()}");
            }
        }
 
       
    }
}
