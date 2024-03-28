namespace Bars.GkhGji.Regions.BaseChelyabinsk.Interceptors
{
    using Entities;
    using System;
    using System.Collections.Generic;

    using Bars.B4;
    using Bars.B4.Utils;
    using Bars.Gkh.Authentification;
    using Bars.Gkh.Entities;
    using Enums.SMEV;
    using Bars.GkhGji.Entities;
    using System.Linq;

    class MVDLivingPlaceRegistrationInterceptor : EmptyDomainInterceptor<MVDLivingPlaceRegistration>
    {
        public IGkhUserManager UserManager { get; set; }
        public IDomainService<MVDLivingPlaceRegistrationFile> MVDLivingPlaceRegistrationFileDomain { get; set; }

       public override IDataResult BeforeCreateAction(IDomainService<MVDLivingPlaceRegistration> service, MVDLivingPlaceRegistration entity)
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
        private bool ValidateFields(MVDLivingPlaceRegistration entity)
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
        public override IDataResult BeforeDeleteAction(IDomainService<MVDLivingPlaceRegistration> service, MVDLivingPlaceRegistration entity)
        {
            try
            {
                var reportRow = MVDLivingPlaceRegistrationFileDomain.GetAll()
               .Where(x => x.MVDLivingPlaceRegistration.Id == entity.Id)
               .Select(x => x.Id).ToList();
                foreach (var id in reportRow)
                {
                    MVDLivingPlaceRegistrationFileDomain.Delete(id);
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
