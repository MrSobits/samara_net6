namespace Sobits.GisGkh.StateChange
{
    using System;
    using System.Collections.Generic;

    using Bars.B4;
    using Bars.B4.Modules.States;
    using Bars.Gkh.Authentification;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Entities.Suggestion;
    using Castle.Windsor;
    using Sobits.GisGkh.DomainService;
    using Sobits.GisGkh.Entities;
    using Sobits.GisGkh.Enums;

    public class CitizenSuggestionChangeStateForGisGkhInWorkRule : IRuleChangeStatus
    {
        public IDomainService<GisGkhRequests> GisGkhRequestsDomain { get; set; }
        public IGkhUserManager UserManager { get; set; }

        /// <summary>
        /// Статус успешного завершения
        /// </summary>
        protected bool IsSuccess { get; }

        public string Id => "gji_citizen_suggestion_for_gis_gkh_in_work_rule";

        public string Name => "Формирование запроса в ГИС ЖКХ при принятии обращения ФКР в работу";

        public string TypeId => "gkh_cit_sug";

        public string Description => this.Name;

        public IWindsorContainer Container { get; set; }

        /// <inheritdoc />
        public virtual ValidateResult Validate(IStatefulEntity statefulEntity, State oldState, State newState)
        {
            //IDataResult result = null;
            var citizenSuggestion = statefulEntity as CitizenSuggestion;
            if (citizenSuggestion.GisWork)
            {
                Operator thisOperator = UserManager.GetActiveOperator();
                if (thisOperator.GisGkhContragent == null)
                {
                    return ValidateResult.No("К учётной записи текущего пользователя не привязана организация для работы с ГИС ЖКХ");
                }
                if (string.IsNullOrEmpty(thisOperator.GisGkhContragent.GisGkhOrgPPAGUID))
                {
                    IExportOrgRegistryService exportOrgRegistryService = Container.Resolve<IExportOrgRegistryService>();
                    try
                    {
                        exportOrgRegistryService.SaveRequest(null, new List<long>(){ thisOperator.GisGkhContragent.Id});
                        return ValidateResult.No("У контрагента ГИС ЖКХ текущего пользователя отсутствует идентификатор. Создан запрос на получение информации об организацции");
                    }
                    catch (Exception e)
                    {
                        return ValidateResult.No("У контрагента ГИС ЖКХ текущего пользователя отсутствует идентификатор. Не удалось создать запрос на получение информации об организацции");
                    }
                    finally
                    {
                        Container.Release(exportOrgRegistryService);
                    }
                }
                var req = new GisGkhRequests
                {
                    TypeRequest = GisGkhTypeRequest.importAnswerCR,
                    //ReqDate = DateTime.Now,
                    RequestState = GisGkhRequestState.NotFormed
                };

                GisGkhRequestsDomain.Save(req);
                //IGisGkhRegionalService gisGkhRegionalService = Container.Resolve<IGisGkhRegionalService>();
                IImportAnswerCRService importAnswerCRService = Container.Resolve<IImportAnswerCRService>();
                try
                {
                    //Inspector inspector = gisGkhRegionalService.GetAppealPerformerForGisGkh(appeal);
                    if (citizenSuggestion.ExecutorCrFund == null)
                    {
                        return ValidateResult.No($"Перед переводом обращения в работу выберите исполнителя");
                    }
                    if (string.IsNullOrEmpty(citizenSuggestion.GisGkhGuid))
                    {
                        return ValidateResult.No($"У указанного инспектора-исполнителя отсутствует идентификатор ГИС ЖКХ");
                    }

                    try
                    {
                        importAnswerCRService.SaveAppealRequest(req, citizenSuggestion);
                        return ValidateResult.Yes();
                    }
                    catch (Exception e)
                    {
                        req.RequestState = GisGkhRequestState.Error;
                        GisGkhRequestsDomain.Update(req);
                        return ValidateResult.No($"Не удалось создать запрос в ГИС ЖКХ на назначение обращению ФКР инспектора: {e.GetType()} {e.Message} {e.InnerException} {e.StackTrace}");
                    }
                }
                finally
                {
                    Container.Release(importAnswerCRService);
                }
            }
            else
            {
                return ValidateResult.Yes();
            }
        }
    }
}