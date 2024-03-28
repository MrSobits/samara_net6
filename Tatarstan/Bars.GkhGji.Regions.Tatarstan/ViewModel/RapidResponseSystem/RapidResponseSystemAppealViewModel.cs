namespace Bars.GkhGji.Regions.Tatarstan.ViewModel.RapidResponseSystem
{
    using System.Linq;

    using Bars.B4;
    using Bars.Gkh.Domain;
    using Bars.Gkh.Utils;
    using Bars.GkhGji.Regions.Tatarstan.Entities.RapidResponseSystem;

    /// <summary>
    /// View-модель для <see cref="RapidResponseSystemAppeal"/>
    /// </summary>
    public class RapidResponseSystemAppealViewModel : BaseViewModel<RapidResponseSystemAppeal>
    {
        /// <inheritdoc />
        public override IDataResult List(IDomainService<RapidResponseSystemAppeal> domainService, BaseParams baseParams)
        {
            var appealCitsId = baseParams.Params.GetAsId("appealCitizensId");

            return domainService.GetAll()
                .Where(x => x.AppealCits.Id == appealCitsId)
                .Select(x => new
                {
                    x.Id,
                    ContragentId = x.Contragent.Id,
                    ContragentName = x.Contragent.Name
                })
                .ToListDataResult(baseParams.GetLoadParam());
        }
    }
}