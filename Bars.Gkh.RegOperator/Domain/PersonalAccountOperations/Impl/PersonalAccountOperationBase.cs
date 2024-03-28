namespace Bars.Gkh.RegOperator.Domain.PersonalAccountOperations.Impl
{
    using Bars.B4;

    /// <summary>
    /// Базовый класс операции ЛС
    /// </summary>
    public class PersonalAccountOperationBase : IPersonalAccountOperation
    {
        #region Implementation of IPersonalAccountOperation

        /// <summary>
        /// Код операции
        /// </summary>
        public virtual string Code { get; private set; }

        /// <summary>
        /// Наименование операции
        /// </summary>
        public virtual string Name { get; private set; }

        /// <summary>
        /// Ключ прав доступа
        /// </summary>
        public virtual string PermissionKey => string.Empty;

        /// <summary>
        /// Метод выполнения операции
        /// </summary>
        /// <param name="baseParams">Базовые параметры запроса</param>
        /// <returns>Результат операции</returns>
        public virtual IDataResult Execute(BaseParams baseParams)
        {
            return new BaseDataResult();
        }

        /// <summary>
        /// Метод получения данных пользовательского интерфейса
        /// </summary>
        /// <param name="baseParams">Базовые параметры запроса</param>
        /// <returns>Результат операции</returns>
        public virtual IDataResult GetDataForUI(BaseParams baseParams)
        {
            return new ListDataResult();
        }

        #endregion
    }
}