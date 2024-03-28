namespace Bars.GkhCr.DomainService
{
    using System.Linq;
    using B4;
    using Entities;

    public class QualificationMemberViewModel : BaseViewModel<QualificationMember>
    {
        public override IDataResult List(IDomainService<QualificationMember> domainService, BaseParams baseParams)
        {
            var loadParams = GetLoadParam(baseParams);

            //получаем словарь Ролей через запятую Key  - Id квалификационного отбора
            var dictRoles =
                this.Container.Resolve<IDomainService<QualificationMemberRole>>()
                    .GetAll()
                    .Select(x => new { MemberId = x.QualificationMember.Id, RoleName = x.Role.Name })
                    .AsEnumerable()
                    .GroupBy(x => x.MemberId)
                    .ToDictionary(
                        x => x.Key,
                        y => y.Select(x => x.RoleName).ToList().Aggregate((current, next) => current + ", " + next));

            var data = domainService.GetAll()
                .AsEnumerable()
                .Select(x => new
                {
                    x.Id,
                    x.IsPrimary,
                    x.Name,
                    x.Role,
                    RoleName = dictRoles.ContainsKey(x.Id) ? dictRoles[x.Id] : string.Empty,
                    PeriodName = x.Period.Name
                })
                .AsQueryable()
                .Filter(loadParams, this.Container);

            var totalCount = data.Count();

            data = data.Order(loadParams).Paging(loadParams);

            return new ListDataResult(data.ToList(), totalCount);
        }
    }
}
