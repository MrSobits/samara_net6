namespace Bars.GkhGji.ViewModel
{
    using System;
    using System.Linq;

    using B4;
    using Bars.Gkh.Authentification;
    using Bars.Gkh.Entities;
    using Entities;

    public class MKDLicRequestQueryViewModel : BaseViewModel<MKDLicRequestQuery>
    {
        public IGkhUserManager UserManager { get; set; }
        public IDomainService<OperatorContragent> OperatorContragentDomain { get; set; }

        public override IDataResult List(IDomainService<MKDLicRequestQuery> domainService, BaseParams baseParams)
        {
            var loadParams = baseParams.GetLoadParam();
            var MkdLicRequest = loadParams.Filter.GetAs("MkdLicRequest", 0L);
            DateTime? sendDatenull = null;
            if (MkdLicRequest > 0)
            {

                var data = domainService.GetAll()
                    .Where(x => x.MKDLicRequest.Id == MkdLicRequest)
                    .Select(x => new
                    {
                        x.Id,
                        x.PerfomanceDate,
                        x.PerfomanceFactDate,
                        StatementDate = x.MKDLicRequest.StatementDate.Date,
                        StatementNumber = x.MKDLicRequest.StatementNumber,
                        SendDate = x.SignedFile != null ? x.ObjectCreateDate.Date : sendDatenull,
                        Inspector = Inspector(x.MKDLicRequest.Id),
                        x.DocumentNumber,
                        x.DocumentDate,
                        CompetentOrg = x.CompetentOrg.Name,
                        x.SignedFile,
                        x.ObjectCreateDate
                    })
                    .Filter(loadParams, Container);

                int totalCount = data.Count();

                return new ListDataResult(data.Order(loadParams).ToList(), totalCount);
            }
            else
            {
                var dateStart2 = baseParams.Params.GetAs("dateStart", new DateTime());
                var dateEnd2 = baseParams.Params.GetAs("dateEnd", new DateTime());
                Operator thisOperator = UserManager.GetActiveOperator();
                if (thisOperator?.Inspector == null)
                {
                    var contragent = thisOperator.Contragent;
                    var contragentList = OperatorContragentDomain.GetAll()
                     .Where(x => x.Contragent != null)
                     .Where(x => x.Operator == thisOperator)
                     .Select(x => x.Contragent.Inn).Distinct().ToList();
                    if (contragent != null)
                    {
                        if (!contragentList.Contains(contragent.Inn))
                        {
                            contragentList.Add(contragent.Inn);
                        }
                    }
                    if (contragentList.Count == 0)
                    {
                        return null;
                    }
                    var data = domainService.GetAll()
                        .Where(x=> x.DocumentDate.HasValue)
                        .Where(x=> x.DocumentDate.Value >= dateStart2 && x.DocumentDate.Value <= dateEnd2)
                    //    .Where(x=> x.SignedFile != null)
                     .Where(x => x.CompetentOrg != null)
                     .Where(x => contragentList.Contains(x.CompetentOrg.Code))
                     .Select(x => new
                     {
                         x.Id,
                         x.PerfomanceDate,
                         x.PerfomanceFactDate,
                         StatementDate = x.MKDLicRequest.StatementDate,
                         StatementNumber = x.MKDLicRequest.StatementNumber,
                         SendDate = x.SignedFile != null ? x.ObjectCreateDate : sendDatenull,
                         Inspector = Inspector(x.MKDLicRequest.Id),
                         x.DocumentNumber,
                         x.DocumentDate,
                         CompetentOrg = x.CompetentOrg.Name,
                         x.SignedFile,
                         x.ObjectCreateDate
                     })
                     .Filter(loadParams, Container);

                    int totalCount = data.Count();

                    return new ListDataResult(data.Order(loadParams).Paging(loadParams).ToList(), totalCount);
                }
                else
                {
                    var data = domainService.GetAll()
                  .Select(x => new
                  {
                      x.Id,
                      x.PerfomanceDate,
                      x.PerfomanceFactDate,
                      StatementDate = x.MKDLicRequest.StatementDate,
                      StatementNumber = x.MKDLicRequest.StatementNumber,
                      SendDate = x.SignedFile != null ? x.ObjectCreateDate : sendDatenull,
                      Inspector = Inspector(x.MKDLicRequest.Id),
                      x.DocumentNumber,
                     // Inspector = x.MKDLicRequest.Inspector != null ? x.MKDLicRequest.Inspector.Fio : "",
                      x.DocumentDate,
                      CompetentOrg = x.CompetentOrg.Name,
                      x.SignedFile,
                      x.ObjectCreateDate
                  })
                  .Filter(loadParams, Container);

                    int totalCount = data.Count();

                    return new ListDataResult(data.Order(loadParams).Paging(loadParams).ToList(), totalCount);
                }
            }
        }

        private string Inspector(long mKDLicRequestId) 
        {
            var data = Container.Resolve<IDomainService<MKDLicRequestInspector>>().GetAll()
                .Where(x => x.MKDLicRequest.Id == mKDLicRequestId)
                .Select(x => x.Inspector.Fio).ToList();

            var stringData = string.Join("; ", data.ToArray());

            return stringData;
        }
    }
}