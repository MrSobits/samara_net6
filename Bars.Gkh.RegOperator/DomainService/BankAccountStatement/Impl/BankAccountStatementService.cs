namespace Bars.Gkh.RegOperator.DomainService.Impl
{
    using B4;
    using B4.DataAccess;
    using B4.IoC;
    using B4.Modules.FileStorage;
    using Entities;
    using Entities.ValueObjects;
    using Enums;
    using Gkh.Domain;
    using Gkh.Utils;
    using System;
    using System.Linq;

    using Bars.B4.Utils;
    using Bars.Gkh.DomainEvent.Events;
    using Bars.Gkh.DomainEvent.Infrastructure;
    using Microsoft.AspNetCore.Mvc;
    using System.IO;
    using B4.Modules.DataExport;
    using Bars.B4.Modules.Reports;

    using Castle.MicroKernel;
    using Castle.Windsor;

    /// <summary>
    /// Сервис для банковской выписки
    /// </summary>
    public class BankAccountStatementService : IBankAccountStatementService
    {
        /// <summary>
        /// Контейнер.
        /// </summary>
        public IWindsorContainer Container { get; set; }

        /// <summary>
        /// Привязать документ.
        /// </summary>
        /// <param name="baseParams">Параметры запроса</param>
        /// <returns>Результат операции</returns>
        public IDataResult LinkDocument(BaseParams baseParams)
        {
            var statementId = baseParams.Params.GetAsId("statementId");
            var bankDocIds = baseParams.Params.GetAs<long[]>("docIds");

            var statementDomain = this.Container.ResolveDomain<BankAccountStatement>();
            var bankDocDomain = this.Container.ResolveDomain<BankDocumentImport>();
            var linkDomain = this.Container.ResolveDomain<BankAccountStatementLink>();

            try
            {
                var statement = statementDomain.Get(statementId);

                if (statement == null)
                {
                    return BaseDataResult.Error("Не удалось получить банковскую выписку");
                }

                if (linkDomain.GetAll().Any(x => x.Statement.Id == statementId))
                {
                    return BaseDataResult.Error("Банковская выписка уже имеет привязанные документы!");
                }

                var bankDocs = bankDocDomain.GetAll()
                    .Where(x => bankDocIds.Contains(x.Id))
                    .ToArray();

                var sum = bankDocs.Sum(x => x.ImportedSum) ?? 0m;

                if (sum != statement.Sum)
                {
                    return BaseDataResult.Error("Суммы не совпадают. Привязка не возможна.");
                }

                var statementInfo = string.Format("{0} от {1}", statement.DocumentNum, statement.DocumentDate.ToShortDateString());

                this.Container.InTransaction(() =>
                {
                    var linkedDocs =
                        bankDocs.AggregateWithSeparator(
                            x => string.Format("{0} от {1}", x.DocumentNumber, x.DocumentDate.ToDateString()), ", ");

                    foreach (var bankDoc in bankDocs)
                    {
                        linkDomain.Save(new BankAccountStatementLink(statement, bankDoc));

                        bankDoc.BankStatement = statementInfo;
                    }

                    var oldState = statement.DistributeState;

                    statement.LinkedDocuments = linkedDocs.Cut(2000);
                    statement.DistributeState = DistributionState.Distributed;
                    statementDomain.Save(statement);

                    DomainEvents.Raise(new GeneralStateChangeEvent(statement, oldState, DistributionState.Distributed));
                });
            }
            catch (ValidationException e)
            {
                return BaseDataResult.Error(e.Message);
            }
            finally
            {
                this.Container.Release(statementDomain);
                this.Container.Release(bankDocDomain);
                this.Container.Release(linkDomain);
            }

            return new BaseDataResult();
        }

        /// <summary>
        /// Показать детализацию.
        /// </summary>
        /// <param name="baseParams">Параметры запроса</param>
        /// <returns>Результат операции</returns>
        public IDataResult ListDetail(BaseParams baseParams)
        {
            var statementId = baseParams.Params.GetAsId("statementId");

            var statementDomain = this.Container.ResolveDomain<BankAccountStatement>();
            var detailDomain = this.Container.ResolveDomain<DistributionDetail>();

            using (this.Container.Using(statementDomain, detailDomain))
            {
                var statement = statementDomain.Get(statementId);

                if (statement.DistributeState != DistributionState.Distributed)
                {
                    return new ListDataResult();
                }

                var loadParams = baseParams.GetLoadParam();

                var details = detailDomain.GetAll()
                    .Where(x => x.EntityId == statementId)
                    .Where(x => x.Source == DistributionSource.BankStatement)
                    .Filter(loadParams, this.Container);

                return new ListDataResult(details.Order(loadParams).Paging(loadParams).ToList(), details.Count());
            }
        }

        /// <summary>
        /// Сохранение банковской выписки с файлом.
        /// </summary>
        /// <param name="baseParams">Параметры запроса</param>
        /// <returns>Результат выполнения</returns>
        public IDataResult SaveStatement(BaseParams baseParams)
        {
            var statementService = this.Container.Resolve<IDomainService<BankAccountStatement>>();
            var fileManager = this.Container.Resolve<IFileManager>();

            var saveParam = baseParams
                .Params
                .Read<SaveParam<BankAccountStatement>>()
                .Execute(container => B4.DomainService.BaseParams.Converter.ToSaveParam<BankAccountStatement>(container, true));
            try
            {
                foreach (var record in saveParam.Records)
                {
                    var value = record.AsObject();

                    var file = baseParams.Files.FirstOrDefault();

                    if (file.Value != null)
                    {
                        value.File = fileManager.SaveFile(file.Value);
                    }

                    if (value.Id == 0)
                    {
                        statementService.Save(value);
                    }
                    else
                    {
                        statementService.Update(value);
                    }
                }
            }
            catch (Exception exc)
            {
                return BaseDataResult.Error(exc.Message);
            }
            finally
            {
                this.Container.Release(statementService);
                this.Container.Release(fileManager);
            }

            return new BaseDataResult();
        }

        /// <summary>
        /// Список доступных Р/С для распределения
        /// </summary>
        /// <param name="baseParams">Базовые параметры запроса</param>
        /// <returns>Список Р/С</returns>
        public IDataResult ListAccountNumbers(BaseParams baseParams)
        {
            var calcAccountRoDomain = this.Container.ResolveDomain<CalcAccountRealityObject>();
            var calcAccountDomain = this.Container.ResolveDomain<CalcAccount>();
            var bankAccountStatementDomain = this.Container.ResolveDomain<BankAccountStatement>();

            var showDistributed = baseParams.Params.GetAs<bool>("showDistributed");
            var showCancelled = baseParams.Params.GetAs<bool>("showDeleted");

            var loadParams = baseParams.GetLoadParam();

            using (this.Container.Using(calcAccountRoDomain, calcAccountDomain, bankAccountStatementDomain))
            {
                // данный момент нужен, чтобы фильтр по Р/С учитывал фильтр по банковским операциям
                var bsParams = new BaseParams();
                bsParams.Params.Set("complexFilter", baseParams.Params.Get("complexFilterBs"));

                var statementsQuery = bankAccountStatementDomain.GetAll()
                    .WhereIf(!showDistributed, x => x.DistributeState != DistributionState.Distributed)
                    .WhereIf(!showCancelled, x => x.DistributeState != DistributionState.Deleted)
                    .Filter(bsParams.GetLoadParam(), this.Container);

                var query = calcAccountDomain.GetAll()
                    .Where(x => calcAccountRoDomain.GetAll().Any(y => y.Account == x))
                    .Where(x => statementsQuery.Any(y => y.RecipientAccountNum == x.AccountNumber))
                    .Where(x => x.AccountNumber != null && x.AccountNumber != string.Empty && x.TypeAccount == TypeCalcAccount.Special)
                    .Select(x => new
                    {
                        x.Id,
                        x.AccountNumber
                    })
                    .Filter(loadParams, this.Container);

                return new ListDataResult(query.Order(loadParams).Paging(loadParams).ToList(), query.Count());
            }
        }

        /// <summary>
        /// Экспорт реестра банковских операций
        /// </summary>
        /// <param name="viewModel">Вьюмодель</param>
        /// <param name="baseParams">параметры клиента</param>
        /// <returns></returns>
        public ActionResult Export(IViewModel<BankAccountStatement> viewModel, BaseParams baseParams)
        {
            var loadParams = baseParams.GetLoadParam();
   
            Stream excel = null;
            var statementDomain = this.Container.ResolveDomain<BankAccountStatement>();

            var DataTotalCount = viewModel.List(statementDomain, baseParams);

            var totalCount = ((ListDataResult)DataTotalCount).TotalCount;

            baseParams.Params["limit"] = totalCount;

            //для нормальной печати номер счета 
            if (!baseParams.Params.ContainsKey("addAnApostrophe"))
            {
                baseParams.Params.Add("addAnApostrophe", true);
            }

            var Data = viewModel.List(statementDomain, baseParams);

            try
            {
                // Вызывает метод выше несколько раз и грузит поток из него в excel фаил который потом попадает в поток zip
                for (int start = 0; start < totalCount; start ++)
                {
                    baseParams.Params["start"] = start;
                    excel = this.DataList(Data.Data, baseParams);
                    if (excel == null) break;
                }

                // сохраняем фаил потока в специальный тип zip архива 
                var result = new ReportStreamResult(excel, "export.xlsx");
                return result;
            }
            finally
            {
                this.Container.Release(statementDomain);
            }
        }

        #region Private Methods
        // Метод формирует поток со строками для экспорта
        private Stream DataList(object data, BaseParams baseParams)
        {
            var generator = this.Container.Resolve<B4.Modules.Reports.IReportGenerator>("XlsIoGenerator");

            var argument = new Arguments { { "Data", data }, { "BaseParams", baseParams } };
            var report = this.Container.Resolve<IDataExportReport>("DataExportReport.PrintForm",
                argument);

            var rp = new ReportParams();

            report.PrepareReport(rp);
            var template = report.GetTemplate();

       var result = new MemoryStream();

            generator.Open(template);
            generator.Generate(result, rp);
            result.Seek(0, SeekOrigin.Begin);
            return result;
        }
        #endregion
    }
}