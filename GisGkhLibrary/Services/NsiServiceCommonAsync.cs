using GisGkhLibrary.Crypto;
using GisGkhLibrary.Enums;
using GisGkhLibrary.Exceptions;
using GisGkhLibrary.NsiCommonAsync;
using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.ServiceModel.Dispatcher;

namespace GisGkhLibrary.Services
{
    /// <summary>
    /// Сервис экспорта общих справочников подсистемы НСИ
    /// </summary>
    public static class NsiServiceCommonAsync
    {
        static NsiPortsTypeAsyncClient service;

        // static NsiPortsTypeClient ServiceInstance => service ?? (service = ServiceHelper<NsiPortsTypeClient, NsiPortsType>.MakeNew());
        // static string endpointaddress = "http://127.0.0.1:8080/ext-bus-home-management-service/services/HomeManagement";
        //  static NsiPortsTypeClient ServiceInstance => service ?? (service = ServiceHelper<NsiPortsTypeClient, NsiPortsType>.MakeNew());
        static NsiPortsTypeAsyncClient ServiceInstance => service ?? (service = ServiceHelper<NsiPortsTypeAsyncClient, NsiPortsTypeAsync>.MakeNew());
        

        /// <summary>
        /// Получить перечень общесистемных справочников с указанием даты последнего изменения каждого из них.
        /// </summary>
        public static AckRequest exportNsiList(string version)
        {
            var request = new exportNsiListRequest
            {
                ListGroupSpecified = true,
                version = version,
                ListGroup = ListGroup.NSI,
                Id = Params.ContainerId
            };
           
         
            AckRequest responce;
            try
            {
              //  var tmpdata = RegOrgManager.GetOrganization("1126316006640", "745301001");

                ServiceInstance.exportNsiList(MakeHeader(), request, out responce);
              

                return responce;
            }
            catch (Exception eeeee)
            {
                throw eeeee;
            }
            
        }

        /// <summary>
        /// Получить перечень справочников с указанием даты последнего изменения каждого из них (создать запрос)
        /// </summary>
        public static object exportNsiListReq(string version, ListGroup ListGroup = ListGroup.NSI)
        {
            var request = new exportNsiListRequest
            {
                ListGroupSpecified = true,
                version = version,
                ListGroup = ListGroup,
                Id = Params.ContainerId
            };
            return (object)request;
        }

        /// <summary>
        /// Получить перечень справочников с указанием даты последнего изменения каждого из них (отправить запрос)
        /// </summary>
        public static AckRequest exportNsiListSend(exportNsiListRequest request)
        {
            AckRequest responce;
            try
            {
                ServiceInstance.exportNsiList(MakeHeader(), request, out responce);
                return responce;
            }
            catch (Exception eeeee)
            {
                throw eeeee;
            }
        }

        /// <summary>
        /// Получить данные общесистемного справочника exportDataProviderNsiItem
        /// </summary>
        /// <param name="dictionaryNumber">Реестровый номер справочника.</param>
        ///<param name="ModifiedAfter">Дата и время, измененные после которой элементы справочника должны быть возвращены в ответе. Если не указана, возвращаются все элементы справочника.</param>
        public static AckRequest exportNsiItem(string dictionaryNumber, DateTime? ModifiedAfter = null, ListGroup ListGroup = ListGroup.NSI)
        {
            var request = new exportNsiItemRequest
            {
                RegistryNumber = dictionaryNumber,
                ListGroup = ListGroup,
                ModifiedAfter = ModifiedAfter ?? default(DateTime),
                ModifiedAfterSpecified = ModifiedAfter.HasValue,
                Id = Params.ContainerId
            };

            AckRequest responce;
            try
            {

                ServiceInstance.exportNsiItem(MakeHeader(), request, out responce);

                return responce;
            }
            catch (Exception eeeee)
            {
                throw eeeee;
            }

            //var elements = (NsiItemType)responce.Item;

            //if (elements.NsiItemRegistryNumber != ((int)(dictionary)).ToString())
            //    throw new GISGKHAnswerException($"Запрашивался словарь {(int)dictionary}, а пришел {elements.NsiItemRegistryNumber}");

            //return elements.NsiElement;
        }

