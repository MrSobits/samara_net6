using Bars.B4.Modules.Tasks.Common.Service;
using Bars.GkhCr.Entities;

namespace Bars.GkhCr.DomainService
{
    /// <summary>
    /// Сервис работы с ЕГРЮЛ через СМЭВ
    /// </summary>
    public interface ICrFileRegisterService
    {
        /// <summary>
        /// Отправка запроса выписки ЕГРЮЛ
        /// </summary>
        bool FormArchieve(CrFileRegister requestData, IProgressIndicator indicator = null);
    }
}
