namespace Bars.GkhCr.DomainService
{
    using System.Linq;

    using Bars.B4;
    using Bars.B4.Utils;
    using Bars.GkhCr.Entities;
    using Bars.GkhCr.Enums;

    public class SpecialQualificationViewModel : BaseViewModel<SpecialQualification>
    {
        public override IDataResult List(IDomainService<SpecialQualification> domainService, BaseParams baseParams)
        {
            var loadParams = this.GetLoadParam(baseParams);

            var objectCrId = baseParams.Params.GetAs("objectCrId", 0);

            if (objectCrId == 0)
            {
                objectCrId = loadParams.Filter.GetAs("objectCrId", 0);
            }

            var voiceMemberDict = this.Container.Resolve<IDomainService<SpecialVoiceMember>>().GetAll()
                         .Where(x => x.Qualification.ObjectCr.Id == objectCrId)
                         .GroupBy(x => x.Qualification.Id)
                         .ToDictionary(x => x.Key, y => y
                             .GroupBy(x => x.QualificationMember.Name) 
                             .ToDictionary(x => x.Key, x => x.Select(z => z.TypeAcceptQualification).FirstOrDefault()));

            var data = domainService.GetAll()
                .Where(x => x.ObjectCr.Id == objectCrId)
                .Select(x => new
                    {
                        x.Id,
                        x.Sum,
                        Builder = new {x.Builder.Id, x.Builder.Contragent.Name}
                    })
                .AsEnumerable()
                .Select(x => new
                    {
                        x.Id,
                        BuilderName = x.Builder.Name,
                        x.Builder,
                        x.Sum,
                        Rating = voiceMemberDict.ContainsKey(x.Id)
                            ? string.Format("{0} из {1}", voiceMemberDict[x.Id].Count(y => y.Value == TypeAcceptQualification.Yes), voiceMemberDict[x.Id].Count)
                            : "0 из 0",
                        Dict = voiceMemberDict.ContainsKey(x.Id) ? voiceMemberDict[x.Id] : null
                    })
                .AsQueryable()
                .Filter(loadParams, this.Container);

            var totalCount = data.Count();

            return new ListDataResult(data.Order(loadParams).Paging(loadParams).ToList(), totalCount);
        }

        public override IDataResult Get(IDomainService<SpecialQualification> domainService, BaseParams baseParams)
        {
            var obj = domainService.Get(baseParams.Params["id"].ToLong());

            return new BaseDataResult(new
                {
                    obj.Id,
                    obj.Sum,
                    Builder = new {obj.Builder.Id, ContragentName = obj.Builder.Contragent.Name}
                });
        }
    }
}