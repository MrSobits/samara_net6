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

    class GASUInterceptor : EmptyDomainInterceptor<GASU>
    {
        public IGkhUserManager UserManager { get; set; }
        public IDomainService<GASUFile> SMEVMVDFileDomain { get; set; }

       public override IDataResult BeforeCreateAction(IDomainService<GASU> service, GASU entity)
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

        public override IDataResult BeforeUpdateAction(IDomainService<GASU> service, GASU entity)
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
                        return Failure("бмен информацией со СМЭВ доступен только сотрудникам ГЖИ");

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

        public override IDataResult BeforeDeleteAction(IDomainService<GASU> service, GASU entity)
        {
            try
            {
                var reportRow = SMEVMVDFileDomain.GetAll()
               .Where(x => x.GASU.Id == entity.Id)
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
