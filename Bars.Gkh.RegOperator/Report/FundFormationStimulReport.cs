namespace Bars.Gkh.RegOperator.Report
{
	using System;
	using System.Collections.Generic;
	using System.IO;
	using System.Linq;

	using Bars.B4;
	using Bars.B4.DataAccess;
	
	using B4.Modules.Reports;

	using Bars.B4.Modules.Analytics.Reports.Enums;
	using Bars.B4.Modules.Analytics.Reports.Generators.Models;
	using Bars.B4.Utils;
	using Bars.Gkh.Decisions.Nso.Entities;
	using Bars.Gkh.Entities;
	using Bars.Gkh.Enums;
	using Bars.Gkh.Enums.Decisions;
	using Bars.Gkh.Modules.RegOperator.Entities.RegOperator;
	using Bars.Gkh.RegOperator.Entities;
	using Bars.Gkh.StimulReport;

	using Castle.Windsor;

	public class FundFormationStimulReport : StimulReport, IGeneratedPrintForm
	{
		/// <summary>
		/// Муниципальный район.
		/// </summary>
		private long municipality;

		/// <summary>
		/// Способ формирования счета.
		/// </summary>
		private int formationType;

		/// <summary>
		/// Дата с.
		/// </summary>
		private DateTime dateFrom;

		/// <summary>
		/// Дата по.
		/// </summary>
		private DateTime dateTo;

		private Dictionary<Type, UltimateDecision> decisionsCache;

		/// <summary>
		/// IoC container.
		/// </summary>
		public IWindsorContainer Container { get; set; }

		/// <summary>
		/// Имя отчета.
		/// </summary>
		public string Name
		{
			get
			{
				return "Отчет по разделу уведомлений о способе формирования фонда (на Стимуле)";
			}
		}

		/// <summary>
		/// Описание.
		/// </summary>
		public string Desciption
		{
			get
			{
				return "Отчет по разделу уведомлений о способе формирования фонда (на Стимуле)";
			}
		}

		/// <summary>
		/// Наименование группы шаблонов.
		/// </summary>
		public string GroupName
		{
			get
			{
				return "Протоколы решений";
			}
		}

		/// <summary>
		/// Контроллер.
		/// </summary>
		public string ParamsController
		{
			get
			{
				return "B4.controller.report.FundFormationStimulReport";
			}
		}

		/// <summary>
		/// Разрешение.
		/// </summary>
		public string RequiredPermission
		{
			get
			{
				return "Reports.DecisionsNso.FundFormationStimul";
			}
		}

		/// <summary>
		/// Получить шаблон.
		/// </summary>
		/// <returns>
		/// Поток.
		/// </returns>
		public Stream GetTemplate()
		{
			return new MemoryStream(Properties.Resources.FundFormationStimul);
		}

		public override StiExportFormat ExportFormat
		{
			get
			{
				return StiExportFormat.Excel2007;
			}
		}

		/// <summary>
		/// Выполнить сборку отчета.
		/// </summary>
		/// <param name="reportParams">
		/// Параметры отчета.
		/// </param>
		public void PrepareReport(ReportParams reportParams)
		{
			var roDomain = this.Container.ResolveDomain<RealityObject>();
			var manOrgContractDomain = this.Container.ResolveDomain<ManOrgContractRealityObject>();
			var roDecisionProtocolDomain = this.Container.ResolveDomain<RealityObjectDecisionProtocol>();
			var crFundFormationDecisionDomain = this.Container.ResolveDomain<CrFundFormationDecision>();
			var notificationDomain = this.Container.ResolveDomain<DecisionNotification>();
			var regOperatorDomain = this.Container.ResolveDomain<RegOperator>();
			var regopCalcAccountDomain = this.Container.ResolveDomain<RegopCalcAccount>();
			var calcAccountRoDomain = this.Container.ResolveDomain<CalcAccountRealityObject>();

			var roDict = roDomain.GetAll()
				.WhereIf(this.municipality > 0, x => x.Municipality.Id == this.municipality)
				.Where(x => x.TypeHouse == TypeHouse.ManyApartments && x.ConditionHouse != ConditionHouse.Razed)
				.Select(x => new RealityObjectProxy
								 {
									 Id = x.Id,
									 Municipality = x.Municipality,
									 AddressName = x.FiasAddress.AddressName,
									 BuildYear = x.BuildYear,
									 AreaMkd = x.AreaMkd
								 })
				.AsEnumerable()
				.GroupBy(x => x.Id)
				.ToDictionary(x => x.Key, y => y.First());

			var roIds = roDict.Select(x => x.Key).ToList();

			var roManOrgContract = manOrgContractDomain.GetAll()
				.Where(x => x.RealityObject != null)
				.Where(x => roIds.Contains(x.RealityObject.Id))
				.GroupBy(x => x.RealityObject.Id)
				.ToDictionary(
					x => x.Key, 
					y => y.Where(x => x.ManOrgContract != null)
										.FirstOrDefault(x => !x.ManOrgContract.EndDate.HasValue));

			var roDecisionProtocols = roDecisionProtocolDomain.GetAll()
				.Where(x => roIds.Contains(x.RealityObject.Id))
				.GroupBy(x => x.RealityObject.Id)
				.ToDictionary(x => x.Key, y => y.ToList());

			var infoList = new List<Info>();

			foreach (var realityObjectProxy in roDict)
			{
				var roValue = realityObjectProxy.Value;
				var roId = realityObjectProxy.Key;

				var info = new Info
							   {
								   МуниципальноеОбразование = roValue.Municipality.Name,
								   Адрес = roValue.AddressName,
								   ГодПостройки = roValue.BuildYear.HasValue ? roValue.BuildYear.Value.ToStr() : string.Empty,
								   ОбщаяПлощадь = roValue.AreaMkd.HasValue ? roValue.AreaMkd.Value.ToStr() : string.Empty,
								   СправкаБанка = string.Empty
							   };

				if (roManOrgContract.ContainsKey(roId) && roManOrgContract[roId] != null)
				{
					var manOrgContract = roManOrgContract[roId].ManOrgContract;
					
					if (manOrgContract != null && 
						manOrgContract.ManagingOrganization != null &&
						manOrgContract.ManagingOrganization.Contragent != null)
					{
						info.НаименованиеУправляющейОрганизации = manOrgContract.ManagingOrganization.Contragent.Name;
					}
				}

				if (roDecisionProtocols.ContainsKey(roId) && roDecisionProtocols[roId] != null)
				{
					var protocols = roDecisionProtocols[roId];
					var crFundFormations = new List<CrFundFormationDecisionType>();
					var accountOwnerDesicionsTypes = new List<AccountOwnerDecisionType>();
					var datesOpen = new List<DateTime>();
					var datesClose = new List<DateTime>();
					var protocolDetails = new List<string>();

					foreach (var protocol in protocols)
					{
						this.WarmProtocolDecisionCache(protocol);
						var crFund = this.decisionsCache.Get(typeof(CrFundFormationDecision)) as CrFundFormationDecision;
						var accountOwnerDecision = this.decisionsCache.Get(typeof(AccountOwnerDecision)) as AccountOwnerDecision;

						if (crFund != null)
						{
							crFundFormations.Add(crFund.Decision);

							if (crFund.Decision == CrFundFormationDecisionType.SpecialAccount)
							{
								var notification = notificationDomain.GetAll().FirstOrDefault(x => x.Protocol.Id == protocol.Id);
								if (notification != null)
								{
									datesOpen.Add(notification.OpenDate);
									datesClose.Add(notification.CloseDate);

									info.СправкаБанка = notification.BankDoc != null ? "Да" : "Нет";
								}
							}
							else if (crFund.Decision == CrFundFormationDecisionType.RegOpAccount)
							{
								var calcAccountIdFilter = calcAccountRoDomain.GetAll()
									.Where(x => x.RealityObject.Id == roId)
									.Select(x => x.Id)
									.ToList();

								var calcAccounts = regopCalcAccountDomain.GetAll()
									.Where(x => calcAccountIdFilter.Contains(x.Id))
									.ToList();

								foreach (var regopCalcAccount in calcAccounts)
								{
									datesOpen.Add(regopCalcAccount.DateOpen);
										
									if (regopCalcAccount.DateClose.HasValue)
									{
										datesClose.Add(regopCalcAccount.DateClose.Value);
									}
								}
							}
						}

						if (accountOwnerDecision != null)
						{
							accountOwnerDesicionsTypes.Add(accountOwnerDecision.DecisionType);
						}

						var details = string.Format(
							"Номер протокола {0}, Дата протокола {1}, Дата вступления в силу {2}",
							protocol.DocumentNum,
							protocol.ProtocolDate.ToShortDateString(),
							protocol.DateStart.ToShortDateString());
						protocolDetails.Add(details);
					}

					info.СпособФормированияФонда = string.Join(
						"\n",
						crFundFormations.Distinct().Select(x => x.GetEnumMeta().Display).ToArray());

					info.ВладелецСпецСчета = string.Join(
						"\n",
						accountOwnerDesicionsTypes.Distinct().Select(x => x.GetEnumMeta().Display).ToArray());

					info.ДатаОткрытияСчета = string.Join(
						"\n",
						datesOpen.Distinct().Select(x => x.ToShortDateString()).ToArray());

					info.ДатаЗакрытияСчета = string.Join(
						"\n",
						datesClose.Distinct().Select(x => x.ToShortDateString()).ToArray());

					info.РеквизитыПротоколаСобранияСобственников = string.Join("\n", protocolDetails);
				}

				infoList.Add(info);
			}

			this.DataSources.Add(new MetaData
			{
				SourceName = "Сведения",
				MetaType = nameof(Info),
				Data = infoList
			});

			this.ReportParams["ДатаГенерации"] = DateTime.Now.ToShortDateString();
			this.ReportParams["Квартал"] = ((DateTime.Now.Month / 4) + 1).ToString();

			string formationTypeText = null;

			switch (this.formationType)
			{
				case 0:
					formationTypeText = "На специальном счете";
					break;
				case 1:
					formationTypeText = "На счете регионального оператора";
					break;
				case 2:
					formationTypeText = "На специальном счете, владелец региональный оператор";
					break;
				case 3:
					formationTypeText = "Не выбрано";
					break;
			}

			this.ReportParams["СпособФормированияФонда"] = formationTypeText;

			this.Container.Release(roDomain);
			this.Container.Release(manOrgContractDomain);
			this.Container.Release(roDecisionProtocolDomain);
			this.Container.Release(crFundFormationDecisionDomain);
			this.Container.Release(notificationDomain);
			this.Container.Release(regOperatorDomain);
			this.Container.Release(regopCalcAccountDomain);
			this.Container.Release(calcAccountRoDomain);
		}

		/// <summary>
		/// Установить значения пользовательских параметров.
		/// </summary>
		/// <param name="baseParams">
		/// Базовые параметры запроса.
		/// </param>
		public void SetUserParams(BaseParams baseParams)
		{
			this.municipality = baseParams.Params["municipality"].ToLong();
			this.formationType = baseParams.Params["formationType"].ToInt();
			this.dateFrom = baseParams.Params["dateFrom"].ToDateTime();
			this.dateTo = baseParams.Params["dateTo"].ToDateTime();
		}

		private void WarmProtocolDecisionCache(RealityObjectDecisionProtocol protocol)
		{
			var decisions = Container.ResolveDomain<UltimateDecision>().GetAll()
				.Where(x => x.Protocol.Id == protocol.Id)
				.ToList();

			//используется фича хибера
			//при получении сущностей базового класса загружает все сабклассы
			decisionsCache = decisions.Select(x => new
													   {
														   Type = x.GetType(), 
														   Decision = x
													   })
				.GroupBy(x => x.Type)
				.ToDictionary(x => x.Key, y => y.First().Decision);
		}

		private sealed class RealityObjectProxy
		{
			public long Id;

			public Municipality Municipality;

			public string AddressName;

			public int? BuildYear;

			public decimal? AreaMkd;
		}

		private class Info
		{
			public string МуниципальноеОбразование { get; set; }
			public string Адрес { get; set; }
			public string ГодПостройки { get; set; }
			public string ОбщаяПлощадь { get; set; }
			public string НаименованиеУправляющейОрганизации { get; set; }
			public string СпособФормированияФонда { get; set; }
			public string ВладелецСпецСчета { get; set; }
			public string ДатаОткрытияСчета { get; set; }
			public string ДатаЗакрытияСчета { get; set; }
			public string РеквизитыПротоколаСобранияСобственников { get; set; }
			public string СправкаБанка { get; set; }
		}
	}
}
