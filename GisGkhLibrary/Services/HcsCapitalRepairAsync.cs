using GisGkhLibrary.Crypto;
using GisGkhLibrary.Enums;
using GisGkhLibrary.Exceptions;
using GisGkhLibrary.HcsCapitalRepairAsync;
using System;
using System.ServiceModel.Dispatcher;
using System.Linq;
using System.Collections.Generic;

namespace GisGkhLibrary.Services
{
    /// <summary>
    /// Сервис экспорта общих справочников подсистемы НСИ
    /// </summary>
    public static class HcsCapitalRepairAsync
    {
        static CapitalRepairAsyncPortClient service;

        static CapitalRepairAsyncPortClient ServiceInstance => service ?? (service = ServiceHelper<CapitalRepairAsyncPortClient, CapitalRepairAsyncPort>.MakeNew());

        /// <summary>
        /// Экспорт региональной программы капремонта.
        /// </summary>
        /// <param name="OKTMO">ОКТМО территории</param>
        public static AckRequest exportRegionalProgram(string OKTMO, string orgPPAGUID)
        {
            var request = new exportRegionalProgramRequest
            {
                Item = new OKTMORefType() { code = OKTMO },
                Id = Params.ContainerId,
                version = "11.2.0.10"
            };

            AckRequest responce;
            try
            {
                ServiceInstance.exportRegionalProgram(MakeHeader(orgPPAGUID), request, out responce);
                return responce;
            }
            catch (Exception eeeee)
            {
                throw eeeee;
            }
        }

        /// <summary>
        /// Импорт региональной программы капремонта.
        /// </summary>
        /// <param name="OKTMO">ОКТМО территории</param>
        public static AckRequest importRegionalProgram(string OKTMO, string orgPPAGUID)
        {
            var request = new importRegionalProgramRequest
            {
                importRegionalProgram = new importRegionalProgramRequestImportRegionalProgram() { },
                Id = Params.ContainerId,
                version = "11.2.0.10"
            };

            AckRequest responce;
            try
            {
                ServiceInstance.importRegionalProgram(MakeHeader(orgPPAGUID), request, out responce);
                return responce;
            }
            catch (Exception eeeee)
            {
                throw eeeee;
            }
        }

        /// <summary>
        /// Экспорт работ региональной программы капремонта с фильтром по датам.
        /// </summary>
        /// <param name="regProgGIUD">GUID регпрограммы</param>
        /// <param name="startDate">Начальная дата</param>
        /// <param name="endDate">Конечная дата</param>
        public static AckRequest exportRegionalProgramWork(string regProgGIUD, DateTime startDate, DateTime endDate, string orgPPAGUID)
        {
            var request = new exportRegionalProgramWorkRequest
            {
                RegionalProgramGUID = regProgGIUD,
                FilterByEndYearMonth = new exportRegionalProgramWorkRequestFilterByEndYearMonth()
                {
                    StartPeriodYearMonth = startDate.ToShortDateString(),
                    EndPeriodYearMonth = endDate.ToShortDateString()
                },
                Id = Params.ContainerId,
                version = "11.2.0.10"
            };


            AckRequest responce;
            try
            {
                ServiceInstance.exportRegionalProgramWork(MakeHeader(orgPPAGUID), request, out responce);
                return responce;
            }
            catch (Exception eeeee)
            {
                throw eeeee;
            }
        }

        /// <summary>
        /// Импорт работ региональной программы капремонта с фильтром по датам.
        /// </summary>
        /// <param name="regProgGIUD">GUID регпрограммы</param>
        /// <param name="startDate">Начальная дата</param>
        /// <param name="endDate">Конечная дата</param>
        public static AckRequest importRegionalProgramWork(string regProgGIUD, DateTime startDate, DateTime endDate, string orgPPAGUID)
        {
            //тут создать работу
            var work = new importRegionalProgramWorkRequestImportRegionalProgramWork();
            var request = new importRegionalProgramWorkRequest
            {
                RegionalProgramGuid = regProgGIUD,

                importRegionalProgramWork = new importRegionalProgramWorkRequestImportRegionalProgramWork[1] { work },
                Id = Params.ContainerId,
                version = "11.2.0.10"
            };


            AckRequest responce;
            try
            {
                ServiceInstance.importRegionalProgramWork(MakeHeader(orgPPAGUID), request, out responce);
                return responce;
            }
            catch (Exception eeeee)
            {
                throw eeeee;
            }
        }

