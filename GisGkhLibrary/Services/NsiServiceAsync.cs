using GisGkhLibrary.Crypto;
using GisGkhLibrary.Enums;
using GisGkhLibrary.Exceptions;
using GisGkhLibrary.NsiServiceAsync;
using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.ServiceModel.Dispatcher;

namespace GisGkhLibrary.Services
{
    /// <summary>
    /// Асинхронный сервис частной НСИ
    /// </summary>
    public static class NsiServiceAsync
    {
        static NsiPortsTypeAsyncClient service;

        static NsiPortsTypeAsyncClient ServiceInstance => service ?? (service = ServiceHelper<NsiPortsTypeAsyncClient, NsiPortsTypeAsync>.MakeNew());
        
        /// <summary>
        /// Экспортировать данные справочников поставщика информации (создать запрос)
        /// </summary>
        /// <param name="dictionaryNumber">Реестровый номер справочника.</param>
        ///<param name="ModifiedAfter">Дата и время, измененные после которой элементы справочника должны быть возвращены в ответе. Если не указана, возвращаются все элементы справочника.</param>
        public static object exportDataProviderNsiItemReq(exportDataProviderNsiItemRequestRegistryNumber dictionaryNumber, DateTime? ModifiedAfter = null)
        {
            var request = new exportDataProviderNsiItemRequest
            {
                RegistryNumber = dictionaryNumber,
                ModifiedAfter = ModifiedAfter ?? default(DateTime),
                ModifiedAfterSpecified = ModifiedAfter.HasValue,
                Id = Params.ContainerId,
                version = "10.0.1.2"
            };
            return (object)request;
        }

        /// <summary>
        /// Экспортировать данные справочников поставщика информации (отправить запрос)
        /// </summary>
        public static AckRequest exportDataProviderNsiItemSend(exportDataProviderNsiItemRequest request, string orgPPAGUID)
        {
            AckRequest responce;
            try
            {
                ServiceInstance.exportDataProviderNsiItem(MakeHeader(orgPPAGUID), request, out responce);
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

        ///// <summary>
        ///// Получить данные общесистемного справочника постранично (создать запрос)
        ///// </summary>
        ///// <param name="dictionaryNumber">Реестровый номер справочника.</param>
        ///// <param name="page">Страница выборки. Возвращается по 1000 элементов.</param>
        ///// <param name="ModifiedAfter">Дата и время, измененные после которой элементы справочника должны быть возвращены в ответе. Если не указана, возвращаются все элементы справочника.</param>
        //public static object exportNsiPagingItemReq(string dictionaryNumber, ListGroup ListGroup, int page, DateTime? ModifiedAfter = null)
        //{
        //    var request = new exportNsiPagingItemRequest
        //    {
        //        RegistryNumber = dictionaryNumber,
        //        ListGroup = ListGroup,
        //        ModifiedAfter = ModifiedAfter ?? default(DateTime),
        //        ModifiedAfterSpecified = ModifiedAfter.HasValue,
        //        Page = page,
        //        Id = Params.ContainerId,
        //        version = "10.0.1.2"
        //    };
        //    return (object)request;
        //}

        ///// <summary>
        ///// Получить данные общесистемного справочника постранично (отправить запрос)
        ///// </summary>
        //public static AckRequest exportNsiPagingItemSend(exportNsiPagingItemRequest request)
        //{
        //    AckRequest responce;
        //    try
        //    {
        //        ServiceInstance.exportNsiPagingItem(MakeHeader(), request, out responce);
        //        return responce;
        //    }
        //    catch (Exception eeeee)
        //    {
        //        throw eeeee;
        //    }
        //}

        /// <summary>
        /// Получить статус обработки запроса
        /// </summary>
        /// <param name="MessageGUID">Идентификатор сообщения, присвоенный ГИС ЖКХ</param>
        /// <returns></returns>
        public static getStateResult GetState(string MessageGUID, string orgPPAGUID)
        {
            var request = new getStateRequest
            {
                MessageGUID = MessageGUID,                
            };
            getStateResult responce;
            try
            {
                //  var tmpdata = RegOrgManager.GetOrganization("1126316006640", "745301001");

                ServiceInstance.getState(MakeHeader(orgPPAGUID), request, out responce);

                return responce;
            }
            catch (Exception eeeee)
            {
                throw eeeee;
            }
        }

        private static RequestHeader MakeHeader(string orgPPAGUID)
        {
            return new RequestHeader
            {

                Date = DateTime.Now,
                MessageGUID = Guid.NewGuid().ToString(),
                ItemElementName = ItemChoiceType1.orgPPAGUID,
                Item = orgPPAGUID
            };
        }
    }
}
