﻿namespace Bars.GkhGji.Regions.Habarovsk.DomainService.Impl.Inspection
{
    using System.Collections.Generic;

    using Bars.B4;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Enums;

    public class InspectionMenuService : BaseChelyabinsk.DomainService.Impl.Inspection.InspectionMenuService
    {
		public virtual MenuItem ItemBaseItem(InspectionGji inspection)
		{
			var baseInspection = new MenuItem { Caption = "Основание проверки" };
			baseInspection.AddOption("inspectionId", inspection.Id);
			baseInspection.AddOption("title", "Основание проверки");

			switch (inspection.TypeBase)
			{
				case TypeBase.PlanJuridicalPerson:
					{
						baseInspection.Caption = "Плановая проверка юр. лица";
						baseInspection.Href = "B4.controller.basejurperson.Edit";
					}

					break;

				case TypeBase.PlanOMSU:
					{
						baseInspection.Caption = "Плановая проверка ОМСУ";
						baseInspection.Href = "B4.controller.baseomsu.Edit";
					}

					break;

				case TypeBase.DisposalHead:
					{
						baseInspection.Caption = "Поручение руководителя";
						baseInspection.Href = "B4.controller.basedisphead.Edit";
					}

					break;

				case TypeBase.Inspection:
					{
						baseInspection.Caption = "Инспекционная проверка";
						baseInspection.Href = "B4.controller.baseinscheck.Edit";
					}

					break;

				case TypeBase.ProsecutorsClaim:
					{
						baseInspection.Caption = "Требование прокуратуры";
						baseInspection.Href = "B4.controller.baseprosclaim.Edit";
					}

					break;

				case TypeBase.CitizenStatement:
					{
						baseInspection.Caption = "Проверка по обращению граждан";
						baseInspection.Href = "B4.controller.basestatement.Edit";
					}

					break;

				case TypeBase.LicenseApplicants:
					{
						baseInspection.Caption = "Проверка соискателей лицензии";
						baseInspection.Href = "B4.controller.baselicenseapplicants.Edit";
					}

					break;

				case TypeBase.PlanAction:
					{
						baseInspection.Caption = "Проверка по плану мероприятий";
						baseInspection.Href = "B4.controller.baseplanaction.Edit";
					}

					break;

				case TypeBase.Default:
					{
						baseInspection.Caption = "Основание проверки";
						baseInspection.Href = "B4.controller.basedefault.Edit";
					}

					break;

				case TypeBase.LicenseReissuance:
					{
						baseInspection.Caption = "Проверка лицензиата";
						baseInspection.Href = "B4.controller.baselicensereissuance.Edit";
					}

					break;
			}

			baseInspection.WithIcon("icon-page-world");
			return baseInspection;
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
						item.Caption = this.DisposalTextService.SubjectiveCase;
						item.WithIcon("icon-rosette");

						if (cnt == 1)
						{
							item.Href = "B4.controller.Disposal";
						}
					}

					break;


				case TypeStage.Decision:
					{
						item.Caption = "Решение";
						item.WithIcon("icon-rosette");

						if (cnt == 1)
						{
							item.Href = "B4.controller.Decision";
						}
					}

					break;

				case TypeStage.DecisionPrescription:
					{
						item.Caption = "Решение о проверке предписания";
						item.WithIcon("icon-rosette");

						if (cnt == 1)
						{
							item.Href = "B4.controller.Decision";
						}
					}

					break;

				case TypeStage.DisposalPrescription:
					{
						item.Caption = "Приказ на проверку предписания";
						item.WithIcon("icon-rosette");

						if (cnt == 1)
						{
							item.Href = "B4.controller.Disposal";
						}
					}

					break;

                case TypeStage.PreventiveVisit:
                    {
                        item.Caption = "Профвизит";
                        item.WithIcon("icon-page-white-edit");

                        if (cnt == 1)
                        {
                            item.Href = "B4.controller.PreventiveVisit";
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
				
				// TODO: Добавить значение в enum или убрать
				/*   case TypeStage.ProtocolRSO:
				       {
				           item.Caption = "Протокол РСО";
				           if (cnt == 1)
				           {
				               item.Href = "B4.controller.protocolrso.Edit";
				           }
				       }

				       break;*/

                case TypeStage.Protocol197:
                    {
                        item.Caption = "Протокол";
                        if (cnt == 1)
                        {
                            item.Href = "B4.controller.protocol197.Edit";
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