        /// <summary>
        /// Экспорт региональной программы капремонта (создать запрос)
        /// </summary>
        public static object exportRegionalProgramReq(string OKTMO)
        {
            var request = new exportRegionalProgramRequest
            {
                Item = new OKTMORefType() { code = OKTMO },
                Id = Params.ContainerId,
                version = "11.2.0.10"
            };
            return (object)request;
        }

        /// <summary>
        /// Экспорт региональной программы капремонта (отправить запрос)
        /// </summary>
        public static AckRequest exportRegionalProgramSend(exportRegionalProgramRequest request, string orgPPAGUID)
        {
            AckRequest responce;
            try
            {
                ServiceInstance.exportRegionalProgram(MakeHeader(orgPPAGUID), request, out responce);
                return responce;
            }
            catch (Exception eeeee)
            {
                throw eeeee;
            }
        }

        /// <summary>
        /// Импорт региональной программы капремонта (создать запрос)
        /// </summary>
        public static object importRegionalProgramReq(string startYear, string endYear, string transportGuid)
        {
            var request = new importRegionalProgramRequest
            {
                importRegionalProgram = new importRegionalProgramRequestImportRegionalProgram
                {
                    LoadRegionalProgram = new RegionalProgramPasportType
                    {
                        StartYear = startYear,
                        EndYear = endYear,
                        ProgramName = "Региональная программа капитального ремонта общего имущества в многоквартирных домах"
                    },
                    TransportGuid = transportGuid,
                    ItemElementName = ItemChoiceType2.PublishRegionalProgram,
                    Item = true
                },
                Id = Params.ContainerId,
                version = "11.2.0.10"
            };
            return (object)request;
        }

        /// <summary>
        /// Импорт региональной программы капремонта (отправить запрос)
        /// </summary>
        public static AckRequest importRegionalProgramSend(importRegionalProgramRequest request, string orgPPAGUID)
        {
            AckRequest responce;
            try
            {
                ServiceInstance.importRegionalProgram(MakeHeader(orgPPAGUID), request, out responce);
                return responce;
            }
            catch (Exception eeeee)
            {
                throw eeeee;
            }
        }

        /// <summary>
        /// Экспорт работ региональной программы капремонта (создать запрос)
        /// </summary>
        public static object exportRegionalProgramWorkReq(string programGuid)
        {
            var request = new exportRegionalProgramWorkRequest
            {
                RegionalProgramGUID = programGuid,
                Id = Params.ContainerId,
                version = "11.2.0.10"
            };
            return (object)request;
        }

        /// <summary>
        /// Экспорт работ региональной программы капремонта (отправить запрос)
        /// </summary>
        public static AckRequest exportRegionalProgramWorkSend(exportRegionalProgramWorkRequest request, string orgPPAGUID)
        {
            AckRequest responce;
            try
            {
                ServiceInstance.exportRegionalProgramWork(MakeHeader(orgPPAGUID), request, out responce);
                return responce;
            }
            catch (Exception eeeee)
            {
                throw eeeee;
            }
        }

        /// <summary>
        /// Импорт работ региональной программы капремонта (создать запрос)
        /// </summary>
        public static object importRegionalProgramWorkReq(string programGuid, List<importRegionalProgramWorkRequestImportRegionalProgramWork> works)
        {
            var request = new importRegionalProgramWorkRequest
            {
                RegionalProgramGuid = programGuid,
                importRegionalProgramWork = works.ToArray(),
                Id = Params.ContainerId,
                version = "11.2.0.10"
            };
            return (object)request;
        }

        /// <summary>
        /// Импорт работ региональной программы капремонта (отправить запрос)
        /// </summary>
        public static AckRequest importRegionalProgramWorkSend(importRegionalProgramWorkRequest request, string orgPPAGUID)
        {
            AckRequest responce;
            try
            {
                ServiceInstance.importRegionalProgramWork(MakeHeader(orgPPAGUID), request, out responce);
                return responce;
            }
            catch (Exception eeeee)
            {
                throw eeeee;
            }
        }

