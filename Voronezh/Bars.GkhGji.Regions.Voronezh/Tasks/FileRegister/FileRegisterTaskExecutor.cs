using Bars.B4;
using Bars.B4.Modules.Tasks.Common.Service;
using Bars.GkhGji.Regions.BaseChelyabinsk.Entities;
using Bars.GkhGji.Regions.Voronezh.DomainService;
using Bars.GkhGji.Regions.Voronezh.Entities;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Threading;

namespace Bars.GkhGji.Regions.Voronezh.Tasks
{
    public class FileRegisterTaskExecutor : ITaskExecutor
    {
        private IFileRegisterService _FileRegisterService;

        /// <summary>
        /// Идентификатор
        /// </summary>
        public static string Id = MethodBase.GetCurrentMethod().DeclaringType.FullName;

        public IDomainService<FileRegister> FileRegisterDomain { get; set; }

        /// <summary>
        /// Код задачи
        /// </summary>
        public string ExecutorCode { get; private set; }

        public FileRegisterTaskExecutor(IFileRegisterService FileRegisterService)
        {
            _FileRegisterService = FileRegisterService;
        }

        public IDataResult Execute(BaseParams @params, B4.Modules.Tasks.Common.Contracts.ExecutionContext ctx, IProgressIndicator indicator, CancellationToken ct)
        {
            FileRegister fileRegisterData = FileRegisterDomain.GetAll().FirstOrDefault(x => x.RealityObject.Id == (long.Parse((string)@params.Params["roId"])));
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
