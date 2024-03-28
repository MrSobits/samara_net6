namespace Bars.GkhGji.Regions.Voronezh.Interceptors
{
    using Bars.B4;
    using Bars.Gkh.Authentification;
    using Bars.Gkh.Entities;
    using Entities;
    using System;
    using System.Linq;
    using Bars.GkhGji.Regions.BaseChelyabinsk.Enums.SMEV;

    class SMEVDISKVLICInterceptor : EmptyDomainInterceptor<SMEVDISKVLIC>
    {
        public IGkhUserManager UserManager { get; set; }

        public IDomainService<SMEVDISKVLICFile> SMEVDISKVLICFileDomain { get; set; }

        public IDomainService<Inspector> InspectorDomain { get; set; }

        public override IDataResult BeforeCreateAction(IDomainService<SMEVDISKVLIC> service, SMEVDISKVLIC entity)
        {
            try
            {
          
                if (entity.Inspector == null)
                {
                    Operator thisOperator = UserManager.GetActiveOperator();
               
                    if (thisOperator?.Inspector == null)
                    {
                        return Failure("Обмен информацией с ГИС ГМП доступен только сотрудникам ГЖИ");
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
                return Failure($"Ошибка интерцептора BeforeCreateAction<SMEVDISKVLIC>: {e.Message}");
            }
        }

        public override IDataResult BeforeUpdateAction(IDomainService<SMEVDISKVLIC> service, SMEVDISKVLIC entity)
        {
            try
            {
                entity.ObjectEditDate = DateTime.Now;

                return Success();
            }
            catch (Exception e)
            {
                return Failure($"Ошибка интерцептора BeforeUpdateAction<SMEVDISKVLIC>: {e.Message}");
            }
        }

        public override IDataResult BeforeDeleteAction(IDomainService<SMEVDISKVLIC> service, SMEVDISKVLIC entity)
        {
            try
            {
                SMEVDISKVLICFileDomain.GetAll()
               .Where(x => x.SMEVDISKVLIC.Id == entity.Id)
               .Select(x => x.Id)
               .ToList()
               .ForEach(x => SMEVDISKVLICFileDomain.Delete(x));

                return Success();
            }
            catch (Exception e)
            {
                return Failure($"Ошибка интерцептора BeforeDeleteAction<SMEVDISKVLIC>: {e.Message}");
            }

        }
    }
}
