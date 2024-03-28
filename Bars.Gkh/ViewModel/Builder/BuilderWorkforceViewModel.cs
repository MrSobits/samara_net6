namespace Bars.Gkh.ViewModel
{
    using System.Linq;

    using Bars.B4;
    using Bars.Gkh.Entities;

    public class BuilderWorkforceViewModel : BaseViewModel<BuilderWorkforce>
    {
        public override IDataResult List(IDomainService<BuilderWorkforce> domain, BaseParams baseParams)
        {
            var loadParams = GetLoadParam(baseParams);
            var builderId = baseParams.Params.GetAs<long>("builderId");

            var data = domain.GetAll()
                .Where(x => x.Builder.Id == builderId)
                .Select(x => new
                {
                    x.Id,
                    x.DocumentDate,
                    x.DocumentNum,
                    x.DocumentQualification,
                    x.EmploymentDate,
                    x.Fio,
                    x.Position,
                    SpecialtyName = x.Specialty.Name,
                    InstitutionsName = x.Institutions.Name,
                    x.File
                })
                .Filter(loadParams, Container);

            return new ListDataResult(data.Order(loadParams).Paging(loadParams).ToList(), data.Count());
        }
    }
}