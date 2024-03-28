namespace Bars.GkhCr.Report
{
	using System.Collections.Generic;
	using System.Linq;
	using B4;
	using B4.Modules.Reports;
	using B4.Utils;

	using Bars.B4.Modules.Analytics.Reports.Enums;

	using Entities;
	using Gkh.Entities;
	using Gkh.Modules.RegOperator.Entities.RegOperator;
	using Gkh.Report;
	using Gkh.Utils;
	using GkhCr.Properties;

	/// <summary>
	/// Печатка договора подряда
	/// </summary>
	public class BuildContractCrReport : GkhBaseStimulReport
    {
		/// <summary>
		/// Конструктор
		/// </summary>
        public BuildContractCrReport() : base(new ReportTemplateBinary(Resources.BuildContractCr))
        {
        }

		/// <summary>
		/// Домен сервис объекта КР
		/// </summary>
		public IDomainService<ObjectCr> ObjectCrDomain { get; set; } 

		/// <summary>
		/// Домен сервис договора подряда
		/// </summary>
		public IDomainService<BuildContract> BuildContractDomain { get; set; } 

		/// <summary>
		/// Домен сервис регионального оператора
		/// </summary>
		public IDomainService<RegOperator> RegOperatorDomain { get; set; } 

		/// <summary>
		/// Домен сервис контакта контргагента
		/// </summary>
		public IDomainService<ContragentContact> ContragentContactDomain { get; set; } 

		/// <summary>
		/// Домен сервис вида работ договора подряда
		/// </summary>
		public IDomainService<BuildContractTypeWork> BuildContractTypeWorkDomain { get; set; } 

		/// <summary>
		/// Домен сервис вида работ объекта КР
		/// </summary>
		public IDomainService<TypeWorkCr> TypeWorkCrDomain { get; set; }

        /// <summary>
        /// Домен сервис вида конкурса лота тип работы
        /// </summary>
        public IDomainService<CompetitionLotTypeWork> CompetitionLotTypeWorkDomain { get; set; }

        /// <summary>
        /// Домен сервис вида заявка лота
        /// </summary>
        public IDomainService<CompetitionLotBid> CompetitionLotBidDomain { get; set; }


        /// <summary>
        /// Идентификатор отчета
        /// </summary>
        public override string Id
        {
            get { return "BuildContractCr"; }
        }

		/// <summary>
		/// Код отчета
		/// </summary>
        public override string CodeForm
        {
            get { return "BuildContractCr"; }
        }

		/// <summary>
		/// Наименование
		/// </summary>
        public override string Name
        {
            get { return "Договор объекта кап.ремонта"; }
        }

		/// <summary>
		/// Описание
		/// </summary>
        public override string Description
        {
            get { return "Договор объекта кап.ремонта"; }
        }

		/// <summary>
		/// Код шаблона
		/// </summary>
        protected override string CodeTemplate { get; set; }

		/// <summary>
		/// Идентификатор договора подряда
		/// </summary>
		protected long BuildContractId;

		/// <summary>
		/// Установить пользовательские параметры отчета
		/// </summary>
		/// <param name="userParamsValues">Параметры</param>
		public override void SetUserParams(UserParamsValues userParamsValues)
        {
			BuildContractId = userParamsValues.GetValue<long>("BuildContractId");
		}

        /// <summary>Формат печатной формы</summary>
        public override StiExportFormat ExportFormat
        {
            get { return StiExportFormat.Word2007; }
        }

		/// <summary>
		/// Получить информацию о шаблонах
		/// </summary>
		/// <returns>Информация о шаблонах</returns>
        public override List<TemplateInfo> GetTemplateInfo()
        {
            return new List<TemplateInfo>
            {
                new TemplateInfo
                {
                    Code = "BuildContractCr",
                    Description = "Договор объекта кап.ремонта",
                    Name = "BuildContractCr",
                    Template = Resources.BuildContractCr
				}
            };
        }

		/// <summary>
		/// Подготовить отчет
		/// </summary>
		/// <param name="reportParams">Параметры отчета</param>
		public override void PrepareReport(ReportParams reportParams)
		{
			var buildContract = BuildContractDomain.Get(BuildContractId);
			if (buildContract == null)
			{
				throw new ValidationException("Не удалось получить договор подряда");
			}

			var objectCr = buildContract.ObjectCr;

			this.ReportParams["АдресДома"] = string.Format("{0}, {1}, {2}, {3}, {4}, {5}, {6}",
				objectCr.RealityObject.Municipality.Name,
				objectCr.RealityObject.FiasAddress.PlaceName,
				objectCr.RealityObject.FiasAddress.StreetName,
				objectCr.RealityObject.FiasAddress.House,
				objectCr.RealityObject.FiasAddress.Letter,
				objectCr.RealityObject.FiasAddress.Housing,
				objectCr.RealityObject.FiasAddress.Building);

			this.ReportParams["НомерДома"] = string.Format("{0}, {1}, {2}, {3}",
				objectCr.RealityObject.FiasAddress.House,
				objectCr.RealityObject.FiasAddress.Letter,
				objectCr.RealityObject.FiasAddress.Housing,
				objectCr.RealityObject.FiasAddress.Building);

			this.ReportParams["НомерДоговора"] = buildContract.DocumentNum;
			this.ReportParams["ДатаДоговора"] = buildContract.DocumentDateFrom.ToDateString();

			var regop = RegOperatorDomain.GetAll()
					.FirstOrDefault(x => x.Contragent.Municipality.Id == objectCr.RealityObject.Municipality.Id);

			if (regop != null)
			{
				this.ReportParams["РегОператор"] = regop.Contragent.Name;

				var contact = ContragentContactDomain.GetAll()
					.FirstOrDefault(x => x.Contragent.Id == regop.Contragent.Id);

				if (contact != null)
				{
					this.ReportParams["Руководитель"] = contact.FullName;
                }
			}

			if (buildContract.Builder != null)
			{
				this.ReportParams["Подрядчик"] = buildContract.Builder.Contragent.Name;
            }

			this.ReportParams["ПротоколКвалОтбора"] = string.Format("№ {0} от {1}", 
				buildContract.ProtocolNum, 
				buildContract.ProtocolDateFrom != null ? buildContract.ProtocolDateFrom.Value.ToString("dd MMMM yyyy") : "");

			var works = BuildContractTypeWorkDomain.GetAll()
				.Where(x => x.BuildContract.Id == buildContract.Id)
				.Select(x => x.TypeWork.Work.Name)
				.ToArray();

			if (works.Length == 0)
			{
				works = TypeWorkCrDomain.GetAll()
					.Where(x => x.ObjectCr.Id == objectCr.Id)
					.Select(x => x.Work.Name)
					.ToArray();
			}

			this.ReportParams["Работы"] = works.AggregateWithSeparator(", ");
			this.ReportParams["СредстваСобственников"] = buildContract.OwnerMeans.ToStr();
			this.ReportParams["СредстваБюджетов"] = (buildContract.BudgetMo + buildContract.BudgetSubject + buildContract.FundMeans).ToString();

		    var queryTypeWorks = CompetitionLotTypeWorkDomain.GetAll()
		        .Where(x => x.TypeWork.ObjectCr.Id == buildContract.ObjectCr.Id);

		    var winnerByLot = CompetitionLotBidDomain
		        .GetAll()
		        .Where(x => queryTypeWorks.Any(y => y.Lot.Id == x.Lot.Id))
		        .Where(x => x.Builder.Contragent.Id == buildContract.Builder.Contragent.Id).FirstOrDefault(x => x.IsWinner);

		    if (winnerByLot != null)
		    {
                this.ReportParams ["ДатаПротокола"] = winnerByLot.Lot.ContractDate.ToDateString();
            }
            this.ReportParams ["СтоимостьРаботЦифрами"] = buildContract.Sum.ToString();
            this.ReportParams ["ДатаНачалаРаботПоДоговору"] = buildContract.DateStartWork.ToDateString();
            this.ReportParams ["ДатаОкончанияРаботПоДоговору"] = buildContract.DateEndWork.ToDateString();

        }
	}
}