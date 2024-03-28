namespace Bars.Gkh.Qa.Steps
{
    using Bars.Gkh.Qa.Utils;
    using Bars.GkhCr.Entities;

    using TechTalk.SpecFlow;

    internal class ObjectCrHelper : BindingBase
    {
        /// <summary>
        /// Текущий Объект капитального ремонта
        /// </summary>
        public static ObjectCr Current
        {
            get
            {
                if (!ScenarioContext.Current.ContainsKey("currentObjectCr"))
                {
                    throw new SpecFlowException("Нет текущего объекта капитального ремонта");
                }

                var current = ScenarioContext.Current.Get<ObjectCr>("currentObjectCr");

                return current;
            }

            set
            {
                if (ScenarioContext.Current.ContainsKey("currentObjectCr"))
                {
                    ScenarioContext.Current.Remove("currentObjectCr");
                }

                ScenarioContext.Current.Add("currentObjectCr", value);
            }
        }
    }
}
