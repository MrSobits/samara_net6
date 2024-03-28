namespace Bars.GkhDi.DomainService.Dict.TemplateOtherService
{
    using System.Linq;

    using Bars.B4;
    using Bars.Gkh.Modules.GkhDi.Entities;
    using Bars.Gkh.Utils;

    public class TemplateOtherServiceViewModel : BaseViewModel<TemplateOtherService>
    {
        /// <inheritdoc />
        public override IDataResult List(IDomainService<TemplateOtherService> domainService, BaseParams baseParams)
        {
            return domainService.GetAll()
                                .Select(x => new
                                {
                                    x.Id,
                                    x.Code,
                                    x.Name,
                                    x.Characteristic,
                                    x.UnitMeasure,
                                    UnitMeasureName = x.UnitMeasure.Name
                                })
                                .ToListDataResult(this.GetLoadParam(baseParams), this.Container);
        }
    }
}

