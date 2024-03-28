using Bars.B4;
using Bars.B4.Modules.FileStorage;
using Bars.B4.Modules.Tasks.Common.Service;
using Bars.Gkh.Entities;
using Bars.GkhGji.Regions.BaseChelyabinsk.Entities;
using Bars.GkhGji.Regions.Voronezh.DomainService;
using Bars.GkhGji.Regions.Voronezh.Entities;
using Bars.GkhGji.Regions.Voronezh.Entities.SMEVPremises;
using Bars.GkhGji.Regions.Voronezh.Enums;
using SMEV3Library.Entities.GetResponseResponse;
using SMEV3Library.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Threading;
using Bars.GkhGji.Regions.BaseChelyabinsk.Enums.SMEV;

namespace Bars.GkhGji.Regions.Voronezh.Tasks.GetSMEVAnswers
{
    /// <summary>
    /// Задача запрос смэв2
    /// </summary>
    public class GetSMEV2AnswersTaskExecutor : ITaskExecutor
    {
        public static string Id = MethodBase.GetCurrentMethod().DeclaringType.FullName;

        public IDomainService<SMEVLivingPlace> SMEVLivingPlaceDomain { get; set; }

        public IDomainService<SMEVStayingPlace> SMEVStayingPlaceDomain { get; set; }

        private ISMEVLivingPlaceService _SMEVLivingPlaceService;

        private ISMEVStayingPlaceService _SMEVStayingPlaceService;

        /// <summary>
        /// Код задачи
        /// </summary>
        public string ExecutorCode { get; private set; }

        public GetSMEV2AnswersTaskExecutor(ISMEVLivingPlaceService SMEVLivingPlaceService, ISMEVStayingPlaceService SMEVStayingPlaceService)
        {            
            _SMEVLivingPlaceService = SMEVLivingPlaceService;
            _SMEVStayingPlaceService = SMEVStayingPlaceService;
        }

        public IDataResult Execute(BaseParams @params, B4.Modules.Tasks.Common.Contracts.ExecutionContext ctx, IProgressIndicator indicator, CancellationToken ct)
        {
            var processLog = new List<string>();
            uint number = 0;

            try
            {
                var hasTaskIdLivingPlace = SMEVLivingPlaceDomain.GetAll()
                    .Where(x => x.TaskId != null || x.TaskId != "")
                    .Where(x => x.RequestState != RequestState.ResponseReceived)
                    .ToList();

                foreach (var entity in hasTaskIdLivingPlace)
                {
                    indicator?.Report(null, 0, $"Запрос ответа {++number}");
                    var result = _SMEVLivingPlaceService.GetResult(entity, indicator);

                    if (result)
                    {
                        processLog.Add($"Запрос по месту жительства {entity.Id} - выполнен успешно.");
                    }
                    else
                    {
                        processLog.Add($"Запрос по месту жительства {entity.Id} - вернул ошибку.");
                    }
                   
                }

                var hasTaskIdStayingPlace = SMEVStayingPlaceDomain.GetAll()
                    .Where(x => x.TaskId != null || x.TaskId != "")
                    .Where(x => x.RequestState != RequestState.ResponseReceived)
                    .ToList();

                foreach (var entity in hasTaskIdStayingPlace)
                {
                    indicator?.Report(null, 0, $"Запрос ответа {++number}");
                    var result = _SMEVStayingPlaceService.GetResult(entity, indicator);

                    if (result)
                    {
                        processLog.Add($"Запрос по месту жительства {entity.Id} - выполнен успешно.");
                    }
                    else
                    {
                        processLog.Add($"Запрос по месту жительства {entity.Id} - вернул ошибку.");
                    }

                }

                return new BaseDataResult(true);
            }
            catch (HttpRequestException e)
            {
                return new BaseDataResult(false, $"Ошибка связи: {e.InnerException}");
            }
            catch (Exception e)
            {
                return new BaseDataResult(false, $"{e.GetType()} {e.Message} {e.InnerException} {e.StackTrace}");
            }
        }
    }
}
