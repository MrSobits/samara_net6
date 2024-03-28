namespace Bars.GisIntegration.Smev.Tasks.SendData
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.IoC;
    using Bars.GisIntegration.Base.Enums;
    using Bars.GisIntegration.Base.Tasks.SendData;
    using Bars.Gkh.SignalR;
    using Bars.GisIntegration.Smev.SmevExchangeService.Erp;
    using Bars.GisIntegration.Smev.Tasks.SendData.Base;
    using Bars.GkhGji.Regions.Tatarstan.Entities;

    using Microsoft.AspNet.SignalR;

    public class ProsecutorOfficeSendDataTask : ErpSendDataTask<MessageFromErpGetType>
    {
        private const string ProsecutorCodeStartSymbols = "105016";
        private const int FederalCenterCode = 16; //РТ
        private const int FederalDistrictCode = 5; //Приволжский ФО

        private readonly List<string> listCodes = new List<string>
        {
            "1000000000", //генеральная прокуратура РФ
            "1050000000" //управление ген прокуратуры в ПФО
        };

        /// <inheritdoc />
        protected override PackageProcessingResult ProcessSmevResponse(MessageFromErpGetType response, Dictionary<Type, Dictionary<string, long>> transportGuidDictByType)
        {
            var prosecutorOffices = this.Container.ResolveDomain<ProsecutorOfficeDict>();

            var responseItems = response.Items;

            if (response.Errors?.Any() ?? false)
            {
                throw new Exception(string.Join(";\r\n",
                    response.Errors.Select(x => x.text)));
            }

            using (this.Container.Using(prosecutorOffices))
            {
                using (var transaction = this.Container.Resolve<IDataTransaction>())
                {
                    foreach (var item in responseItems)
                    {
                        var responseItem = (SpecificDictionaryResponseType) item;
                        var attributes = responseItem.Dictionary.AttributeList;

                        if (!attributes.Any(x => string.Equals(x.Name, "prosecBkId"))
                            || !attributes.Any(x => string.Equals(x.Name, "prosecOfficeName")))
                        {
                            //если не справочник прокуратур
                            continue;
                        }

                        this.SaveProsecutors(responseItem.Dictionary.ValueList, prosecutorOffices);
                    }

                    transaction.Commit();
                }

                var prosecutorListForRemove = prosecutorOffices.GetAll()
                    .Where(x => !this.listCodes.Contains(x.Code))
                    .Select(x => x.Id)
                    .ToList();

                prosecutorListForRemove.ForEach(x => prosecutorOffices.Delete(x));
            }

            // TODO: signalR, пример DatabaseConfigStorageBackend для GkhConfigHub
            GlobalHost.ConnectionManager.GetHubContext<ProsecutorsOfficeHub>().Clients.All.refreshGrid();

            return new PackageProcessingResult
            {
                State = PackageState.SuccessProcessed,
                Objects = new List<ObjectProcessingResult>()
            };
        }

        private void SaveProsecutors(string[][] prosecutorsArray, IDomainService<ProsecutorOfficeDict> prosecutorOffices)
        {
            var prosecutorOfficesDict = prosecutorOffices.GetAll()
                .GroupBy(x => x.Code)
                .ToDictionary(x => x.Key, x => x.First());

            foreach (var item in prosecutorsArray)
            {
                if (item.Length < 2)
                {
                    continue;
                }

                var prosecutorCode = item[0];

                if (!prosecutorCode.StartsWith(ProsecutorOfficeSendDataTask.ProsecutorCodeStartSymbols))
                {
                    //если код прокуратуры начинается не с "105016", пропускаем 
                    continue;
                }

                var prosecutorName = item[1];

                this.listCodes.Add(prosecutorCode);

                if (prosecutorOfficesDict.TryGetValue(prosecutorCode, out var prosecutorOffice))
                {
                    prosecutorOffice.Name = prosecutorName;
                    prosecutorOffices.Update(prosecutorOffice);
                    continue;
                }

                var newProsecutor = new ProsecutorOfficeDict
                {
                    Code = prosecutorCode,
                    Name = prosecutorName,
                    FederalDistrictCode = ProsecutorOfficeSendDataTask.FederalDistrictCode,
                    FederalCenterCode = ProsecutorOfficeSendDataTask.FederalCenterCode
                };
                prosecutorOffices.Save(newProsecutor);
            }
        }
    }
}