namespace Bars.Gkh.RegOperator.ViewModels
{
    using System.Linq;
    using B4;
    using B4.DataAccess;
    using B4.Utils;
    using Entities;
    using Gkh.Domain;
    using Overhaul.Entities;

    /// <summary>
    /// ViewModel Размер взноса на кр
    /// </summary>
    public class PaysizeViewModel : BaseViewModel<Paysize>
    {
        public override IDataResult Get(IDomainService<Paysize> domainService, BaseParams baseParams)
        {
            var obj = domainService.Get(baseParams.Params.GetAsId());

            var chargesDomain = this.Container.ResolveDomain<PersonalAccountCharge>();

            return new BaseDataResult(new
            {
                obj.Id,
                obj.DateEnd,
                obj.DateStart,
                obj.Indicator,
                HasCharges = chargesDomain.GetAll()
                    .Where(x => x.IsFixed)
                    .Where(x => x.ChargeDate >= obj.DateStart)
                    .WhereIf(obj.DateEnd.HasValue, x => x.ChargeDate <= obj.DateEnd)
                    .Any()
            });
        }
    }
}