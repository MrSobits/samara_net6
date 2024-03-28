namespace Bars.Gkh.ViewModel
{
    using System.Linq;
    using Bars.B4;
    using Bars.Gkh.Entities.Dicts.Multipurpose;

    public class MultipurposeGlossaryItemViewModel : BaseViewModel<MultipurposeGlossaryItem>
    {
        public override IDataResult List(IDomainService<MultipurposeGlossaryItem> domainService, BaseParams baseParams)
        {
            var glossaryId = baseParams.Params.GetAs<long>("glossaryId");

            var list = domainService.GetAll().Where(x => x.Glossary.Id == glossaryId);

            return new ListDataResult(list, list.Count());
        }
    }
}