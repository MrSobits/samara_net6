using Bars.B4;
using Bars.B4.Modules.Tasks.Common.Service;
using Bars.B4.Utils;
using Bars.Gkh.Entities;
using Bars.Gkh.Enums;
using Bars.Gkh.ExecutionAction;
using Bars.GkhGji.Regions.Chelyabinsk.Entities;
using Bars.GkhGji.Regions.Chelyabinsk.Tasks.EGRIPSendInformationRequest;
using Bars.GkhGji.Regions.Chelyabinsk.Tasks.EGRULSendInformationRequest;
using Bars.GkhGji.Regions.Chelyabinsk.Tasks.GetSMEVAnswers;
using System;
using System.Collections.Generic;

using System.Linq;

namespace Bars.GkhGji.Regions.Chelyabinsk.ExecutionAction
{
    /// <summary>
    /// Периодическый запрос оплат в СМЭВ
    /// </summary>
    public class SendEGRULRequestsAction : BaseExecutionAction//, IMandatoryExecutionAction
    {
        //сколько запрашивать выписок за раз
        static int numberOfRequests = 20;

        public override string Description => "Запрашивает выписки из ЕГРЮЛ по контрагентам" +
            "Задача запускает проверку по 20 контрагентам с наименее актуальными ЕГРЮЛ";

        public override string Name => "Запросить сведения из ЕГРЮЛ";

        public override Func<IDataResult> Action => SendEGRULRequests;

        //public bool IsNeedAction() => true;

        public IDomainService<SMEVEGRUL> EGRULDomain { get; set; }

        public IDomainService<SMEVEGRIP> EGRIPDomain { get; set; }

        public IDomainService<ManOrgLicense> ManOrgLicenseDomain { get; set; }

        public IDomainService<ManagingOrganization> ManagingOrganizationDomain { get; set; }

        public IDomainService<Contragent> ContragentDomain { get; set; }

        public IDomainService<Inspector> InspectorDomain { get; set; }

        public IDomainService<Operator> OperatorDomain { get; set; }

        private IDataResult SendEGRULRequests()
        {
            var taskManager = Container.Resolve<ITaskManager>();
           
            try
            {
                Operator thisOperator = OperatorDomain.GetAll().Where(x => x.User == this.User).FirstOrDefault();

                if (1 == 1)

                //if (thisOperator?.Inspector != null)

                {
                    List<string> egrulDates = new List<string>();
                    egrulDates.AddRange(EGRULDomain.GetAll()
                        .Where(x => x.INNReq != null)
                        .Where(x => x.ObjectCreateDate > DateTime.Now.AddMonths(-1))
                        .Select(x => x.INNReq).ToList());

                    //уже запрошенные (и полученные(!!!)) выписки из ЕГРИП по контрагентам
                    egrulDates.AddRange(EGRIPDomain.GetAll()
                        .Where(x => x.INNReq != null)
                        .Where(x => x.ObjectCreateDate > DateTime.Now.AddMonths(-1))
                        .Select(x => x.INNReq).ToList());

                    //ОГРН контрагентов с активными и переоформленными лицензиями
                    List<string> contragentsWithActiveLicense = ContragentDomain.GetAll()
                        .Where(x => x.Inn != "" && x.Inn != null)
                        .WhereIf(egrulDates.Count > 0, x => !egrulDates.Contains(x.Inn))
                        .Where(x => !x.EgrulExcDate.HasValue || x.EgrulExcDate.Value < DateTime.Now.AddMonths(-1))
                        .Select(x => x.Inn).ToList();
                

                    List<string> needOGRN = new List<string>();
                    needOGRN.AddRange(contragentsWithActiveLicense);                
                    needOGRN = needOGRN.Distinct().ToList();

                    //уже запрошенные (и полученные(!!!)) выписки из ЕГРЮЛ по контрагентам
                   


                    //сортируем по дате выписки и возвращаем numberOfRequests первых ОГРН
                    List<string> ogrns = needOGRN.Take(numberOfRequests).ToList();

                    BaseParams baseParams = new BaseParams();
                    foreach (string inn in ogrns)
                    {
                        // Если ЮЛ
                        if (inn.Trim().Length == 10)
                        {
                            var oldEGRULIds = EGRULDomain.GetAll()
                                .Where(x => x.INNReq == inn.Trim()).Select(x => x.Id).ToList();
                            foreach (var oldEGRULId in oldEGRULIds)
                            {
                                EGRULDomain.Delete(oldEGRULId);
                            }
                            SMEVEGRUL smevRequestData = new SMEVEGRUL();
                            smevRequestData.InnOgrn = Enums.InnOgrn.INN;
                            smevRequestData.INNReq = inn;
                            if (thisOperator?.Inspector == null)
                            {
                                smevRequestData.Inspector = InspectorDomain.GetAll().FirstOrDefault();
                            }
                            else
                            {
                                smevRequestData.Inspector = thisOperator.Inspector;
                            }
                            EGRULDomain.Save(smevRequestData);

                            baseParams.Params.Clear();
                            if (!baseParams.Params.ContainsKey("taskId"))
                                baseParams.Params.Add("taskId", smevRequestData.Id.ToString());

                            taskManager.CreateTasks(new SendInformationRequestTaskProvider(Container), baseParams);
                        }
                        // Если ИП
                        else if (inn.Trim().Length == 12)
                        {
                            var oldEGRIPIds = EGRIPDomain.GetAll()
                                .Where(x => x.INNReq == inn).Select(x => x.Id).ToList();
                            foreach (var oldEGRIPId in oldEGRIPIds)
                            {
                                EGRIPDomain.Delete(oldEGRIPId);
                            }
                            SMEVEGRIP smevRequestData = new SMEVEGRIP();
                            smevRequestData.InnOgrn = Enums.InnOgrn.INN;
                            smevRequestData.INNReq = inn;
                            if (thisOperator?.Inspector == null)
                            {
                                smevRequestData.Inspector = InspectorDomain.GetAll().FirstOrDefault();
                            }
                            else
                            {
                                smevRequestData.Inspector = thisOperator.Inspector;
                            }
                            EGRIPDomain.Save(smevRequestData);

                            baseParams.Params.Clear();
                            if (!baseParams.Params.ContainsKey("taskId"))
                                baseParams.Params.Add("taskId", smevRequestData.Id.ToString());

                            taskManager.CreateTasks(new SendEGRIPRequestTaskProvider(Container), baseParams);
                        }
                    }

                    //ставим задачу на проверку ответов
                    baseParams.Params.Clear();
                    taskManager.CreateTasks(new GetSMEVAnswersTaskProvider(Container), baseParams);
                    return new BaseDataResult(true, "Задачи успешно поставлены");
                }
                else
                {
                    return new BaseDataResult(false, "Обмен информацией со ГИС ГМП доступен только сотрудникам ГЖИ");
                }
                
            }
            catch (Exception e)
            {
                return new BaseDataResult(false, $"{e.Message} {e.StackTrace}");
            }
            finally
            {
                Container.Release(taskManager);
            }
        }
    }
}
