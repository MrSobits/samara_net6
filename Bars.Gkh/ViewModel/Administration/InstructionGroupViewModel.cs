namespace Bars.Gkh.ViewModel
{
    using System.Linq;
    using Bars.B4;
    using Bars.Gkh.Entities;
    using Bars.B4.Modules.Analytics.Extensions;
    using Bars.B4.Modules.Analytics.Reports.Extensions;

    /// <summary>
    /// Категория документации 
    /// </summary>
    public class InstructionGroupViewModel : BaseViewModel<InstructionGroup>
    {
        /// <summary>
        /// Категория с ролями
        /// </summary>
        public IDomainService<InstructionGroupRole> GroupRole { get; set; }

        public override IDataResult List(IDomainService<InstructionGroup> domainService, BaseParams baseParams)
        {
            var loadParams = GetLoadParam(baseParams);

            var data = domainService.GetAll().LeftJoin(GroupRole.GetAll(), ig => ig.Id, igr => igr.InstructionGroup.Id, (ig, igr) => new {Group = ig, Role = igr != null ? igr.Role.Name : null})
                .GroupBy(x => x.Group)
                .Select(x => new {x.Key.Id, x.Key.DisplayName, Role = x.Count() != x.Count(y => y.Role == null) ? x.AggregateWithSeparator(t => t.Role, ", ") : "Для всех ролей"})
                .AsQueryable().Filter(loadParams, Container);

            return new ListDataResult(data.Order(loadParams).Paging(loadParams).ToList(), data.Count());
        }
    }
}