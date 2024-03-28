namespace Bars.Gkh.Regions.Tatarstan.Controller

{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.ServiceModel;
    using System.ServiceModel.Channels;
    using System.Text;
    using Microsoft.AspNetCore.Mvc;

    using Bars.B4;
    using Bars.B4.IoC;
    using Bars.B4.Modules.FileStorage;
    using Bars.B4.Utils;
    using Bars.Gkh.Authentification;
    using Bars.Gkh.Domain;
    using Bars.Gkh.Regions.Tatarstan.Entities.Egso;
    using Bars.Gkh.Regions.Tatarstan.Enums;
    using Bars.Gkh.Regions.Tatarstan.Ias.Tatar.IndicatorService;
    using Bars.Gkh.Regions.Tatarstan.IntegrationProvider;

    public class EgsoIntegrationController : FileStorageDataController<EgsoIntegration>
    {
        public IDomainService<EgsoIntegrationValues> MunicipalitiesService { get; set; }

        public IDomainService<EgsoIntegration> EgsoIntegrationDomain { get; set; }

        public IIntegrationProvider<IndicatorClient> IntegrationProvider { get; set; }

        private List<EgsoIntegrationValues> Municipalities { get; set; }
        
        public IFileManager FileManager { get; set; }

        public ActionResult CreateTask(BaseParams baseParams)
        {
            var year = baseParams.Params.GetAs<short>("year");
            var taskType = baseParams.Params.GetAs<EgsoTaskType>("taskType");
            var municipalities = baseParams.Params.GetAs<List<EgsoIntegrationValues>>("municipalities");
            var taskId = baseParams.Params.GetAsId("egsoTaskId");
            
            if (taskId != 0)
            {
                this.Municipalities = this.MunicipalitiesService.GetAll().Where(x => x.EgsoIntegration.Id == taskId).ToList();

                var egsoIntegrationDict = this.EgsoIntegrationDomain.Get(taskId);
                this.RunTask(egsoIntegrationDict,
                    this.GetRequestElement(egsoIntegrationDict),
                    this.EgsoIntegrationDomain);
            }
            else
            {
                this.Municipalities = municipalities;

                IGkhUserManager userManager;
                IDomainService<EgsoIntegration> egsoIntegrationDomainService;

                using (this.Container.Using(userManager = this.Container.Resolve<IGkhUserManager>(),
                    egsoIntegrationDomainService = this.Container.Resolve<IDomainService<EgsoIntegration>>()))
                {
                    var user = userManager.GetActiveUser();

                    if (user == null)
                        return this.JsFailure("Не найден пользователь");
                    var entity = new EgsoIntegration
                    {
                        TaskType = taskType,
                        StateType = EgsoTaskStateType.InProgress,
                        User = user,
                        Year = year
                    };

                    egsoIntegrationDomainService.Save(entity);

                    foreach (var municipality in municipalities)
                    {
                        municipality.EgsoIntegration = entity;
                        this.MunicipalitiesService.Save(municipality);
                    }

                    this.RunTask(entity, this.GetRequestElement(entity), egsoIntegrationDomainService);
                }
            }

            return this.JsSuccess();
        }        
        
        public void RunTask(EgsoIntegration entity, indicator request, IDomainService<EgsoIntegration> egsoIntegrationDomainService)
        {
            var logs = new StringBuilder();
            logs.AppendLine($"Задача: {entity.TaskType.GetDisplayName()}");
            logs.AppendLine($"Время создания: {entity.ObjectCreateDate}");
            
            var client = this.IntegrationProvider.GetSoapClient();
            using (new OperationContextScope(client.InnerChannel))
            {
                var httpRequestProperty = new HttpRequestMessageProperty();
                httpRequestProperty.Headers[System.Net.HttpRequestHeader.Authorization] =
                    "Basic " +
                    Convert.ToBase64String(
                        Encoding.ASCII.GetBytes(client.ClientCredentials.UserName.UserName + ":" + client.ClientCredentials.UserName.Password));

                OperationContext.Current.OutgoingMessageProperties[HttpRequestMessageProperty.Name] =
                    httpRequestProperty;

                var result = client.import_indicator(request);

                entity.EndDate = DateTime.Now;

                logs.AppendLine($"Время выполнения: {entity.EndDate}");
                logs.AppendLine($"Пользователь: {entity.User.Name}");

                if (result.error_code == "0")
                {
                    entity.StateType = EgsoTaskStateType.Completed;
                }
                else
                {
                    entity.StateType = EgsoTaskStateType.CompletedWithErrors;
                    logs.AppendLine($"Описание ошибки: {result.error_name}");
                }
            
                var logFile = this.FileManager.SaveFile(
                    $"Лог_интеграции_с_ЕГСО_ОВ_{entity.EndDate:ddMMyyy_hhmmss}",
                    "txt",
                    Encoding.UTF8.GetBytes(logs.ToString()));

                entity.Log = logFile;
                egsoIntegrationDomainService.Save(entity);
            }
        }
        
        private indicator GetRequestElement(EgsoIntegration entity)
        {
            return new indicator
            {
                agent = "bars",
                indicator_passport = this.GetPassport(entity),
                indicator_values = this.GetValues(entity)
            };
        }

        private indicator_passport GetPassport(EgsoIntegration entity)
        {
            return new indicator_passport
            {
                frequencyId = "27",
                id = entity.TaskType == EgsoTaskType.OverhauledManyApartmentsCount
                    ? "number_of_apartment_buildings_repairs"
                    : "number_of_apartment_buildings",
                measureId = "642",
                name = entity.TaskType == EgsoTaskType.OverhauledManyApartmentsCount
                    ? "Число многоквартирных жилых домов, прошедших капитальный ремонт, на конец периода (по данным ГЖФ)"
                    : "Число многоквартирных жилых домов, на конец периода (по данным ГЖФ)",
                active = true,
                ranks = this.GetRanks(),
                reglament = new reglament
                {

                },
                activity_periods = new period[] { }
            };
        }

        private dimension[] GetRanks()
        {
            return new []
            {
                new dimension
                {
                    n = "0",
                    rank = this.Municipalities
                        .Select(x => new rank
                        {
                            code = x.MunicipalityDict.TerritoryCode,
                            dimension = "0",
                            key = x.MunicipalityDict.EgsoKey,
                            name = x.MunicipalityDict.TerritoryName,
                            type = "oktmo"
                        }).ToArray()
                },
            };
        }

        private item[] GetValues(EgsoIntegration entity)
        {
            return this.Municipalities
                .Select(x => new item
                {
                    ranks = new []
                    {
                        new RankReference
                        {
                            dimension = "0",
                            key = x.MunicipalityDict.EgsoKey
                        }, 
                    },
                    value = x.Value,
                    valueSpecified = true,
                    start_date = new DateTime(entity.Year, 1, 1),
                    end_date = new DateTime(entity.Year, 12, 31),
                    input_date = DateTime.Now.ToString("yyyy-MM-dd")
                }).ToArray();
        }
    }
}