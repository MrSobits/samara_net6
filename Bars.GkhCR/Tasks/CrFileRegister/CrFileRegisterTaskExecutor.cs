using Bars.B4;
using Bars.B4.Modules.Tasks.Common.Service;
using Bars.GkhCr.DomainService;
using Bars.GkhCr.Entities;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Threading;

namespace Bars.GkhCR.Tasks
{
    public class CrFileRegisterTaskExecutor : ITaskExecutor
    {
        private ICrFileRegisterService _FileRegisterService;

        /// <summary>
        /// Идентификатор
        /// </summary>
        public static string Id = MethodBase.GetCurrentMethod().DeclaringType.FullName;

        public IDomainService<CrFileRegister> FileRegisterDomain { get; set; }

        /// <summary>
        /// Код задачи
        /// </summary>
        public string ExecutorCode { get; private set; }

        public CrFileRegisterTaskExecutor(ICrFileRegisterService FileRegisterService)
        {
            _FileRegisterService = FileRegisterService;
        }

        public IDataResult Execute(BaseParams @params, B4.Modules.Tasks.Common.Contracts.ExecutionContext ctx, IProgressIndicator indicator, CancellationToken ct)
        {
            CrFileRegister fileRegisterData = FileRegisterDomain.GetAll().FirstOrDefault(x => x.RealityObject.Id == (long.Parse((string)@params.Params["roId"])));
            if (fileRegisterData == null)
                return new BaseDataResult(false, $"Запись в реестре с ID дома {@params.Params["roId"]} не найдена");

            try
            {
                var result = _FileRegisterService.FormArchieve(fileRegisterData, indicator);
                if(!result)
                    return new BaseDataResult(false, "Ошибка при формировании архива");
                else
                    return new BaseDataResult(true, "Успешно");
            }
            catch(HttpRequestException e)
            {
                return new BaseDataResult(false, $"Ошибка связи: {e.InnerException}");
            }
        }
    }
}
