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
    /// Задача подготовки данных записей справочника «Работы и услуги организации»
    /// </summary>
    public class OrganizationWorksPrepareDataTask : BasePrepareDataTask<importOrganizationWorksRequest>
    {
        /// <summary>
        /// Размер блока предаваемых данных (максимальное количество записей)
        /// </summary>
        private const int Portion = 1000;

        private List<RisOrganizationWork> organizationWorksToExport;
        
        protected override void ExtractData(DynamicDictionary parameters)
        {
            var extractor = this.Container.Resolve<IDataExtractor<RisOrganizationWork>>("OrganizationWorksDataExtractor");

            try
            {
                this.organizationWorksToExport = this.RunExtractor(extractor, parameters);
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

            var itemsToRemove = new List<RisOrganizationWork>();

            foreach (var item in this.organizationWorksToExport)
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
                this.organizationWorksToExport.Remove(itemToRemove);
            }

            return result;
        }

        /// <summary>
        /// Сформировать объекты запросов к асинхронному сервису ГИС
        /// </summary>
        /// <returns>Словарь Объект запроса - Словарь Транспортных идентификаторов: Тип обектов - Словарь: Транспортный идентификатор - Идентификатор объекта</returns>
        protected override Dictionary<importOrganizationWorksRequest, Dictionary<Type, Dictionary<string, long>>> GetRequestData()
        {
            var result = new Dictionary<importOrganizationWorksRequest, Dictionary<Type, Dictionary<string, long>>>();

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
        private ValidateObjectResult CheckListItem(RisOrganizationWork item)
        {
            var messages = new StringBuilder();

            if (item.Name.IsEmpty())
            {
                messages.Append("Name ");
            }

            if (item.ServiceTypeCode.IsEmpty() || item.ServiceTypeGuid.IsEmpty())
            {
                messages.Append("ServiceType ");
            }

            if (item.RequiredServices.Count == 0)
            {
                messages.Append("RequiredServices ");
            }

            if (item.StringDimensionUnit.IsEmpty())
            {
                messages.Append("StringDimensionUnit ");
            }

            return new ValidateObjectResult
            {
                Id = item.Id,
                State = messages.Length == 0 ? ObjectValidateState.Success : ObjectValidateState.Error,
                Message = messages.ToString(),
                Description = "Работа/услуга"
            };
        }

        /// <summary>
        /// Получает список порций объектов ГИС для формирования объектов для запроса.
        /// </summary>
        /// <returns>Список порций объектов ГИС</returns>
        private List<IEnumerable<RisOrganizationWork>> GetPortions()
        {
            var result = new List<IEnumerable<RisOrganizationWork>>();

            if (this.organizationWorksToExport.Count > 0)
            {
                var startIndex = 0;
                do
                {
                    result.Add(this.organizationWorksToExport.Skip(startIndex).Take(OrganizationWorksPrepareDataTask.Portion));
                    startIndex += OrganizationWorksPrepareDataTask.Portion;
                }
                while (startIndex < this.organizationWorksToExport.Count);
            }

            return result;
        }

        /// <summary>
        /// Получить объект запроса.
        /// </summary>
        /// <param name="listForImport">Список объектов для импорта</param>
        /// <param name="transportGuidDictionary">Список объектов для импорта</param>
        /// <returns>Объект запроса</returns>
        private importOrganizationWorksRequest GetRequestObject(IEnumerable<RisOrganizationWork> listForImport, Dictionary<Type, Dictionary<string, long>> transportGuidDictionary)
        {
            var orgWorkTransportGuidDictionary = new Dictionary<string, long>();

            var orgWorkListToCreateUpdate = new List<ImportOrganizationWorkType>();
            var orgWorkListToDelete = new List<importOrganizationWorksRequestDeleteOrganizationWork>();

            foreach (var orgWork in listForImport.Where(x => x.Operation != RisEntityOperation.Delete))
            {
                var listItem = this.GetImportOrganizationWorkType(orgWork);
                orgWorkListToCreateUpdate.Add(listItem);

                orgWorkTransportGuidDictionary.Add(listItem.TransportGUID, orgWork.Id);
            }

            foreach (var orgWork in listForImport.Where(x => x.Operation == RisEntityOperation.Delete && x.Guid.IsNotEmpty()))
            {
                var listItem = this.GetDeleteOrganizationWorkType(orgWork);
                orgWorkListToDelete.Add(listItem);

                orgWorkTransportGuidDictionary.Add(listItem.TransportGUID, orgWork.Id);
            }

            transportGuidDictionary.Add(typeof(RisOrganizationWork), orgWorkTransportGuidDictionary);

            return new importOrganizationWorksRequest
            {
                ImportOrganizationWork = orgWorkListToCreateUpdate.ToArray(),
                DeleteOrganizationWork = orgWorkListToDelete.ToArray()
            };
        }

        /// <summary>
        /// Создать объект ImportOrganizationWorkType по RisOrganizationWork
        /// </summary>
        /// <param name="orgWork">Объект типа RisOrganizationWork</param>
        /// <returns>Объект типа ImportOrganizationWorkType</returns>
        private ImportOrganizationWorkType GetImportOrganizationWorkType(RisOrganizationWork orgWork)
        {
            return new ImportOrganizationWorkType
            {
                TransportGUID = Guid.NewGuid().ToString(),
                ElementGuid = orgWork.Guid,
                WorkName = orgWork.Name,
                ServiceTypeRef = new nsiRef
                {
                    Code = orgWork.ServiceTypeCode,
                    GUID = orgWork.ServiceTypeGuid
                },
                RequiredServiceRef = orgWork.RequiredServices
                                            .Select(x => new nsiRef
                                            {
                                                Code = x.RequiredServiceCode,
                                                GUID = x.RequiredServiceGuid
                                            })
                                            .ToArray(),
                Item = orgWork.StringDimensionUnit,
                ItemElementName = ItemChoiceType2.StringDimensionUnit
            };
        }

        /// <summary>
        /// Создать объект GetDeleteOrganizationWorkType по RisOrganizationWork
        /// </summary>
        /// <param name="orgWork">Объект типа RisOrganizationWork</param>
        /// <returns>Объект типа GetDeleteOrganizationWorkType</returns>
        private importOrganizationWorksRequestDeleteOrganizationWork GetDeleteOrganizationWorkType(RisOrganizationWork orgWork)
        {
            return new importOrganizationWorksRequestDeleteOrganizationWork
            {
                TransportGUID = Guid.NewGuid().ToString(),
                ElementGuid = orgWork.Guid
            };
        }
    }
}