using GisGkhLibrary.Crypto;
using GisGkhLibrary.Exceptions;
using GisGkhLibrary.BillsServiceAsync;
using System;
using System.Linq;

namespace GisGkhLibrary.Services
{
    using Org.BouncyCastle.Asn1.Ocsp;
    using System.Collections.Generic;

    /// <summary>
    /// Сервис управления начислениями асинхронный
    /// </summary>
    public static class BillServiceAsync
    {


        static BillsPortsTypeAsyncClient service;

        static BillsPortsTypeAsyncClient ServiceInstance => service ?? (service = ServiceHelper<BillsPortsTypeAsyncClient, BillsPortsTypeAsync>.MakeNew());

        /// <summary>
        /// Экспорт сведений о платежных документах (создать запрос)
        /// </summary>
        public static object exportPaymentDocumentDataReq(short year, int month, List<string> accountGuids)
        {
            List<ItemsChoiceType6> ItemsElementName = new List<ItemsChoiceType6>()
            {
                ItemsChoiceType6.Year,
                ItemsChoiceType6.Month
            };
            List<object> Items = new List<object>()
            {
                year,
                month
            };

            foreach (var accGuid in accountGuids)
            {
                ItemsElementName.Add(ItemsChoiceType6.AccountGUID);
                Items.Add(accGuid);
            }

            var request = new exportPaymentDocumentRequest
            {
                ItemsElementName = ItemsElementName.ToArray(),
                Items = Items.ToArray(),
                Id = Params.ContainerId,
                version = "13.1.0.1"
            };
            return (object)request;
        }

        /// <summary>
        /// Экспорт сведений о платежных документах (отправить запрос)
        /// </summary>
        public static AckRequest exportPaymentDocumentDataSend(exportPaymentDocumentRequest request, string orgPPAGUID)
        {
            AckRequest responce;
            try
            {
                ServiceInstance.exportPaymentDocumentData(MakeHeader(orgPPAGUID), request, out responce);
                return responce;
            }
            catch (Exception eeeee)
            {
                throw eeeee;
            }
        }

        /// <summary>
        /// Импорт сведений о платежных документах (создать запрос)
        /// </summary>
        public static object importPaymentDocumentDataReq(short year, int month, List<importPaymentDocumentRequestPaymentInformation> paymentInfos,
                                                            List<importPaymentDocumentRequestPaymentDocument> paymentDocs)
        {
            List<object> ItemsList = new List<object>
            {
                true, // ConfirmAmountsCorrect
                month,
                year
            };
            paymentInfos.ForEach(x =>
            {
                ItemsList.Add(x);
            });
            paymentDocs.ForEach(x =>
            {
                ItemsList.Add(x);
            });
            var request = new importPaymentDocumentRequest
            {
                Items = ItemsList.ToArray(),
                Id = Params.ContainerId,
                version = "11.2.0.16"
            };
            return (object)request;
        }

        /// <summary>
        /// Импорт сведений о платежных документах (отправить запрос)
        /// </summary>
        public static AckRequest importPaymentDocumentDataSend(importPaymentDocumentRequest request, string orgPPAGUID)
        {
            AckRequest responce;
            try
            {
                ServiceInstance.importPaymentDocumentData(MakeHeader(orgPPAGUID), request, out responce);
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
            //var orgPPAGUID = RegOrgManager.GetOrganization(Params.OGRN, Params.KPP).First().PPAGUID.ToString();
            //var tmpdata = RegOrgManager.GetOrganization("1107451016860", "745301001");
            //var orgPPAGUID = tmpdata.First().PPAGUID.ToString();
            //Минсвязи ЧО
            //var orgPPAGUID = "9d029b46-f205-42ca-bdee-4485274b3264";
            //ФКР ЧО
            //var orgPPAGUID = "52d55897-b16b-4bd0-9ed2-e6ce8f9c81e8";
            return new RequestHeader
            {
                
                Date = DateTime.Now,
                MessageGUID = Guid.NewGuid().ToString(),
                ItemElementName = ItemChoiceType4.orgPPAGUID,
                Item = orgPPAGUID
                //IsOperatorSignature = true,
                //IsOperatorSignatureSpecified = true
            };
        }
    }
}
