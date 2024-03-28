namespace Bars.GkhDi.DomainService
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using B4;
    using B4.Utils;
    using Bars.GkhDi.Enums;

    using Entities;

	/// <summary>
	/// ViewModel для сущности "Коммунальные услуги"
	/// </summary>
	public class FinActivityCommunalViewModel : BaseViewModel<FinActivityCommunalService>
    {
		/// <summary>
		/// Домен сервис для сущности "Деятельность управляющей организации в периоде раскрытия информации"
		/// </summary>
		public virtual IDomainService<DisclosureInfo> DiDomain { get; set; }

		/// <summary>
		/// Получить список
		/// </summary>
		/// <param name="domainService">Домен сервис</param>
		/// <param name="baseParams">Базовые параметры</param>
		/// <returns>Результат выполнения запроса</returns>
		public override IDataResult List(IDomainService<FinActivityCommunalService> domainService, BaseParams baseParams)
        {
            var loadParams = GetLoadParam(baseParams);

            var disclosureInfoId = baseParams.Params.GetAs<long>("disclosureInfoId");

            var di = DiDomain.Get(disclosureInfoId);

            if(di == null)
            {
                return new BaseDataResult(false, string.Format("Не удалось по идентификатору {0} получить объект раскрытия информации", disclosureInfoId));
            }

            var year = DateTime.Now.Year;

            if(di.PeriodDi != null && di.PeriodDi.DateStart.HasValue)
            {
                year = di.PeriodDi.DateStart.Value.Year;
            }

            // группируем по категории, т.к. по одной категории не может быть 2 значений
            var dataFinActivityCommunalService = domainService.GetAll()
                .Where(x => x.DisclosureInfo.Id == disclosureInfoId)
                .AsEnumerable()
                .GroupBy(x => x.TypeServiceDi)
                .ToDictionary(x => x.Key, y => y.First());

            var dataNewFinActivityCommunalService = new List<FinActivityCommunalService>();

            foreach (TypeServiceDi type in Enum.GetValues(typeof(TypeServiceDi)))
            {

                if(year < 2015 && (type == TypeServiceDi.OtherSource || 
					type == TypeServiceDi.ThermalEnergyForHeating || 
					type == TypeServiceDi.ThermalEnergyForNeedsOfHotWater))
                {
					//Тепловая энергия для нужд отопления, тепловая энергия для нужд горячего водоснабжения и
					//и прочие услуги показываются только с 2015 года 
					continue;
                }

                var record = dataFinActivityCommunalService.Get(type);

                var finActivityCommunalService = new FinActivityCommunalService { Id = (int)type, TypeServiceDi = type };
                if (record != null)
                {
                    finActivityCommunalService.DisclosureInfo = record.DisclosureInfo;
                    finActivityCommunalService.Exact = record.Exact;
                    finActivityCommunalService.IncomeFromProviding = record.IncomeFromProviding;
                    finActivityCommunalService.DebtPopulationStart = record.DebtPopulationStart;
                    finActivityCommunalService.DebtPopulationEnd = record.DebtPopulationEnd;
                    finActivityCommunalService.DebtManOrgCommunalService = record.DebtManOrgCommunalService;
                    finActivityCommunalService.PaidByMeteringDevice = record.PaidByMeteringDevice;
                    finActivityCommunalService.PaidByGeneralNeeds = record.PaidByGeneralNeeds;
                    finActivityCommunalService.PaymentByClaim = record.PaymentByClaim;
                }

                dataNewFinActivityCommunalService.Add(finActivityCommunalService);
            }

            var totalCount = dataNewFinActivityCommunalService.AsQueryable().Count();
            var data = dataNewFinActivityCommunalService.AsQueryable().Order(loadParams).Paging(loadParams);

            return new ListDataResult(data.ToList(), totalCount);
        }
    }
}