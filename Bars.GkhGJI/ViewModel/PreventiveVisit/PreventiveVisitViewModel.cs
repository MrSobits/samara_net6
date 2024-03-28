namespace Bars.GkhGji.ViewModel
{
    using System;
    using System.Linq;
    using System.Collections.Generic;
    using B4;

    using Bars.B4.Utils;

    using Entities;
    using Bars.Gkh.Authentification;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Utils;

    public class PreventiveVisitViewModel : BaseViewModel<PreventiveVisit>
    {
        public IGkhUserManager UserManager { get; set; }
        public IDomainService<DocumentGjiInspector> PreventiveVisitInspectorDomain { get; set; }
        public IDomainService<OperatorContragent> OperatorContragentDomain { get; set; }
        public override IDataResult List(IDomainService<PreventiveVisit> domainService, BaseParams baseParams)
        {
            var loadParams = GetLoadParam(baseParams);

            var dateStart = baseParams.Params.GetAs<DateTime>("dateStart");
            var dateEnd = baseParams.Params.GetAs<DateTime>("dateEnd");
            Dictionary<long, string> dict = new Dictionary<long, string>();
            PreventiveVisitInspectorDomain.GetAll()
                .Where(x => x.DocumentGji.TypeDocumentGji == Enums.TypeDocumentGji.PreventiveVisit)
                .Where(x => (x.DocumentGji.DocumentDate.HasValue && x.DocumentGji.DocumentDate.Value >= dateStart && x.DocumentGji.DocumentDate.Value <= dateEnd) || !x.DocumentGji.DocumentDate.HasValue)
                .Select(x => new
                {
                    x.DocumentGji.Id,
                    x.Inspector.Fio
                }).ToList().ForEach(x =>
                {
                    if (!dict.ContainsKey(x.Id))
                    {
                        dict.Add(x.Id, x.Fio);
                    }
                    else
                    {
                        dict[x.Id] += "; "+x.Fio;
                    }
                });           

                  

            Operator thisOperator = UserManager.GetActiveOperator();
            if (thisOperator?.Inspector == null)
            {                 

                return new ListDataResult();
            }
            else
            {
                var data = domainService.GetAll()
                        .Where(x => (x.DocumentDate.HasValue && x.DocumentDate.Value >= dateStart && x.DocumentDate.Value <= dateEnd)|| !x.DocumentDate.HasValue)
                        .Select(x => new
                        {
                            x.Id,
                            x.State,
                            x.DocumentDate,
                            x.TypeDocumentGji,
                            x.Inspection.TypeBase,
                            InspectionId = x.Inspection.Id,
                            x.TypePreventiveAct,
                            Contragent = x.Contragent != null?x.Contragent.Name:"",
                            x.KindKND,
                            InspectorNames = dict.ContainsKey(x.Id) ? dict[x.Id] : "",
                            x.DocumentNumber,
                            x.PersonInspection,
                            x.PhysicalPerson
                        }).Filter(loadParams, Container);

                int totalCount = data.Count();

                return new ListDataResult(data.Order(loadParams).Paging(loadParams).ToList(), data.Count());
            }
        }
    }
}