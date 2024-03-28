namespace Bars.GisIntegration.Tor.Service.SendData
{
    using Bars.B4;

    public interface ISendDataService<T>
    {
        /// <summary>
        /// Метод для отправки данных
        /// </summary>
        IDataResult PrepareData(BaseParams baseParams);

        /// <summary>
        /// Метод получения данных
        /// </summary>
        IDataResult GetData(BaseParams baseParams);

        /// <summary>
        /// Отправить запрос в ТОР КНД
        /// </summary> 
        /// <param name="saveFile">Сохранять файлы?</param>
        IDataResult SendRequest(bool saveFile);
    }
}