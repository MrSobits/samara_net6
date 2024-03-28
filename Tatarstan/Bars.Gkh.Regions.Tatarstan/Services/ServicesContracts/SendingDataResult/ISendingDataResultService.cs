namespace Bars.Gkh.Regions.Tatarstan.Services.ServicesContracts.SendingDataResult
{
    using Bars.B4;

    public interface ISendingDataResultService
    {
        /// <summary>
        /// Получить информацию о задолженностях за ЖКУ
        /// </summary>
        /// <param name="risToken">токен кросс-авторизации</param>
        /// <returns></returns>
        IDataResult GetSendingDataResult(string risToken);
    }
}