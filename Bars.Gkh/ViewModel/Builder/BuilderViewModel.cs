namespace Bars.Gkh.ViewModel
{
    using System.Linq;
    using Bars.B4;
    using Bars.B4.Utils;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Enums;

    public class BuilderViewModel : BaseViewModel<Builder>
    {
        public override IDataResult List(IDomainService<Builder> domain, BaseParams baseParams)
        {
            var loadParams = GetLoadParam(baseParams);
            /*
             * Галочка над гридом показывать не действующие организации
             */
            var showNotValid = !baseParams.Params.ContainsKey("showNotValid") || baseParams.Params["showNotValid"].ToBool();

            var data = domain.GetAll()
                .WhereIf(!showNotValid, x => x.ActivityGroundsTermination == GroundsTermination.NotSet)
                .Select(x => new
                {
                    x.Id,
                    Municipality = x.Contragent.Municipality.Name,
                    ContragentName = x.Contragent.Name,
                    ContragentId = x.Contragent.Id,
                    x.Contragent.Inn,
                    x.Contragent.Kpp,
                    x.Contragent.Ogrn,
                    x.AdvancedTechnologies,
                    x.ConsentInfo,
                    x.WorkWithoutContractor,
                    x.Rating,
                    x.ActivityGroundsTermination,
                    x.File,
                    x.FileLearningPlan,
                    x.FileManningShedulle,
                    ContragentPhone = x.Contragent.Phone,
                })
                .OrderIf(loadParams.Order.Length == 0, true, x => x.Municipality)
                .Filter(loadParams, Container);

            return new ListDataResult(data.Order(loadParams).Paging(loadParams).ToList(), data.Count());
        }
    }
}