namespace Bars.B4.Modules.FIAS.AutoUpdater.ExecutionActions
{
    using System;
    using System.Collections.Generic;

    using Bars.B4;
    using Bars.B4.Modules.FIAS.AutoUpdater;
    using Bars.B4.Utils;
    using Bars.Gkh.ExecutionAction;
    using Bars.Gkh.ExecutionAction.Impl;

    [Repeatable]
    public class FiasAutoUpdaterAction : BaseExecutionAction
    {
        public override string Description => "Автоматическое обновление ФИАС";

        public override string Name => "Автоматическое обновление ФИАС";

        public override Func<IDataResult> Action => this.Execute;

        public IFiasAutoUpdater FiasAutoUpdater { get; set; }

        public BaseDataResult Execute()
        {
            var result = false;
            try
            {
                this.FiasAutoUpdater.CancellationToken = this.CancellationToken;
                var downloadResult = this.FiasAutoUpdater.Download(this.ExecutionParams);

                this.CancellationToken.ThrowIfCancellationRequested();
                this.ExecutionParams.Params["archFilePath"] = downloadResult.Data as string;

                if (downloadResult?.Success == false)
                {
                    return BaseDataResult.Error(downloadResult?.Message);
                }

                var updateResult = this.FiasAutoUpdater.Update(this.ExecutionParams);
                this.CancellationToken.ThrowIfCancellationRequested();

                this.StartSetHouseGuidFromFiasHouseAction();

                result = (downloadResult?.Success ?? false) && (updateResult?.Success ?? false);
                return new BaseDataResult(new Dictionary<string, string>
                {
                    { "downloadResult.Success", downloadResult?.Success.ToString() },
                    { "downloadResult.Message", downloadResult?.Message },
                    { "downloadResult.Data", downloadResult?.Data.ToStr() },
                    { "updateResult.Success", updateResult?.Success.ToString() },
                    { "updateResult.Message", updateResult?.Message },
                    { "updateResult.Data", updateResult?.Data.ToStr() }
                })
                {
                    Success = result
                };
            }
            finally
            {
                if (this.ExecutionParams.Params.GetAs("collectfiles", false, true))
                {
                    this.FiasAutoUpdater.CollectUpdateFiles();
                }
            }
        }

        private void StartSetHouseGuidFromFiasHouseAction()
        {
            if (this.ExecutionParams.Params.GetAs("FixHouseGuid", false, true))
            {
                this.ActionCodeList.Add(nameof(SetHouseGuidFromFiasHouseAction));
            }
            if (this.ExecutionParams.Params.GetAs("CheckOktmo", false, true))
            {
                this.ActionCodeList.Add(nameof(CheckFiasOktmoAction));
            }
        }
    }
}