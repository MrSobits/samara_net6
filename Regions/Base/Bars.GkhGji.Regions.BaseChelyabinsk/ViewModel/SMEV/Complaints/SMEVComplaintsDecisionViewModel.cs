namespace Bars.GkhGji.Regions.BaseChelyabinsk.ViewModel
{
    using Entities;
    using B4;
    using B4.Utils;
    using System.Linq;
    using System;
    using Castle.Core.Internal;
    using Bars.Gkh.DocIoGenerator;
    using System.Collections.Generic;

    public class SMEVComplaintsDecisionViewModel : BaseViewModel<SMEVComplaintsDecision>
    {
        public IDomainService<SMEVComplaintsDecisionLifeSituation> SMEVComplaintsDecisionLifeSituationDomain { get; set; }
        public override IDataResult List(IDomainService<SMEVComplaintsDecision> domain, BaseParams baseParams)
        {
            var loadParams = baseParams.GetLoadParam();
            var lsText = baseParams.Params.GetAs<string>("lsText","");
            //var isFiltered = loadParams.Filter.GetAs("isFiltered", false);
            if (!string.IsNullOrEmpty(lsText))
            {
                var decByLSIds = GetDecisions(lsText);
                if (decByLSIds.Count > 0)
                {
                    var data = domain.GetAll()
                        .Where(x => decByLSIds.Contains(x.Id))
                   .AsQueryable()
                   .Filter(loadParams, Container);
                    return new ListDataResult(data.Order(loadParams).Paging(loadParams).ToList(), data.Count());
                }
                else
                {
                    var data = domain.GetAll()
                    .AsQueryable()
                    .Filter(loadParams, Container);
                    return new ListDataResult(data.Order(loadParams).Paging(loadParams).ToList(), data.Count());
                }
            }
            else
            {
                var data = domain.GetAll()      
                .AsQueryable()
                .Filter(loadParams, Container);
                return new ListDataResult(data.Order(loadParams).Paging(loadParams).ToList(), data.Count());
            }        
        }
        private List<long> GetDecisions(string lstext)
        {
            var list = new List<long>();
            var lscode = lstext.Split(null)[0];
            if (!string.IsNullOrEmpty(lscode))
            {
                list.AddRange(SMEVComplaintsDecisionLifeSituationDomain.GetAll()
                    .Where(x=> x.Code == lscode)
                    .Select(x=> x.SMEVComplaintsDecision.Id).Distinct().ToList());
            }
            return list;
        }
    }
}