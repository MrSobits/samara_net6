namespace Bars.GisIntegration.Base.Tasks.PrepareData.Nsi
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    using Bars.B4.Utils;
    using Bars.GisIntegration.Base.DataExtractors;
    using Bars.GisIntegration.Base.Entities.Nsi;
    using Bars.GisIntegration.Base.Enums;
    using Bars.GisIntegration.Base.NsiAsync;
    using Bars.GisIntegration.Base.Tasks.PrepareData;

    /// <summary>
    /// Класс экспортер записей справочника «Коммунальные услуги»
    /// </summary>
    public class MunicipalServicesPrepareDataTask : BasePrepareDataTask<importMunicipalServicesRequest>
    {
        /// <summary>
        /// Размер блока предаваемых данных (максимальное количество записей)
        /// </summary>
        private const int Portion = 1000;

        private List<RisMunicipalService> municipalServicesToExport;

        protected override void ExtractData(DynamicDictionary parameters)
        {
            var extractor = this.Container.Resolve<IGisIntegrationDataExtractor>("MunicipalServicesDataExtractor");

            try
            {
                extractor.Contragent = this.Contragent;

                var extractedDataDict = extractor.Extract(parameters);

                this.municipalServicesToExport = extractedDataDict.ContainsKey(typeof(RisMunicipalService))
                    ? extractedDataDict[typeof(RisMunicipalService)].Cast<RisMunicipalService>().ToList()
                    : new List<RisMunicipalService>();
            }
            finally
            {
                this.Container.Release(extractor);
            }

        }

        /// <summary>
        /// Валидация данных
        /// </summary>
        /// <returns>Результат валидации</returns>
        protected override List<ValidateObjectResult> ValidateData()
        {
            var result = new List<ValidateObjectResult>();

            var itemsToRemove = new List<RisMunicipalService>();

            foreach (var item in this.municipalServicesToExport)
            {
                var validateResult = this.CheckListItem(item);

                if (validateResult.State != ObjectValidateState.Success)
                {
                    result.Add(validateResult);
                    itemsToRemove.Add(item);
                }
            }

            foreach (var itemToRemove in itemsToRemove)
            {
                this.municipalServicesToExport.Remove(itemToRemove);
            }

            return result;
        }

        /// <summary>
        /// Сформировать объекты запросов к асинхронному сервису ГИС
        /// </summary>
        /// <returns>Словарь Объект запроса - Словарь Транспортных идентификаторов: Тип обектов - Словарь: Транспортный идентификатор - Идентификатор объекта</returns>
        protected override Dictionary<importMunicipalServicesRequest, Dictionary<Type, Dictionary<string, long>>> GetRequestData()
        {
            var result = new Dictionary<importMunicipalServicesRequest, Dictionary<Type, Dictionary<string, long>>>();

            foreach (var iterationList in this.GetPortions())
            {
                var transportGuidDictionary = new Dictionary<Type, Dictionary<string, long>>();
                var request = this.GetRequestObject(iterationList, transportGuidDictionary);
                request.Id = Guid.NewGuid().ToString();

                result.Add(request, transportGuidDictionary);
            }

            return result;
        }

        /// <summary>
        /// Проверить валидность объекта RisContract
        /// </summary>
        /// <param name="item">Объект RisContract</param>
        /// <returns>Результат валидации</returns>
        private ValidateObjectResult CheckListItem(RisMunicipalService item)
        {
            var messages = new StringBuilder();

            if (item.MunicipalServiceRefCode.IsEmpty() || item.MunicipalServiceRefGuid.IsEmpty())
            {
                messages.Append("MunicipalServiceRef ");
            }

            if (item.MainMunicipalServiceName.IsEmpty())
            {
                messages.Append("MainMunicipalServiceName ");
            }

            if (item.MunicipalResourceRefCode.IsEmpty() || item.MunicipalResourceRefGuid.IsEmpty())
            {
                messages.Append("MunicipalResourceRef ");
            }

            return new ValidateObjectResult
            {
                Id = item.Id,
                State = messages.Length == 0 ? ObjectValidateState.Success : ObjectValidateState.Error,
                Message = messages.ToString(),
                Description = "Коммунальная услуга"
            };
        }

        /// <summary>
        /// Получает список порций объектов ГИС для формирования объектов для запроса.
        /// </summary>
        /// <returns>Список порций объектов ГИС</returns>
        private List<IEnumerable<RisMunicipalService>> GetPortions()
        {
            var result = new List<IEnumerable<RisMunicipalService>>();

            if (this.municipalServicesToExport.Count > 0)
            {
                var startIndex = 0;
                do
                {
                    result.Add(this.municipalServicesToExport.Skip(startIndex).Take(MunicipalServicesPrepareDataTask.Portion));
                    startIndex += MunicipalServicesPrepareDataTask.Portion;
                }
                while (startIndex < this.municipalServicesToExport.Count);
            }

            return result;
        }

        /// <summary>
        /// Получить объект запроса.
        /// </summary>
        /// <param name="listForImport">Список объектов для импорта</param>
        /// <param name="transportGuidDictionary">Список объектов для импорта</param>
        /// <returns>Объект запроса</returns>
        private importMunicipalServicesRequest GetRequestObject(
            IEnumerable<RisMunicipalService> listForImport,
            Dictionary<Type, Dictionary<string, long>> transportGuidDictionary)
        {
            var munServList = new List<importMunicipalServicesRequestImportMainMunicipalService>();

            var munServTransportGuidDictionary = new Dictionary<string, long>();

            foreach (var munServ in listForImport)
            {
                var listItem = this.GetImportMunicipalServiceImportType(munServ);
                munServList.Add(listItem);

                munServTransportGuidDictionary.Add(listItem.TransportGUID, munServ.Id);
            }

            transportGuidDictionary.Add(typeof(RisMunicipalService), munServTransportGuidDictionary);

            return new importMunicipalServicesRequest {ImportMainMunicipalService = munServList.ToArray()};
        }

        /// <summary>
        /// Создать объект importMunicipalServicesRequestImportMainMunicipalService по RisMunicipalService
        /// </summary>
        /// <param name="munServ">Объект типа RisMunicipalService</param>
        /// <returns>Объект типа importMunicipalServicesRequestImportMainMunicipalService</returns>
        private importMunicipalServicesRequestImportMainMunicipalService GetImportMunicipalServiceImportType(RisMunicipalService munServ)
        {
            var transportGuid = Guid.NewGuid().ToString();

            object item;

            if (munServ.SortOrderNotDefined)
            {
                item = true;
            }
            else
            {
                item = munServ.SortOrder;
            }

            return new importMunicipalServicesRequestImportMainMunicipalService
            {
                TransportGUID = transportGuid,
                MunicipalServiceRef = new nsiRef
                {
                    Code = munServ.MunicipalServiceRefCode,
                    GUID = munServ.MunicipalServiceRefGuid
                },
                GeneralNeeds = munServ.GeneralNeeds,
                MainMunicipalServiceName = munServ.MainMunicipalServiceName,
                MunicipalResourceRef = new[]
                {
                    new nsiRef
                    {
                        Code = munServ.MunicipalResourceRefCode,
                        GUID = munServ.MunicipalResourceRefGuid
                    }
                },
                Item = item
            };
        }
    }
}