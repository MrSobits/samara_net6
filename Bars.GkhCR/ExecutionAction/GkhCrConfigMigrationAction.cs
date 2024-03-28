namespace Bars.GkhCr.ExecutionAction
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.Utils;
    using Bars.Gkh.Config;
    using Bars.Gkh.DomainService.GkhParam;
    using Bars.Gkh.ExecutionAction;

    /// <summary>
    /// Перенос старых настроек модуля в Единые настройки приложения
    /// </summary>
    public class GkhCrConfigMigrationAction : BaseExecutionAction
    {
        /// <summary>
        ///     Действие
        /// </summary>
        public override Func<IDataResult> Action => this.Execute;

        /// <summary>
        ///     Код для регистрации
        /// </summary>
        /// <summary>
        ///     Описание действия
        /// </summary>
        public override string Description => "Перенос старых настроек модуля в Единые настройки приложения";

        /// <summary>
        ///     Название для отображения
        /// </summary>
        public override string Name => "[GkhCr] Перенос старых настроек модуля";

        private BaseDataResult Execute()
        {
            var oldProvider = this.Container.Resolve<IGkhParamService>();
            try
            {
                var newProvider = this.Container.Resolve<IGkhConfigProvider>();
                var parameters = (IDictionary<string, object>) oldProvider.GetParams("GkhCr").Data;
                foreach (var param in parameters)
                {
                    var holder =
                        newProvider.ValueHolders.FirstOrDefault(
                            x => x.Key.StartsWith("GkhCr.") && x.Key.EndsWith(string.Format(".{0}", param.Key)))
                            .Return(x => x.Value);
                    if (holder == null)
                    {
                        continue;
                    }

                    holder.SetValue(param.Value, true);
                }

                var exception = newProvider.SaveChanges();
                return exception == null ? new BaseDataResult() : new BaseDataResult(false, exception.Message);
            }
            finally
            {
                this.Container.Release(oldProvider);
            }
        }
    }
}