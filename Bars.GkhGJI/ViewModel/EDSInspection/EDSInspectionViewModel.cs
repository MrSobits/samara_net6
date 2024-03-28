namespace Bars.GkhGji.ViewModel
{
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.Utils;
    using Bars.Gkh.Authentification;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Utils;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Enums;

    public class EDSInspectionViewModel : BaseViewModel<EDSInspection>
    {
        public IGkhUserManager UserManager { get; set; }

        public IDomainService<OperatorContragent> OperatorContragentDomain { get; set; }
        public IDomainService<InspectionAppealCits> InspectionAppealCitsDomain { get; set; }
        public IDomainService<BaseLicenseApplicants> BaseLicenseApplicantsDomain { get; set; }
        public IDomainService<BaseDispHead> BaseDispHeadDomain { get; set; }
        public IDomainService<DocumentGjiInspector> DocumentGjiInspectorDomain { get; set; }
        public IDomainService<DocumentGji> DocumentGjiDomain { get; set; }

        public override IDataResult List(IDomainService<EDSInspection> domainService, BaseParams baseParams)
        {
            var loadParams = this.GetLoadParam(baseParams);
            Operator thisOperator = UserManager.GetActiveOperator();
            //Dictionary<long, string> disposalsDict = new Dictionary<long, string>();
            Dictionary<long, string> inspectorsDict = new Dictionary<long, string>();
         
            if (thisOperator?.Inspector == null)
            {
                var contragent = thisOperator.Contragent;
                var contragentList = OperatorContragentDomain.GetAll()
                 .Where(x => x.Contragent != null)
                 .Where(x => x.Operator == thisOperator)
                 .Select(x => x.Contragent.Id).Distinct().ToList();
                if (contragent != null)
                {
                    if (!contragentList.Contains(contragent.Id))
                    {
                        contragentList.Add(contragent.Id);
                    }
                }
                if (contragentList.Count == 0)
                {
                    return null;
                }
                var data = domainService.GetAll()
                             .Where(x => contragentList.Contains(x.Contragent.Id))
                                        .Select(x => new
                                        {
                                            x.Id,
                                            Contragent = x.Contragent.Name,
                                            ContragentINN = x.Contragent.Inn,
                                            x.InspectionNumber,
                                            x.InspectionDate,
                                            x.NotOpened,
                                            Disposals = Domain(domainService).ContainsKey(x.InspectionGji.Id) ? Domain(domainService)[x.InspectionGji.Id] : "",
                                            inspectors = Inspector(domainService).ContainsKey(x.InspectionGji.Id) ? Inspector(domainService)[x.InspectionGji.Id] : "",
                                            x.TypeBase,
                                            BaseDoc = GetBaseDocNumber(x.InspectionGji.Id, x.TypeBase).BaseDoc,
                                            DocNumber = GetBaseDocNumber(x.InspectionGji.Id, x.TypeBase).DocNumber,
                                        })
                                        .AsEnumerable()
                                        .Select(x=> new
                                        {
                                            x.Id,
                                            x.Contragent,
                                            x.ContragentINN,
                                            x.InspectionNumber,
                                            x.InspectionDate,
                                            x.NotOpened,
                                            x.Disposals,
                                            x.inspectors,
                                            x.TypeBase,
                                            x.BaseDoc,
                                            x.DocNumber
                                        }).AsQueryable()
                                        .Filter(loadParams, this.Container);

                int totalCount = data.Count();

                return new ListDataResult(data.Order(loadParams).Paging(loadParams).ToList(), totalCount);
            }
            else
            {
                var listEdsInsp = domainService.GetAll()
                    .Select(x => x.InspectionGji.Id).Distinct().ToList();
              
                DocumentGjiInspectorDomain.GetAll()
                   .Where(x => listEdsInsp.Contains(x.DocumentGji.Inspection.Id) && x.DocumentGji.TypeDocumentGji == Enums.TypeDocumentGji.Disposal)
                   .Select(x => new
                   {
                       InspId = x.DocumentGji.Inspection.Id,
                       Inspector = x.Inspector.Fio,
                   }).ToList()
                    .ForEach(x =>
                    {
                        if (inspectorsDict.ContainsKey(x.InspId))
                        {
                            if (!inspectorsDict[x.InspId].Contains(x.Inspector))
                                inspectorsDict[x.InspId] += "; " + x.Inspector;
                        }
                        else
                        {
                            inspectorsDict.Add(x.InspId, x.Inspector);
                        }
                    });
                var data = domainService.GetAll()
                                       .Select(x => new
                                       {
                                           x.Id,
                                           Contragent = x.Contragent.Name,
                                           ContragentINN = x.Contragent.Inn,
                                           x.InspectionNumber,
                                           x.InspectionDate,
                                           Disposals = Domain(domainService).ContainsKey(x.InspectionGji.Id) ? Domain(domainService)[x.InspectionGji.Id] : "",
                                           inspectors = Inspector(domainService).ContainsKey(x.InspectionGji.Id) ? Inspector(domainService)[x.InspectionGji.Id] : "",
                                           x.NotOpened,
                                           x.TypeBase,
                                           BaseDoc = GetBaseDocNumber(x.InspectionGji.Id, x.TypeBase).BaseDoc,
                                           DocNumber = GetBaseDocNumber(x.InspectionGji.Id, x.TypeBase).DocNumber,
                                       }).AsEnumerable()
                                        .Select(x => new
                                        {
                                            x.Id,
                                            x.Contragent,
                                            x.ContragentINN,
                                            x.InspectionNumber,
                                            x.InspectionDate,
                                            x.NotOpened,
                                            x.Disposals,
                                            x.inspectors,
                                            x.TypeBase,
                                            x.BaseDoc,
                                            x.DocNumber
                                        }).AsQueryable()
                                        .Filter(loadParams, this.Container)
                                        .OrderByDescending(x => x.NotOpened);

                int totalCount = data.Count();

                return new ListDataResult(data.Paging(loadParams).ToList(), totalCount);
            }
        }
        public override IDataResult Get(IDomainService<EDSInspection> domain, BaseParams baseParams)
        {
            var id = baseParams.Params.GetAs<long>("id");
            var obj = domain.Get(id);

            if (obj != null)
            {
                return new BaseDataResult(
                    new
                    {
                        obj.Id,
                        obj.Contragent,
                        obj.InspectionDate,
                        obj.InspectionGji,
                        obj.InspectionNumber,
                        obj.NotOpened,
                        obj.TypeBase,
                        obj.ObjectCreateDate,
                        obj.ObjectEditDate,
                        obj.ObjectVersion,
                        ContragentName = obj.Contragent.Name,
                        BaseDoc = GetBaseDocNumber(obj.InspectionGji.Id, obj.TypeBase).BaseDoc,
                        DocNumber = GetBaseDocNumber(obj.InspectionGji.Id, obj.TypeBase).DocNumber
                    });
            }

            return new BaseDataResult();
        }

        private Dictionary<long, string> Domain(IDomainService<EDSInspection> domainService)
        {
            Dictionary<long, string> disposalsDict = new Dictionary<long, string>();
            var listEdsInsp = domainService.GetAll()
                 .Select(x => x.InspectionGji.Id).Distinct().ToList();
            DocumentGjiDomain.GetAll()
                             .Where(x => listEdsInsp.Contains(x.Inspection.Id) && x.TypeDocumentGji == Enums.TypeDocumentGji.Disposal)
                             .Select(x => new
                             {
                                 InspId = x.Inspection.Id,
                                 DocumentNumber = x.DocumentNumber + " от " + (x.DocumentDate.HasValue ? x.DocumentDate.Value.ToString("dd.MM.yyyy") : ""),
                             }).ToList()
                             .ForEach(x =>
                             {
                                 if (disposalsDict.ContainsKey(x.InspId))
                                 {
                                     disposalsDict[x.InspId] += "; " + x.DocumentNumber;
                                 }
                                 else
                                 {
                                     disposalsDict.Add(x.InspId, x.DocumentNumber);
                                 }
                             });
            return disposalsDict;
        }


        private Dictionary<long, string> Inspector(IDomainService<EDSInspection> domainService)
        {
            Dictionary<long, string> inspectorsDict = new Dictionary<long, string>();
            var listEdsInsp = domainService.GetAll()
                 .Select(x => x.InspectionGji.Id).Distinct().ToList();

            DocumentGjiInspectorDomain.GetAll()
                .Where(x => listEdsInsp.Contains(x.DocumentGji.Inspection.Id) && x.DocumentGji.TypeDocumentGji == Enums.TypeDocumentGji.Disposal)
                .Select(x => new
                {
                    InspId = x.DocumentGji.Inspection.Id,
                    Inspector = x.Inspector.Fio,
                }).ToList()
                 .ForEach(x =>
                 {
                     if (inspectorsDict.ContainsKey(x.InspId))
                     {
                         if (!inspectorsDict[x.InspId].Contains(x.Inspector))
                             inspectorsDict[x.InspId] += "; " + x.Inspector;
                     }
                     else
                     {
                         inspectorsDict.Add(x.InspId, x.Inspector);
                     }
                 });
            return inspectorsDict;
        }

        private BaseDocProxy GetBaseDocNumber(long inspId, TypeBase typeBase)
        {
            switch (typeBase)
            {
                case TypeBase.CitizenStatement:
                    {
                        var appcits = InspectionAppealCitsDomain.GetAll()
                            .Where(x => x.Inspection.Id == inspId)
                            .Select(x => new
                            {
                                AppcitNum = x.AppealCits.NumberGji + " от " + x.AppealCits.DateFrom.Value.ToString("dd.MM.yyyy")
                            })
                            .AggregateWithSeparator(x => x.AppcitNum, ";");
                        var newBaseDocProxy = new BaseDocProxy 
                        { 
                            BaseDoc = "Обращение граждан",
                            DocNumber = appcits
                        };
                       
                        return newBaseDocProxy;                        
                    }
                case TypeBase.LicenseApplicants:
                    {
                        var licrequest = BaseLicenseApplicantsDomain.GetAll()
                            .FirstOrDefault(x => x.Id == inspId)?.ManOrgLicenseRequest;
                        if (licrequest != null)
                        {
                            var newBaseDocProxy = new BaseDocProxy
                            {
                                BaseDoc = "Обращение за выдачей лицензии",
                                DocNumber = $"{ licrequest.RegisterNumber } от {(licrequest.DateRequest.HasValue ? licrequest.DateRequest.Value.ToString("dd.MM.yyyy") : licrequest.ObjectCreateDate.ToString("dd.MM.yyyy"))}"
                                };

                            return newBaseDocProxy;
                           
                        }
                        break;
                    }
                case TypeBase.DisposalHead:
                    {
                        var disphead = BaseDispHeadDomain.GetAll()
                           .FirstOrDefault(x => x.Id == inspId);
                        if (disphead != null)
                        {
                            var newBaseDocProxy = new BaseDocProxy
                            {
                                BaseDoc = disphead.DocumentName,
                                DocNumber = $"{disphead.DocumentNumber} от {(disphead.DocumentDate.HasValue ? disphead.DocumentDate.Value.ToString("dd.MM.yyyy") : disphead.ObjectCreateDate.ToString("dd.MM.yyyy"))}"
                            };
                            return newBaseDocProxy;
                           
                        }
                        break;
                    }
            }
            return new BaseDocProxy
            {
                BaseDoc = "",
                DocNumber = ""
            };
        }

        private class BaseDocProxy
        {
            public string BaseDoc { get; set; }

            public string DocNumber { get; set; }
        }


    }
   
}