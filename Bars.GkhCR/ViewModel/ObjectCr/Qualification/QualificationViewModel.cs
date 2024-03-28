namespace Bars.GkhCr.DomainService
{
    using System.Linq;

    using Bars.B4;
    using Bars.B4.Utils;
    using Bars.Gkh.Utils;
    using Bars.GkhCr.Entities;
    using Bars.GkhCr.Enums;

    public class QualificationViewModel : BaseViewModel<Qualification>
    {
        public override IDataResult List(IDomainService<Qualification> domainService, BaseParams baseParams)
        {
            var loadParams = GetLoadParam(baseParams);

            var objectCrId = baseParams.Params.GetAs("objectCrId", 0);

            if (objectCrId == 0)
            {
                objectCrId = loadParams.Filter.GetAs("objectCrId", 0);
            }

            var voiceMemberDict = Container.Resolve<IDomainService<VoiceMember>>().GetAll()
                         .Where(x => x.Qualification.ObjectCr.Id == objectCrId)
                         .GroupBy(x => x.Qualification.Id)
                         .ToDictionary(x => x.Key, y => y
                             .GroupBy(x => x.QualificationMember.Name) 
                             .ToDictionary(x => x.Key, x => x.Select(z => z.TypeAcceptQualification).FirstOrDefault()));

            return domainService.GetAll()
                .Where(x => x.ObjectCr.Id == objectCrId)
                .Select(x => new
                    {
                        x.Id,
                        x.Sum,
                        Builder = new {x.Builder.Id, x.Builder.Contragent.Name}
                    })
                .AsEnumerable()
                .Select(x => 
                    {
                        var voiceMember = voiceMemberDict.Get(x.Id);

                        return new
                        {
                            x.Id,
                            BuilderName = x.Builder.Name,
                            x.Builder,
                            x.Sum,
                            Rating = string.Format("{0} из {1}",
                                    voiceMember?.Count(y => y.Value == TypeAcceptQualification.Yes) ?? 0,
                                    voiceMember?.Count ?? 0),
                            Dict = voiceMember
                        };

                    })
                .AsQueryable()
                .ToListDataResult(loadParams, this.Container);
        }

        public override IDataResult Get(IDomainService<Qualification> domainService, BaseParams baseParams)
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