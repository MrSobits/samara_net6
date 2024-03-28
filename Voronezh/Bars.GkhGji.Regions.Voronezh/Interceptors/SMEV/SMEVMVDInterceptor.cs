namespace Bars.GkhGji.Regions.Voronezh.Interceptors
{
    using Entities;
    using System;
    using System.Collections.Generic;
    using Bars.GkhGji.Regions.BaseChelyabinsk.Enums.SMEV;
    using Bars.B4;
    using Bars.B4.Utils;
    using Bars.Gkh.Authentification;
    using Bars.Gkh.Entities;
    using Bars.GkhGji.Entities;
    using System.Linq;

    class SMEVMVDInterceptor : EmptyDomainInterceptor<SMEVMVD>
    {
        public IGkhUserManager UserManager { get; set; }
        public IDomainService<SMEVMVDFile> SMEVMVDFileDomain { get; set; }

       public override IDataResult BeforeCreateAction(IDomainService<SMEVMVD> service, SMEVMVD entity)
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
                    if (entity.ContragentContact != null)
                    {
                        if (string.IsNullOrEmpty(entity.Surname))
                        {
                            entity.Surname = entity.ContragentContact.Surname;
                            entity.Name = entity.ContragentContact.Name;
                            entity.PatronymicName = entity.ContragentContact.Patronymic;
                            
                        }
                        if ((entity.BirthDate == null || entity.BirthDate == DateTime.MinValue) && entity.ContragentContact.BirthDate.HasValue)
                        {
                            entity.BirthDate = entity.ContragentContact.BirthDate.Value;
                        }
                        else if (!entity.ContragentContact.BirthDate.HasValue)
                        {
                            return Failure("У контакта контрагента не проставлена дата рождения, укажите вручную в карточке запроса, или в карточке контакта");
                        }
                    }
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

        public override IDataResult BeforeUpdateAction(IDomainService<SMEVMVD> service, SMEVMVD entity)
        {
            try
            {
                if (1==2)
                {
                    Operator thisOperator = UserManager.GetActiveOperator();
                    if (thisOperator!= null && thisOperator.Inspector != null)
                    {
                        entity.Inspector = thisOperator.Inspector;
                        entity.ObjectEditDate = DateTime.Now;
                    }
                    else if(thisOperator != null && thisOperator.Inspector == null)
                        return Failure("Расчет категорий риска доступен только сотрудникам ГЖИ");

                    return Success();
                }
             
            }
            catch (Exception e)
            {
              //  return Failure("Не удалось сохранить запрос");
            }
            return Success();
        }

        public override IDataResult BeforeDeleteAction(IDomainService<SMEVMVD> service, SMEVMVD entity)
        {
            try
            {
                var reportRow = SMEVMVDFileDomain.GetAll()
               .Where(x => x.SMEVMVD.Id == entity.Id)
               .Select(x => x.Id).ToList();
                foreach (var id in reportRow)
                {
                    SMEVMVDFileDomain.Delete(id);
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
