using GisGkhLibrary.Crypto;
using System;

namespace GisGkhLibrary.Services
{
    using GisGkhLibrary.LicenseServiceAsync;
    using System.Collections.Generic;

    /// <summary>
    /// Сервис управления лицензиями асинхронный
    /// </summary>
    public static class LicenseServiceAsync
    {


        static LicensePortsTypeAsyncClient service;

        static LicensePortsTypeAsyncClient ServiceInstance => service ?? (service = ServiceHelper<LicensePortsTypeAsyncClient, LicensePortsTypeAsync>.MakeNew());

        /// <summary>
        /// Экспорт лицензий (создать запрос)
        /// </summary>
        public static object exportLicenseReq(List<string> ogrns)
        {
            List<exportLicenseRequestLicenseOrganization> orgList = new List<exportLicenseRequestLicenseOrganization>();
            foreach (var ogrn in ogrns)
            {
                if (ogrn.Length == 13)
                {
                    orgList.Add(new exportLicenseRequestLicenseOrganization
                    {
                        ItemElementName = ItemChoiceType.OGRN,
                        Item = ogrn
                    });
                }
                else if(ogrn.Length == 15)
                {
                    orgList.Add(new exportLicenseRequestLicenseOrganization
                    {
                        ItemElementName = ItemChoiceType.OGRNIP,
                        Item = ogrn
                    });
                }
            }
            var request = new exportLicenseRequest
            {
                Id = Params.ContainerId,
                Items = orgList.ToArray()
            };
            return (object)request;
        }

        /// <summary>
        /// Экспорт лицензий (отправить запрос)
        /// </summary>
        public static AckRequest exportLicenseSend(exportLicenseRequest request, string orgPPAGUID)
        {
            AckRequest responce;
            try
            {
                ServiceInstance.exportLicense(MakeHeader(orgPPAGUID), request, out responce);
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
                ItemElementName = ItemChoiceType1.orgPPAGUID,
                Item = orgPPAGUID
            };
        }
    }
}