        /// <summary>
        /// Экспорт краткосрочной программы капремонта (создать запрос)
        /// </summary>
        public static object exportPlanReq(string[] reqParams)
        {
            var request = new exportPlanRequest
            {
                Items = new object[]
                {
                    (exportPlanRequestType)Enum.Parse(typeof(exportPlanRequestType), reqParams[0]),
                    new OKTMORefType
                    {
                        code = reqParams[1]
                    }
                },
                Id = Params.ContainerId,
                version = "11.2.0.10"
            };
            return (object)request;
        }

        /// <summary>
        /// Экспорт краткосрочной программы капремонта (отправить запрос)
        /// </summary>
        public static AckRequest exportPlanSend(exportPlanRequest request, string orgPPAGUID)
        {
            AckRequest responce;
            try
            {
                ServiceInstance.exportPlan(MakeHeader(orgPPAGUID), request, out responce);
                return responce;
            }
            catch (Exception eeeee)
            {
                throw eeeee;
            }
        }

        /// <summary>
        /// Импорт краткосрочной программы капремонта (создать запрос)
        /// </summary>
        public static object importPlanReq(importPlanRequestImportPlan importPlan)
        {
            var request = new importPlanRequest
            {
                importPlan = importPlan,
                Id = Params.ContainerId,
                version = "11.2.0.10"
            };
            return (object)request;
        }

        /// <summary>
        /// Импорт краткосрочной программы капремонта (отправить запрос)
        /// </summary>
        public static AckRequest importPlanSend(importPlanRequest request, string orgPPAGUID)
        {
            AckRequest responce;
            try
            {
                ServiceInstance.importPlan(MakeHeader(orgPPAGUID), request, out responce);
                return responce;
            }
            catch (Exception eeeee)
            {
                throw eeeee;
            }
        }

        /// <summary>
        /// Экспорт работ краткосрочной программы капремонта (создать запрос)
        /// </summary>
        public static object exportPlanWorkReq(string PlanGuid)
        {
            var request = new exportPlanWorkRequest
            {
                PlanGuid = PlanGuid,
                Id = Params.ContainerId,
                version = "11.2.0.10",
                Limit = 1000
            };
            return (object)request;
        }

        /// <summary>
        /// Экспорт работ краткосрочной программы капремонта (отправить запрос)
        /// </summary>
        public static AckRequest exportPlanWorkSend(exportPlanWorkRequest request, string orgPPAGUID)
        {
            AckRequest responce;
            try
            {
                ServiceInstance.exportPlanWork(MakeHeader(orgPPAGUID), request, out responce);
                return responce;
            }
            catch (Exception eeeee)
            {
                throw eeeee;
            }
        }

        /// <summary>
        /// Импорт работ краткосрочной программы капремонта (создать запрос)
        /// </summary>
        public static object importPlanWorkReq(string PlanGuid, List<importPlanWorkRequestImportPlanWork> works)
        {
            var request = new importPlanWorkRequest
            {
                PlanGUID = PlanGuid,
                importPlanWork = works.ToArray(),
                Id = Params.ContainerId,
                version = "11.2.0.10",
            };
            return (object)request;
        }

        /// <summary>
        /// Имспорт работ краткосрочной программы капремонта (отправить запрос)
        /// </summary>
        public static AckRequest importPlanWorkSend(importPlanWorkRequest request, string orgPPAGUID)
        {
            AckRequest responce;
            try
            {
                var header = MakeHeader(orgPPAGUID);
                responce = ServiceInstance.importPlanWork(ref header, request);
                return responce;
            }
            catch (Exception eeeee)
            {
                throw eeeee;
            }
        }

        /// <summary>
        /// Импорт договора краткосрочной программы капремонта (создать запрос)
        /// </summary>
        public static object importBuildContractReq(string PlanGuid, importContractsRequestImportContract[] importContracts)
        {
            var request = new importContractsRequest
            {
                PlanGUID = PlanGuid,
                importContract = importContracts,
                Id = Params.ContainerId,
                version = "11.10.0.3",
            };
            return (object)request;
        }

