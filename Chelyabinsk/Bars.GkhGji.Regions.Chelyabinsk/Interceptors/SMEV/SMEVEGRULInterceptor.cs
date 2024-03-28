namespace Bars.GkhGji.Regions.Chelyabinsk.Interceptors
{
    using Bars.B4;
    using Bars.Gkh.Authentification;
    using Bars.Gkh.Entities;
    using Entities;
    using System;
    using Bars.GkhGji.Regions.BaseChelyabinsk.Enums.SMEV;
    using System.Linq;

    class SMEVEGRULInterceptor : EmptyDomainInterceptor<SMEVEGRUL>
    {
        public IGkhUserManager UserManager { get; set; }

        public IDomainService<SMEVEGRULFile> SMEVEGRULFileDomain { get; set; }

        public IDomainService<Inspector> InspectorDomain { get; set; }

        public override IDataResult BeforeCreateAction(IDomainService<SMEVEGRUL> service, SMEVEGRUL entity)
        {
            try
            {
          
                if (entity.Inspector == null)
                {
#if DEBUG
                    entity.Inspector = InspectorDomain.GetAll().FirstOrDefault();
#else
                    Operator thisOperator = UserManager.GetActiveOperator();
               
                    if (thisOperator?.Inspector == null)
                    {
                        return Failure("Обмен информацией с ГИС ГМП доступен только сотрудникам ГЖИ");
                    }
                    else
                    {
                        entity.Inspector = thisOperator.Inspector;
                    }                                                   
#endif
                }
                if (entity.InnOgrn == Enums.InnOgrn.INN)
                {
                    if (entity.INNReq.Length > 10)
                    {
                        return Failure($"Превышена длина поля ИНН");
                    }
                }
                if (entity.InnOgrn == Enums.InnOgrn.OGRN)
                {
                    if (entity.INNReq.Length > 13)
                    {
                        return Failure($"Превышена длина поля ОГРН");
                    }
                }
                entity.CalcDate = DateTime.Now;
                entity.ObjectCreateDate = DateTime.Now;
                entity.ObjectEditDate = DateTime.Now;
                entity.ObjectVersion = 1;
                entity.RequestState = RequestState.NotFormed;

                return Success();
            }
            catch (Exception e)
            {
                return Failure($"Ошибка интерцептора BeforeCreateAction<SMEVEGRUL>: {e.Message}");
            }
        }

        public override IDataResult BeforeUpdateAction(IDomainService<SMEVEGRUL> service, SMEVEGRUL entity)
        {
            try
            {
                entity.ObjectEditDate = DateTime.Now;

                return Success();
            }
            catch (Exception e)
            {
                return Failure($"Ошибка интерцептора BeforeUpdateAction<SMEVEGRUL>: {e.Message}");
            }
        }

        public override IDataResult BeforeDeleteAction(IDomainService<SMEVEGRUL> service, SMEVEGRUL entity)
        {
            try
            {
                SMEVEGRULFileDomain.GetAll()
               .Where(x => x.SMEVEGRUL.Id == entity.Id)
               .Select(x => x.Id)
               .ToList()
               .ForEach(x => SMEVEGRULFileDomain.Delete(x));

                return Success();
            }
            catch (Exception e)
            {
                return Failure($"Ошибка интерцептора BeforeDeleteAction<SMEVEGRUL>: {e.Message}");
            }

        }
    }
}
