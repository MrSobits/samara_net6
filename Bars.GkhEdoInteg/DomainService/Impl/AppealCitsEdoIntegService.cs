namespace Bars.GkhEdoInteg.DomainService.Impl
{
    using System;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.Utils;
    using Bars.GkhEdoInteg.Entities;
    using Bars.GkhGji.Contracts;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Enums;

    using Castle.Windsor;

    public class AppealCitsEdoIntegService : IAppealCitsEdoIntegService
    {
        public IWindsorContainer Container { get; set; }

        /// <summary>
        /// Получение документов
        /// </summary>
        /// <param name="baseParams"></param>
        /// <returns></returns>
        public IDataResult ListInspectionDocsAndAnswers(BaseParams baseParams)
        {
            var loadParam = baseParams.GetLoadParam();

            // Идентификатор обращения граждан
            var appealCitsId = baseParams.Params.ContainsKey("appealCitsId") ? baseParams.Params["appealCitsId"].ToLong() : 0;

            // Получаем основание Проверка по обращению граждан
            var idsStatement =
                Container.Resolve<IDomainService<InspectionAppealCits>>().GetAll()
                    .Where(x => x.AppealCits.Id == appealCitsId)
                    .Select(x => x.Inspection.Id)
                    .ToList();

            var docs = 
                Container.Resolve<IDomainService<DisposalAnnex>>().GetAll()
                     .Where(x => idsStatement.Contains(x.Disposal.Inspection.Id) && x.Disposal.TypeDocumentGji != TypeDocumentGji.ActRemoval)
                     .Select(x => new { x.Id, DocumentName = GetName(x.Disposal), Type = (int)x.Disposal.TypeDocumentGji })
                     .ToList();

            docs.AddRange(Container.Resolve<IDomainService<ActCheckAnnex>>()
                     .GetAll()
                     .Where(x => idsStatement.Contains(x.ActCheck.Inspection.Id) && x.ActCheck.TypeDocumentGji != TypeDocumentGji.ActRemoval)
                     .Select(x => new { x.Id, DocumentName = GetName(x.ActCheck), Type = (int)x.ActCheck.TypeDocumentGji })
                     .ToList());

            docs.AddRange(Container.Resolve<IDomainService<ActSurveyAnnex>>()
                     .GetAll()
                     .Where(x => idsStatement.Contains(x.ActSurvey.Inspection.Id) && x.ActSurvey.TypeDocumentGji != TypeDocumentGji.ActRemoval)
                     .Select(x => new { x.Id, DocumentName = GetName(x.ActSurvey), Type = (int)x.ActSurvey.TypeDocumentGji })
                     .ToList());
            docs.AddRange(Container.Resolve<IDomainService<ResolutionAnnex>>()
                     .GetAll()
                     .Where(x => idsStatement.Contains(x.Resolution.Inspection.Id) && x.Resolution.TypeDocumentGji != TypeDocumentGji.ActRemoval)
                     .Select(x => new { x.Id, DocumentName = GetName(x.Resolution), Type = (int)x.Resolution.TypeDocumentGji })
                     .ToList());
            docs.AddRange(Container.Resolve<IDomainService<PrescriptionAnnex>>()
                     .GetAll()
                     .Where(x => idsStatement.Contains(x.Prescription.Inspection.Id) && x.Prescription.TypeDocumentGji != TypeDocumentGji.ActRemoval)
                     .Select(x => new { x.Id, DocumentName = GetName(x.Prescription), Type = (int)x.Prescription.TypeDocumentGji })
                     .ToList());

            docs.AddRange(Container.Resolve<IDomainService<ProtocolAnnex>>()
                     .GetAll()
                     .Where(x => idsStatement.Contains(x.Protocol.Inspection.Id) && x.Protocol.TypeDocumentGji != TypeDocumentGji.ActRemoval)
                     .Select(x => new { x.Id, DocumentName = GetName(x.Protocol), Type = (int)x.Protocol.TypeDocumentGji })
                     .ToList());

            docs.AddRange(Container.Resolve<IDomainService<AppealCitsAnswer>>()
                     .GetAll()
                     .Where(x => x.AppealCits.Id == appealCitsId && x.File != null)
                     .Select(x => new { x.Id, DocumentName = string.Format("Ответ №{0}{1}", x.DocumentNumber, x.DocumentDate.HasValue ? " от " + x.DocumentDate.Value.ToShortDateString() : string.Empty), Type = 0 })
                     .ToList());

            var data = docs.AsQueryable().Filter(loadParam, Container);
            var totalCount = data.Count();
            return new ListDataResult(data.Order(loadParam).Paging(loadParam).ToList(), totalCount);
        }

        public IDataResult ListAppealCitsLog(BaseParams baseParams)
        {
            var loadParams = baseParams.GetLoadParam();
            var dateCreateStart = baseParams.Params.ContainsKey("dateCreateStart") ? baseParams.Params["dateCreateStart"].ToDateTime() : DateTime.MinValue;
            var dateCreateEnd = baseParams.Params.ContainsKey("dateCreateEnd") ? baseParams.Params["dateCreateEnd"].ToDateTime() : DateTime.MinValue;
            var dateActualStart = baseParams.Params.ContainsKey("dateActualStart") ? baseParams.Params["dateActualStart"].ToDateTime() : DateTime.MinValue;
            var dateActualEnd = baseParams.Params.ContainsKey("dateActualEnd") ? baseParams.Params["dateActualEnd"].ToDateTime() : DateTime.MinValue;

            if (dateCreateStart != DateTime.MinValue)
            {
                dateCreateStart = dateCreateStart.AddDays(-1);
            }

            if (dateCreateEnd != DateTime.MinValue)
            {
                dateCreateEnd = dateCreateEnd.AddDays(1);
            }

            if (dateActualStart != DateTime.MinValue)
            {
                dateActualStart = dateActualStart.AddDays(-1);
            }

            if (dateActualEnd != DateTime.MinValue)
            {
                dateActualEnd = dateActualEnd.AddDays(1);
            }

            var data = Container.Resolve<IDomainService<AppealCitsCompareEdo>>().GetAll()
                .WhereIf(dateCreateStart != DateTime.MinValue && dateCreateEnd != DateTime.MinValue, x => x.ObjectCreateDate > dateCreateStart && x.ObjectCreateDate < dateCreateEnd)
                .WhereIf(dateActualStart != DateTime.MinValue && dateActualEnd != DateTime.MinValue, x => x.DateActual > dateActualStart && x.DateActual < dateActualEnd)
               .Select(x => new
               {
                   x.AppealCits.Id,
                   x.IsEdo,
                   x.AppealCits.NumberGji,
                   DateFrom = x.ObjectCreateDate,
                   DateActual = x.DateActual != null ? x.DateActual.Value : DateTime.MinValue,
                   Document = (x.DateDocLoad != null && x.DateDocLoad.Value != DateTime.MinValue && x.AppealCits.File != null) || (x.DateDocLoad == null  && x.AppealCits.File != null) ? "Загружен" : x.IsDocEdo ? x.MsgLoadDoc : "Отсутствует"
               })
               .Order(loadParams)
               .Filter(loadParams, Container);

            var totalCount = data.Count();

            data = data.Order(loadParams).Paging(loadParams);
            return new ListDataResult(data.ToList(), totalCount);
        }

        public IDataResult ListLogRequests(BaseParams baseParams)
        {
            var loadParams = baseParams.GetLoadParam();
            var dateStart = baseParams.Params.GetAs("dateStart", DateTime.MinValue);
            var dateEnd = baseParams.Params.GetAs("dateEnd", DateTime.MinValue);

            if (dateEnd != DateTime.MinValue)
            {
                dateEnd = dateEnd.AddDays(1);
            }

            var data = Container.Resolve<IDomainService<LogRequests>>().GetAll()
                .WhereIf(dateStart != DateTime.MinValue, x => x.DateStart >= dateStart)
                .WhereIf(dateEnd != DateTime.MinValue, x => x.DateEnd < dateEnd)
               .Select(x => new
               {
                   x.Id,
                   x.DateStart,
                   x.DateEnd,
                   x.ObjectCreateDate,
                   x.ObjectEditDate,
                   TimeExecution = x.TimeExecution.HasValue ? x.TimeExecution.Value / 1000 : 0,
                   x.Count,
                   x.CountAdded,
                   x.CountUpdated,
                   x.Uri,
                   x.File
               })
               .Order(loadParams)
               .Filter(loadParams, Container);

            var totalCount = data.Count();

            data = data.Order(loadParams).Paging(loadParams);
            return new ListDataResult(data.ToList(), totalCount);
        }

        public IDataResult ListRequestsAppealCits(BaseParams baseParams)
        {
            var loadParams = baseParams.GetLoadParam();
            var logRequestsId = baseParams.Params.GetAs<long>("logRequestsId");

            var data = Container.Resolve<IDomainService<LogRequestsAppCitsEdo>>().GetAll()
                .WhereIf(logRequestsId != 0, x => x.LogRequests.Id == logRequestsId)
               .Select(x => new
               {
                   x.Id,
                   x.DateActual,
                   x.AppealCitsCompareEdo.AppealCits.NumberGji,
                   x.ActionIntegrationRow
               })
               .Order(loadParams)
               .Filter(loadParams, Container);

            return new ListDataResult(data.Order(loadParams).Paging(loadParams).ToList(), data.Count());
        }

        /// <summary>
        /// Получение наименования документа
        /// </summary>
        /// <param name="doc"></param>
        /// <returns></returns>
        private string GetName(DocumentGji doc)
        {
            switch (doc.TypeDocumentGji)
            {
                case TypeDocumentGji.Disposal:
                    {
                        var dispText = Container.Resolve<IDisposalText>();
                        return string.Format("Документ {0} {1}{2}", dispText.GenetiveCase.ToLower(), doc.DocumentNumber, doc.DocumentDate.HasValue ? " от " + doc.DocumentDate.Value.ToShortDateString() : string.Empty);
                    }

                case TypeDocumentGji.ActCheck:
                    return string.Format(
                        "Документ акта проверки {0}{1}",
                        doc.DocumentNumber,
                        doc.DocumentDate.HasValue ? " от " + doc.DocumentDate.Value.ToShortDateString() : string.Empty);

                case TypeDocumentGji.ActRemoval:
                    return string.Format(
                        "Документ акта устранения нарушений {0}{1}",
                        doc.DocumentNumber,
                        doc.DocumentDate.HasValue ? " от " + doc.DocumentDate.Value.ToShortDateString() : string.Empty);

                case TypeDocumentGji.ActSurvey:
                    return string.Format(
                        "Документ акта обследования {0}{1}",
                        doc.DocumentNumber,
                        doc.DocumentDate.HasValue ? " от " + doc.DocumentDate.Value.ToShortDateString() : string.Empty);

                case TypeDocumentGji.Resolution:
                    return string.Format(
                        "Документ постановление {0}{1}",
                        doc.DocumentNumber,
                        doc.DocumentDate.HasValue ? " от " + doc.DocumentDate.Value.ToShortDateString() : string.Empty);

                case TypeDocumentGji.Prescription:
                    return string.Format(
                        "Документ предписания {0}{1}",
                        doc.DocumentNumber,
                        doc.DocumentDate.HasValue ? " от " + doc.DocumentDate.Value.ToShortDateString() : string.Empty);

                case TypeDocumentGji.Protocol:
                    return string.Format(
                        "Документ протокола {0}{1}",
                        doc.DocumentNumber,
                        doc.DocumentDate.HasValue ? " от " + doc.DocumentDate.Value.ToShortDateString() : string.Empty);
            }

            return string.Format("Документ {0}{1}", doc.DocumentNumber, doc.DocumentDate.HasValue ? " от " + doc.DocumentDate.Value.ToShortDateString() : string.Empty);
        }
    }
}
