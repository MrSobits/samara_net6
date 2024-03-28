namespace Bars.GisIntegration.Base.Tasks.SendData.CapitalRepair
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using Bars.B4.DataAccess;
    using Bars.B4.Utils;
    using Bars.GisIntegration.Base.CapitalRepair;
    using Bars.GisIntegration.Base.Entities;
    using Bars.GisIntegration.Base.Enums;
    using Bars.GisIntegration.Base.Tasks.SendData;
    using Entities.CapitalRepair;

    public class PlanImportDataTask : BaseSendDataTask<exportPlanRequest, getStateResult, CapitalRepairAsyncPortClient, RequestHeader>
    {
        protected override string ExecuteRequest(RequestHeader header, exportPlanRequest request)
        {
            var soapClient = this.ServiceProvider.GetSoapClient();

            if (soapClient == null)
            {
                throw new Exception("Не удалось получить SOAP клиент");
            }

            AckRequest result;

            soapClient.exportPlan(header, request, out result);

            if (result?.Ack == null)
            {
                throw new Exception("Пустой результат выполенния запроса");
            }

            return result.Ack.MessageGUID;
        }

        /// <summary>
        /// Получить заголовок запроса
        /// </summary>
        /// <returns>Заголовок запроса</returns>
        protected override RequestHeader GetHeader(string messageGuid, RisPackage package)
        {
            return new RequestHeader
            {
                Date = DateTime.Now,
                MessageGUID = messageGuid,
                Item = package.RisContragent.OrgPpaGuid,
                ItemElementName = ItemChoiceType1.orgPPAGUID,
                IsOperatorSignature = package.IsDelegacy,
                IsOperatorSignatureSpecified = package.IsDelegacy
            };
        }

        protected override sbyte GetStateResult(RequestHeader header, string ackMessageGuid, out getStateResult result)
        {
            var request = new getStateRequest { MessageGUID = ackMessageGuid };
            var soapClient = this.ServiceProvider.GetSoapClient();
            result = null;

            if (soapClient != null)
            {
                soapClient.getState(header, request, out result);
            }

            return result?.RequestState ?? 0;
        }

        protected override PackageProcessingResult ProcessResult(
            getStateResult response, Dictionary<Type, Dictionary<string, long>> transportGuidDictByType)
        {
            var packageResult = new PackageProcessingResult
            {
                State = PackageState.SuccessProcessed,
                Objects = new List<ObjectProcessingResult>()
            };

            List<RisCrPlan> existingPlans;
            List<RisCrPlanWork> existingWorks;

            var risPlanDomain = this.Container.ResolveDomain<RisCrPlan>();
            var risPlanWorkDomain = this.Container.ResolveDomain<RisCrPlanWork>();

            try
            {
                existingPlans = risPlanDomain.GetAll().ToList();
                existingWorks = risPlanWorkDomain.GetAll().ToList();
            }
            finally
            {
                this.Container.Release(risPlanDomain);
                this.Container.Release(risPlanWorkDomain);
            }
            

            foreach (var responseItem in response.Items)
            {
                // Ожидается, что responseItem либо exportPlanType, либо ErrorMessageType
                var plan = responseItem as exportPlanType;
                if (plan != null)
                {
                    packageResult.Objects.AddRange(this.ProcessPlan(plan, existingPlans, existingWorks));
                    continue;
                }

                var errorMessage = responseItem as ErrorMessageType;
                if (errorMessage != null)
                {
                    packageResult.Objects.Add(new ObjectProcessingResult
                    {
                        State = ObjectProcessingState.Error,
                        Message = $"Код ошибки: {errorMessage.ErrorCode}. Описание: {errorMessage.Description}"
                    });
                    continue;
                }

                packageResult.Objects.Add(new ObjectProcessingResult
                {
                    State = ObjectProcessingState.Error,
                    Message = "Неожидаемый тип результата запроса"
                });
            }

            return packageResult;
        }

        private List<ObjectProcessingResult> ProcessPlan(exportPlanType gisPlan, List<RisCrPlan> existingPlans, List<RisCrPlanWork> existingWorks)
        {
            var result = new List<ObjectProcessingResult>();

            var risPlan = existingPlans.FirstOrDefault(x => x.Guid == gisPlan.PlanGUID) ?? new RisCrPlan();

            var planProcessingResult = new ObjectProcessingResult
            {
                GisId = gisPlan.PlanGUID,
                RisId = risPlan.Id,
                Description = "План ремонта"
            };

            var mo = gisPlan.Territory.Item as OKTMORefType; // Ожидаем, что придут данные об МО

            if (mo == null)
            {
                planProcessingResult.State = ObjectProcessingState.Error;
                planProcessingResult.Message = "Не получены данные о муниципальном образовании";
                result.Add(planProcessingResult);
                return result;
            }

            DateTime startDate;

            if (string.IsNullOrEmpty(gisPlan.StartMonthYear))
            {
                startDate = DateTime.MinValue;
            }
            else if (DateTime.TryParseExact(gisPlan.StartMonthYear, "yyyy-MM", CultureInfo.InvariantCulture, DateTimeStyles.None, out startDate))
            {
                startDate = new DateTime(startDate.Year, startDate.Month, 1);
            }
            else
            {
                planProcessingResult.State = ObjectProcessingState.Error;
                planProcessingResult.Message = "Не удалось распарсить StartMonthYear";
                result.Add(planProcessingResult);
                return result;
            }

            DateTime endDate;

            if (string.IsNullOrEmpty(gisPlan.EndMonthYear))
            {
                endDate = DateTime.MaxValue;
            }
            else if (DateTime.TryParseExact(gisPlan.EndMonthYear, "yyyy-MM", CultureInfo.InvariantCulture, DateTimeStyles.None, out endDate))
            {
                endDate = new DateTime(endDate.Year, endDate.Month, DateTime.DaysInMonth(endDate.Year, endDate.Month));
            }
            else
            {
                planProcessingResult.State = ObjectProcessingState.Error;
                planProcessingResult.Message = "Не удалось распарсить EndMonthYear";
                result.Add(planProcessingResult);
                return result;
            }

            risPlan.ExternalSystemName = "gis";
            risPlan.Guid = gisPlan.PlanGUID;
            risPlan.Name = gisPlan.Name;
            risPlan.MunicipalityCode = mo.code;
            risPlan.MunicipalityName = mo.name;
            risPlan.StartMonthYear = startDate;
            risPlan.EndMonthYear = endDate;

            planProcessingResult.ObjectsToSave = new List<PersistentObject> { risPlan };
            planProcessingResult.State = ObjectProcessingState.Success;
            planProcessingResult.Message = "OK";
            result.Add(planProcessingResult);


            if (gisPlan.Work.IsEmpty())
            {
                return result;
            }

            foreach (var work in gisPlan.Work)
            {
                var risWork = existingWorks.FirstOrDefault(x => x.Guid == work.WorkGUID) ?? new RisCrPlanWork();

                var workProcessingResult = new ObjectProcessingResult()
                {
                    GisId = work.WorkGUID,
                    RisId = risWork.Id,
                    Description = "Работа плана ремонта"
                };
                
                DateTime workEndDate;
                if (string.IsNullOrEmpty(gisPlan.EndMonthYear))
                {
                    workEndDate = DateTime.MaxValue;
                }
                else if (!DateTime.TryParseExact(work.EndMonthYear, "yyyy-MM", CultureInfo.InvariantCulture, DateTimeStyles.None, out workEndDate))
                {
                    workProcessingResult.State = ObjectProcessingState.Error;
                    workProcessingResult.Message = "Не удалось распарсить EndMonthYear";
                    result.Add(workProcessingResult);
                    continue;
                }

                //TODO: надо ли добавить проверки по остальным полям?

                risWork.ExternalSystemName = "gis";
                risWork.PlanGuid = gisPlan.PlanGUID;
                risWork.Guid = work.WorkGUID;
                risWork.ApartmentBuildingFiasGuid = work.ApartmentBilding;
                risWork.WorkKindCode = work.WorkKind.Code;
                risWork.WorkKindGuid = work.WorkKind.GUID;
                risWork.EndMonthYear = workEndDate.ToString("yyyy-MM");
                risWork.MunicipalityCode = work.OKTMO.code;
                risWork.MunicipalityName = work.OKTMO.name;

                workProcessingResult.ObjectsToSave = new List<PersistentObject> { risWork };
                workProcessingResult.State = ObjectProcessingState.Success;
                workProcessingResult.Message = "OK";

                result.Add(workProcessingResult);
            }

            return result;
        }
    }
}