        /// <summary>
        /// Импорт договора краткосрочной программы капремонта (отправить запрос)
        /// </summary>
        public static AckRequest importBuildContractSend(importContractsRequest request, string orgPPAGUID)
        {
            AckRequest responce;
            try
            {
                ServiceInstance.importContracts(MakeHeader(orgPPAGUID), request, out responce);
                return responce;
            }
            catch (Exception eeeee)
            {
                throw eeeee;
            }
        }

        /// <summary>
        /// Импорт акта краткосрочной программы капремонта (создать запрос)
        /// </summary>
        public static object importPerfWorkActReq(string ContractGuid, importCertificatesRequestImportCertificate[] importCertificate)
        {
            var request = new importCertificatesRequest
            {
                ContractGuid = ContractGuid,
                importCertificate = importCertificate,
                Id = Params.ContainerId,
                version = "11.0.0.1",
            };
            return (object)request;
        }

        /// <summary>
        /// Импорт акта краткосрочной программы капремонта (отправить запрос)
        /// </summary>
        public static AckRequest importPerfWorkActSend(importCertificatesRequest request, string orgPPAGUID)
        {
            AckRequest responce;
            try
            {
                ServiceInstance.importCertificates(MakeHeader(orgPPAGUID), request, out responce);
                return responce;
            }
            catch (Exception eeeee)
            {
                throw eeeee;
            }
        }

        /// <summary>
        /// Экспорт решений о выборе способа формирования фонда капитального ремонта (создать запрос)
        /// НЕ ИСПОЛЬЗУЕТСЯ И НЕ РАБОТАЕТ!!!!!
        /// </summary>
        public static object exportDecisionsFormingFundReq(List<string> HouseGuid)
        {
            //var request = new exportDecisionsFormingFundRequest
            //{
            //    ItemsElementName = new ItemsChoiceType6[]
            //    {
            //        ItemsChoiceType6.FIASHouseGuid
            //    },
            //    Items = new object[]
            //    {
            //        HouseGuid.ToString()
            //    },
            //    Id = Params.ContainerId,
            //    version = "11.3.0.3"
            //};
            var request = new exportDecisionsFormingFundRequest
            {
                Id = Params.ContainerId,
                version = "11.3.0.3"
            };
            List<ItemsChoiceType6> ItemsElementName = new List<ItemsChoiceType6>();
            List<object> Items = new List<object>();
            foreach (string houseGuid in HouseGuid)
            {
                //ItemsElementName.Add(ItemsChoiceType6.FIASHouseGuid);
                Items.Add(houseGuid);
            }
            //request.ItemsElementName = ItemsElementName.ToArray();
            request.Items = Items.ToArray();
            return (object)request;
        }

        /// <summary>
        /// Экспорт решений о выборе способа формирования фонда капитального ремонта (отправить запрос)
        /// </summary>
        public static AckRequest exportDecisionsFormingFundSend(exportDecisionsFormingFundRequest request, string orgPPAGUID)
        {
            AckRequest responce;
            try
            {
                ServiceInstance.exportDecisionsFormingFund(MakeHeader(orgPPAGUID), request, out responce);
                return responce;
            }
            catch (Exception eeeee)
            {
                throw eeeee;
            }
        }

        /// <summary>
        /// Импорт решений о выборе способа формирования фонда капитального ремонта (создать запрос)
        /// </summary>
        public static object importDecisionsFormingFundReq(importDecisionsFormingFundRequestImportDecision[] importDecision)
        {
            var request = new importDecisionsFormingFundRequest
            {
                importDecision = importDecision,
                Id = Params.ContainerId,
                version = "11.0.0.1"
            };
            return (object)request;
        }

        /// <summary>
        /// Импорт решений о выборе способа формирования фонда капитального ремонта (отправить запрос)
        /// </summary>
        public static AckRequest importDecisionsFormingFundSend(importDecisionsFormingFundRequest request, string orgPPAGUID)
        {
            AckRequest responce;
            try
            {
                ServiceInstance.importDecisionsFormingFund(MakeHeader(orgPPAGUID), request, out responce);
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
                Item = orgPPAGUID,
                //IsOperatorSignature = true,
                //IsOperatorSignatureSpecified = true
            };
        }
    }
}
