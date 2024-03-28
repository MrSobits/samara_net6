namespace Bars.GkhCr.DomainService
{
    using System;
    using System.Linq;

    using B4;
    using B4.DomainService.BaseParams;
    using B4.Utils;
    using B4.IoC;

    using Bars.Gkh.Entities.Dicts;
    using Bars.Gkh.Utils;

    using Enums;

    using Gkh.DataResult;
    using Gkh.Entities;
    using Gkh.Authentification;
    using Entities;
    using Castle.Windsor;

	/// <summary>
	/// Сервиса для Акт выполненных работ
	/// </summary>
	public class PerformedWorkActService : IPerformedWorkActService
    {
	    private const string AcceptedGjiStatusName = "Принято ГЖИ";
	    private const string AcceptedTodkStatusName = "Принят ТОДК";

		/// <summary>
		/// Контейнер
		/// </summary>
		public IWindsorContainer Container { get; set; }

		/// <summary>
		/// Домен сервис для Работы
		/// </summary>
        public IDomainService<Work> WorkDomain { get; set; }

		/// <summary>
		/// Получить список актов по новым активным программам
		/// </summary>
		public IDataResult ListByActiveNewOpenPrograms(BaseParams baseParams)
        {
            var actDomain = this.Container.Resolve<IDomainService<PerformedWorkAct>>();
            var paymentDomain = this.Container.Resolve<IDomainService<PerformedWorkActPayment>>();

            var loadParams = baseParams.Params.Read<LoadParam>().Execute(Converter.ToLoadParam);

            using (this.Container.Using(actDomain, paymentDomain))
            {
                var notPaidActs = paymentDomain.GetAll()
                    .Where(x => x.Paid == 0)
                    .Select(x => x.PerformedWorkAct.Id)
                    .ToArray();

                var data = actDomain.GetAll()
                    .Where(x => x.ObjectCr.ProgramCr.TypeProgramStateCr == TypeProgramStateCr.Active
                        || x.ObjectCr.ProgramCr.TypeProgramStateCr == TypeProgramStateCr.Open
                        || x.ObjectCr.ProgramCr.TypeProgramStateCr == TypeProgramStateCr.New)
                    .Where(x => notPaidActs.Contains(x.Id))
                    .Select(x => new
                    {
                        x.Id,
                        TypeWorkCr = x.TypeWorkCr.Work.Name,
                        x.ObjectCr.RealityObject.Address,
                        x.Volume,
                        x.Sum,
                        State = x.State.Name
                    })
                    .Filter(loadParams, this.Container);

                var totalCount = data.Count();

                return new ListDataResult(data.Order(loadParams).Paging(loadParams).ToList(), totalCount);
            }
        }

		/// <summary>
		/// Получить список актов
		/// </summary>
		public IDataResult ListAct(BaseParams baseParams)
        {
            var loadParam = baseParams.Params.Read<LoadParam>().Execute(Converter.ToLoadParam);

            var realityObjId = baseParams.Params.GetAs<long>("realityObjId");
            var programFilterId = baseParams.Params.GetAs<long>("programFilterId");
            var municipalities = baseParams.Params.GetAs("municipalities", string.Empty);

            var municipalityIds = !string.IsNullOrEmpty(municipalities)
                ? municipalities.Split(';').Select(x => x.ToLong()).ToArray()
                : new long[0];

            var data = this.GetFilteredByOperator()
                .WhereIf(municipalityIds.Length > 0, x => municipalityIds.Contains(x.ObjectCr.RealityObject.Municipality.Id))
                .WhereIf(realityObjId > 0, x => x.ObjectCr.RealityObject.Id == realityObjId)
                .WhereIf(programFilterId > 0, x => x.ObjectCr.ProgramCr.Id == programFilterId)
                .Where(x => x.ObjectCr.ProgramCr.TypeVisibilityProgramCr == TypeVisibilityProgramCr.Full)
                .Select(x => new
                    {
                        x.Id,
                        ObjectCrId = x.ObjectCr.Id,
                        x.ObjectCr.RealityObject.Address,
                        x.Volume,
                        x.Sum,
                        x.State,
                        WorkName = x.TypeWorkCr.Work.Name,
                        Municipality = x.ObjectCr.RealityObject.Municipality.Name,
                        WorkFinanceSource = x.TypeWorkCr.FinanceSource.Name,
                        x.DocumentNum,
                        x.DateFrom,
                        RealityObject = x.ObjectCr.RealityObject.Id
                    })
                .Filter(loadParam, this.Container);

            var volume = data.Sum(x => x.Volume);
            var summary = data.Sum(x => x.Sum);
            var totalCount = data.Count();

            data = data
                .OrderIf(loadParam.Order.Length == 0, true, x => x.Municipality)
                .OrderThenIf(loadParam.Order.Length == 0, true, x => x.Address);

            var result = loadParam.Order.Length == 0 ? data.Paging(loadParam) : data.Order(loadParam).Paging(loadParam);

            return new ListSummaryResult(result.ToList(), totalCount, new { Sum = summary, Volume = volume });
        }

		/// <summary>
		/// Получить информацию по акту
		/// </summary>
		public IDataResult GetInfo(BaseParams baseParams)
        {
            var objectCrId = baseParams.Params.GetAs<long>("objectCrId");

            if (objectCrId > 0)
            {
                var objCr = this.Container.Resolve<IDomainService<Entities.ObjectCr>>().Get(objectCrId);

                var objCrProgram = objCr.RealityObject.Address + " (" + objCr.ProgramCr.Name + ")";

                return new BaseDataResult(new { objCrProgram });
            }

            return new BaseDataResult
                {
                    Success = false,
                    Message = "Не удалось получить объект кап.ремонта"
                };
        }

		/// <summary>
		/// Получить отфильтрованный запрос по оператору
		/// </summary>
		public IQueryable<PerformedWorkAct> GetFilteredByOperator()
        {
            var userManager = this.Container.Resolve<IGkhUserManager>();

            var municipalityIds = userManager.GetMunicipalityIds();
            var contragentIds = userManager.GetContragentIds();

            var serviceManOrgRealityObject = this.Container.Resolve<IDomainService<ManOrgContractRealityObject>>();

            return this.Container.Resolve<IDomainService<PerformedWorkAct>>().GetAll()
                .WhereIf(municipalityIds.Count > 0, x => municipalityIds.Contains(x.ObjectCr.RealityObject.Municipality.Id))
                .WhereIf(contragentIds.Count > 0, y => serviceManOrgRealityObject.GetAll().Any(
                    x => x.RealityObject.Id == y.ObjectCr.RealityObject.Id
                        && contragentIds.Contains(x.ManOrgContract.ManagingOrganization.Contragent.Id)
                        && x.ManOrgContract.StartDate <= DateTime.Today
                        && (!x.ManOrgContract.EndDate.HasValue || x.ManOrgContract.EndDate.Value >= DateTime.Today)));
        }

		/// <summary>
		/// Получить сводную информацию по актам
		/// </summary>
		public IDataResult ListDetails(BaseParams baseParams)
		{
			var loadParam = baseParams.Params.Read<LoadParam>().Execute(Converter.ToLoadParam);

			string workNameFilterValue = null;
			var workNameFilter = loadParam.FindInComplexFilter("WorkName");
			if (workNameFilter != null)
			{
				workNameFilterValue = workNameFilter.Value.ToStr().Trim().ToLower();
				loadParam.SetComplexFilterNull("WorkName");
			}

			var query = this.GetQueryForActDetails(baseParams)
				.WhereIf(!workNameFilterValue.IsEmpty(), x => x.TypeWorkCr.Work.Name.Trim().ToLower().Contains(workNameFilterValue));

			var totalCount = query.GroupBy(x => x.TypeWorkCr.Work.Id)
				.Select(x => x.Key)
				.AsEnumerable()
				.Count();

			var resultQuery = query
				.Select(x => new
				{
					ObjectCrId = x.ObjectCr.Id,
					WorkName = x.TypeWorkCr.Work.Name,
					PlanVolume = x.TypeWorkCr.Volume,
					PlanSum = x.TypeWorkCr.Sum,
					ActVolume = x.Volume,
					ActSum = x.Sum
				})
				.AsEnumerable()
				.GroupBy(x => x.WorkName)
				.Select(x => new
				{
					WorkName = x.Key,
					PlanVolume = x.GroupBy(y => y.ObjectCrId).Sum(y => y.Max(z => z.PlanVolume)),
					PlanSum = x.GroupBy(y => y.ObjectCrId).Sum(y => y.Max(z => z.PlanSum)),
					ActVolume = x.Sum(y => y.ActVolume),
					ActSum = x.Sum(y => y.ActSum)
				})
				.Select(x => new
				{
					x.WorkName, //Вид работы
					x.PlanVolume, //Плановый объем
					x.PlanSum, //Плановая сумма
					x.ActVolume, //Объем по принятым актам
					x.ActSum, //Сумма по принятым актам
					CompleteVolumePercent = x.PlanVolume == 0 ? 0 : (x.ActVolume / x.PlanVolume) * 100, //Процент выполненного объема
					UsedResourcesPercent = x.PlanSum == 0 ? 0 : (x.ActSum / x.PlanSum) * 100, //Процент использованных средств
					TypeWorkLimit = x.PlanSum - x.ActSum //Лимит по виду работы
				})
				.AsQueryable()
				.Filter(loadParam, this.Container)
				.Order(loadParam).Paging(loadParam);
			
			var result = resultQuery.ToArray();

			return new ListSummaryResult(
				result,
				totalCount,
				new
				{
					PlanVolume = result.Sum(x => x.PlanVolume),
					PlanSum = result.Sum(x => x.PlanSum),
					ActVolume = result.Sum(x => x.ActVolume),
					ActSum = result.Sum(x => x.ActSum),
					TypeWorkLimit = result.Sum(x => x.TypeWorkLimit),
					CompleteVolumePercent = result.Average(x => x.CompleteVolumePercent),
					UsedResourcesPercent = result.Average(x => x.UsedResourcesPercent)
				});
		}

		/// <summary>
		/// Проверить допустимые акты для сводной информации
		/// </summary>
		public IDataResult CheckActsForDetails(BaseParams baseParams)
	    {
		    var actStates = this.GetQueryForActDetails(baseParams)
				.Where(x => x.State != null)
				.GroupBy(x => x.State.Name)
			    .Select(x => new
			    {
				    Name = x.Key
			    })
			    .ToArray();

			if (!actStates.Any(x => x.Name == AcceptedGjiStatusName) && !actStates.Any(x => x.Name == AcceptedTodkStatusName))
			{
				return new BaseDataResult(
					false,
					string.Format(
						"В реестре нет актов выполненных работ на статусах \"{0}\", \"{1}\"",
						AcceptedGjiStatusName,
						AcceptedTodkStatusName));
			}
			if (!actStates.Any(x => x.Name == AcceptedGjiStatusName) && actStates.Any(x => x.Name == AcceptedTodkStatusName))
			{
				return new BaseDataResult(
					true,
					string.Format(
						"Нет актов выполненных работ со статусом \"{0}\". Информация будет показана только по актам на статусе \"{1}\"",
						AcceptedGjiStatusName,
						AcceptedTodkStatusName));
			}
			if (!actStates.Any(x => x.Name == AcceptedTodkStatusName) && actStates.Any(x => x.Name == AcceptedGjiStatusName))
			{
				return new BaseDataResult(
					true,
					string.Format(
						"Нет актов выполненных работ со статусом \"{0}\". Информация будет показана только по актам на статусе \"{1}\"",
						AcceptedTodkStatusName,
						AcceptedGjiStatusName));
			}
			
			return new BaseDataResult(true);
		}

	    private IQueryable<PerformedWorkAct> GetQueryForActDetails(BaseParams baseParams)
	    {
			var realityObjId = baseParams.Params.GetAs<long>("realityObjId");
			var programFilterId = baseParams.Params.GetAs<long>("programFilterId");
			var municipalities = baseParams.Params.GetAs("municipalities", string.Empty);
			var objectCrId = baseParams.Params.GetAs<long>("objectCrId");

			var municipalityIds = !string.IsNullOrEmpty(municipalities)
				? municipalities.Split(';').Select(x => x.ToLong()).ToArray()
				: new long[0];
			
			var query = this.GetFilteredByOperator()
				.WhereIf(municipalityIds.Length > 0, x => municipalityIds.Contains(x.ObjectCr.RealityObject.Municipality.Id))
				.WhereIf(realityObjId > 0, x => x.ObjectCr.RealityObject.Id == realityObjId)
				.WhereIf(programFilterId > 0, x => x.ObjectCr.ProgramCr.Id == programFilterId)
				.WhereIf(objectCrId > 0, x => x.ObjectCr.Id == objectCrId)
				.Where(x => x.ObjectCr.ProgramCr.TypeVisibilityProgramCr == TypeVisibilityProgramCr.Full)
				.Where(x => x.State.Name == AcceptedGjiStatusName || x.State.Name == AcceptedTodkStatusName)
				.Where(x => x.TypeWorkCr != null);

		    return query;
	    }
    }
}
