using Bars.B4;
// TODO: Расскоментировать
//using Bars.B4.Modules.FIAS.AutoUpdater;
using Bars.B4.Modules.Tasks.Common.Service;
using Bars.B4.Utils;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading;

namespace Bars.Gkh.Tasks.FiasUpdater
{
    public class FiasUpdaterTaskExecutor : ITaskExecutor
    {
        public static string Id = MethodBase.GetCurrentMethod().DeclaringType.FullName;

        /// <summary>
        /// Номер таски - присваивается экзекутором
        /// </summary>
        public string ExecutorCode { get; private set; }

        //public IFiasAutoUpdater FiasAutoUpdater { get; set; }

        public IDataResult Execute(BaseParams @params, B4.Modules.Tasks.Common.Contracts.ExecutionContext ctx, IProgressIndicator indicator, CancellationToken ct)
        {
         /*   var result = false;

            try
            {
                //скачивание файла
                var downloadResult = FiasAutoUpdater.Download(@params, ct, indicator);
                @params.Params["archFilePath"] = downloadResult.Data as string;
                if (downloadResult?.Success == false)
                    return BaseDataResult.Error(downloadResult?.Message);

                //обновление
                var updateResult = FiasAutoUpdater.Update(@params, ct, indicator);
                if (updateResult?.Success == false)
                    return BaseDataResult.Error(downloadResult?.Message);

                //проставление HouseGuid
                //if (@params.Params.GetAs("FixHouseGuid", false, true))
                //{
                //    this.ActionCodeList.Add(nameof(SetHouseGuidFromFiasHouseAction));
                //}

                return new BaseDataResult(new Dictionary<string, string>
                {
                    { "downloadResult.Success", downloadResult?.Success.ToString() },
                    { "downloadResult.Message", downloadResult?.Message },
                    { "downloadResult.Data", downloadResult?.Data.ToStr() },
                    { "updateResult.Success", updateResult?.Success.ToString() },
                    { "updateResult.Message", updateResult?.Message },
                    { "updateResult.Data", updateResult?.Data.ToStr() }
                });
            }
            catch(Exception e)
            {
                return new BaseDataResult(false, $"Сообщение: {e.Message} {e.StackTrace} InnerException: { e.InnerException?.Message ?? "" }");
            }
            finally
            {
                if (result && @params.Params.GetAs("collectfiles", false, true))
                    FiasAutoUpdater.CleanFiles(ct, indicator);
            }*/
         return null;
        }
    }
}
