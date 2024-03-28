namespace Bars.Gkh.Overhaul.Nso.ViewModel
{
    using System.Linq;

    using Bars.B4;
    using Bars.Gkh.DataResult;
    using Bars.Gkh.Overhaul.Nso.Entities;

    public class PublishedProgramRecordViewModel : BaseViewModel<PublishedProgramRecord>
    {
        public override IDataResult List(IDomainService<PublishedProgramRecord> domainService, BaseParams baseParams)
        {
            var loadParam = GetLoadParam(baseParams);

            // Поулчаем опубликованную программу по основной версии
            var data = domainService.GetAll()
                .Where(x => x.PublishedProgram.ProgramVersion.IsMain)
                .Select(x => new
                {
                    Municipality = x.Stage2.Stage3Version.RealityObject.Municipality.Name,
                    RealityObject = x.Stage2.Stage3Version.RealityObject.Address,
                    x.Sum,
                    x.CommonEstateobject,
                    x.PublishedYear,
                    x.IndexNumber
                })
                .OrderBy(x => x.IndexNumber)
                .ThenBy(x => x.PublishedYear)
                .Filter(loadParam, Container);

            var summary = data.AsEnumerable().Sum(x => x.Sum);

            return new ListSummaryResult(data.Order(loadParam).Paging(loadParam), data.Count(), new { Sum = summary });
        }
    }
}