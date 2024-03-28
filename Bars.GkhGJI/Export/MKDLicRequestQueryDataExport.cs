namespace Bars.GkhGji.DataExport
{
    using System;
    using System.Collections;
    using System.Linq;
    using System.Collections.Generic;
    using Bars.B4;
    using Bars.B4.Modules.DataExport.Domain;
    using Bars.B4.Utils;
    using Bars.GkhGji.DomainService;
    using Entities;
    using Bars.GkhGji.Entities;
    using Bars.Gkh.Utils;
    using Bars.Gkh.Authentification;
    using Bars.Gkh.Entities;
    using Bars.GkhGji.ViewModel;

    public class MKDLicRequestQueryDataExport : BaseDataExportService
    {
        public IGkhUserManager UserManager { get; set; }
        public IDomainService<MKDLicRequestQuery> MKDLicRequestQueryDomain { get; set; }
        public IDomainService<OperatorContragent> OperatorContragentDomain { get; set; }

        public override IList GetExportData(BaseParams baseParams)
        {
            var loadParams = baseParams.GetLoadParam();
            var MkdLicRequest = loadParams.Filter.GetAs("MkdLicRequest", 0L);
            DateTime? sendDatenull = null;
            if (MkdLicRequest > 0)
            {

                var data = MKDLicRequestQueryDomain.GetAll()
                    .Where(x => x.MKDLicRequest.Id == MkdLicRequest)
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
                    .Filter(loadParams, Container).ToList();

                return data;
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
                    var data = MKDLicRequestQueryDomain.GetAll()
                        .Where(x => x.DocumentDate.HasValue)
                        .Where(x => x.DocumentDate.Value >= dateStart2 && x.DocumentDate.Value <= dateEnd2)
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
                     .Filter(loadParams, Container).ToList();

                    return data;
                }
                else
                {
                    var data = MKDLicRequestQueryDomain.GetAll()
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
                  .Filter(loadParams, Container).ToList();

                    return data;
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