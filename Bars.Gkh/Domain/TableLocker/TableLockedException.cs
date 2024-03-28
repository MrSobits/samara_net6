namespace Bars.Gkh.Domain.TableLocker
{
    using System;

    public class TableLockedException : Exception
    {
        public const string StandardMessage =
            "Выполнение действия невозможно, так как таблица заблокирована. Повторите попытку после снятия блокировки. В зависимости от настроек приложения, "
            + "блокировка будет автоматически выключена либо после завершения расчета, либо после успешного завершения закрытия периода.";

        public TableLockedException(Exception innerException)
            : base(StandardMessage, innerException)
        {
        }

        public TableLockedException()
            : this(null)
        {
        }
    }
}