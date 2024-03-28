namespace Bars.Gkh.Overhaul.Tat.ViewModel
{
    using System.Linq;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.IoC;
    using Bars.B4.Utils;
    using Bars.Gkh.Utils;

    using Entities;

    public class VersionRecordViewModel : BaseViewModel<VersionRecord>
    {
        public override IDataResult List(IDomainService<VersionRecord> domainService, BaseParams baseParams)
        {
            var loadParams = this.GetLoadParam(baseParams);

            var version = loadParams.Filter.GetAs<long>("version");

            var municipalityId = loadParams.Filter.GetAs<long>("municipalityId");

            if (municipalityId == 0)
            {
                return new BaseDataResult(false, "Не задан параметр \"Муниципальное образование\"");   
            }

            if (loadParams.Order == null || loadParams.Order.Length == 0)
            {
                loadParams.Order = new[] { new OrderField { Asc = true, Name = "IndexNumber" } };
            }


            var stage1Domain = this.Container.ResolveDomain<VersionRecordStage1>();

            using (this.Container.Using(stage1Domain))
            {

                var structElDict = stage1Domain.GetAll()
                    .Where(x => x.Stage2Version.Stage3Version.ProgramVersion.Id == version)
                    .Select(
                        x => new
                        {
                            x.Stage2Version.Stage3Version.Id,
                            StructuralElement = x.StrElement.Name
                        })
                    .AsEnumerable()
                    .GroupBy(x => x.Id)
                    .ToDictionary(x => x.Key, y => y.AggregateWithSeparator(x => x.StructuralElement, ", "));


                var data =
                    domainService.GetAll()
                        .Where(x => x.ProgramVersion.Id == version && x.ProgramVersion.Municipality.Id == municipalityId)
                        .Select(x => new
                        {
                            x.Id,
                            Municipality = x.RealityObject.Municipality.Name,
                            RealityObject = x.RealityObject.Address,
                            x.CommonEstateObjects,
                            x.Year,
                            x.CorrectYear,
                            x.IndexNumber,
                            x.Point,
                            x.Sum,
                            x.TypeDpkrRecord
                        })
                        .AsEnumerable()
                        .Select(x => new
                        {
                            x.Id,
                            x.Municipality,
                            x.RealityObject,
                            x.CommonEstateObjects,
                            x.Year,
                            x.CorrectYear,
                            x.IndexNumber,
                            x.Point,
                            x.Sum,
                            x.TypeDpkrRecord,
                            StructuralElements = structElDict.Get(x.Id) ?? string.Empty
                        })
                        .AsQueryable()
                        .Filter(loadParams, this.Container);

                int totalCount = data.Count();

                return new ListDataResult(data.Order(loadParams).Paging(loadParams).ToList(), totalCount);

            }
        }
    }
}