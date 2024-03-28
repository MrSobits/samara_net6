using GisGkhLibrary.Crypto;
using GisGkhLibrary.AppealsServiceAsync;
using System;

namespace GisGkhLibrary.Services
{
    using System.Collections.Generic;

    /// <summary>
    /// Сервис управления обращениями асинхронный
    /// </summary>
    public static class AppealsServiceAsync
    {


        static AppealsAsyncPortClient service;

        static AppealsAsyncPortClient ServiceInstance => service ?? (service = ServiceHelper<AppealsAsyncPortClient, AppealsAsyncPort>.MakeNew());

        /// <summary>
        /// Экспорт сведений об обращениях (создать запрос)
        /// </summary>
        public static object exportAppealReq(DateTime? start = null, DateTime? end = null)
        {

            List<ItemsChoiceType5> ItemsElementName = new List<ItemsChoiceType5>();
            List<object> Items = new List<object>();
            //if (start != null)
            //{
            //    ItemsElementName.Add(ItemsChoiceType5.StartDate);
            //    Items.Add(start.Value.Date);
            //}
            //if (end != null)
            //{
            //    ItemsElementName.Add(ItemsChoiceType5.EndDate);
            //    Items.Add(end.Value.Date);
            //}
            ItemsElementName.Add(ItemsChoiceType5.StatusOfAppeal);
            Items.Add(new exportAppealRequestStatusOfAppeal
            {
                Sent = true,
                SentSpecified = true
            });
            var request = new exportAppealRequest
            {
                ItemsElementName = ItemsElementName.ToArray(),
                Items = Items.ToArray(),
                Id = Params.ContainerId,
                version = "12.2.0.11"
            };
            return (object)request;
        }

        /// <summary>
        /// Экспорт сведений об обращениях (отправить запрос)
        /// </summary>
        public static AckRequest exportAppealSend(exportAppealRequest request, string orgPPAGUID)
        {
            AckRequest responce;
            try
            {
                ServiceInstance.exportAppeal(MakeHeader(orgPPAGUID), request, out responce);
                
                return responce;
            }
            catch (Exception eeeee)
            {
                throw eeeee;
            }
        }

        /// <summary>
        /// Импорт ответов на обращени / изменения обращения (создать запрос)
        /// </summary>
        public static object importAnswerReq(importAnswerRequestAppealAction[] appeealAction = null, importAnswerRequestAnswerAction[] answerAction = null)
        {

            List<ItemsChoiceType5> ItemsElementName = new List<ItemsChoiceType5>();
            List<object> Items = new List<object>();
            var request = new importAnswerRequest
            {
                Id = Params.ContainerId,
                version = "12.2.0.13"
            };
            if (answerAction != null)
            {
                request.AnswerAction = answerAction;
            }
            if (appeealAction != null)
            {
                request.AppealAction = appeealAction;
            }

            return (object)request;
        }

        /// <summary>
        /// Импорт ответов на обращени / изменения обращения (отправить запрос)
        /// </summary>
        public static AckRequest importAnswerSend(importAnswerRequest request, string orgPPAGUID)
        {
            AckRequest responce;
            try
            {
                ServiceInstance.importAnswer(MakeHeader(orgPPAGUID), request, out responce);
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
                ItemElementName = ItemChoiceType.orgPPAGUID,
                Item = orgPPAGUID
                //IsOperatorSignature = true,
                //IsOperatorSignatureSpecified = true
            };
        }
    }
}