namespace Bars.Gkh.Regions.Tatarstan.Services.ServicesContracts.RisDebtInfo
{
    using Microsoft.AspNetCore.Mvc;

    using Bars.B4;

    public interface IDebtSubRequestInformationService
    {
        /// <summary>
        /// Получить информацию о задолженностях за ЖКУ
        /// </summary>
        /// <param name="risToken">токен кросс-авторизации</param>
        /// <returns></returns>
        IDataResult GetDebtInfo(string risToken);
    }
}