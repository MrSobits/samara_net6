using Bars.B4;
using Bars.B4.Modules.Tasks.Common.Service;
using Bars.Gkh.Entities;
using Bars.Gkh.ExecutionAction;
using Bars.GkhGji.Regions.BaseChelyabinsk.Entities;
using Bars.GkhGji.Regions.Voronezh.Entities;
using Bars.GkhGji.Regions.Voronezh.Tasks.GetSMEVAnswers;
using Bars.GkhGji.Regions.Voronezh.Tasks.SendPaymentRequest;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace Bars.GkhGji.Regions.Voronezh.ExecutionAction
{
    /// <summary>
    /// Периодическый запрос жалоб по досудебному
    /// </summary>
    public class GetComplaintsAction : BaseExecutionAction//, IMandatoryExecutionAction
    {
        public override string Description => "Запрашивает из СМЭВа все жалобы на документы/услуги ГЖИ";

        public override string Name => "Запросить жалобы";

        public override Func<IDataResult> Action => SendComplaintsRequest;

        //public bool IsNeedAction() => true;

        public IDomainService<SMEVComplaintsRequest> SMEVComplaintsRequestDomain { get; set; }

        public IDomainService<Inspector> InspectorDomain { get; set; }

        public IDomainService<Operator> OperatorDomain { get; set; }

        private IDataResult SendComplaintsRequest()
        {
            var taskManager = Container.Resolve<ITaskManager>();

            try
            {
                Operator thisOperator = OperatorDomain.GetAll().Where(x => x.User == this.User).FirstOrDefault();

                SMEVComplaintsRequest smevRequestData = new SMEVComplaintsRequest();
                if (thisOperator?.Inspector == null)
                {
                    smevRequestData.Inspector = InspectorDomain.GetAll().FirstOrDefault();
                }
                else
                {
                    smevRequestData.Inspector = thisOperator.Inspector;
                }
                KndRequest req = new KndRequest
                {
                    Item = new getComplaintsType
                    {
                        Item = true,
                        unit = GetComplaintUnit()
                    }
                };
                var element = ToXElement<KndRequest>(req);
                if (element != null)
                {
                    smevRequestData.TextReq = element.ToString();
                }
                smevRequestData.Answer = "";
                smevRequestData.MessageId = "";
                SMEVComplaintsRequestDomain.Save(smevRequestData);

                var baseParams = new BaseParams();

                if (!baseParams.Params.ContainsKey("taskId"))
                    baseParams.Params.Add("taskId", smevRequestData.Id.ToString());

                taskManager.CreateTasks(new SendComplaintsCustomRequestTaskProvider(Container), baseParams);

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

        private XElement ToXElement<T>(object obj)
        {
            using (var memoryStream = new MemoryStream())
            {
                using (TextWriter streamWriter = new StreamWriter(memoryStream))
                {
                    var xmlSerializer = new XmlSerializer(typeof(T));
                    xmlSerializer.Serialize(streamWriter, obj);
                    return XElement.Parse(Encoding.UTF8.GetString(memoryStream.ToArray()));
                }
            }
        }
        private unitType[] GetComplaintUnit()
        {
            List<unitType> unitList = new List<unitType>();
            unitList.Add(new unitType
            {
               // id = "1030200000000001",
               id = "68566872-7cc6-ea4b-0f69-eac0cf88819f",
                Value = "ГОСУДАРСТВЕННАЯ ЖИЛИЩНАЯ ИНСПЕКЦИЯ ВОРОНЕЖСКОЙ ОБЛАСТИ"
            });
            return unitList.ToArray();
        }
    }
}
