namespace Bars.GkhGji.Regions.Voronezh.ViewModel
{
    using Entities;
    using B4;
    using B4.Utils;
    using System;
    using Enums;
    using System.Linq;

    public class SMEVEGRNViewModel : BaseViewModel<SMEVEGRN>
    {
        public override IDataResult List(IDomainService<SMEVEGRN> domainService, BaseParams baseParams)
        {
            var loadParams = GetLoadParam(baseParams);

            var data = domainService.GetAll()
                .Select(x=> new
                {
                    x.Id,
                    ReqId = x.Id,
                    x.MessageId,
                    RequestDate = x.ObjectCreateDate,
                    Inspector = x.Inspector.Fio,
                    x.RequestState,
                    RequestType = x.RequestType,
                    Declarant = x.PersonSurname + " " + x.PersonName,
                    x.Answer
                })               
                .Filter(loadParams, Container);

            int totalCount = data.Count();

            return new ListDataResult(data.Order(loadParams).Paging(loadParams).ToList(), data.Count());
        }


        //public override IDataResult Get(IDomainService<SMEVEGRN> domain, BaseParams baseParams)
        //{

        //    var loadParams = baseParams.GetLoadParam();
        //    int id = Convert.ToInt32(baseParams.Params.Get("id"));

        //    var data = domain.GetAll()
        //         .WhereIf(id != 0, x => x.Id == id)
        //           .Select(x => new
        //           {
        //               x.Id,
        //               Inspector = x.Inspector.Fio,
        //               x.PersonName,
        //               x.PersonSurname,
        //               x.DeclarantId,
        //               x.EGRNApplicantType,
        //               x.IdDocumentRef,
        //               x.MessageId,
        //               x.RegionCode,
        //               x.RequestState,
        //               x.RequestType,
        //               x.Answer
        //           })
        //        .AsQueryable()
        //        .Filter(loadParams, Container);

        //    return new ListDataResult(data.Order(loadParams).Paging(loadParams).ToList(), data.Count());
        //}
    }
}
