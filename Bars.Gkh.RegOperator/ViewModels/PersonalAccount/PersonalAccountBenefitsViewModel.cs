namespace Bars.Gkh.RegOperator.ViewModels.PersonalAccount
{
    using System;
    using B4.DataAccess;
    using B4.Modules.Analytics.Utils;
    using Bars.B4;
    using Bars.Gkh.RegOperator.Entities;
    using DataResult;
    using Entities.PersonalAccount;
    using Gkh.Domain.CollectionExtensions;
	using System.Linq;
	using Bars.B4.Utils;
    using Bars.Gkh.Entities;

    /// <summary>
	/// Вьюмодель для Информация по начисленным льготам
	/// </summary>
	public class PersonalAccountBenefitsViewModel : BaseViewModel<PersonalAccountBenefits>
    {
		/// <summary>
		/// Получить список
		/// </summary>
		/// <param name="domainService">Домен</param>
		/// <param name="baseParams">Базовые параметры</param>
		/// <returns>Результат получения списка</returns>
		public override IDataResult List(IDomainService<PersonalAccountBenefits> domainService, BaseParams baseParams)
        {
            var periodIds = baseParams.Params.GetAs<string>("periodIds", ignoreCase: true)
                .ToLongArray()
                .Where(x => x != 0)
                .ToArray();

            var loadParams = this.GetLoadParam(baseParams);

            var chargeDomain = this.Container.ResolveDomain<ChargePeriod>();
            var persAccPrivCategoryDomain = this.Container.ResolveDomain<PersonalAccountPrivilegedCategory>();

            try
            {
                var data = domainService.GetAll()
                    .WhereIf(periodIds.Any(), x => periodIds.Contains(x.Period.Id))
                    .Select(x => new
                    {
						x.Id,
                        Period = x.Period.Name,
                        PersAccNum = x.PersonalAccount.PersonalAccountNum,
                        Owner = (x.PersonalAccount.AccountOwner as IndividualAccountOwner).Name ?? (x.PersonalAccount.AccountOwner as LegalAccountOwner).Contragent.Name,
                        Address = x.PersonalAccount.Room.RealityObject.Address + ", кв. " + x.PersonalAccount.Room.RoomNum,
                        x.Sum,
                        BenefitsName = persAccPrivCategoryDomain.GetAll()
                                            .Where(y => (!x.Period.EndDate.HasValue || y.DateFrom <= x.Period.EndDate) && (!y.DateTo.HasValue || y.DateTo >= x.Period.StartDate))
                                            .Where(y => y.PersonalAccount.Id == x.PersonalAccount.Id)
                                            .OrderByDescending(y => y.DateFrom)
                                            .Select(y => y.PrivilegedCategory.Name).FirstOrDefault(),
                        BenefitsDateStart = persAccPrivCategoryDomain.GetAll()
                                            .Where(y => (!x.Period.EndDate.HasValue || y.DateFrom <= x.Period.EndDate) && (!y.DateTo.HasValue || y.DateTo >= x.Period.StartDate))
                                            .Where(y => y.PersonalAccount.Id == x.PersonalAccount.Id)
                                            .OrderByDescending(y => y.DateFrom)
                                            .Select(y => (DateTime?)y.DateFrom).FirstOrDefault(),
                        BenefitsDateEnd = persAccPrivCategoryDomain.GetAll()
                                            .Where(y => (!x.Period.EndDate.HasValue || y.DateFrom <= x.Period.EndDate) && (!y.DateTo.HasValue || y.DateTo >= x.Period.StartDate))
                                            .Where(y => y.PersonalAccount.Id == x.PersonalAccount.Id)
                                            .OrderByDescending(y => y.DateFrom)
                                            .Select(y => y.DateTo).FirstOrDefault()
                    })
                    .Filter(loadParams, this.Container);

                var totalCount = data.Count();
                data = data.Order(loadParams).Paging(loadParams);

                return new ListSummaryResult(data.ToList(), totalCount, new
                {
                    Sum = data.SafeSum(x => x.Sum)
                });
            }
            finally
            {
                this.Container.Release(chargeDomain);
                this.Container.Release(persAccPrivCategoryDomain);
            }
        }
    }
}
