namespace Bars.GisIntegration.Base.Tasks.PrepareData.OrgRegistryCommon
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Bars.B4.Utils;
    using Bars.GisIntegration.Base.DataExtractors;
    using Bars.GisIntegration.Base.Enums;
    using Bars.GisIntegration.Base.OrgRegistryCommonAsync;
    using Bars.GisIntegration.Base.Tasks.SendData.OrgRegistryCommon;

    /// <summary>
    /// Задача подготовки данных для получения orgRootEntityGuid и orgVersionGuid
    /// </summary>
    public class OrgRegistryPrepareDataTask : BasePrepareDataTask<exportOrgRegistryRequest>
    {
        /// <summary>
        /// Все контрагенты в ЖКХ с ролями: 
        /// Управляющие организации, Поставщики коммунальных услуг, Поставщики жилищных услуг, Органы местного самоуправления, Региональные операторы, Поставщики ресурсов
        /// </summary>
        private List<ContragentProxy> gkhContragents;
       
        private const int Portion = 100;

        /// <summary>
        /// Собрать данные
        /// </summary>
        /// <param name="parameters">Параметры экспорта</param>
        protected override void ExtractData(DynamicDictionary parameters)
        {
            var selector = this.Container.Resolve<IDataSelector<ContragentProxy>>("ContragentSelector");

            try
            {
                this.gkhContragents = selector.GetExternalEntities(parameters);
            }
            finally 
            {
                this.Container.Release(selector);
            }
        }

        /// <summary>
        /// Валидация данных
        /// </summary>
        /// <returns>Результат валидации</returns>
        protected override List<ValidateObjectResult> ValidateData()
        {
            var result = new List<ValidateObjectResult>();
            var itemsToRemove = new List<ContragentProxy>();

            //TODO таски не должны зависеть от сущностей внешней системы! Переделать на использование рис-сущности!
            //var contactRepo = this.Container.ResolveRepository<ContragentContact>();
            Dictionary<long, string> contactsByContragentId = new Dictionary<long, string>();

            try
            {
                //TODO таски не должны зависеть от сущностей внешней системы! Переделать на использование рис-сущности!
                //contactsByContragentId =
                //    contactRepo.GetAll()
                //        .Where(x => x.Contragent != null)
                //        .Select(x => new
                //        {
                //            x.Contragent.Id,
                //            x.FullName,
                //            x.Phone
                //        })
                //        .ToList()
                //        .GroupBy(x => x.Id)
                //        .ToDictionary(
                //            x => x.Key,
                //            x =>
                //                new string(x.SelectMany(y => y.FullName + " (" + y.Phone + "), ").ToArray())
                //                .Replace(" ()", "").TrimEnd(' ').TrimEnd(','));
            }
            finally 
            {
                //TODO таски не должны зависеть от сущностей внешней системы! Переделать на использование рис-сущности!
                //this.Container.Release(contactRepo);
            }

            foreach (var contragent in this.gkhContragents)
            {
                if (string.IsNullOrEmpty(contragent.Ogrn))
                {
                    result.Add(new ValidateObjectResult
                    {
                        Id = contragent.Id,
                        Description = contragent.Name + " Контакты: " + contactsByContragentId.Get(contragent.Id),
                        Message = "Не заполнен ОГРН",
                        State = ObjectValidateState.Error
                    });

                    itemsToRemove.Add(contragent);
                }
                else if (contragent.OrganizationFormCode == "91" && contragent.Ogrn.Length != 15) // Индивидуальный предприниматель
                {
                    result.Add(new ValidateObjectResult
                    {
                        Id = contragent.Id,
                        Description = contragent.Name + " Контакты: " + contactsByContragentId.Get(contragent.Id),
                        Message = $"Не корректный ОГРНИП : {contragent.Ogrn}",
                        State = ObjectValidateState.Error
                    });

                    itemsToRemove.Add(contragent);
                }
                else if (contragent.OrganizationFormCode != "91" && contragent.Ogrn.Length != 13)
                {
                    result.Add(new ValidateObjectResult
                    {
                        Id = contragent.Id,
                        Description = contragent.Name + " Контакты: " + contactsByContragentId.Get(contragent.Id),
                        Message = $"Не корректный ОГРН : {contragent.Ogrn}",
                        State = ObjectValidateState.Error
                    });

                    itemsToRemove.Add(contragent);
                }
            }

            foreach (var itemToRemove in itemsToRemove)
            {
                this.gkhContragents.Remove(itemToRemove);
            }

            return result;
        }

        /// <summary>
        /// Сформировать объекты запросов к асинхронному сервису ГИС
        /// </summary>
        /// <returns>Словарь Объект запроса - Словарь Транспортных идентификаторов: Тип обектов - Словарь: Транспортный идентификатор - Идентификатор объекта</returns>
        protected override Dictionary<exportOrgRegistryRequest, Dictionary<Type, Dictionary<string, long>>> GetRequestData()
        {
            var result = new Dictionary<exportOrgRegistryRequest, Dictionary<Type, Dictionary<string, long>>>();

            foreach (var iterationList in this.GetPortions())
            {
                var transportGuidDictionary = new Dictionary<Type, Dictionary<string, long>>();
                var request = new exportOrgRegistryRequest
                {
                    SearchCriteria = this.CreateSearchCriterias(iterationList)
                };

                if (this.DataForSigning)
                {
                    request.Id = "block-to-sign";
                }

                result.Add(request, transportGuidDictionary);
            }

            return result;
        }

        /// <summary>
        /// Создать критерии поиска
        /// </summary>
        /// <param name="iterationKeys">Значения ОГРН</param>
        /// <returns>Список объектов критериев</returns>
        private exportOrgRegistryRequestSearchCriteria[] CreateSearchCriterias(IEnumerable<string> iterationKeys)
        {
            var result = new List<exportOrgRegistryRequestSearchCriteria>();

            foreach (var iterationKey in iterationKeys)
            {
                if (iterationKey.Length == 13)
                {
                    var searchCriteria = new exportOrgRegistryRequestSearchCriteria
                    {
                        Items = new[] { iterationKey },
                        ItemsElementName = new[] { ItemsChoiceType3.OGRN }
                    };

                    result.Add(searchCriteria);
                }
                else if (iterationKey.Length == 15)
                {
                    var searchCriteria = new exportOrgRegistryRequestSearchCriteria
                    {
                        Items = new[] { iterationKey },
                        ItemsElementName = new[] { ItemsChoiceType3.OGRNIP }
                    };

                    result.Add(searchCriteria);
                }
            }

            return result.ToArray();
        }

        /// <summary>
        /// Получить порции объектов запроса
        /// </summary>
        /// <returns>Результат</returns>
        private List<IEnumerable<string>> GetPortions()
        {
            var result = new List<IEnumerable<string>>();
            var ogrnValues = this.gkhContragents.Select(x => x.Ogrn).ToList();

            if (ogrnValues.Count > 0)
            {
                var startIndex = 0;
                do
                {
                    result.Add(ogrnValues.Skip(startIndex).Take(OrgRegistryPrepareDataTask.Portion));
                    startIndex += OrgRegistryPrepareDataTask.Portion;
                }
                while (startIndex < ogrnValues.Count);
            }

            return result;
        }
    }
}