namespace Bars.GisIntegration.Base.Tasks.PrepareData.OrgRegistry
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    using Bars.B4.Utils;
    using Bars.GisIntegration.Base.DataExtractors;
    using Bars.GisIntegration.Base.Entities.OrgRegistry;
    using Bars.GisIntegration.Base.Enums;
    using Bars.GisIntegration.Base.OrgRegistryAsync;
    using Bars.GisIntegration.Base.Tasks.PrepareData;

    /// <summary>
    /// Задача подготовки данных для экспорта обособленных подразделений
    /// </summary>
    public class SubsidiaryPrepareDataTask : BasePrepareDataTask<importSubsidiaryRequest>
    {
        /// <summary>
        /// Максимальное количество записей обособленных подразделений, которые можно экспортировать одним запросом
        /// </summary>
        private const int PortionSize = 100;

        private List<RisSubsidiary> subsidiariesToExport = new List<RisSubsidiary>();

        /// <summary>
        /// Собрать данные
        /// </summary>
        /// <param name="parameters">Параметры извлечения данных</param>
        protected override void ExtractData(DynamicDictionary parameters)
        {
            var extractor = this.Container.Resolve<IDataExtractor<RisSubsidiary>>("SubsidiaryDataExtractor");

            try
            {
                this.subsidiariesToExport = this.RunExtractor(extractor, parameters);
            }
            finally
            {
                this.Container.Release(extractor);
            }

        }

        /// <summary>
        /// Сформировать объекты запросов к асинхронному сервису ГИС
        /// </summary>
        /// <returns>Словарь Объект запроса - Словарь Транспортных идентификаторов: Тип обектов - Словарь: Транспортный идентификатор - Идентификатор объекта</returns>
        protected override Dictionary<importSubsidiaryRequest, Dictionary<Type, Dictionary<string, long>>> GetRequestData()
        {
            var result = new Dictionary<importSubsidiaryRequest, Dictionary<Type, Dictionary<string, long>>>();

            var subsidiariesToCreateOrUpdate = this.subsidiariesToExport
                .Where(x => x.Operation != RisEntityOperation.Delete)
                .ToList();

            var subsidiaryPortions = this.SplitSubsidiariesToPortions(
                subsidiariesToCreateOrUpdate,
                SubsidiaryPrepareDataTask.PortionSize);

            foreach (var subsidiaryPortion in subsidiaryPortions)
            {
                var transportGuidDictionary = new Dictionary<Type, Dictionary<string, long>>();
                var request = this.CreateImportSubsidiaryRequest(subsidiaryPortion, transportGuidDictionary);

                result.Add(request, transportGuidDictionary);
            }

            return result;
        }

        /// <summary>
        /// Валидация подготовленных данных
        /// </summary>
        /// <returns>Список результатов валидации</returns>
        protected override List<ValidateObjectResult> ValidateData()
        {
            var result = new List<ValidateObjectResult>();

            var subsidiariesToRemove = new List<RisSubsidiary>();

            var subsidiariesToValidate = this.subsidiariesToExport
                .Where(x => x.Operation != RisEntityOperation.Delete)
                .ToList();

            foreach (var subsidiary in subsidiariesToValidate)
            {
                var messages = new StringBuilder();

                if (subsidiary.Parent == null)
                {
                    messages.Append("Parent ");
                }

                if (subsidiary.Operation == RisEntityOperation.Create)
                {
                    if (string.IsNullOrEmpty(subsidiary.FullName))
                    {
                        messages.Append("FullName ");
                    }

                    if (string.IsNullOrEmpty(subsidiary.Ogrn))
                    {
                        messages.Append("Ogrn ");
                    }

                    if (string.IsNullOrEmpty(subsidiary.Inn))
                    {
                        messages.Append("Inn ");
                    }

                    if (string.IsNullOrEmpty(subsidiary.Kpp))
                    {
                        messages.Append("Kpp ");
                    }

                    if (string.IsNullOrEmpty(subsidiary.Okopf))
                    {
                        messages.Append("Okopf ");
                    }

                    if (string.IsNullOrEmpty(subsidiary.Address))
                    {
                        messages.Append("Address ");
                    }

                    if (string.IsNullOrEmpty(subsidiary.SourceName))
                    {
                        messages.Append("SourceName ");
                    }

                    if (!subsidiary.SourceDate.HasValue)
                    {
                        messages.Append("SourceDate");
                    }
                }

                result.Add(new ValidateObjectResult
                {
                    Id = subsidiary.Id,
                    State = messages.Length == 0 ? ObjectValidateState.Success : ObjectValidateState.Error,
                    Message = messages.ToString(),
                    Description = "Сведения об обособленном подразделении"
                });

                if (messages.Length > 0)
                {
                    subsidiariesToRemove.Add(subsidiary);
                }
            }

            foreach (var risSubsidiary in subsidiariesToRemove)
            {
                this.subsidiariesToExport.Remove(risSubsidiary);
            }

            return result;
        }

        /// <summary>
        /// Разбивание списка сведений об обособленных подразделений на порции
        /// </summary>
        /// <param name="subsidiaries">Исходный список обособленных подразделений</param>
        /// <param name="portionSize">Размер порции</param>
        /// <returns>Список порций</returns>
        private List<IEnumerable<RisSubsidiary>> SplitSubsidiariesToPortions(List<RisSubsidiary> subsidiaries, int portionSize)
        {
            var result = new List<IEnumerable<RisSubsidiary>>();

            if (!subsidiaries.Any())
            {
                return result;
            }

            var startIndex = 0;

            do
            {
                result.Add(subsidiaries.Skip(startIndex).Take(portionSize));
                startIndex += portionSize;
            }
            while (startIndex < subsidiaries.Count);


            return result;
        }

        /// <summary>
        /// Создание объекта запроса importSubsidiaryRequest
        /// </summary>
        /// <param name="subsidiaries">Список сведений об обособленных подразделениях</param>
        /// <param name="transportGuidDictionary">Словарь транспортных гуидов</param>
        /// <returns></returns>
        private importSubsidiaryRequest CreateImportSubsidiaryRequest(
            IEnumerable<RisSubsidiary> subsidiaries,
            Dictionary<Type, Dictionary<string, long>> transportGuidDictionary)
        {
            var subsidiaryElements = new List<importSubsidiaryRequestSubsidiary>();
            var subsidiaryTransportGuidDictionary = new Dictionary<string, long>();

            foreach (var subsidiary in subsidiaries)
            {
                object item ;

                var orgVersionGuid = !string.IsNullOrEmpty(subsidiary.Parent.OrgVersionGuid)
                    ? subsidiary.Parent.OrgVersionGuid
                    : subsidiary.Parent.OrgRootEntityGuid;

                if (subsidiary.Operation == RisEntityOperation.Create || string.IsNullOrEmpty(subsidiary.Guid))
                {
                    item = new SubsidiaryImportTypeCreateSubsidiary
                    {
                        orgVersionGUID = orgVersionGuid,
                        FullName = subsidiary.FullName,
                        ShortName = subsidiary.ShortName,
                        OGRN = subsidiary.Ogrn,
                        INN = subsidiary.Inn,
                        KPP = subsidiary.Kpp,
                        OKOPF = subsidiary.Okopf,
                        Address = subsidiary.Address,
                        FIASHouseGuid = subsidiary.FiasHouseGuid,
                        ActivityEndDate = subsidiary.ActivityEndDate.GetValueOrDefault(),
                        ActivityEndDateSpecified = subsidiary.ActivityEndDate.HasValue,
                        SourceName = new SubsidiaryTypeSourceName
                        {
                            Date = subsidiary.SourceDate.GetValueOrDefault(),
                            Value = subsidiary.SourceName
                        }
                    };
                }
                else
                {
                    item = new SubsidiaryImportTypeUpdateSubsidiary()
                    {
                        orgVersionGUID = orgVersionGuid,
                        FullName = subsidiary.FullName,
                        INN = subsidiary.Inn,
                        OKOPF = subsidiary.Okopf,
                        Address = subsidiary.Address,
                        FIASHouseGuid = subsidiary.FiasHouseGuid,
                        ActivityEndDate = subsidiary.ActivityEndDate.GetValueOrDefault(),
                        ActivityEndDateSpecified = subsidiary.ActivityEndDate.HasValue,
                        SourceName = new SubsidiaryImportTypeUpdateSubsidiarySourceName()
                        {
                            Date = subsidiary.SourceDate.GetValueOrDefault(),
                            Value = subsidiary.SourceName
                        }
                    };
                }

                var transportGuid = Guid.NewGuid().ToString();

                var subsidiaryElement = new importSubsidiaryRequestSubsidiary
                {
                    TransportGUID = transportGuid,
                    Item = item
                };

                subsidiaryElements.Add(subsidiaryElement);
                subsidiaryTransportGuidDictionary.Add(transportGuid, subsidiary.Id);
            }

            transportGuidDictionary.Add(typeof(RisSubsidiary), subsidiaryTransportGuidDictionary);

            return new importSubsidiaryRequest { Subsidiary = subsidiaryElements.ToArray() };
        }
    }
}
