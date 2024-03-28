namespace Bars.GkhGji.Regions.BaseChelyabinsk.Interceptors
{
    using Entities;
    using System;
    using System.Collections.Generic;

    using Bars.B4;
    using Bars.B4.Utils;
    using Enums.SMEV;
    using Bars.Gkh.Authentification;
    using Bars.Gkh.Entities;
    using Bars.GkhGji.Entities;
    using System.Linq;

    class MVDStayingPlaceRegistrationInterceptor : EmptyDomainInterceptor<MVDStayingPlaceRegistration>
    {
        public IGkhUserManager UserManager { get; set; }
        public IDomainService<MVDStayingPlaceRegistrationFile> MVDStayingPlaceRegistrationFileDomain { get; set; }

       public override IDataResult BeforeCreateAction(IDomainService<MVDStayingPlaceRegistration> service, MVDStayingPlaceRegistration entity)
        {
            try {   

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

                return ValidateFields(entity)? Success(): Failure("Не заполнено одно или несколько полей запроса");

              
            }
            catch (Exception e)
            {
                return Failure("Не удалось создать запрос");
            }
            
        }
        private bool ValidateFields(MVDStayingPlaceRegistration entity)
        {

            if (entity.BirthDate.HasValue && !string.IsNullOrEmpty(entity.Surname) && !string.IsNullOrEmpty(entity.Name))
            {
                if (entity.IssueDate.HasValue && !string.IsNullOrEmpty(entity.PassportSeries) && !string.IsNullOrEmpty(entity.PassportSeries))
                    return true;
                else
                    return false;
            }                      
            else
            return false;

            
        }        
        public override IDataResult BeforeDeleteAction(IDomainService<MVDStayingPlaceRegistration> service, MVDStayingPlaceRegistration entity)
        {
            try
            {
                var reportRow = MVDStayingPlaceRegistrationFileDomain.GetAll()
               .Where(x => x.MVDStayingPlaceRegistration.Id == entity.Id)
               .Select(x => x.Id).ToList();
                foreach (var id in reportRow)
                {
                    MVDStayingPlaceRegistrationFileDomain.Delete(id);
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
