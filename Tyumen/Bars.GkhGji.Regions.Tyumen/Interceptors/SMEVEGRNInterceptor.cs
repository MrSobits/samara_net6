namespace Bars.GkhGji.Regions.Tyumen.Interceptors
{
    using Entities;
    using System;
    using System.Collections.Generic;

    using Bars.B4;
    using Bars.B4.Utils;
    using Bars.Gkh.Authentification;
    using Bars.Gkh.Entities;
    using Bars.GkhGji.Entities;
    using System.Linq;

    class SMEVEGRNInterceptor : EmptyDomainInterceptor<SMEVEGRN>
    {
        public IGkhUserManager UserManager { get; set; }
        public IDomainService<SMEVEGRNFile> SMEVEGRNFileDomain { get; set; }

       public override IDataResult BeforeCreateAction(IDomainService<SMEVEGRN> service, SMEVEGRN entity)
        {
            try {   

                Operator thisOperator = UserManager.GetActiveOperator();
                if (thisOperator.Inspector != null)
                {
                    entity.Inspector = thisOperator.Inspector;
                    entity.RequestDate = DateTime.Now;
                    entity.ObjectCreateDate = DateTime.Now;
                    entity.ObjectEditDate = DateTime.Now;
                    entity.ObjectVersion = 1;
                    entity.RequestState = Enums.RequestState.NotFormed;
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

        public override IDataResult BeforeUpdateAction(IDomainService<SMEVEGRN> service, SMEVEGRN entity)
        {
            try
            {
                if (1==1)
                {
                    Operator thisOperator = UserManager.GetActiveOperator();
                    if (thisOperator.Inspector != null)
                    {
                        entity.Inspector = thisOperator.Inspector;
                        entity.ObjectEditDate = DateTime.Now;
                    }
                    else
                        return Failure("Обмен информацией со СМЭВ доступен только пользователям с указанным сотрудником.");

                    return Success();
                }
                else
                {
                    return Failure("Данный запрос исполнен, для возобновления обмена данными создайте новый запрос");
                }
            }
            catch (Exception e)
            {
                return Failure("Не удалось сохранить запрос");
            }
        }

        public override IDataResult BeforeDeleteAction(IDomainService<SMEVEGRN> service, SMEVEGRN entity)
        {
            try
            {
                var reportRow = SMEVEGRNFileDomain.GetAll()
               .Where(x => x.SMEVEGRN.Id == entity.Id)
               .Select(x => x.Id).ToList();
                foreach (var id in reportRow)
                {
                    SMEVEGRNFileDomain.Delete(id);
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
