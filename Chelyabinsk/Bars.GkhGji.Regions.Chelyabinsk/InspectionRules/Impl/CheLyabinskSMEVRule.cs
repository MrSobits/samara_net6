namespace Bars.GkhGji.Regions.Chelyabinsk.InspectionRules.Impl
{
    using Bars.B4;
    using Bars.B4.Modules.Tasks.Common.Service;
    using Bars.Gkh.Authentification;
    using Bars.Gkh.Entities;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Regions.Chelyabinsk.Tasks.EGRIPSendInformationRequest;
    using Bars.GkhGji.Regions.Chelyabinsk.Tasks.EGRULSendInformationRequest;
    using Bars.GkhGji.Regions.Chelyabinsk.Tasks.GetSMEVAnswers;
    using Castle.Windsor;
    using Entities;
    using System.Linq;

    /// <summary>
    /// Правило отправки запросов в СМЭВ при создании проверки
    /// 
    /// </summary>
    public class CheLyabinskSMEVRule : Bars.GkhGji.InspectionRules.SMEVRule
    {
        public IDomainService<SMEVEGRUL> EGRULDomain { get; set; }

        public IDomainService<SMEVEGRIP> EGRIPDomain { get; set; }

        public IDomainService<Operator> OperatorDomain { get; set; }

        public IGkhUserManager UserManager { get; set; }

        #region Fields

        private readonly ITaskManager _taskManager;
        private readonly IWindsorContainer _container;
        #endregion

        #region Constructors

        public CheLyabinskSMEVRule(ITaskManager taskManager, IWindsorContainer container)
        {
            _taskManager = taskManager;
            _container = container;
        }

        #endregion

        public override void SendRequests(InspectionGji inspection)
        {
            if (inspection.Contragent != null)
            {
                var taskManager = Container.Resolve<ITaskManager>();
                try
                {
                    Operator thisOperator = UserManager.GetActiveOperator();
                    if (thisOperator?.Inspector == null)
                    {

                    }
                    else
                    {
                        if (inspection.Contragent.Inn.Length == 10)
                        {
                            SMEVEGRUL smevRequestData = new SMEVEGRUL
                            {
                                Inspector = thisOperator.Inspector,
                                InnOgrn = Enums.InnOgrn.INN,
                                INNReq = inspection.Contragent.Inn
                            };
                            EGRULDomain.Save(smevRequestData);
                            BaseParams bparams = new BaseParams();
                            bparams.Params.Add("taskId", smevRequestData.Id.ToString());
                            var taskInfo = _taskManager.CreateTasks(new SendInformationRequestTaskProvider(_container), bparams).Data.Descriptors.FirstOrDefault();
                            if (taskInfo == null)
                            {
                               
                            }
                            else
                            {
                                var id = taskInfo.TaskId;
                            }
                            bparams.Params.Clear();
                            taskManager.CreateTasks(new GetSMEVAnswersTaskProvider(Container), bparams);



                        }
                        if (inspection.Contragent.Inn.Length == 12)
                        {
                            SMEVEGRIP smevRequestData = new SMEVEGRIP
                            {
                                Inspector = thisOperator.Inspector,
                                InnOgrn = Enums.InnOgrn.INN,
                                INNReq = inspection.Contragent.Inn
                            };
                            EGRIPDomain.Save(smevRequestData);
                            BaseParams bparams = new BaseParams();
                            bparams.Params.Add("taskId", smevRequestData.Id.ToString());
                            taskManager.CreateTasks(new SendEGRIPRequestTaskProvider(Container), bparams);
                            bparams.Params.Clear();
                            taskManager.CreateTasks(new GetSMEVAnswersTaskProvider(Container), bparams);
                        }

                      


                    }
                }
                finally
                {
                    Container.Release(taskManager);
                }
            }
           
        }
        public override void GetResponce()
        {
            var taskManager = Container.Resolve<ITaskManager>();
            try
            {
                BaseParams bparams = new BaseParams();
                taskManager.CreateTasks(new GetSMEVAnswersTaskProvider(Container), bparams);
            }
            catch
            {
                Container.Release(taskManager);
            }
        }
    }
}
