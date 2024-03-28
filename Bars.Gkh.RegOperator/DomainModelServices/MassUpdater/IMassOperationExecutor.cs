namespace Bars.Gkh.RegOperator.DomainModelServices.MassUpdater
{
    /// <summary>
    /// Интерфейс массового обработчика изменений
    /// </summary>
    public interface IMassOperationExecutor
    {
        /// <summary>
        /// Обработать все изменения
        /// </summary>
        /// <param name="useStatelessSession">Используем интерфейс StatelessSession</param>
        void ProcessChanges(bool useStatelessSession);
    }
}