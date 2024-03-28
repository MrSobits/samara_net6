namespace Bars.GkhGji.Regions.Nso.DomainService.Impl
{
	using System.Collections.Generic;
	using System.Linq;
	using Bars.B4;
	using Bars.GkhGji.Entities;
	using Bars.GkhGji.Enums;

	public class InspectionMenuService : Bars.GkhGji.DomainService.InspectionMenuService
    {
		public override IList<MenuItem> GetMenuItems(InspectionGji inspection)
		{
			var menu = new List<MenuItem>();

			if (inspection.TypeBase != TypeBase.ProsecutorsResolution
				&& inspection.TypeBase != TypeBase.ActivityTsj
				&& inspection.TypeBase != TypeBase.HeatingSeason
				&& inspection.TypeBase != TypeBase.ProtocolMvd
				&& inspection.TypeBase != TypeBase.AdministrativeCase
				&& inspection.TypeBase != TypeBase.ProtocolMhc)
			{
				// Если это не Постановление прокуратыры, Не Деятельность ТСЖ и не Отопительный сезон и не Протокол МВД
				// То у проверки есть основание, которое будет идти первым пунктом в дереве
				menu.Add(ItemBaseItem(inspection));
			}

			// Получаем количество документов по каждому этапу 
			var dictCounts =
				DocumentDomain.GetAll()
							  .Where(x => x.Stage.Inspection.Id == inspection.Id)
							  .Select(x => new { x.Id, StageId = x.Stage.Id, x.DocumentNumber })
							  .AsEnumerable()
							  .GroupBy(x => x.StageId)
							  .ToDictionary(
								  x => x.Key,
								  y =>
								  y.Select(x => new DocumentProxy { Id = x.Id, DocumentNumber = x.DocumentNumber })
								   .ToList());

			var dictStages =
				InspectionStageDomain.GetAll()
									 .Where(
										 x => x.Inspection.Id == inspection.Id)
									 .Select(
										 x =>
										 new
										 {
											 x.Id,
											 x.TypeStage,
											 Parent = x.Parent == null ? 0 : x.Parent.Id,
											 x.Position
										 })
									 .OrderBy(x => x.Parent)
									 .ThenBy(x => (int)x.TypeStage)
									 .AsEnumerable()
									 .GroupBy(x => x.Parent)
									 .ToDictionary(
										 x => x.Key,
										 y =>
										 y.Select(
											 z =>
											 new InspectionGjiStageProxyRow
											 {
												 Id = z.Id,
												 TypeStage = z.TypeStage,
												 Parent = z.Parent,
												 Position = z.Position
											 })
												.OrderBy(x => x.Position)
												.ThenBy(x => (int)x.TypeStage)
												.ToList());

			foreach (var item in dictStages.Where(item => item.Key == 0))
			{
				// Если есть Этап но нет ниодного документа то недобавляем этап в меню
				menu.AddRange(item.Value
					.Where(stage => dictCounts.ContainsKey(stage.Id))
					.OrderBy(stage => stage.Position)
					.Select(stage => InitItem(inspection, stage, dictStages, dictCounts)));
			}

			return menu;
		}

		public override MenuItem GetItem(InspectionGjiStageProxyRow curentStage, Dictionary<long, List<DocumentProxy>> dictCounts)
		{
			var item = new MenuItem
			{
				Caption = "Этап проверки",
				Href = "B4.controller.dict.PlanJurPersonGji"
			};

			int cnt = 0;

			var number = "";

			if (dictCounts.ContainsKey(curentStage.Id))
			{
				cnt = dictCounts[curentStage.Id].Count;
				if (cnt == 1)
				{
					item.AddOption("documentId", dictCounts[curentStage.Id][0].Id);
					number = dictCounts[curentStage.Id][0].DocumentNumber;
				}
				else
				{
					item.AddOption("stageId", curentStage.Id);
				}
			}

			switch (curentStage.TypeStage)
			{
				case TypeStage.Disposal:
					{
						item.Caption = DisposalTextService.SubjectiveCase;
						item.WithIcon("icon-rosette");

						if (cnt == 1)
						{
							item.Href = "B4.controller.Disposal";
						}
					}

					break;

				case TypeStage.DisposalPrescription:
					{
						item.Caption = DisposalTextService.SubjectiveCase;
						item.WithIcon("icon-rosette");

						if (cnt == 1)
						{
							item.Href = "B4.controller.Disposal";
						}
					}

					break;

				case TypeStage.ActCheck:
					{
						item.Caption = "Aкт проверки";
						item.Href = "B4.controller.ActCheck";
						item.WithIcon("icon-page-white-edit");

						if (cnt > 1)
						{
							item.Caption = "Aкты проверок";
							item.Href = "B4.controller.actcheck.ListPanel";
						}
					}

					break;

				case TypeStage.ActCheckGeneral:
					{
						item.Caption = "Aкт проверки (общий)";
						item.Href = "B4.controller.ActCheck";
						item.WithIcon("icon-page-white-edit");

						if (cnt > 1)
						{
							item.Caption = "Aкты проверок (общие)";
							item.Href = "B4.controller.actcheck.ListPanel";
						}
					}

					break;


				case TypeStage.ActView:
					{
						item.Caption = "Aкт осмотра";
						item.Href = "B4.controller.ActCheck";
						item.WithIcon("icon-page-white-edit");

						if (cnt > 1)
						{
							item.Caption = "Aкты осмотра";
							item.Href = "B4.controller.actcheck.ListPanel";
						}
					}

					break;


				case TypeStage.ActSurvey:
					{
						item.Caption = "Aкт обследования";
						item.Href = "B4.controller.ActSurvey";
						item.WithIcon("icon-page-white-magnify");

						if (cnt > 1)
						{
							item.Caption = "Aкты обследования";
							item.Href = "B4.controller.actsurvey.ListPanel";
						}
					}

					break;

				case TypeStage.ActVisual:
					{
						item.Caption = "Акт визуального осмотра";
						item.Href = "B4.controller.ActVisual";
						item.WithIcon("icon-page-white-magnify");
					}

					break;

				case TypeStage.Prescription:
					{
						item.Caption = "Предписание";
						item.Href = "B4.controller.Prescription";
						item.WithIcon("icon-page-white-error");

						if (cnt > 1)
						{
							item.Caption = "Предписания";
							item.Href = "B4.controller.prescription.ListPanel";
						}

					}

					break;

				case TypeStage.Protocol:
					{
						item.Caption = "Протокол";
						item.Href = "B4.controller.ProtocolGji";
						item.WithIcon("icon-page-white-medal");

						if (cnt > 1)
						{
							item.Caption = "Протоколы";
							item.Href = "B4.controller.protocolgji.ListPanel";
						}
					}

					break;

				case TypeStage.Resolution:
					{
						item.Caption = "Постановление";
						item.Href = "B4.controller.Resolution";
						item.WithIcon("icon-page-white-star");

						if (cnt > 1)
						{
							item.Caption = "Постановления";
							item.Href = "B4.controller.resolution.ListPanel";
						}
					}

					break;

				case TypeStage.ActRemoval:
					{
						item.Caption = "Aкт проверки предписания";
						item.Href = "B4.controller.ActRemoval";
						item.WithIcon("icon-page-white-edit");

						if (cnt > 1)
						{
							item.Caption = "Aкты проверок предписаний";
							item.Href = "B4.controller.actremoval.ListPanel";
						}
					}

					break;

				case TypeStage.AdministrativeCase:
					{
						item.Caption = "Административное дело";
						if (cnt == 1)
						{
							item.Href = "B4.controller.admincase.Edit";
						}
					}

					break;

				case TypeStage.ResolutionProsecutor:
					{
						item.Caption = "Постановление прокуратуры";
						if (cnt == 1)
						{
							item.Href = "B4.controller.resolpros.Edit";
						}
					}

					break;

				case TypeStage.Presentation:
					{
						item.Caption = "Представление";
						item.Href = "B4.controller.Presentation";
						item.WithIcon("icon-page-white-text");
						if (cnt > 1)
						{
							item.Caption = "Представления";
							item.Href = "B4.controller.presentation.ListPanel";
						}
					}

					break;

				case TypeStage.ProtocolMvd:
					{
						item.Caption = "Протокол МВД";
						if (cnt == 1)
						{
							item.Href = "B4.controller.protocolmvd.Edit";
						}
					}

					break;

				case TypeStage.ProtocolMhc:
					{
						item.Caption = "Протокол МЖК";
						if (cnt == 1)
						{
							item.Href = "B4.controller.protocolmhc.Edit";
						}
					}

					break;
			}

			if (cnt == 1 && !string.IsNullOrWhiteSpace(number))
			{
				item.Caption = string.Format("{0} ({1})", item.Caption, number);
			}

			return item;
		}
    }
}