namespace Bars.Gkh.RegOperator.Domain.PersonalAccountOperations.Impl
{
    using B4;
    using Castle.Windsor;

    /// <summary>
    /// Операция для Зачет средств за ранее выполненные работы
    /// </summary>
    public class DistributeFundsForPerformedWorkOperation : PersonalAccountOperationBase
    {
        /// <summary>
        /// Контейнер
        /// </summary>
        public IWindsorContainer Container { get; set; }

        /// <summary>
        /// Идентификатор
        /// </summary>
        public static string Key
        {
            get { return "DistributeFundsForPerformedWorkOperation"; }
        }

        /// <summary>
        /// Код
        /// </summary>
        public override string Code
        {
            get { return Key; }
        }

        /// <summary>
        /// Наименование
        /// </summary>
        public override string Name
        {
            get { return "Зачет средств за ранее выполненные работы"; }
        }

        /// <summary>
        /// Права доступа
        /// </summary>
        public override string PermissionKey
        {
            get { return "GkhRegOp.PersonalAccount.Registry.Action.PerformedWorkFundsDistribution"; }
        }

        /// <summary>
        /// Выполнить
        /// </summary>
        public override IDataResult Execute(BaseParams baseParams)
        {
            return new BaseDataResult(true);
        }
    }
}