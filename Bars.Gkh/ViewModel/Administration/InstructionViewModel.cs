namespace Bars.Gkh.ViewModel
{
    using System.Linq;
    using Bars.B4;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Domain;
    using Bars.B4.Utils;

    /// <summary>
    /// Документация
    /// </summary>
    public class InstructionViewModel : BaseViewModel<Instruction>
    {
        public override IDataResult List(IDomainService<Instruction> domainService, BaseParams baseParams)
        {
            var loadParams = GetLoadParam(baseParams);
            var groupId= loadParams.Filter.GetAsId("instGroupId");
            var data = domainService.GetAll().Where(x => x.InstructionGroup.Id == groupId).Filter(loadParams, Container);
            return new ListDataResult(data.Order(loadParams).Paging(loadParams).ToList(), data.Count());
        }
    }
}