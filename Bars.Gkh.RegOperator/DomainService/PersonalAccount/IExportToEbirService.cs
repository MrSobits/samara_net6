namespace Bars.Gkh.RegOperator.DomainService
{
    internal interface IExportToEbirService
    {
        /// <summary>
        /// Экспортировать
        /// </summary>
        long Export(string type, long periodId);
    }
}