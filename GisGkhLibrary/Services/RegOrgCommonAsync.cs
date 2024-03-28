using GisGkhLibrary.Crypto;
using GisGkhLibrary.Exceptions;
using GisGkhLibrary.RegOrgCommonAsync;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GisGkhLibrary.Services
{
    /// <summary>
    /// Сервис работы с организациями (асинхронный)
    /// </summary>
    public static class RegOrgCommonAsync
    {
        static RegOrgPortsTypeAsyncClient service;

        static RegOrgPortsTypeAsyncClient ServiceInstance => service ?? (service = ServiceHelper<RegOrgPortsTypeAsyncClient, RegOrgPortsTypeAsync>.MakeNew());

        /// <summary>
        /// экспорт сведений об организациях
        /// </summary>
        /// <returns></returns>
        public static object exportOrgRegistryReq(List<Tuple<Dictionary<ItemsChoiceType3, string>, bool?>> searchCriteria, DateTime? lastEditingDate = null)
        {
            var request = new exportOrgRegistryRequest
            {
                Id = Params.ContainerId,
                lastEditingDateFromSpecified = lastEditingDate.HasValue,
                lastEditingDateFrom = !lastEditingDate.HasValue ? default(DateTime) : lastEditingDate.Value,
                SearchCriteria = getSearchCriteria(searchCriteria)
            };

            return request;
        }

        /// <summary>
        /// экспорт сведений об организациях
        /// </summary>
        /// <returns></returns>
        public static AckRequest exportOrgRegistrySend(exportOrgRegistryRequest request)
        {
            var username = Params.UserName;

            AckRequest responce;
            try
            {
                ServiceInstance.exportOrgRegistry(MakeHeader(), request, out responce);
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
                ServiceInstance.getState(MakeHeader(), request, out responce);

                return responce;
            }
            catch (Exception eeeee)
            {
                throw eeeee;
            }
        }

        private static exportOrgRegistryRequestSearchCriteria[] getSearchCriteria(List<Tuple<Dictionary<ItemsChoiceType3, string>, bool?>> searchCriteria)
        {
            var result = new exportOrgRegistryRequestSearchCriteria[1];
            List<exportOrgRegistryRequestSearchCriteria> criterias = new List<exportOrgRegistryRequestSearchCriteria>();
            int i = 0;
            if (searchCriteria.Any())
            {
                foreach (var cr in searchCriteria)
                {
                    criterias.Add(new exportOrgRegistryRequestSearchCriteria
                    {
                        ItemsElementName = cr.Item1 == null ? new ItemsChoiceType3[0] : cr.Item1.Select(x => x.Key).ToArray(),
                        Items = cr.Item1 == null ? new string[0] : cr.Item1.Select(x => x.Value).ToArray(),
                        isRegistered = !cr.Item2.HasValue ? default(bool) : cr.Item2.Value,
                        isRegisteredSpecified = cr.Item2.HasValue
                    });
                }
            }
            //result[0] =
            //        new exportOrgRegistryRequestSearchCriteria
            //        {
            //            ItemsElementName = searchCriteria == null ? new ItemsChoiceType3[0] : searchCriteria.Select(x => x.Key).ToArray(),
            //            Items = (searchCriteria == null) ? new string[0] : searchCriteria.Select(x => x.Value).ToArray(),
            //            isRegistered = !isRegistered.HasValue ? default(bool) : isRegistered.Value,
            //            isRegisteredSpecified = isRegistered.HasValue
            //        };

            return criterias.ToArray();
        }

        private static ISRequestHeader MakeHeader()
        {
            return new ISRequestHeader
            {
                Date = DateTime.Now,
                MessageGUID = Guid.NewGuid().ToString()
            };
        }
    }
}
