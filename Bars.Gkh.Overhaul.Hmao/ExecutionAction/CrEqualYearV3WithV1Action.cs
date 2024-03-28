namespace Bars.Gkh.Overhaul.Hmao.ExecutionAction
{
    using System;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.Utils;
    using Bars.Gkh.ExecutionAction;
    using Bars.Gkh.Overhaul.Hmao.Entities;

    /// <summary>
    /// Приравнять плановый год - из третьего этапа в первый
    /// </summary>
    public class CrEqualYearV3WithV1Action : BaseExecutionAction
    {
        /// <summary>
        /// Контейнер
        /// </summary>
        /// <summary>
        /// Код
        /// </summary>
        /// <summary>
        /// Код
        /// </summary>
        /// <summary>
        /// Описание
        /// </summary>
        public override string Description => "Приравнять плановый год - из третьего этапа в первый ";

        /// <summary>
        /// Название
        /// </summary>
        public override string Name => "КПКР: Приравнять плановый год - из третьего этапа в первый ";

        /// <summary>
        /// Действие
        /// </summary>
        public override Func<IDataResult> Action => this.Execute;

        private BaseDataResult Execute()
        {
            var v1 = this.Container.Resolve<IDomainService<VersionRecordStage1>>();
            var count = 0;
            try
            {
                v1.GetAll().Where(x => x.Year != x.Stage2Version.Stage3Version.Year).ForEach(
                    x =>
                    {
                        x.Year = x.Stage2Version.Stage3Version.Year;
                        v1.Update(x);
                        count++;
                    });
            }
            finally
            {
                this.Container.Release(v1);
            }

            return new BaseDataResult {Success = true, Message = "Кол-во обновленных записей: " + count};
        }
    }
}