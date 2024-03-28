namespace Bars.Gkh.InspectorMobile.Api.Version1.Services.Impl
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    using Bars.B4.DataAccess;
    using Bars.B4.Utils;
    using Bars.Gkh.Authentification;
    using Bars.Gkh.BaseApiIntegration.Controllers;
    using Bars.Gkh.Enums;
    using Bars.Gkh.InspectorMobile.Api.Version1.Models.Common;
    using Bars.Gkh.InspectorMobile.Api.Version1.Models.Decision;
    using Bars.Gkh.InspectorMobile.Api.Version1.Models.Enums;
    using Bars.Gkh.InspectorMobile.Api.Version1.Services;
    using Bars.GkhGji.Enums;
    using Bars.GkhGji.Regions.Tatarstan.Entities.Decision;

    using NHibernate.Transform;

    /// <summary>
    /// Сервис для взаимодействия с <see cref="Decision"/>
    /// </summary>
    public class DocumentDecisionService : DocumentWithParentService<Decision, DocumentDecision, object, object, DecisionQueryParams>, IDocumentDecisionService
    {
        /// <inheritdoc />
        protected override IEnumerable<DocumentDecision> GetDocumentList(long? documentId = null, DecisionQueryParams queryParams = null, params long[] parentDocumentIds)
        {
            var queryBuilder = new StringBuilder($@"
                    SELECT 
                         decision.id                                                            AS ""Id"",
                         array_to_string(array_agg(DISTINCT pd.id), ',')                        AS ""ParentIdsStr"",
                         decision_doc.inspection_id                                             AS ""InspectionId"",
                         decision_doc.document_number                                           AS ""DocumentNumber"",
                         decision_doc.document_date                                             AS ""DocumentDate"",
                         disp.date_start                                                        AS ""StartDate"",
                         disp.date_end                                                          AS ""EndDate"",
                         dkc.id                                                                 AS ""TypeCheckId"",
                         tat_disp.control_type_id                                               AS ""TypeControlId"",
                         CASE WHEN inspection.person_inspection = {(int)PersonInspection.Official} 
                                OR inspection.person_inspection = {(int)PersonInspection.Organization} 
                              THEN inspection.contragent_id   
                              ELSE NULL     
                         END                                                                    AS ""OrganizationId"",
                         CASE WHEN inspection.person_inspection = {(int)PersonInspection.Official} 
                                OR inspection.person_inspection = {(int)PersonInspection.PhysPerson} 
                              THEN inspection.physical_person   
                              ELSE NULL  
                         END                                                                    AS ""Individual"",
                         dict_insp.fio                                                          AS ""Executive"",
                         string_agg(DISTINCT CAST(gdi.inspector_id AS TEXT), CAST(',' AS TEXT)) AS ""InspectorIdsString"",
                         string_agg(DISTINCT CAST(ro.id AS TEXT), CAST(',' AS TEXT))            AS ""AddressesString"",
                         CASE WHEN gdc.id ISNULL
                              THEN FALSE
                              ELSE TRUE
                         END                                                                    AS ""RelatedDocuments"",
                         CAST(inspection.type_base AS TEXT)                                     AS ""BasisSurvey"",
                         CASE WHEN inspection.person_inspection = {(int)PersonInspection.Official} 
                                OR inspection.person_inspection = {(int)PersonInspection.PhysPerson} 
                              THEN COALESCE(statement.inn, prosclaim.inn, disphead.inn) 
                              ELSE NULL  
                         END                                                                    AS ""Inn"",
                         CASE WHEN disphead.id NOTNULL
                              THEN disphead.document_name || ' №' || disphead.document_number || ' от ' || COALESCE(CAST(date(disphead.document_date) AS TEXT), 'Не задано')
                              WHEN prosclaim.id NOTNULL
                              THEN prosclaim.document_name || ' №' || prosclaim.document_number || ' от ' || COALESCE(CAST(date(prosclaim.document_date) AS TEXT), 'Не задано')
                              WHEN statement.id NOTNULL
                              THEN 'Обращение № ' || string_agg(DISTINCT gac.gji_number, CAST(', ' AS TEXT))
                              WHEN jurperson.id NOTNULL
                              THEN planjur.name
                              WHEN actionisolated.id NOTNULL
                              THEN task_action.document_number || ' ' || COALESCE(CAST(date(task_action.document_date) AS TEXT), '')
                            END                                                                 AS ""DocumentBase""
                    FROM gji_decision decision
                    JOIN gji_disposal disp
					     ON disp.id = decision.id
                    JOIN gji_tat_disposal tat_disp
						 ON tat_disp.id = decision.id
                    JOIN gji_document decision_doc
                         ON decision.id = decision_doc.id
                    JOIN gji_inspection inspection
                         ON inspection.id = decision_doc.inspection_id
                    JOIN gkh_dict_inspector dict_insp
                         ON dict_insp.id = disp.issued_disposal_id
                    JOIN gji_dict_kind_check dkc
                         ON dkc.id = disp.kind_check_id
                    JOIN gji_inspection_robject inro
                         ON inro.inspection_id = decision_doc.inspection_id
                    JOIN gkh_reality_object ro
                         ON inro.reality_object_id = ro.id
                    LEFT JOIN 
                    (
                        gji_document_children dc
                        JOIN gji_document pd
                             ON (dc.parent_id = pd.id AND pd.type_document = {(int)TypeDocumentGji.Prescription})
                    ) ON decision.id = dc.children_id
                    LEFT JOIN 
                    (
                        gji_inspection_statement statement
                        JOIN gji_basestat_appcit gba
                             ON gba.inspection_id = statement.id
                        JOIN gji_appeal_citizens gac
                             ON gba.gji_appcit_id = gac.id
                    ) ON statement.id = decision_doc.inspection_id
                    LEFT JOIN
                    (
                        gji_inspection_jurperson jurperson
                        JOIN gji_dict_planjurperson planjur
                             ON planjur.id = jurperson.plan_id
                    ) ON decision_doc.inspection_id = jurperson.id
                    LEFT JOIN
                    (
                        gji_inspection_actionisolated actionisolated
                        JOIN gji_document task_action
                             ON task_action.inspection_id = actionisolated.action_isolated_id
                             AND task_action.type_document = {(int)TypeDocumentGji.TaskActionIsolated}
                             
                    ) ON actionisolated.id = decision_doc.inspection_id
                    LEFT JOIN gji_inspection_prosclaim prosclaim
                         ON prosclaim.id = decision_doc.inspection_id
                    LEFT JOIN gji_inspection_disphead disphead
                         ON disphead.id = decision_doc.inspection_id
                    LEFT JOIN gji_document_inspector gdi
                         ON gdi.document_id = decision.id
                    LEFT JOIN 
                    (
                        gji_document_children gdc
                        JOIN gji_document doc
                             ON doc.id = gdc.children_id
                        JOIN b4_state state
                             ON state.id = doc.state_id
                             AND NOT state.final_state
                    ) ON gdc.parent_id = decision.id
                ");

            if (queryParams != null)
            {
                this.AppendQueryParams(queryBuilder, queryParams);
            }

            if (documentId != null)
            {
                queryBuilder.AppendLine($"WHERE decision.id = {documentId}");
            }

            queryBuilder.AppendLine(
                @"GROUP BY decision.id,
                           decision_doc.id, disp.id, tat_disp.id, 
                           inspection.id, dict_insp.id, statement.id,
                           jurperson.id, prosclaim.id, disphead.id, planjur.id,
                           actionisolated.id, task_action.id, dkc.id, 
                           CASE WHEN gdc.id ISNULL THEN FALSE ELSE TRUE END");

            var session = this.Container.Resolve<ISessionProvider>().GetCurrentSession();

            var documents = session.CreateSQLQuery(queryBuilder.ToString())
                .SetResultTransformer(Transformers.AliasToBean<DocumentDecision>())
                .List<DocumentDecision>();
            
            this.GetRelatedDocs(documents);

            return documents;
        }

        /// <summary>
        /// Добавить к запросу фильтры из <see cref="DecisionQueryParams"/>
        /// </summary>
        private void AppendQueryParams(StringBuilder queryBuilder, DecisionQueryParams queryParams)
        {
            if (queryParams.PeriodParameter == PeriodParameter.OutOfPeriod && (!queryParams.TypeCheckId.HasValue || !queryParams.DocumentDate.HasValue))
                throw new ApiServiceException($"При значении параметра \"periodParameter\"=4 " + 
                    $"также должны быть заданы параметры \"typeCheckId\" и \"documentDate\"");
            
            const int periodOffset = 4;
            var currentDate = DateTime.Now;
            var userManager = this.Container.Resolve<IGkhUserManager>();
            var userRoles = userManager.GetActiveOperatorRoles().Select(x => x.Name);
            var userInspector = userRoles.Contains("ГЖИ руководство") 
                ? null
                : userManager.GetActiveOperator()?.Inspector;
            var userMunicipalityIds = userManager.GetMunicipalityIds();

            if (!userMunicipalityIds.Any())
            {
                throw new Exception("У пользователя не определено муниципальное образование");
            }

            switch (queryParams.PeriodParameter)
            {
                case PeriodParameter.Current:

                    queryBuilder.AppendLine($@"
                        LEFT JOIN 
                        (
                           gji_actcheck act 
                           JOIN gji_document act_doc
							    ON act_doc.id = act.id
                           JOIN gji_actcheck_robject aro 
                                ON aro.actcheck_id = act.id
                        ) ON  decision_doc.inspection_id = act_doc.inspection_id
                        WHERE disp.date_start <= '{currentDate.ToShortDateString()}'
                              AND (act.id ISNULL OR aro.have_violation = {(int)YesNoNotSet.NotSet})
                    ");

                    break;
                
                case PeriodParameter.Upcoming:

                    queryBuilder.AppendLine($@"
                        WHERE disp.date_start > '{currentDate:d}' 
                              AND disp.date_start <= '{currentDate.AddDays(periodOffset):d}'
                    ");

                    break;
                
                case PeriodParameter.Completed:

                    queryBuilder.AppendLine($@"
                        JOIN  gji_document act_doc
							  ON act_doc.inspection_id = decision_doc.inspection_id
                              AND act_doc.type_document = {(int)TypeDocumentGji.ActCheck}
                        JOIN  gji_actcheck_robject aro
                              ON aro.actcheck_id = act_doc.id
                        WHERE disp.date_end < '{currentDate:d}'
                              AND disp.date_end >= '{currentDate.AddDays(-1 * periodOffset):d}'
                              AND aro.have_violation <> {(int)YesNoNotSet.NotSet}
                    ");

                    break;
                    
                case PeriodParameter.OutOfPeriod:

                    queryBuilder.AppendLine($@"
                        WHERE (disp.date_end < '{currentDate.AddDays(-1 * periodOffset):d}'
                               OR disp.date_start > '{currentDate.AddDays(periodOffset):d}')
                    ");

                    break;
            }

            if (userInspector != null)
            {
                queryBuilder.AppendLine($"AND gdi.inspector_id = {userInspector.Id}");
            }

            if (!string.IsNullOrEmpty(queryParams.Address))
            {
                queryBuilder.AppendLine($"AND LOWER(ro.houseguid) = '{queryParams.Address.ToLower()}'");
            }

            if (!string.IsNullOrEmpty(queryParams.DocumentNumber))
            {
                queryBuilder.AppendLine($"AND decision_doc.document_number ~ '{queryParams.DocumentNumber}'");
            }

            if (!string.IsNullOrEmpty(queryParams.Individual))
            {
                queryBuilder.AppendLine($"AND LOWER(inspection.physical_person) ~ '{queryParams.Individual.ToLower()}'");
            }
            
            if (queryParams.TypeCheckId.HasValue)
            {
                queryBuilder.AppendLine($"AND dkc.code = {(int)queryParams.TypeCheckId}");
            }
            
            if (queryParams.DocumentDate.HasValue)
            {
                queryBuilder.AppendLine($"AND decision_doc.document_date = '{queryParams.DocumentDate:d}'");
            }
            
            if (queryParams.OrganizationId.HasValue)
            {
                queryBuilder.AppendLine($"AND inspection.contragent_id = {queryParams.OrganizationId}");
            }
            
            queryBuilder.AppendLine($"AND  ro.municipality_id IN ({string.Join(", ", userMunicipalityIds)})");
        }

        /// <summary>
        /// Получить связанные документы
        /// </summary>
        private void GetRelatedDocs(ICollection<DocumentDecision> documents)
        {
            var allowedDocumentTypes = new[]
            {
                TypeDocumentGji.Decision,
                TypeDocumentGji.ActCheck,
                TypeDocumentGji.Prescription,
                TypeDocumentGji.Protocol
            };
            var inspectionIds = documents.Select(y => y.InspectionId).ToList();
            var relatedDocsDict = this.DocumentGjiDomain.GetAll()
                .Where(x => inspectionIds.Contains(x.Inspection.Id))
                .Where(x => allowedDocumentTypes.Contains(x.TypeDocumentGji))
                .GroupBy(x => x.Inspection.Id)
                .ToDictionary(x => x.Key,
                    x => new InspectionInfo
                    {
                        InspectionId = x.Key,
                        RelatedDocuments = x.Select(y => new RelatedDocumentInfo
                        {
                            Id = y.Id,
                            DocumentType = y.TypeDocumentGji
                        })
                    });
            
            documents.ForEach(x =>
            {
                var inspectionInfo = relatedDocsDict.Get(x.InspectionId);

                inspectionInfo.RelatedDocuments = inspectionInfo.RelatedDocuments.Where(y => y.Id != x.Id);

                x.Inspection = inspectionInfo;
            });
        }
    }
}