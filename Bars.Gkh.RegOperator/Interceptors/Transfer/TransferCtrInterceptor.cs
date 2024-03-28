namespace Bars.Gkh.RegOperator.Interceptors
{
    using System;
    using System.Globalization;
    using System.Linq;
    using B4.Utils;
    using B4;
    using B4.DataAccess;
    using B4.Modules.States;
    using Entities;
    using Enums;
    using Gkh.Utils;
    using System.Collections.Generic;
    using Bars.B4.Modules.FIAS;

    public class TransferCtrInterceptor : EmptyDomainInterceptor<TransferCtr>
    {
        public IDataResult CheckSumResource(IDomainService<TransferCtr> service, TransferCtr entity)
        {
            /*var sum = entity.FundResource.GetValueOrDefault()
                      + entity.BudgetMu.GetValueOrDefault()
                      + entity.OwnerResource.GetValueOrDefault()
                      + entity.BudgetSubject.GetValueOrDefault();

            // проверяем, что распределенных средств не больше, чем есть у объекта КР
            var resources =
                FinanceSourceResourceService.GetAll()
                    .Where(x => x.ObjectCr.Id == entity.ObjectCr.Id)
                    .GroupBy(x => x.FinanceSource.Id)
                    .Select(x => new
                    {
                        FinanceSourceId = x.Key,
                        BudgetMu = x.Sum(z => z.BudgetMu),
                        FundResource = x.Sum(z => z.FundResource),
                        OwnerResource = x.Sum(z => z.OwnerResource),
                        BudgetSubject = x.Sum(z => z.BudgetSubject)
                    })
                    .ToList();

            if (sum > 0 && !resources.Any())
            {
                return Failure("Для объекта капитального ремонта отсутствуют источники финансирования");
            }

            // Проверим, а хватит ли денег у объекта кр по источникам финансирования
            if (resources.Any() && sum > 0)
            {
                // имеющиеся заявки
                var allTransfers = service.GetAll()
                    .Where(x => x.ObjectCr.Id == entity.ObjectCr.Id)
                    .Where(x => entity.Id != x.Id)
                    .Select(x => new
                    {
                        //x.FinSource,
                        x.FundResource,
                        x.OwnerResource,
                        x.BudgetMu,
                        x.BudgetSubject
                    })
                    .ToList();

                // текущая заявка
                allTransfers.Add(new
                {
                    //entity.FinSource,
                    entity.FundResource,
                    entity.OwnerResource,
                    entity.BudgetMu,
                    entity.BudgetSubject
                });

                var errors = new List<string>();

                if (errors.Any())
                {
                    return Failure(
                            string.Format("Превышена сумма лимита финансирования капремонта объекта по источнику: {0}",
                                errors.Aggregate((x, y) => x + ',' + y)));
                }
            }*/

            return Success();
        }

        public override IDataResult BeforeUpdateAction(IDomainService<TransferCtr> service, TransferCtr entity)
        {
            CheckDocumentNum(service, entity);

            FormPurposeDescription(entity);

            return CheckSumResource(service, entity);
        }

        public override IDataResult BeforeCreateAction(IDomainService<TransferCtr> service, TransferCtr entity)
        {
            // Перед сохранением проставляем начальный статус
            var stateProvider = Container.Resolve<IStateProvider>();
            stateProvider.SetDefaultState(entity);

            //если не указали номер заявки, то генерить нужно
            if (entity.DocumentNum.IsEmpty())
            {
                // присваиваем номер заявки
                var num = service.GetAll()
                    .Where(
                        x =>
                            x.DateFrom.Value.Year ==
                            (entity.DateFrom.HasValue ? entity.DateFrom.Value.Year : DateTime.Now.Year))
                    .Select(x => x.DocumentNum)
                    .ToList()
                    .OrderByDescending(x => x.ToInt())
                    .FirstOrDefault();

                entity.DocumentNum = (num.ToInt() + 1).ToString(CultureInfo.InvariantCulture);
            }
            else
            {
                CheckDocumentNum(service, entity);
            }

            FormPurposeDescription(entity);

            return CheckSumResource(service, entity);
        }

        private void CheckDocumentNum(IDomainService<TransferCtr> service, TransferCtr entity)
        {
            if (entity.DocumentNum != null && entity.DateFrom != null)
            {
                var existsArr = service.GetAll()
                    .Where(v => entity.Id != v.Id && v.DocumentNum == entity.DocumentNum && v.DateFrom != null)
                    .Select(v => v.DateFrom)
                    .ToArray();

                var exists = existsArr.Any(v => v.Value.Year == entity.DateFrom.Value.Year);

                if (exists)
                {
                    throw new ValidationException(
                        "Номер заявки присутствует в реестре. Сохранение заявки не доступно. Измените номер заявки и повторите сохранение.");
                }

            }
        }

        private void FormPurposeDescription(TransferCtr entity)
        {
            if (!entity.IsEditPurpose)
            {
                var localCodeDomain = Container.ResolveDomain<LocationCode>();
                var fiasContainer = Container.ResolveDomain<Fias>();

                try
                {
                    var placeGuid = entity.ObjectCr.RealityObject.FiasAddress.PlaceGuidId;
                    Func<string, IEnumerable<Fias>> getFiasParents = null;
                    getFiasParents = guid => fiasContainer.GetAll().Where(x => x.AOGuid == guid).ToList().SelectMany(x => getFiasParents(x.ParentGuid).Concat(new[] { x }));
                    var fiasParents = getFiasParents(placeGuid).Select(x => x.AOGuid);
                    
                    var locCode =
                        localCodeDomain.GetAll()
                            .Where(
                                x => fiasParents.Contains(x.FiasLevel1.FiasId))
                            .Select(x => x.CodeLevel1)
                            .FirstOrDefault();

                    var nds = string.Empty;
                    switch (entity.TypeCalculationNds)
                    {
                        case TypeCalculationNds.WithoutNds:
                            nds = "НДС не облагается";
                            break;
                        case TypeCalculationNds.TenPercent:
                            nds = "В том числе НДС (10%) {0}".FormatUsing((entity.Sum * 10 / 110).RoundDecimal(2));
                            break;
                        case TypeCalculationNds.EighteenPercent:
                            nds = "В том числе НДС (18%) {0}".FormatUsing((entity.Sum * 18 / 118).RoundDecimal(2));
                            break;
                    }

                    entity.PaymentPurposeDescription =
                        @"{0} по дог. № {1} от {2} : {3}; {4}; {5}; {6}, {7}"
                            .FormatUsing(entity.PaymentType.GetEnumMeta().Display,
                                entity.Contract.DocumentNum,
                                entity.Contract.DocumentDateFrom.ToDateString(),
                                entity.TypeWorkCr.Work.Name,
                                entity.ObjectCr.RealityObject.Address,
                                entity.FinSource.Return(x => x.Name),
                                locCode,
                                nds);

                }
                finally
                {
                    Container.Release(localCodeDomain);
                    Container.Release(fiasContainer);
                }
            }
        }
    }
}
