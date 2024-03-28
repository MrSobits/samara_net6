namespace Bars.Gkh.ConfigSections.RegOperator
{
    using System;

    using Bars.Gkh.Config;
    using Bars.Gkh.Config.Attributes;

    /// <summary>
    ///     Настройка блокировки операций
    /// </summary>
    [Serializable]
    public class OperationLockConfig : IGkhConfigSection
    {
        /// <summary>
        ///     Включить блокировку
        /// </summary>
        [GkhConfigProperty(DisplayName = "Включить блокировку")]
        public virtual bool Enabled { get; set; }

        /// <summary>
        ///     Запрещать выполнение операций с лицевыми счетами после расчета
        /// </summary>
        [GkhConfigProperty(DisplayName = "Запрещать выполнение операций с лицевыми счетами после расчета")]
        public virtual bool PreserveLockAfterCalc { get; set; }

        public Dto ToDto()
        {
            return new Dto { Enabled = Enabled, PreserveLockAfterCalc = PreserveLockAfterCalc };
        }

        [Serializable]
        public class Dto : OperationLockConfig
        {
        }
    }
}