namespace Bars.Gkh.Qa.Steps
{
    using System.Collections.Generic;

    using TechTalk.SpecFlow;

    using Bars.B4;
    using Bars.B4.Utils;

    using Bars.Gkh.DomainService;
    using Bars.Gkh.Entities;

    using Bars.Gkh.Qa.Utils;

    class InspectorHelper : BindingBase
    {
        /// <summary>
        /// Текущий инспектор
        /// </summary>
        public static Inspector Current
        {
            get
            {
                if (!ScenarioContext.Current.ContainsKey("currentInspector"))
                {
                    throw new SpecFlowException("Нет текущего инспектора");
                }
                
                return ScenarioContext.Current.Get<Inspector>("currentInspector");
            }
            set
            {
                //чтобы в случае когда привязываем одного инспектора к другому оба оставались
                if (ScenarioContext.Current.ContainsKey("currentInspector"))
                {
                    ScenarioContext.Current.Add("parrentInspector", value);
                }
                else
                {
                    ScenarioContext.Current.Add("currentInspector", value);
                }
            }
        }

        /// <summary>
        /// Родительский инспектор используется в тестах при привязки одного инспектора к другому
        /// </summary>
        public static Inspector ParrentInspector
        {
            get
            {
                if (!ScenarioContext.Current.ContainsKey("parrentInspector"))
                {
                    throw new SpecFlowException("Нет родительского инспектора");
                }

                return ScenarioContext.Current.Get<Inspector>("parrentInspector");
            }
            set
            {
                ScenarioContext.Current.Add("parrentInspector", value);
            }
        }

        public static void AddZonalInspection(ZonalInspection zonalInspection)
        {
            var inspectorService = Container.Resolve<IInspectorService>();

            var parsedInspector = new KeyValuePair<string, object>("inpectorId",
                InspectorHelper.Current.Id.ToString());

            var parsedZonalInspection = new KeyValuePair<string, object>("objectIds",
                zonalInspection.Id.ToString());

            var paramsDict = new DynamicDictionary { parsedInspector, parsedZonalInspection };

            var baseParams = new BaseParams { Params = paramsDict };

            inspectorService.AddZonalInspection(baseParams);
        }
    }
}
