using Bars.B4;
using Bars.B4.Modules.Tasks.Common.Service;
using Bars.Gkh.Entities;
using Bars.Gkh.ExecutionAction;
using Bars.GkhGji.Regions.Chelyabinsk.Entities;
using Bars.GkhGji.Regions.Chelyabinsk.Tasks.GetSMEVAnswers;
using Bars.GkhGji.Regions.Chelyabinsk.Tasks.SendPaymentRequest;
using System;
using System.Linq;

namespace Bars.GkhGji.Regions.Chelyabinsk.ExecutionAction
{
    /// <summary>
    /// Периодическый запрос оплат в СМЭВ
    /// </summary>
    public class SendPayRegRequestAction : BaseExecutionAction//, IMandatoryExecutionAction
    {
        public override string Description => "Запрашивает из СМЭВа все оплаты за прошедшие сутки";

        public override string Name => "Запросить оплаты в СМЭВ";

        public override Func<IDataResult> Action => SendPayRegRequest;

        //public bool IsNeedAction() => true;

        public IDomainService<PayRegRequests> PayRegRequestsDomain { get; set; }

        public IDomainService<Inspector> InspectorDomain { get; set; }

        public IDomainService<Operator> OperatorDomain { get; set; }

        private IDataResult SendPayRegRequest()
        {
            var taskManager = Container.Resolve<ITaskManager>();

            try
            {
                Operator thisOperator = OperatorDomain.GetAll().Where(x => x.User == this.User).FirstOrDefault();

                PayRegRequests smevRequestData = new PayRegRequests();
                if (thisOperator?.Inspector == null)
                {
                    smevRequestData.Inspector = InspectorDomain.GetAll().FirstOrDefault();
                }
                else
                {
                    smevRequestData.Inspector = thisOperator.Inspector;
                }
                smevRequestData.GetPaymentsStartDate = DateTime.Today.AddDays(-1);
                smevRequestData.GetPaymentsEndDate = DateTime.Today;
                smevRequestData.Answer = "";
                smevRequestData.MessageId = "";
                smevRequestData.PayRegPaymentsType = Enums.GisGmpPaymentsType.AllInTime;
                PayRegRequestsDomain.Save(smevRequestData);

                var baseParams = new BaseParams();

                if (!baseParams.Params.ContainsKey("taskId"))
                    baseParams.Params.Add("taskId", smevRequestData.Id.ToString());

                taskManager.CreateTasks(new SendPaymentRequestTaskProvider(Container), baseParams);

                //ставим задачу на проверку ответов
                baseParams.Params.Clear();
                taskManager.CreateTasks(new GetSMEVAnswersTaskProvider(Container), baseParams);
                return new BaseDataResult(true, "Задача успешно поставлена");
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
