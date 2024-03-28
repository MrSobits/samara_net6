namespace Bars.Gkh.ExecutionAction.Impl
{
    using System;
    using System.Collections.Generic;

    using Bars.B4;
    // TODO: Расскоментировать
        //using Bars.B4.Modules.FIAS.AutoUpdater;
    using Bars.B4.Utils;

    [Repeatable]
    public class FiasAutoUpdaterAction : BaseExecutionAction
    {
        public override string Description => "Автоматическое обновление ФИАС";

        public override string Name => "Автоматическое обновление ФИАС";

        public override Func<IDataResult> Action => this.Execute;

       // public IFiasAutoUpdater FiasAutoUpdater { get; set; }

        public BaseDataResult Execute()
        {
            return null;
            /* var result = false;
              try
              {
                //  var downloadResult = this.FiasAutoUpdater.Download(this.ExecutionParams, CancellationToken);

                  this.CancellationToken.ThrowIfCancellationRequested();
                  this.ExecutionParams.Params["archFilePath"] = downloadResult.Data as string;

                  if (downloadResult?.Success == false)
                  {
                      return BaseDataResult.Error(downloadResult?.Message);
                  }

                 // var updateResult = this.FiasAutoUpdater.Update(this.ExecutionParams, CancellationToken);
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
                  if (result && this.ExecutionParams.Params.GetAs("collectfiles", false, true))
                  {
                     // this.FiasAutoUpdater.CleanFiles(CancellationToken);
                  }
              }*/
        }

        private void StartSetHouseGuidFromFiasHouseAction()
        {
            if (this.ExecutionParams.Params.GetAs("FixHouseGuid", false, true))
            {
                this.ActionCodeList.Add(nameof(SetHouseGuidFromFiasHouseAction));
            }
        }
    }
}