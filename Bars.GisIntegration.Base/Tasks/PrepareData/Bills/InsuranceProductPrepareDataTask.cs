namespace Bars.GisIntegration.Base.Tasks.PrepareData.Bills
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4.Utils;
    using Bars.GisIntegration.Base.BillsAsync;
    using Bars.GisIntegration.Base.DataExtractors;
    using Bars.GisIntegration.Base.Entities.Bills;
    using Bars.GisIntegration.Base.Enums;
    using Bars.Gkh.Quartz.Scheduler.Log;

    /// <summary>
    /// Задача подготовки данных для получения orgRootEntityGuid и orgVersionGuid
    /// </summary>
    public class InsuranceProductPrepareDataTask : BasePrepareDataTask<importInsuranceProductRequest>
    {

        /// <summary>
        /// Размер блока предаваемых данных (максимальное количество записей)
        /// </summary>
        private const int Portion = 100;

        /// <summary>
        /// Страховые продукты
        /// </summary>
        private List<InsuranceProduct> insuranceProducts;

        /// <summary>
        /// Собрать данные
        /// </summary>
        /// <param name="parameters">Параметры экспорта</param>
        protected override void ExtractData(DynamicDictionary parameters)
        {
            var insuranceProduct = this.Container.Resolve<BaseSlimDataExtractor<InsuranceProduct>>("InsuranceProductExtractor");

            try
            {
                this.AddLogRecord(new BaseLogRecord(MessageType.Info, "Начато извлечение данных по страховым объектам"));
                this.insuranceProducts = this.RunExtractor(insuranceProduct, parameters);
                this.AddLogRecord(new BaseLogRecord(MessageType.Info, "Завершено извлечение данных по страховым объектам"));
            }
            finally
            {
                this.Container.Release(insuranceProduct);
            }
        }

        /// <summary>
        /// Валидация данных
        /// </summary>
        /// <returns>Результат валидации</returns>
        protected override List<ValidateObjectResult> ValidateData()
        {
            var result = new List<ValidateObjectResult>();
            var itemsToRemove = new List<InsuranceProduct>();

            foreach (var insuranceProduct in this.insuranceProducts)
            {
                if (string.IsNullOrEmpty(insuranceProduct.Name))
                {
                    result.Add(
                        new ValidateObjectResult
                        {
                            Id = insuranceProduct.Id,
                            Description = insuranceProduct.Description,
                            Message = "Не заполнено название вложения",
                            State = ObjectValidateState.Error
                        });

                    itemsToRemove.Add(insuranceProduct);
                }
                else if (string.IsNullOrEmpty(insuranceProduct.Description))
                {
                    result.Add(
                        new ValidateObjectResult
                        {
                            Id = insuranceProduct.Id,
                            Description = insuranceProduct.Description,
                            Message = "Не заполнено описание вложения",
                            State = ObjectValidateState.Error
                        });

                    itemsToRemove.Add(insuranceProduct);
                }
            }

            foreach (var itemToRemove in itemsToRemove)
            {
                this.insuranceProducts.Remove(itemToRemove);
            }
            return result;
        }

        
        /// <summary>
        /// Сформировать объекты запросов к асинхронному сервису ГИС
        /// </summary>
        /// <returns>Словарь Объект запроса - Словарь Транспортных идентификаторов: Тип обектов - Словарь: Транспортный идентификатор - Идентификатор объекта</returns>
        protected override Dictionary<importInsuranceProductRequest, Dictionary<Type, Dictionary<string, long>>> GetRequestData()
        {
            var result = new Dictionary<importInsuranceProductRequest, Dictionary<Type, Dictionary<string, long>>>();

            foreach (var iterationList in this.GetPortions())
            {
                var transportGuidDictionary = new Dictionary<Type, Dictionary<string, long>>();
                var request = this.GetRequestObject(iterationList, transportGuidDictionary);

                result.Add(request, transportGuidDictionary);
            }

            return result;
        }

        /// <summary>
        /// Получить объект запроса.
        /// </summary>
        /// <param name="listForImport">Список объектов для импорта</param>
        /// <param name="transportGuidDictionary">Список объектов для импорта</param>
        /// <returns>Объект запроса</returns>
        private importInsuranceProductRequest GetRequestObject(IEnumerable<InsuranceProduct> listForImport, Dictionary<Type, Dictionary<string, long>> transportGuidDictionary)
        {
            var insuranceProductList = new List<importInsuranceProductRequestInsuranceProduct>();

            var insuranceProductTransportGuidDictionary = new Dictionary<string, long>();

            foreach (var insuranceProduct in listForImport)
            {
                var listItem = this.GetInsuranceProductRequest(insuranceProduct);
                insuranceProductList.Add(listItem);
                insuranceProductTransportGuidDictionary.Add(listItem.TransportGUID, insuranceProduct.Id);
            }

            transportGuidDictionary.Add(typeof(InsuranceProduct), insuranceProductTransportGuidDictionary);

            return new importInsuranceProductRequest { Id = Guid.NewGuid().ToString(), InsuranceProduct = insuranceProductList.ToArray()};
        }

        /// <summary>
        /// Создать объект importInsuranceProductRequest по InsuranceProduct
        /// </summary>
        /// <param name="product"></param>
        /// <returns></returns>
        public importInsuranceProductRequestInsuranceProduct GetInsuranceProductRequest(InsuranceProduct product)
        {
            var item = new object();
            if (product.Operation == RisEntityOperation.Create || product.Operation == RisEntityOperation.Update)
            {
                item = this.InsuranceProductCreateOrUpdateRequest(product);
            }
            else
            {
                item = this.InsuranceProductRemoveRequest(product);
            }
            return new importInsuranceProductRequestInsuranceProduct
            {
                Item = item,
                TransportGUID = Guid.NewGuid().ToString()
            };
        }

        /// <summary>
        /// Создать или изменить существующий страховой продукт
        /// </summary>
        /// <param name="product">Страховой продукт</param>
        /// <returns></returns>
        public importInsuranceProductRequestInsuranceProductCreateOrUpdate InsuranceProductCreateOrUpdateRequest(InsuranceProduct product)
        {
            var trasportGuid = Guid.NewGuid().ToString();

            return new importInsuranceProductRequestInsuranceProductCreateOrUpdate
            {
                InsuranceProductGUID = trasportGuid,
                InsuranceOrg = this.Contragent.FullName,

                Description = new AttachmentType
                {
                    Name = product.Name,
                    Description = product.Description 
                    //Attachment = product.Attachment,
                    //AttachmentHASH = product.AttachmentHash
                }
            };
        }

        /// <summary>
        /// Страховой продукт на удаление
        /// </summary>
        /// <param name="product">Страховой продукт</param>
        /// <returns></returns>
        public importInsuranceProductRequestInsuranceProductRemove InsuranceProductRemoveRequest(InsuranceProduct product)
        {
            var trasportGuid = Guid.NewGuid().ToString();

            return new importInsuranceProductRequestInsuranceProductRemove
            {
                InsuranceProductGUID = trasportGuid,
                CloseDate = product.CloseDate
            };
        }

        /// <summary>
        /// Получает список порций объектов ГИС для формирования объектов для запроса.
        /// </summary>
        /// <returns>Список порций объектов ГИС</returns>
        private List<IEnumerable<InsuranceProduct>> GetPortions()
        {
            var result = new List<IEnumerable<InsuranceProduct>>();

            if (this.insuranceProducts.Count > 0)
            {
                var startIndex = 0;
                do
                {
                    result.Add(this.insuranceProducts.Skip(startIndex).Take(InsuranceProductPrepareDataTask.Portion));
                    startIndex += InsuranceProductPrepareDataTask.Portion;
                }
                while (startIndex < this.insuranceProducts.Count);
            }

            return result;
        }
    }
}