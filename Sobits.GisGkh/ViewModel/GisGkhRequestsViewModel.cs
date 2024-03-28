namespace Sobits.GisGkh.ViewModel
{
    using Entities;
    using System.Linq;
    using Bars.B4;
    using Sobits.GisGkh.Enums;
    using Bars.B4.DataAccess;
    using System.Collections.Generic;
    using Bars.Gkh.Authentification;

    public class GisGkhRequestsViewModel : BaseViewModel<GisGkhRequests>
    {
        public IGkhUserManager UserManager { get; set; }

        public override IDataResult List(IDomainService<GisGkhRequests> domain, BaseParams baseParams)
        {
            var loadParams = baseParams.GetLoadParam();
            var FileDomain = Container.ResolveDomain<GisGkhRequestsFile>();

            var gisContragent = this.UserManager.GetActiveOperator()?.GisGkhContragent;
            if (gisContragent != null)
            {

                Dictionary<long, long> reqFilesDict = FileDomain.GetAll().Where(x => x.GisGkhFileType == GisGkhFileType.request)
                    .GroupBy(x => x.GisGkhRequests.Id, x => x.FileInfo.Id).ToDictionary(x => x.Key, x => x.FirstOrDefault());
                Dictionary<long, long> respFilesDict = FileDomain.GetAll().Where(x => x.GisGkhFileType == GisGkhFileType.answer)
                    .GroupBy(x => x.GisGkhRequests.Id, x => x.FileInfo.Id).ToDictionary(x => x.Key, x => x.FirstOrDefault());

                var data = domain.GetAll()
                    .Where(x => x.Operator == null || x.Operator.GisGkhContragent == gisContragent)
                    .Select(x => new
                    {
                        x.Id,
                        x.MessageGUID,
                        x.RequesterMessageGUID,
                        x.ObjectCreateDate,
                        OperatorName = x.Operator != null ? x.Operator.User.Name : "Не определен",
                        x.RequestState,
                        x.TypeRequest,
                        x.Answer,
                        LogFile = x.LogFile != null ? (long?)x.LogFile.Id : null,
                }).AsEnumerable()
                    .Select(x => new
                    {
                        x.Id,
                        x.MessageGUID,
                        x.RequesterMessageGUID,
                        x.ObjectCreateDate,
                        x.OperatorName,
                        x.RequestState,
                        x.TypeRequest,
                        x.Answer,
                        x.LogFile,
                        ReqFile = reqFilesDict.ContainsKey(x.Id) ? (long?)reqFilesDict[x.Id] : null,
                        RespFile = respFilesDict.ContainsKey(x.Id) ? (long?)respFilesDict[x.Id] : null
                    })
                    .AsQueryable()
                    .Filter(loadParams, Container);
                return new ListDataResult(data.Order(loadParams).Paging(loadParams).ToList(), data.Count());
            }
            else
            {
                return new ListDataResult();
            }
        }
    }
}