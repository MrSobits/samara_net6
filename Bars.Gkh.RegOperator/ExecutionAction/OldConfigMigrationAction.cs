namespace Bars.Gkh.RegOperator.ExecutionAction
{
    using System;

    using Bars.B4;
    using Bars.B4.IoC;
    using Bars.B4.Utils;
    using Bars.Gkh.Config;
    using Bars.Gkh.ConfigSections.RegOperator;
    using Bars.Gkh.ExecutionAction;
    using Bars.Gkh.RegOperator.DomainService.RegoperatorParams;
    using Bars.Gkh.RegOperator.Enums;

    public class OldConfigMigrationAction : BaseExecutionAction
    {
        public static string Id = "OldConfigMigrationAction";

        private BaseDataResult Execute()
        {
            var oldProvider = this.Container.Resolve<IRegoperatorParamsService>();
            using (this.Container.Using(oldProvider))
            {
                var configProv = this.Container.Resolve<IGkhConfigProvider>();
                var config = configProv.Get<RegOperatorConfig>().GeneralConfig;

                config.TypeAccountNumber =
                    oldProvider.GetParamByKey("TypeAccountNumber").To<TypeAccountNumber>();
                var exception = configProv.SaveChanges();
                return exception == null ? new BaseDataResult() : new BaseDataResult(false, exception.Message);
            }
        }

        #region Implementation of IExecutionAction
        /// <summary>
        /// Код для регистрации
        /// </summary>
        public string Code => OldConfigMigrationAction.Id;

        /// <summary>
        /// Описание действия
        /// </summary>
        public override string Description => "Сейчас мигрируют: Способ генерации номера ЛС";

        /// <summary>
        /// Название для отображения
        /// </summary>
        public override string Name => "Регоператор. Миграция части старых конфигов";

        /// <summary>
        /// Действие
        /// </summary>
        public override Func<IDataResult> Action => this.Execute;
        #endregion
    }
}