﻿namespace Sobits.GisGkh.StateChange
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using Bars.B4;
    using Bars.B4.Modules.States;
    using Bars.Gkh.Authentification;
    using Bars.Gkh.Entities;
    using Bars.GkhGji.DomainService.GisGkhRegional;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Enums;
    using Castle.Windsor;
    using Sobits.GisGkh.DomainService;
    using Sobits.GisGkh.Entities;
    using Sobits.GisGkh.Enums;

    public class AppealChangeStateForGisGkhCloseRule : IRuleChangeStatus
    {
        public IDomainService<GisGkhRequests> GisGkhRequestsDomain { get; set; }
        public IDomainService<AppealCitsAnswer> AppealCitsAnswerDomain { get; set; }
        public IGkhUserManager UserManager { get; set; }

        /// <summary>
        /// Статус успешного завершения
        /// </summary>
        protected bool IsSuccess { get; }

        public string Id => "gji_appeal_citizens_for_gis_gkh_close_rule";

        public string Name => "Формирование запроса в ГИС ЖКХ при закрытии обращения";

        public string TypeId => "gji_appeal_citizens";

        public string Description => this.Name;

        public IWindsorContainer Container { get; set; }

        /// <inheritdoc />
        public virtual ValidateResult Validate(IStatefulEntity statefulEntity, State oldState, State newState)
        {
            //IDataResult result = null;
            var appeal = statefulEntity as AppealCits;
            if (appeal.GisWork)
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
                    TypeRequest = GisGkhTypeRequest.importAnswer,
                    //ReqDate = DateTime.Now,
                    RequestState = GisGkhRequestState.NotFormed
                };
                GisGkhRequestsDomain.Save(req);
                IGisGkhRegionalService gisRegionalService = Container.Resolve<IGisGkhRegionalService>();
                IImportAnswerService importAnswerService = Container.Resolve<IImportAnswerService>();
                try
                {
                    AppealAnswerType closeType = gisRegionalService.GetAppealAnswerTypeForGisGkh(appeal);
                    try
                    {
                        importAnswerService.SaveAnswerRequest(req, appeal, closeType, thisOperator.GisGkhContragent.GisGkhOrgPPAGUID);
                        return ValidateResult.Yes();
                    }
                    catch (Exception e)
                    {
                        req.RequestState = GisGkhRequestState.Error;
                        GisGkhRequestsDomain.Update(req);
                        return ValidateResult.No($"Не удалось создать запрос в ГИС ЖКХ при закрытии обращения: {e.GetType()} {e.Message} {e.InnerException} {e.StackTrace}");
                    }
                }
                finally
                {
                    Container.Release(gisRegionalService);
                    Container.Release(importAnswerService);
                }
            }
            else
            {
                return ValidateResult.Yes();
            }
        }
    }
}