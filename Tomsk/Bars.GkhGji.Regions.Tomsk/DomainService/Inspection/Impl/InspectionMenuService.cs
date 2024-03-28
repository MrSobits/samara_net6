namespace Bars.GkhGji.Regions.Tomsk.DomainService
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
    public class InspectionMenuService : Bars.GkhGji.DomainService.InspectionMenuService
    {
        /// <summary>
        /// Метод получения узла для Основания проверки
        /// </summary>
        public override MenuItem ItemBaseItem(InspectionGji inspection)
        {
            var baseInspection = new MenuItem { Caption = "Основание процесса" };
            baseInspection.AddOption("inspectionId", inspection.Id);
            baseInspection.AddOption("title", "Основание процесса");

            switch (inspection.TypeBase)
            {
                case TypeBase.PlanJuridicalPerson:
                    {
                        baseInspection.Caption = "Плановая проверка юр. лица";
                        baseInspection.Href = "B4.controller.basejurperson.Edit";
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
                        baseInspection.Caption = "Процесс по обращению граждан";
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
            }

            baseInspection.WithIcon("icon-page-world");
            return baseInspection;
        }
	}
}