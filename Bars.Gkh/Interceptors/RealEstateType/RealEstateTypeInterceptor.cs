namespace Bars.Gkh.Interceptors.RealEstateType
{
    using System;
    using System.Linq;
    using B4;
    using Bars.B4.Utils;
    using B4.DataAccess;
    using Entities.Dicts;
    using Entities.RealEstateType;

    /// <summary>
    /// Хук на операции с сущностью "Тип дома"
    /// </summary>
    public class RealEstateTypeInterceptor : EmptyDomainInterceptor<RealEstateType>
    {
        public IDomainService<RealEstateTypePriorityParam> PriorParamService { get; set; }

        public IDomainService<RealEstateTypeCommonParam> CommonParamService { get; set; }

        public IDomainService<RealEstateTypeStructElement> StructElemService { get; set; }

        public IDomainService<RealEstateTypeRate> RateService { get; set; }

        public IDomainService<RealEstateTypeRealityObject> realEstRealObjService { get; set; }

        public override IDataResult BeforeCreateAction(IDomainService<RealEstateType> service, RealEstateType entity)
        {

            return CheckRealEstateTypeForm(entity);
        }

        public override IDataResult BeforeUpdateAction(IDomainService<RealEstateType> service, RealEstateType entity)
        {
            return CheckRealEstateTypeForm(entity);
        }

        public override IDataResult BeforeDeleteAction(IDomainService<RealEstateType> service, RealEstateType entity)
        {
            using (var transaction = BeginTransaction())
            {
                try
                {
                    var commonParams = CommonParamService.GetAll().Where(x => x.RealEstateType.Id == entity.Id).Select(x => x.Id);
                    var structEls = StructElemService.GetAll().Where(x => x.RealEstateType.Id == entity.Id).Select(x => x.Id);
                    var priorParams = PriorParamService.GetAll().Where(x => x.RealEstateType.Id == entity.Id).Select(x => x.Id);
                    var rates = RateService.GetAll().Where(x => x.RealEstateType.Id == entity.Id).Select(x => x.Id);
                    var realEstRealObjs = realEstRealObjService.GetAll().Where(x => x.RealEstateType.Id == entity.Id).Select(x => x.Id);

                    foreach(var id in commonParams)
                    {
                        CommonParamService.Delete(id);
                    }
                    foreach(var id in structEls)
                    {
                        StructElemService.Delete(id);
                    }
                    foreach (var id in priorParams)
                    {
                        PriorParamService.Delete(id);
                    }
                    foreach (var id in rates)
                    {
                        RateService.Delete(id);
                    }
                    foreach (var id in realEstRealObjs)
                    {
                        realEstRealObjService.Delete(id);
                    }
                    transaction.Commit();
                }
                catch (Exception exc)
                {
                    try
                    {
                        transaction.Rollback();
                    }
                    catch (Exception e)
                    {
                        throw new Exception(
                            string.Format(
                                "Произошла не известная ошибка при откате транзакции: \r\nMessage: {0}; \r\nStackTrace:{1};",
                                e.Message,
                                e.StackTrace),
                            exc);
                    }

                    throw;
                }
                return base.BeforeDeleteAction(service, entity);
            }
        }


        public override IDataResult AfterCreateAction(IDomainService<RealEstateType> service, RealEstateType entity)
        {
            RateService.Save(new RealEstateTypeRate { RealEstateType = entity });

            return base.AfterCreateAction(service, entity);
        }


        private IDataTransaction BeginTransaction()
        {
            return Container.Resolve<IDataTransaction>();
        }

        private IDataResult CheckRealEstateTypeForm(RealEstateType entity)
        {
            if (entity.Code.IsNotEmpty() && entity.Code.Length > 100)
            {
                return Failure("Количество знаков в поле Код не должно превышать 100 символов");
            }

            if (entity.Name.IsEmpty())
            {
                return Failure("Не заполнены обязательные поля: Наименование");
            }

            if (entity.Name.Length > 300)
            {
                return Failure("Количество знаков в поле Наименование не должно превышать 300 символов");
            }

            return Success();
        }
    }
}
