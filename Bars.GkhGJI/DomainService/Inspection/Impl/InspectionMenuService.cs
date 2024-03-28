namespace Bars.GkhGji.DomainService
{
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4;
    using Bars.GkhGji.Contracts;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Enums;

    using Castle.Windsor;

    /// <summary>
    /// Короче это типа меню для проверок , его можно в регионах переделывать и елать свои меню если требуется
    /// </summary>
    public class InspectionMenuService : IInspectionMenuService
    {
        public IWindsorContainer Container { get; set; }

        public IDomainService<InspectionGjiStage> InspectionStageDomain { get; set; }

        public IDomainService<InspectionGji> InspectionDomain { get; set; }

        public IDomainService<DocumentGji> DocumentDomain { get; set; }

        public IDisposalText DisposalTextService { get; set; }

        public virtual IDataResult GetMenu(long inspectionId, long? documentId)
        {
            var list = new List<MenuItem>();

            if (inspectionId > 0)
            {
                var inspection = this.InspectionDomain.GetAll().FirstOrDefault(x => x.Id == inspectionId);
                if (inspection != null)
                {
                    list.AddRange(this.GetMenuItems(inspection));
                }
                if (documentId.HasValue && documentId.Value > 0)
                {
                    int i = 0;
                    foreach (var item in list)
                    {
                        if (item.Options.ContainsKey("documentId"))
                        {
                            var docId = item.Options.GetAs<long?>("documentId");
                            if (docId == documentId)
                            {
                                list[i].Options.Add("selected",true);
                            }
                            int k = 0;
                            foreach (var subItem in item.Items)
                            {
                                var subdocId = subItem.Options.GetAs<long?>("documentId");
                                if (subdocId == documentId)
                                {
                                    list[i].Items[k].Options.Add("selected", true);
                                }
                                k++;
                            }
                        }
                        i++;
                    }
                }

                if (!list.Any())
                {
                    //Если вообще нет никаких пунктов то ставим просто чтобы пользователь видел не пустой список
                    var item = new MenuItem { Caption = "Разделы отсутствуют" };
                    list.Add(item);
                }
            }

            return new BaseDataResult(list);
        }

        public virtual IList<MenuItem> GetMenuItems(InspectionGji inspection)
        {
            var menu = new List<MenuItem>();
            
            var excludeInspectionList = new List<TypeBase>()
            {
                TypeBase.ProsecutorsResolution,
                TypeBase.ActivityTsj,
                TypeBase.HeatingSeason,
                TypeBase.ProtocolMvd,
                TypeBase.AdministrativeCase,
                TypeBase.ProtocolMhc,
                TypeBase.ProtocolGji,
                TypeBase.ActionIsolated,
                TypeBase.PreventiveAction,
                TypeBase.MotivatedPresentationAppealCits
            };
            
            if (!excludeInspectionList.Contains(inspection.TypeBase))
            {
                // Если это не основание из списка
                // То у проверки есть основание, которое будет идти первым пунктом в дереве
                menu.Add(this.ItemBaseItem(inspection));
            }

            // Получаем количество документов по каждому этапу 
            var dictCounts = this.DocumentDomain.GetAll()
                .Where(x => x.Stage.Inspection.Id == inspection.Id)
                .Select(x => new
                {
                    x.Id, 
                    StageId = x.Stage.Id, 
                    x.DocumentNumber,
                    x.TypeDocumentGji
                })
                .AsEnumerable()
                .GroupBy(x => x.StageId)
                .ToDictionary(
                    x => x.Key,
                    y => y.Select(x => new DocumentProxy
                    {
                        Id = x.Id,
                        DocumentNumber = x.DocumentNumber,
                        TypeDocumentGji = x.TypeDocumentGji
                    }).ToList());

            // Получаем все этапы проверки
            // За исключением этапа Акты проверки предписаний (они же акты устранения нарушений)
            // Потому что Акты проверки предписаний показываются через карточку Акта проверки во вкладке Акты проверки предписаний
            var dictStages = this.InspectionStageDomain.GetAll()
                .Where(x => x.Inspection.Id == inspection.Id && x.TypeStage != TypeStage.ActRemoval)
                .Select(x => new
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
                    y => y.Select(z => new InspectionGjiStageProxyRow
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
                    .Select(stage => this.InitItem(inspection, stage, dictStages, dictCounts)));
            }

            return menu;
        }

        /// <summary>
        /// Метод получения узла для Основания проверки
        /// </summary>
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

                case TypeBase.GjiWarning:
                    baseInspection.Caption = "Основание предостережения";
                    baseInspection.Href = "B4.controller.warninginspection.Edit";
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
                
                case TypeBase.InspectionActionIsolated:
                    {
                        baseInspection.Caption = "Проверка по КНМ";
                        baseInspection.Href = "B4.controller.inspectionactionisolated.Edit";
                    }

                    break;
                
                case TypeBase.InspectionPreventiveAction:
                {
                    baseInspection.Caption = "Проверка по профилактическому мероприятию";
                    baseInspection.Href = "B4.controller.inspectionpreventiveaction.Edit";
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

        /// <summary>
        /// Метод получения списка узлов-этапов проверки
        /// </summary>
        public virtual MenuItem InitItem(InspectionGji inspection, 
            InspectionGjiStageProxyRow curentStage,
            Dictionary<long, List<InspectionGjiStageProxyRow>> dictStages,
            Dictionary<long, List<DocumentProxy>> dictCounts)
        {
            var item = this.GetItem(curentStage, dictCounts);
            item.AddOption("inspectionId", inspection.Id);
            item.AddOption("typeInspection", inspection.TypeBase);
            item.AddOption("title", item.Caption);

            if (dictStages.ContainsKey(curentStage.Id))
            {
                foreach (var childItem in dictStages[curentStage.Id])
                {
                    if (dictCounts.ContainsKey(childItem.Id))
                    {
                        item.Items.Add(this.InitItem(inspection, childItem, dictStages, dictCounts));
                    }
                }
            }

            return item;
        }

        /// <summary>
        /// Метод получения узла этапа проверки
        /// </summary>
        public virtual MenuItem GetItem(InspectionGjiStageProxyRow curentStage, Dictionary<long, List<DocumentProxy>> dictCounts)
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
                    item.AddOption("typeDocument", dictCounts[curentStage.Id][0].TypeDocumentGji);
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
                    item.Caption = this.DisposalTextService.SubjectiveCase;
                    item.WithIcon("icon-rosette");
                    if (cnt == 1)
                    {
                        item.Href = "B4.controller.Disposal";
                    }
                    break; 

                case TypeStage.WarningDoc:
                    item.Caption = "Предостережение";
                    item.WithIcon("icon-rosette");
                    item.Href = "B4.controller.WarningDoc";
                    if (cnt > 1)
                    {
                        item.Caption = "Предостережения";
                        item.Href = "B4.controller.warningdoc.ListPanel";
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
                
                case TypeStage.MotivationConclusion:
                    item.Caption = "Мотивировочное заключение";
                    item.WithIcon("icon-page-white-edit");
                    item.Href = "B4.controller.MotivationConclusion";
                    if (cnt > 1)

                    {
                        item.Caption = "Мотивировочные заключения";
                        item.Href = "B4.controller.motivationconclusion.ListPanel";
                    }
                    break;

                case TypeStage.TaskDisposal:
                    item.Caption = "Задание";
                    item.WithIcon("icon-rosette");
                    if (cnt == 1)
                    {
                        item.Href = "B4.controller.TaskDisposal";
                    }
                    break;

                case TypeStage.DisposalPrescription:
                    item.Caption = this.DisposalTextService.SubjectiveCase;
                    item.WithIcon("icon-rosette");
                    if (cnt == 1)
                    {
                        item.Href = "B4.controller.Disposal";
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

                case TypeStage.ActCheck:
                    item.Caption = "Aкт проверки";
                    item.Href = "B4.controller.ActCheck";
                    item.WithIcon("icon-page-white-edit");
                    if (cnt > 1)
                    {
                        item.Caption = "Aкты проверок";
                        item.Href = "B4.controller.actcheck.ListPanel";
                    }
                    break;

                case TypeStage.ActCheckGeneral:
                    item.Caption = "Aкт проверки (общий)";
                    item.Href = "B4.controller.ActCheck";
                    item.WithIcon("icon-page-white-edit");
                    if (cnt > 1)
                    {
                        item.Caption = "Aкты проверок (общие)";
                        item.Href = "B4.controller.actcheck.ListPanel";
                    }
                    break;

                case TypeStage.ActIsolated:
                    item.Caption = "Акт без взаимодействия";
                    item.Href = "B4.controller.ActIsolated";
                    item.WithIcon("icon-page-white-edit");
                    if (cnt > 1)
                    {
                        item.Caption = "Aкты без взаимодействия";
                        item.Href = "B4.controller.actisolated.ListPanel";
                    }
                    break;

                case TypeStage.ActView:
                    item.Caption = "Aкт осмотра";
                    item.Href = "B4.controller.ActCheck";
                    item.WithIcon("icon-page-white-edit");
                    if (cnt > 1)
                    {
                        item.Caption = "Aкты осмотра";
                        item.Href = "B4.controller.actcheck.ListPanel";
                    }
                    break;


                case TypeStage.ActSurvey:
                    item.Caption = "Aкт обследования";
                    item.Href = "B4.controller.ActSurvey";
                    item.WithIcon("icon-page-white-magnify");
                    if (cnt > 1)
                    {
                        item.Caption = "Aкты обследования";
                        item.Href = "B4.controller.actsurvey.ListPanel";
                    }
                    break;

                case TypeStage.ActVisual:
                    item.Caption = "Акт визуального осмотра";
                    item.Href = "B4.controller.ActVisual";
                    item.WithIcon("icon-page-white-magnify");
                    break;

                case TypeStage.Prescription:
                    item.Caption = "Предписание";
                    item.Href = "B4.controller.Prescription";
                    item.WithIcon("icon-page-white-error");
                    if (cnt > 1)
                    {
                        item.Caption = "Предписания";
                        item.Href = "B4.controller.prescription.ListPanel";
                    }
                    break;

                case TypeStage.Protocol:
                    item.Caption = "Протокол";
                    item.Href = "B4.controller.ProtocolGji";
                    item.WithIcon("icon-page-white-medal");
                    if (cnt > 1)
                    {
                        item.Caption = "Протоколы";
                        item.Href = "B4.controller.protocolgji.ListPanel";
                    }
                    break;

                case TypeStage.Resolution:
                    item.Caption = "Постановление";
                    item.Href = "B4.controller.Resolution";
                    item.WithIcon("icon-page-white-star");
                    if (cnt > 1)
                    {
                        item.Caption = "Постановления";
                        item.Href = "B4.controller.resolution.ListPanel";
                    }
                    break;

                case TypeStage.ActRemoval:
                    item.Caption = "Aкт проверки предписания";
                    item.Href = "B4.controller.ActRemoval";
                    if (cnt > 1)
                    {
                        item.Caption = "Aкты проверок предписаний";
                        item.Href = "B4.controller.actremoval.ListPanel";
                    }
                    break;

                case TypeStage.AdministrativeCase:
                    item.Caption = "Административное дело";
                    if (cnt == 1)
                    {
                        item.Href = "B4.controller.admincase.Edit";
                    }
                    break;

                case TypeStage.ResolutionProsecutor:
                    item.Caption = "Постановление прокуратуры";
                    if (cnt == 1)
                    {
                        item.Href = "B4.controller.resolpros.Edit";
                    }
                    break;

                case TypeStage.Presentation:
                    item.Caption = "Представление";
                    item.Href = "B4.controller.Presentation";
                    item.WithIcon("icon-page-white-text");
                    if (cnt > 1)
                    {
                        item.Caption = "Представления";
                        item.Href = "B4.controller.presentation.ListPanel";
                    }
                    break;

                case TypeStage.ProtocolMvd:
                    item.Caption = "Протокол МВД";
                    if (cnt == 1)
                    {
                        item.Href = "B4.controller.protocolmvd.Edit";
                    }
                    break;

                case TypeStage.ProtocolMhc:
                    item.Caption = "Протокол МЖК";
                    if (cnt == 1)
                    {
                        item.Caption = "Протокол МЖК";
                        if (cnt == 1)
                        {
                            item.Href = "B4.controller.protocol197.Edit";
                        }
                    }

                    break;

                case TypeStage.Protocol197:
                    {
                        item.Caption = "Протокол по ст.19.7";
                        if (cnt == 1)
                        {
                            item.Href = "B4.controller.protocol197.Edit";
                        }
                        item.Href = "B4.controller.protocolmhc.Edit";
                    }
                    break;

                case TypeStage.ResolutionRospotrebnadzor:
                    item.Caption = "Постановление Роспотребнадзора";
                    item.Href = "B4.controller.ResolutionRospotrebnadzor";
                    item.WithIcon("icon-page-white-star");
                    if (cnt > 1)
                    {
                        item.Caption = "Постановления Роспотребнадзора";
                        item.Href = "B4.controller.resolutionrospotrebnadzor.ListPanel";
                    }
                    break;

                case TypeStage.ProtocolGji:
                    item.Caption = "Протокол ГЖИ";
                    item.Href = "B4.controller.tatarstanprotocolgji.Edit";
                    break;

                case TypeStage.ResolutionGji:
                    item.Caption = "Постановление ГЖИ";
                    item.Href = "B4.controller.tatarstanresolutiongji.Edit";
                    item.WithIcon("icon-page-white-star");
                    break;
                
                case TypeStage.PreventiveAction:
                    item.Caption = "Профилактическое мероприятие";
                    item.Href = "B4.controller.preventiveaction.Edit";
                    break;
                
                case TypeStage.VisitSheet:
                    item.Caption = "Визит";
                    item.Href = "B4.controller.preventiveaction.Visit";
                    item.WithIcon("icon-page-white-star");
                    break;
                
                case TypeStage.ActActionIsolated:
                    item.Caption = "Акт";
                    item.Href = "B4.controller.actionisolated.actaction.ActActionIsolated";
                    break;
                
                case TypeStage.TaskActionIsolated:
                    item.Caption = "Задание";
                    item.Href = "B4.controller.actionisolated.taskaction.TaskAction";
                    break;
                
                case TypeStage.PreventiveActionTask:
                    item.Caption = "Задание";
                    item.Href = "B4.controller.preventiveaction.task.Task";
                    item.AddRequiredPermission("GkhGji.DocumentsGji.PreventiveActionTask.View");
                    break;
                
                case TypeStage.MotivatedPresentation:
                    item.Caption = "Мотивированное представление";
                    item.Href = "B4.controller.actionisolated.motivatedpresentation.MotivatedPresentation";
                    item.AddRequiredPermission("GkhGji.DocumentsGji.MotivatedPresentationActionIsolated.View");
                    break;
                
                case TypeStage.MotivatedPresentationAppealCits:
                    item.Caption = "Мотивированное представление";
                    item.Href = "B4.controller.appealcits.MotivatedPresentation";
                    item.AddRequiredPermission("GkhGji.DocumentsGji.MotivatedPresentationAppealCits.View");
                    break;
            }

            if (cnt == 1 && !string.IsNullOrWhiteSpace(number))
            {
                item.Caption = $"{item.Caption} ({number})";
            }

            return item;
        }

        public class InspectionGjiStageProxyRow
        {
            public long Id { get; set; }

            public long Parent { get; set; }

            public long Position { get; set; }

            public TypeStage TypeStage { get; set; }
        }

        public class DocumentProxy
        {
            public long Id { get; set; }
            public string DocumentNumber { get; set; }
            public TypeDocumentGji TypeDocumentGji { get; set; }
        }
    }
}