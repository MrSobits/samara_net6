namespace Bars.GkhGji
{
    using Bars.B4;

    public class ClientRouteMapRegistrar : IClientRouteMapRegistrar
    {
        public void RegisterRoutes(ClientRouteMap map)
        {
            map.AddRoute(new ClientRoute("reminderInspector", "B4.controller.Reminder", "reminderInspector", requiredPermission: "GkhGji.ManagementTask.ReminderHead.View"));
            map.AddRoute(new ClientRoute("reminderHead", "B4.controller.Reminder", "reminderHead", requiredPermission: "GkhGji.ManagementTask.ReminderInspector.View"));
            map.AddRoute(new ClientRoute("reminderTaskControl/{colorType}/{inspectorId}", "B4.controller.Reminder", "reminderTaskControl"));
            map.AddRoute(new ClientRoute("reminderTaskState/{colorType}/{typeReminder}", "B4.controller.Reminder", "reminderTaskState"));

            map.AddRoute(new ClientRoute("activitytsj", "B4.controller.ActivityTsj", requiredPermission: "GkhGji.ActivityTsj.View"));
            // реестр обращений пока невозможно перевести на роуты, пожтому пока вернул черещ Котроллер
            map.AddRoute(new ClientRoute("appealcits", "B4.controller.AppealCits"));
            //map.AddRoute(new ClientRoute("appealcits/{id}", "B4.controller.AppealCits", "edit"));
            map.AddRoute(new ClientRoute("socialstatus", "B4.controller.dict.SocialStatus", requiredPermission: "GkhGji.AppealCitizens.AppealCitsInfo.View"));
            map.AddRoute(new ClientRoute("basedefault", "B4.controller.BaseDefault", requiredPermission: "GkhGji.Inspection.BaseDefault.View"));
            map.AddRoute(new ClientRoute("basedisphead", "B4.controller.BaseDispHead", requiredPermission: "GkhGji.Inspection.BaseDispHead.View"));
            map.AddRoute(new ClientRoute("baseplanaction", "B4.controller.BasePlanAction", requiredPermission: "GkhGji.Inspection.BasePlanAction.View"));
            map.AddRoute(new ClientRoute("baseinscheck", "B4.controller.BaseInsCheck", requiredPermission: "GkhGji.Inspection.BaseInsCheck.View"));
            map.AddRoute(new ClientRoute("basejurperson", "B4.controller.BaseJurPerson", requiredPermission: "GkhGji.Inspection.BaseJurPerson.View"));
            map.AddRoute(new ClientRoute("OSP", "B4.controller.dict.OSP", requiredPermission: "GkhGji.DocumentsGji.ProtocolMvd.View"));
            map.AddRoute(new ClientRoute("baseprosclaim", "B4.controller.BaseProsClaim", requiredPermission: "GkhGji.Inspection.BaseProsClaim.View"));
            map.AddRoute(new ClientRoute("basestatement", "B4.controller.BaseStatement", requiredPermission: "GkhGji.Inspection.BaseStatement.View"));
            map.AddRoute(new ClientRoute("baselicenseapplicants", "B4.controller.BaseLicenseApplicants", requiredPermission: "GkhGji.Inspection.BaseLicApplicants.View"));
            map.AddRoute(new ClientRoute("businessactivity", "B4.controller.BusinessActivity", requiredPermission: "GkhGji.BusinessActivityViewCreate.View"));
            map.AddRoute(new ClientRoute("docnumvalidationrule", "B4.controller.DocNumValidationRule", requiredPermission: "GkhGji.Settings.DocNumValidationRule.View"));
            map.AddRoute(new ClientRoute("heatseasdocmasschangestate", "B4.controller.HeatSeasDocMassChangeState", requiredPermission: "GkhGji.HeatSeasonDocMassChangeState.View"));
            map.AddRoute(new ClientRoute("heatseason", "B4.controller.HeatSeason", requiredPermission: "GkhGji.HeatSeason.View"));
            map.AddRoute(new ClientRoute("kindcheckrulereplace", "B4.controller.KindCheckRuleReplace", requiredPermission: "GkhGji.Settings.KindCheckRuleReplace.View"));
            map.AddRoute(new ClientRoute("resolpros", "B4.controller.ResolPros", requiredPermission: "GkhGji.DocumentsGji.ResolPros.View"));
            map.AddRoute(new ClientRoute("protocolmvd", "B4.controller.ProtocolMvd", requiredPermission: "GkhGji.DocumentsGji.ProtocolMvd.View"));
            map.AddRoute(new ClientRoute("protocolmhc", "B4.controller.ProtocolMhc", requiredPermission: "GkhGji.DocumentsGji.ProtocolMhc.View"));
            map.AddRoute(new ClientRoute("organmvd", "B4.controller.dict.OrganMvd", requiredPermission: "GkhGji.Dict.OrganMvd.View"));
            map.AddRoute(new ClientRoute("riskcategory", "B4.controller.dict.RiskCategory", requiredPermission: "GkhGji.Dict.RiskCategory.View"));
            map.AddRoute(new ClientRoute("protocolrso", "B4.controller.ProtocolRSO", requiredPermission: "GkhGji.DocumentsGji.ProtocolRSO.View"));
            map.AddRoute(new ClientRoute("controlactivity", "B4.controller.dict.ControlActivityGji", requiredPermission: "GkhGji.Dict.ActionsRemovViol.View"));
            map.AddRoute(new ClientRoute("emailgji", "B4.controller.EmailGji", requiredPermission: "GkhGji.AppealCitizens.EmailGji.View"));
            map.AddRoute(new ClientRoute("specialaccountreport", "B4.controller.SpecialAccountReport"));
            map.AddRoute(new ClientRoute("inspectionreason", "B4.controller.dict.InspectionReason", requiredPermission: "GkhGji.Dict.InspectionReason.View"));
            map.AddRoute(new ClientRoute("inspectionreasonerknm", "B4.controller.dict.InspectionReasonERKNM", requiredPermission: "GkhGji.Dict.InspectionReason.View"));
            map.AddRoute(new ClientRoute("controllist", "B4.controller.dict.ControlList", requiredPermission: "GkhGji.Dict.ControlList.View"));
            map.AddRoute(new ClientRoute("reporteditor/{id}/", "B4.controller.specialaccount.Edit"));
            map.AddRoute(new ClientRoute("answercontentgji", "B4.controller.dict.AnswerContentGji", requiredPermission: "GkhGji.Dict.AnswerContent.View"));
            map.AddRoute(new ClientRoute("articlelawgji", "B4.controller.dict.ArticleLawGji", requiredPermission: "GkhGji.Dict.ArticleLaw.View"));
            map.AddRoute(new ClientRoute("articletsj", "B4.controller.dict.ArticleTsj", requiredPermission: "GkhGji.Dict.ArticleTsj.View"));
            map.AddRoute(new ClientRoute("competentorggji", "B4.controller.dict.CompetentOrgGji", requiredPermission: "GkhGji.Dict.CompetentOrg.View"));
            map.AddRoute(new ClientRoute("courtverdictgji", "B4.controller.dict.CourtVerdictGji", requiredPermission: "GkhGji.Dict.CourtVerdict.View"));
            map.AddRoute(new ClientRoute("executantdocgji", "B4.controller.dict.ExecutantDocGji", requiredPermission: "GkhGji.Dict.ExecutantDoc.View"));
            map.AddRoute(new ClientRoute("expertgji", "B4.controller.dict.ExpertGji", requiredPermission: "GkhGji.Dict.Expert.View"));
            map.AddRoute(new ClientRoute("featureviolgji", "B4.controller.dict.FeatureViolGji", requiredPermission: "GkhGji.Dict.FeatureViol.View"));
            map.AddRoute(new ClientRoute("heatseasonperiodgji", "B4.controller.dict.HeatSeasonPeriodGji", requiredPermission: "GkhGji.Dict.HeatSeasonPeriod.View"));
            map.AddRoute(new ClientRoute("inspectedpartgji", "B4.controller.dict.InspectedPartGji", requiredPermission: "GkhGji.Dict.InspectedPart.View"));
            map.AddRoute(new ClientRoute("instancegji", "B4.controller.dict.InstanceGji", requiredPermission: "GkhGji.Dict.Instance.View"));
            map.AddRoute(new ClientRoute("typeoffeedback", "B4.controller.dict.TypeOfFeedback", requiredPermission: "GkhGji.Dict.TypeOfFeedback.View"));
            map.AddRoute(new ClientRoute("kindcheckgji", "B4.controller.dict.KindCheckGji", requiredPermission: "GkhGji.Dict.KindCheck.View"));
            map.AddRoute(new ClientRoute("kindprotocoltsj", "B4.controller.dict.KindProtocolTsj", requiredPermission: "GkhGji.Dict.KindProtocolTsj.View"));
            map.AddRoute(new ClientRoute("kindstatementgji", "B4.controller.dict.KindStatementGji", requiredPermission: "GkhGji.Dict.KindStatement.View"));
            map.AddRoute(new ClientRoute("kindworknotifgji", "B4.controller.dict.KindWorkNotifGji", requiredPermission: "GkhGji.Dict.KindWorkNotif.View"));
            map.AddRoute(new ClientRoute("planinscheckgji", "B4.controller.dict.PlanInsCheckGji", requiredPermission: "GkhGji.Dict.PlanInsCheck.View"));
            map.AddRoute(new ClientRoute("planjurpersongji", "B4.controller.dict.PlanJurPersonGji", requiredPermission: "GkhGji.Dict.PlanJurPerson.View"));
            map.AddRoute(new ClientRoute("provideddocgji", "B4.controller.dict.ProvidedDocGji", requiredPermission: "GkhGji.Dict.ProvidedDoc.View"));
            map.AddRoute(new ClientRoute("redtapeflaggji", "B4.controller.dict.RedtapeFlagGji", requiredPermission: "GkhGji.Dict.RedtapeFlag.View"));
            map.AddRoute(new ClientRoute("resolvegji", "B4.controller.dict.ResolveGji", requiredPermission: "GkhGji.Dict.Resolve.View"));
            map.AddRoute(new ClientRoute("revenueformgji", "B4.controller.dict.RevenueFormGji", requiredPermission: "GkhGji.Dict.RevenueForm.View"));
            map.AddRoute(new ClientRoute("sanctiongji", "B4.controller.dict.SanctionGji", requiredPermission: "GkhGji.Dict.Sanction.View"));
            map.AddRoute(new ClientRoute("statsubjectgji", "B4.controller.dict.StatSubjectGji", requiredPermission: "GkhGji.Dict.StatSubject.View"));
            map.AddRoute(new ClientRoute("statsubsubjectgji", "B4.controller.dict.StatsubsubjectGji", requiredPermission: "GkhGji.Dict.StatSubsubject.View"));
            map.AddRoute(new ClientRoute("typecourtgji", "B4.controller.dict.TypeCourtGji", requiredPermission: "GkhGji.Dict.TypeCourt.View"));
            map.AddRoute(new ClientRoute("mkdlictyperequest", "B4.controller.dict.MKDLicTypeRequest", requiredPermission: "GkhGji.Dict.MKDLicTypeRequest.View"));
            map.AddRoute(new ClientRoute("mkdlicrequest", "B4.controller.MKDLicRequest", requiredPermission: "Gkh.ManOrgLicense.MKDLicRequest.View"));
            map.AddRoute(new ClientRoute("edslicrequest", "B4.controller.LicRequestEDS", requiredPermission: "Gkh.ManOrgLicense.EDSLicRequest.View"));
            map.AddRoute(new ClientRoute("typesurveygji", "B4.controller.dict.TypeSurveyGji", requiredPermission: "GkhGji.Dict.TypeSurvey.View"));
            map.AddRoute(new ClientRoute("violationgji", "B4.controller.dict.ViolationGji", requiredPermission: "GkhGji.Dict.Violation.View"));
            map.AddRoute(new ClientRoute("violationfeaturegji", "B4.controller.dict.ViolationFeatureGji", requiredPermission: "GkhGji.Dict.ViolationGroup.View"));
            map.AddRoute(new ClientRoute("revenuesourcegji", "B4.controller.dict.RevenueSourceGji", requiredPermission: "GkhGji.Dict.RevenueSource.View"));
            map.AddRoute(new ClientRoute("actionsremovviolgji", "B4.controller.dict.ActionsRemovViol", requiredPermission: "GkhGji.Dict.ActionsRemovViol.View"));
            map.AddRoute(new ClientRoute("planactiongji", "B4.controller.dict.PlanActionGji", requiredPermission: "GkhGji.Dict.PlanActionGji.View"));
            map.AddRoute(new ClientRoute("boilerrooms", "B4.controller.BoilerRoom", requiredPermission: "GkhGji.HeatSeason.BoilerRooms.View"));
            map.AddRoute(new ClientRoute("heatinputinformation", "B4.controller.HeatInputPeriod", requiredPermission: "GkhGji.HeatInputInformation.View"));
            map.AddRoute(new ClientRoute("workwintercondition", "B4.controller.WorkWinterCondition", requiredPermission: "GkhGji.WorkWinterCondition.View"));
            map.AddRoute(new ClientRoute("surveypurpose", "B4.controller.dict.SurveyPurpose", requiredPermission: "GkhGji.Dict.SurveyPurpose.View"));
            map.AddRoute(new ClientRoute("surveyobjective", "B4.controller.dict.SurveyObjective", requiredPermission: "GkhGji.Dict.SurveyObjective.View"));
            map.AddRoute(new ClientRoute("activitydirection", "B4.controller.dict.ActivityDirection", requiredPermission: "GkhGji.Dict.ActivityDirection.View"));
            map.AddRoute(new ClientRoute("documentcode", "B4.controller.dict.DocumentCode", requiredPermission: "GkhGji.Dict.DocumentCode.View"));
            map.AddRoute(new ClientRoute("surveysubject", "B4.controller.dict.SurveySubject", requiredPermission: "GkhGji.Dict.SurveySubject.View"));
            map.AddRoute(new ClientRoute("typefactviolation", "B4.controller.dict.TypeFactViolation", requiredPermission: "GkhGji.Dict.TypeFactViolation.View"));
            map.AddRoute(new ClientRoute("surveysubjectrequirement", "B4.controller.dict.SurveySubjectRequirement", requiredPermission: "GkhGji.Dict.SurveySubjectRequirement.View"));
            map.AddRoute(new ClientRoute("resolveviolationclaim", "B4.controller.dict.ResolveViolationClaim", requiredPermission: "GkhGji.Dict.ResolveViolationClaim.View"));
            map.AddRoute(new ClientRoute("kindbasedocument", "B4.controller.dict.KindBaseDocument", requiredPermission: "GkhGji.Dict.KindBaseDocument.View"));
            map.AddRoute(new ClientRoute("notificationcauses", "B4.controller.dict.NotificationCause", requiredPermission: "GkhGji.Dict.NotificationCause.View"));
            map.AddRoute(new ClientRoute("mkdmanagementmethods", "B4.controller.dict.MkdManagementMethod", requiredPermission: "GkhGji.Dict.MkdManagementMethod.View"));
            map.AddRoute(new ClientRoute("inspectionbasetype", "B4.controller.dict.InspectionBaseType", requiredPermission: "GkhGji.Dict.InspectionBaseType.View"));
            map.AddRoute(new ClientRoute("concederationresult", "B4.controller.dict.ConcederationResult", requiredPermission: "GkhGji.Dict.ConcederationResult.View"));

            map.AddRoute(new ClientRoute("prodcalendar", "B4.controller.dict.ProdCalendar", requiredPermission: "GkhGji.Dict.RiskCategory.View"));
            map.AddRoute(new ClientRoute("physicalpersondoctype", "B4.controller.dict.PhysicalPersonDocType", requiredPermission: "GkhGji.Dict.PhysicalPersonDocType.View"));

            map.AddRoute(new ClientRoute("importappeal", "B4.controller.Import.Appeal", requiredPermission: "Import.Appeal.View"));
            map.AddRoute(new ClientRoute("decisionmakeingauthoritygji", "B4.controller.dict.DecisionMakingAuthorityGji", requiredPermission: "GkhGji.Dict.DecisionMakingAuthorityGji.View"));
            map.AddRoute(new ClientRoute("paramsgji", "B4.controller.ParamsGji", requiredPermission: "GkhGji.Settings.Params.View"));
            map.AddRoute(new ClientRoute("auditpurposegji", "B4.controller.dict.AuditPurposeGji", requiredPermission: "GkhGji.Dict.AuditPurposeGji.View"));

            map.AddRoute(new ClientRoute("surveyplan", "B4.controller.SurveyPlan", requiredPermission: "GkhGji.SurveyPlan.View"));
            map.AddRoute(new ClientRoute("surveyplanedit/{id}", "B4.controller.surveyplan.Edit", requiredPermission: "GkhGji.SurveyPlan.View"));
            map.AddRoute(new ClientRoute("annextoappealforlicenseissuance", "B4.controller.dict.AnnexToAppealForLicenseIssuance", requiredPermission: "GkhGji.Dict.AnnexToAppealForLicenseIssuance.View"));
            map.AddRoute(new ClientRoute("fuelinfoperiod", "B4.controller.fuelinfo.FuelInfoPeriod", requiredPermission: "GkhGji.HeatSeason.FuelInfoPeriod.View"));
            map.AddRoute(new ClientRoute("fuelinfodetail/{id}", "B4.controller.fuelinfo.FuelInfoDetail", requiredPermission: "GkhGji.HeatSeason.FuelInfoPeriod.View"));

            map.AddRoute(new ClientRoute("appealcitsinfo", "B4.controller.AppealCitsInfo", requiredPermission: "GkhGji.AppealCitizens.AppealCitsInfo.View"));
            map.AddRoute(new ClientRoute("entitylogregistry", "B4.controller.EntityChangeLogRecord", requiredPermission: "GkhGji.AppealCitizens.AppealCitsInfo.View"));

            map.AddRoute(new ClientRoute("specaccownergrid", "SpecialAccountOwner", requiredPermission: "Gkh.Orgs.SpecAccOwner.View"));

            map.AddRoute(new ClientRoute("directoryerknm", "B4.controller.dict.DirectoryERKNM", requiredPermission: "GkhGji.Dict.DirectoryERKNM.View"));

            map.AddRoute(new ClientRoute("emailgji", "B4.controller.EmailGji"));
        }
    }
}