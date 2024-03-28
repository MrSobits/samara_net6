using GisGkhLibrary.Crypto;
using System;

namespace GisGkhLibrary.Services
{
    using GisGkhLibrary.RapServiceAsync;

    /// <summary>
    /// Сервис управления проверками асинхронный
    /// </summary>
    public static class RapServiceAsync
    {


        static RapPortAsyncClient service;

        static RapPortAsyncClient ServiceInstance => service ?? (service = ServiceHelper<RapPortAsyncClient, RapPortAsync>.MakeNew());

        /// <summary>
        /// Импорт постановлений (создать запрос)
        /// </summary>
        public static object importDecreesAndDocumentsDataReq(ImportDecreesAndDocumentsRequestImportDecreesAndDocuments[] decrees)
        {


            var request = new ImportDecreesAndDocumentsRequest
            {
                Id = Params.ContainerId,
                importDecreesAndDocuments = decrees
            };
            return (object)request;
        }

        /// <summary>
        /// Импорт постановлений (отправить запрос)
        /// </summary>
        public static AckRequest importDecreesAndDocumentsDataSend(ImportDecreesAndDocumentsRequest request, string orgPPAGUID)
        {
            AckRequest responce;
            try
            {
                ServiceInstance.importDecreesAndDocumentsData(MakeHeader(orgPPAGUID), request, out responce);
                return responce;
            }
            catch (Exception eeeee)
            {
                throw eeeee;
            }
        }

        /// <summary>
        /// Экспорт постановлений (создать запрос)
        /// </summary>
        public static object exportDecreesAndDocumentsDataReq(DateTime from, DateTime to)
        {


            var request = new ExportDecreesAndDocumentsRequest
            {
                Id = Params.ContainerId,
                ItemsElementName = new ItemsChoiceType3[]
                {
                    ItemsChoiceType3.ProceedingDateFrom,
                    ItemsChoiceType3.ProceedingDateTo
                },
                Items = new object[]
                {
                    from,
                    to
                }
            };
            return (object)request;
        }

        /// <summary>
        /// Экспорт постановлений (отправить запрос)
        /// </summary>
        public static AckRequest exportDecreesAndDocumentsDataSend(ExportDecreesAndDocumentsRequest request, string orgPPAGUID)
        {
            AckRequest responce;
            try
            {
                ServiceInstance.exportDecreesAndDocumentsData(MakeHeader(orgPPAGUID), request, out responce);
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
        public static getStateResult GetState(string MessageGUID, string orgPPAGUID)
        {
            var request = new getStateRequest
            {
                MessageGUID = MessageGUID,
            };
            getStateResult responce;
            try
            {
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
                ItemElementName = ItemChoiceType2.orgPPAGUID,
                Item = orgPPAGUID
                //IsOperatorSignature = true,
                //IsOperatorSignatureSpecified = true
            };
        }
    }
}
