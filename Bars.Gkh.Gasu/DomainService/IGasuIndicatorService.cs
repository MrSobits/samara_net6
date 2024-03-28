namespace Bars.Gkh.Gasu.DomainService
{
    using Bars.B4;

    public interface IGasuImportExportService
    {
        /// <summary>
        ///  Возваращается данные сервиса ГАСУ.
        /// </summary>
        IDataResult GetServiceData(BaseParams baseParams);

		/// <summary>
		///  Сохраняются данные сервиса ГАСУ.
		/// </summary>
		IDataResult SetServiceData(BaseParams baseParams);

        /// <summary>
        ///  Сформировать и отправить данные для ГАСУ на указанный веб-сервис
        /// </summary>
        IDataResult SendGasu(BaseParams baseParams);
    }
}
