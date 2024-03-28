namespace Bars.Gkh.Overhaul.Tat.ExecutionAction
{
    using System;

    using Bars.B4;
    using Bars.Gkh.ExecutionAction;
    using Bars.Gkh.Overhaul.Tat.DomainService;

    public class PublishedProgramMassiveCreationAction : BaseExecutionAction
    {
        public ILongProgramService LongProgramService { get; set; }

        public override string Name => "Массовое создание опубликованных программ";

        public override string Description => @"Массовое создание опубликованных программ на основе результатов корректировки.";

        public override Func<IDataResult> Action => this.Execute;

        private BaseDataResult Execute()
        {
            var baseParams = new BaseParams();
            baseParams.Params.Add("isMass", true);
            return (BaseDataResult) this.LongProgramService.CreateDpkrForPublish(baseParams);
        }
    }
}