﻿using Bars.B4;
using Bars.B4.Modules.Tasks.Common.Service;
using Bars.GkhGji.Regions.BaseChelyabinsk.Entities;
using Bars.GkhGji.Regions.Habarovsk.DomainService;
using Bars.GkhGji.Regions.Habarovsk.Entities;
using Bars.GkhGji.Regions.Habarovsk.Entities.SMEVEmergencyHouse;
using System.Net.Http;
using System.Reflection;
using System.Threading;

namespace Bars.GkhGji.Regions.Habarovsk.Tasks
{
    public class EmergencyHouseTaskExecutor : ITaskExecutor
    {
        private ISMEVEmergencyHouseService _SMEVEmergencyHouseService;

        /// <summary>
        /// Идентификатор
        /// </summary>
        public static string Id = MethodBase.GetCurrentMethod().DeclaringType.FullName;

        public IDomainService<SMEVEmergencyHouse> SMEVEmergencyHouseDomain { get; set; }

        public IDomainService<SMEVEmergencyHouseFile> SMEVEmergencyHouseFileDomain { get; set; }

        /// <summary>
        /// Код задачи
        /// </summary>
        public string ExecutorCode { get; private set; }

        public EmergencyHouseTaskExecutor(ISMEVEmergencyHouseService SMEVEmergencyHouseService)
        {
            _SMEVEmergencyHouseService = SMEVEmergencyHouseService;
        }

        public IDataResult Execute(BaseParams @params, B4.Modules.Tasks.Common.Contracts.ExecutionContext ctx, IProgressIndicator indicator, CancellationToken ct)
        {
            SMEVEmergencyHouse smevRequestData = SMEVEmergencyHouseDomain.Get(long.Parse((string)@params.Params["taskId"]));
            if(smevRequestData == null)
                return new BaseDataResult(false, $"Запрос с ID {@params.Params["taskId"]} не найден");

            //отправка
            try
            {
                var result = _SMEVEmergencyHouseService.SendRequest(smevRequestData, indicator);
                if(!result)
                    return new BaseDataResult(false, smevRequestData.Answer);
                else
                    return new BaseDataResult(true, "Сообщение поставлено в очередь СМЭВ");
            }
            catch(HttpRequestException e)
            {
                return new BaseDataResult(false, $"Ошибка связи: {e.InnerException}");
            }
        }
    }
}
