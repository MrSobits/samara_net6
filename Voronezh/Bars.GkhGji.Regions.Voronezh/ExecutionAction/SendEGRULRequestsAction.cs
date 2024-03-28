using Bars.B4;
using Bars.B4.Modules.Tasks.Common.Service;
using Bars.B4.Utils;
using Bars.Gkh.Entities;
using Bars.Gkh.Enums;
using Bars.Gkh.ExecutionAction;
using Bars.GkhGji.Regions.Voronezh.Entities;
using Bars.GkhGji.Regions.Voronezh.Tasks.EGRIPSendInformationRequest;
using Bars.GkhGji.Regions.Voronezh.Tasks.EGRULSendInformationRequest;
using Bars.GkhGji.Regions.Voronezh.Tasks.GetSMEVAnswers;
using System;
using System.Collections.Generic;

using System.Linq;

namespace Bars.GkhGji.Regions.Voronezh.ExecutionAction
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
                        .Where(x => x.ObjectEditDate > DateTime.Now.AddMonths(-1))
                        .Select(x => x.INNReq).ToList());

                    //уже запрошенные (и полученные(!!!)) выписки из ЕГРИП по контрагентам
                    egrulDates.AddRange(EGRIPDomain.GetAll()
                        .Where(x => x.INNReq != null)
                        .Where(x => x.ObjectEditDate > DateTime.Now.AddMonths(-1))
                        .Select(x => x.INNReq).ToList());

                    //ОГРН контрагентов с активными и переоформленными лицензиями
                    List<string> contragentsWithActiveLicense = ManOrgLicenseDomain.GetAll()
                        .WhereIf(egrulDates.Count > 0, x => !egrulDates.Contains(x.Contragent.Inn))
                        .Where(x => x.State.Code == "002" || x.State.Code == "004")
                        .Select(x => x.Contragent.Inn).ToList();

                    ////ОГРН всех контрагентов
                    //List<string> contragentsORGN = ContragentDomain.GetAll().Where(x => x.Ogrn != null && x.Ogrn != "").Select(x => x.Ogrn).ToList();

                    //ОГРН контрагентов - ТСЖ и ЖСК
                    List<string> tsjjskOGRN = ManagingOrganizationDomain.GetAll()
                        .Where(x => x.Contragent.Inn != null && x.Contragent.Inn != "")
                        .WhereIf(egrulDates.Count > 0, x => !egrulDates.Contains(x.Contragent.Inn))
                        .Select(x => x.Contragent.Inn).ToList();

                    List<string> needOGRN = new List<string>();
                    needOGRN.AddRange(contragentsWithActiveLicense);
                    needOGRN.AddRange(tsjjskOGRN);
                    needOGRN = needOGRN.Distinct().ToList();

                    //уже запрошенные (и полученные(!!!)) выписки из ЕГРЮЛ по контрагентам



                    //сортируем по дате выписки и возвращаем numberOfRequests первых ОГРН
                    List<string> ogrns = needOGRN.Take(numberOfRequests).ToList();

                    BaseParams baseParams = new BaseParams();
                    foreach (string inn in ogrns)
                    {
                        // Если ЮЛ
                        if (inn.Trim().Length < 12)
                        {
                            var oldEGRULIds = EGRULDomain.GetAll()
                                .Where(x => x.INNReq == inn).Select(x => x.Id);
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
                                .Where(x => x.INNReq == inn).Select(x => x.Id);
                            foreach (var oldEGRIPId in oldEGRIPIds)
                            {
                                EGRULDomain.Delete(oldEGRIPId);
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
                return new BaseDataResult(false, e.Message);
            }
            finally
            {
                Container.Release(taskManager);
            }
        }
    }
}
