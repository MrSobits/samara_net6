using Bars.B4;
using Bars.B4.Modules.Tasks.Common.Service;
using Bars.B4.Utils;
using Bars.Gkh.Authentification;
using Bars.Gkh.Entities;
using Bars.GkhCr.Entities;
using Sobits.GisGkh.DomainService;
using Sobits.GisGkh.Entities;
using Sobits.GisGkh.Enums;
using System;
using System.Linq;
using System.Reflection;
using System.Threading;

namespace Sobits.GisGkh.Tasks.ProcessGisGkhAnswers
{
    /// <summary>
    /// Задача на массовую выгрузку договоров КПР в ГИС ЖКХ
    /// </summary>
    public class ImportBuildContractTaskExecutor : ITaskExecutor
    {
        public static string Id = MethodBase.GetCurrentMethod().DeclaringType.FullName;

        public IDomainService<GisGkhRequests> GisGkhRequestsDomain { get; set; }
        public IDomainService<ProgramCr> ProgramDomain { get; set; }
        public IDomainService<BuildContract> ContractDomain { get; set; }

        private IImportBuildContractService _ImportBuildContractService;
        public IGkhUserManager UserManager { get; set; }

        /// <summary>
        /// Код задачи
        /// </summary>
        public string ExecutorCode { get; private set; }

        public ImportBuildContractTaskExecutor(IImportBuildContractService ImportBuildContractService)
        {
            _ImportBuildContractService = ImportBuildContractService;
        }

        public IDataResult Execute(BaseParams @params, Bars.B4.Modules.Tasks.Common.Contracts.ExecutionContext ctx, IProgressIndicator indicator, CancellationToken ct)
        {
            try
            {
                Operator thisOperator = UserManager.GetActiveOperator();

                var program = ProgramDomain.GetAll()
                    .Where(x => x.ImportContract == true)
                    .FirstOrDefault();

                if (program.GisGkhGuid == null)
                    return new BaseDataResult(false, "Выбранная программа не выгружена");

                var contracts = ContractDomain.GetAll()
                    .Where(x => x.ObjectCr.ProgramCr == program)
                    .Where(x => x.State.FinalState == true)
                    .ToList();

                if (contracts.IsEmpty())
                    return new BaseDataResult(false, "По выбранной программе нет ни одного утвержденного договора");

                foreach (var contract in contracts)
                {
                    var req = new GisGkhRequests
                    {
                        TypeRequest = GisGkhTypeRequest.importBuildContract,
                        RequestState = GisGkhRequestState.NotFormed
                    };
                    
                    GisGkhRequestsDomain.Save(req);

                    string[] reqParams = new string[]
                    {
                        program.Id.ToString(),
                        contract.Id.ToString()
                    };

                    try
                    {
                        _ImportBuildContractService.SaveRequest(req, reqParams, thisOperator.GisGkhContragent.GisGkhOrgPPAGUID);
                    }
                    catch (Exception e)
                    {
                        req.RequestState = GisGkhRequestState.Error;
                        GisGkhRequestsDomain.Update(req);
                        return new BaseDataResult(false, $"{e.GetType()} {e.Message} {e.InnerException} {e.StackTrace}");
                    }
                }
                return new BaseDataResult(true, "Запросы созданы");
            }
            catch (Exception e)
            {
                return new BaseDataResult(false, $"{e.GetType()} {e.Message} {e.InnerException} {e.StackTrace}");
            }
        }
    }
}
