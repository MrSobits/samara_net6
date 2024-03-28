namespace Bars.GkhGji.Regions.Smolensk.DomainService.Disposal.Impl
{
    using System.Linq;
    using Bars.B4;
    using Bars.B4.Utils;
    using Bars.GkhGji.Entities.Dict;
    using Bars.GkhGji.Regions.Smolensk.DomainService.Disposal;
    using Bars.GkhGji.Regions.Smolensk.Entities.Disposal;

    public class DisposalSurveySubjectService : IDisposalSurveySubjectService
    {
        public IDomainService<DisposalSurveySubject> Service { get; set; }

        public IDataResult AddDisposalSurveySubject(BaseParams baseParams)
        {
            var dispId = baseParams.Params.GetAs<long>("disposalId");
            var objectIds = baseParams.Params.GetAs<string>("objectIds");

            if (dispId == 0 || string.IsNullOrEmpty(objectIds))
            {
                return new BaseDataResult { Message = "Нет приказа или предметов проверки для сохранения", Success = false };
            }

            var existingIds = this.Service.GetAll().Where(x => x.Disposal.Id == dispId).Select(x => x.Id).ToList();

            foreach (var objectId in objectIds.Split(',').Select(x => x.Trim().ToLong()))
            {
                if (!existingIds.Contains(objectId))
                {
                    this.Service.Save(new DisposalSurveySubject { Disposal = new GkhGji.Entities.Disposal { Id = dispId }, SurveySubject = new SurveySubject { Id = objectId } });
                }
            }

            return new BaseDataResult();
        }
    }
}