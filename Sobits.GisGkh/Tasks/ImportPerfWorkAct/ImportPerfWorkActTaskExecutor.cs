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
    /// Задача на массовую выгрузку актов КПР в ГИС ЖКХ
    /// </summary>
    public class ImportPerfWorkActTaskExecutor : ITaskExecutor
    {
        public static string Id = MethodBase.GetCurrentMethod().DeclaringType.FullName;

        public IDomainService<GisGkhRequests> GisGkhRequestsDomain { get; set; }
        public IDomainService<ProgramCr> ProgramDomain { get; set; }
        public IDomainService<BuildContractTypeWork> TypeWorkCrDomain { get; set; }
        public IDomainService<PerformedWorkAct> WorkActDomain { get; set; }
        public IDomainService<BuildContract> ContractDomain { get; set; }

        private readonly IImportPerfWorkActService _ImportPerfWorkActService;
        public IGkhUserManager UserManager { get; set; }

        /// <summary>
        /// Код задачи
        /// </summary>
        public string ExecutorCode { get; private set; }

        public ImportPerfWorkActTaskExecutor(IImportPerfWorkActService ImportPerfWorkActService)
        {
            _ImportPerfWorkActService = ImportPerfWorkActService;
        }

        public IDataResult Execute(BaseParams @params, Bars.B4.Modules.Tasks.Common.Contracts.ExecutionContext ctx, IProgressIndicator indicator, CancellationToken ct)
        {
            try
            {
                Operator thisOperator = UserManager.GetActiveOperator();

                var program = ProgramDomain.GetAll()
                    .Where(x => x.ImportContract == true)
                    .FirstOrDefault();

                if (program == null)
                    return new BaseDataResult(false, "Не выбрана программа КПР");

                if (program.GisGkhGuid == null)
                    return new BaseDataResult(false, "Выбранная программа не выгружена");

                var contracts = ContractDomain.GetAll()
                    .Where(x => x.ObjectCr.ProgramCr == program)
                    .Where(x => x.GisGkhGuid != null && x.GisGkhGuid != "")
                    .ToList();

                if (contracts.IsEmpty())
                    return new BaseDataResult(false, "По выбранной программе нет ни одного выгруженного договора");

                foreach (var contract in contracts)
                {
                    var works = TypeWorkCrDomain.GetAll()
                        .Where(x => contracts.Contains(x.BuildContract))
                        .Select(x => x.TypeWork)
                        .ToList();

                    var acts = WorkActDomain.GetAll()
                        .Where(x => works.Contains(x.TypeWorkCr))
                        .Where(x => x.State.FinalState)
                        .ToList();

                    foreach (var act in acts)
                    {
                        var req = new GisGkhRequests
                        {
                            TypeRequest = GisGkhTypeRequest.importPerfWorkAct,
                            RequestState = GisGkhRequestState.NotFormed
                        };

                        GisGkhRequestsDomain.Save(req);

                        string[] reqParams = new string[]
                        {
                            program.Id.ToString(),
                            contract.Id.ToString(),
                            act.Id.ToString()
                        };

                        try
                        {
                            _ImportPerfWorkActService.SaveRequest(req, reqParams, thisOperator.GisGkhContragent.GisGkhOrgPPAGUID);
                        }
                        catch (Exception e)
                        {
                            req.RequestState = GisGkhRequestState.Error;
                            GisGkhRequestsDomain.Update(req);
                            return new BaseDataResult(false, $"{e.GetType()} {e.Message} {e.InnerException} {e.StackTrace}");
                        }
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
