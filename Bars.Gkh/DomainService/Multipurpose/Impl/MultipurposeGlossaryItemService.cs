namespace Bars.Gkh.DomainService.Multipurpose.Impl
{
    using System.Linq;
    using Bars.B4;
    using Bars.B4.Utils;
    using Bars.Gkh.DomainService.Multipurpose;
    using Bars.Gkh.Entities.Dicts.Multipurpose;

    public class MultipurposeGlossaryItemService : IMultipurposeGlossaryItemService
    {
        public IDomainService<MultipurposeGlossaryItem> Service { get; set; } 

        public ListDataResult ListByGlossaryCode(BaseParams baseParams)
        {
            var code = baseParams.Params.GetAs<string>("glossaryCode");
            if (code.IsEmpty())
            {
                return new ListDataResult(new object[0], 0);
            }

            var data = this.Service.GetAll()
                .Where(x => x.Glossary.Code == code)
                .Where(x => x.Value != null && x.Value != "")
                .OrderBy(x => x.Id)
                .ToArray();

            return new ListDataResult(data, data.Count());
        }
    }
}