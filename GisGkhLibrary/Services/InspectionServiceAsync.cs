using GisGkhLibrary.Crypto;
using System;

namespace GisGkhLibrary.Services
{
    using GisGkhLibrary.InspectionServiceAsync;

    /// <summary>
    /// Сервис управления проверками асинхронный
    /// </summary>
    public static class InspectionServiceAsync
    {


        static InspectionPortsTypeAsyncClient service;

        static InspectionPortsTypeAsyncClient ServiceInstance => service ?? (service = ServiceHelper<InspectionPortsTypeAsyncClient, InspectionPortsTypeAsync>.MakeNew());

        /// <summary>
        /// Импорт проверки (создать запрос)
        /// </summary>
        public static object importExaminationsReq(importExaminationsRequestImportExamination[] examinations)
        {


            var request = new importExaminationsRequest
            {
                Id = Params.ContainerId,
                ImportExamination = examinations
            };
            return (object)request;
        }

        /// <summary>
        /// Импорт проверки (отправить запрос)
        /// </summary>
        public static AckRequest importExaminationsSend(importExaminationsRequest request, string orgPPAGUID)
        {
            AckRequest responce;
            try
            {
                ServiceInstance.importExaminations(MakeHeader(orgPPAGUID), request, out responce);
                return responce;
            }
            catch (Exception eeeee)
            {
                throw eeeee;
            }
        }

        /// <summary>
        /// Экспорт проверки (создать запрос)
        /// </summary>
        public static object exportExaminationsReq(DateTime from, DateTime to)
        {
            var request = new exportExaminationsRequest
            {
                Id = Params.ContainerId,
                ItemsElementName = new ItemsChoiceType11[]
                {
                    ItemsChoiceType11.From,
                    ItemsChoiceType11.To
                },
                Items = new object[]
                {
                    from,
                    to
                },
                version = "11.2.0.1"
            };
            return (object)request;
        }

        /// <summary>
        /// Экспорт проверки (отправить запрос)
        /// </summary>
        public static AckRequest exportExaminationsSend(exportExaminationsRequest request, string orgPPAGUID)
        {
            AckRequest responce;
            try
            {
                ServiceInstance.exportExaminations(MakeHeader(orgPPAGUID), request, out responce);
                return responce;
            }
            catch (Exception eeeee)
            {
                throw eeeee;
            }
        }

        /// <summary>
        /// Экспорт плана проверок (создать запрос)
        /// </summary>
        public static object exportInspectionPlansReq(short? from, short? to)
        {
            var request = new exportInspectionPlansRequest
            {
                Id = Params.ContainerId,
                version = "11.2.0.1"
            };
            if (from.HasValue)
            {
                request.YearFrom = from.Value;
                request.YearFromSpecified = true;
            }
            if (to.HasValue)
            {
                request.YearTo = to.Value;
                request.YearToSpecified = true;
            }
            return (object)request;
        }

        /// <summary>
        /// Экспорт плана проверок (отправить запрос)
        /// </summary>
        public static AckRequest exportInspectionPlansSend(exportInspectionPlansRequest request, string orgPPAGUID)
        {
            AckRequest responce;
            try
            {
                ServiceInstance.exportInspectionPlans(MakeHeader(orgPPAGUID), request, out responce);
                return responce;
            }
            catch (Exception eeeee)
            {
                throw eeeee;
            }
        }

        /// <summary>
        /// Импорт плана проверок (создать запрос)
        /// </summary>
        public static object importInspectionPlanReq(importInspectionPlanRequestImportInspectionPlan[] examinations)
        {


            var request = new importInspectionPlanRequest
            {
                Id = Params.ContainerId,
                ImportInspectionPlan = new importInspectionPlanRequestImportInspectionPlan[] { },
                version = "11.5.0.1"
            };
            return (object)request;
        }

        /// <summary>
        /// Импорт проверки (отправить запрос)
        /// </summary>
        public static AckRequest importInspectionPlanSend(importInspectionPlanRequest request, string orgPPAGUID)
        {
            AckRequest responce;
            try
            {
                ServiceInstance.importInspectionPlan(MakeHeader(orgPPAGUID), request, out responce);
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
            return new RequestHeader
            {

                Date = DateTime.Now,
                MessageGUID = Guid.NewGuid().ToString(),
                ItemElementName = ItemChoiceType.orgPPAGUID,
                Item = orgPPAGUID
            };
        }
    }
}