        /// <summary>
        /// Получить данные общесистемного справочника exportDataProviderNsiItem (создать запрос)
        /// </summary>
        /// <param name="dictionaryNumber">Реестровый номер справочника.</param>
        ///<param name="ModifiedAfter">Дата и время, измененные после которой элементы справочника должны быть возвращены в ответе. Если не указана, возвращаются все элементы справочника.</param>
        public static object exportNsiItemReq(string dictionaryNumber, ListGroup ListGroup, DateTime? ModifiedAfter = null)
        {
            var request = new exportNsiItemRequest
            {
                RegistryNumber = dictionaryNumber,
                ListGroup = ListGroup,
                ModifiedAfter = ModifiedAfter ?? default(DateTime),
                ModifiedAfterSpecified = ModifiedAfter.HasValue,
                Id = Params.ContainerId,
                version = "10.0.1.2"
            };
            return (object)request;
        }

        /// <summary>
        /// Получить данные общесистемного справочника exportDataProviderNsiItem (отправить запрос)
        /// </summary>
        public static AckRequest exportNsiItemSend(exportNsiItemRequest request)
        {
            AckRequest responce;
            try
            {
                ServiceInstance.exportNsiItem(MakeHeader(), request, out responce);
                return responce;
            }
            catch (Exception eeeee)
            {
                throw eeeee;
            }
            //var elements = (NsiItemType)responce.Item;

            //if (elements.NsiItemRegistryNumber != ((int)(dictionary)).ToString())
            //    throw new GISGKHAnswerException($"Запрашивался словарь {(int)dictionary}, а пришел {elements.NsiItemRegistryNumber}");

            //return elements.NsiElement;
        }

        /// <summary>
        /// Получить данные общесистемного справочника постранично (создать запрос)
        /// </summary>
        /// <param name="dictionaryNumber">Реестровый номер справочника.</param>
        /// <param name="page">Страница выборки. Возвращается по 1000 элементов.</param>
        /// <param name="ModifiedAfter">Дата и время, измененные после которой элементы справочника должны быть возвращены в ответе. Если не указана, возвращаются все элементы справочника.</param>
        public static object exportNsiPagingItemReq(string dictionaryNumber, ListGroup ListGroup, int page, DateTime? ModifiedAfter = null)
        {
            var request = new exportNsiPagingItemRequest
            {
                RegistryNumber = dictionaryNumber,
                ListGroup = ListGroup,
                ModifiedAfter = ModifiedAfter ?? default(DateTime),
                ModifiedAfterSpecified = ModifiedAfter.HasValue,
                Page = page,
                Id = Params.ContainerId,
                version = "10.0.1.2"
            };
            return (object)request;
        }

        /// <summary>
        /// Получить данные общесистемного справочника постранично (отправить запрос)
        /// </summary>
        public static AckRequest exportNsiPagingItemSend(exportNsiPagingItemRequest request)
        {
            AckRequest responce;
            try
            {
                ServiceInstance.exportNsiPagingItem(MakeHeader(), request, out responce);
                return responce;
            }
            catch (Exception eeeee)
            {
                throw eeeee;
            }
        }

        /// <summary>
        /// Получить статус обработки запроса
        /// </summary>
        /// <param name="MessageGUID">Идентификатор сообщения, присвоенный ГИС ЖКХ</param>
        /// <returns></returns>
        public static getStateResult GetState(string MessageGUID)
        {
            var request = new getStateRequest
            {
                MessageGUID = MessageGUID,                
            };
            getStateResult responce;
            try
            {
                //  var tmpdata = RegOrgManager.GetOrganization("1126316006640", "745301001");

                ServiceInstance.getState(MakeHeader(), request, out responce);

                return responce;
            }
            catch (Exception eeeee)
            {
                throw eeeee;
            }
        }

        private static ISRequestHeader MakeHeader()
        {
            return new ISRequestHeader
            {
                Date = DateTime.Now,
                MessageGUID = Guid.NewGuid().ToString(),
            };
        }
    }
}
