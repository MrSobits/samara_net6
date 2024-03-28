namespace Bars.Gkh.Qa.Steps
{
    using System;

    using Bars.B4;
    using Bars.Gkh.Qa.Utils;
    using Bars.Gkh.RegOperator.Entities;

    using TechTalk.SpecFlow;

    internal class BasePersonalAccountHelper : BindingBase
    {
        private static Type _type;

        public static Type Type
        {
            get
            {
                return typeof(BasePersonalAccount);
            }
        }

        public static IDomainService<BasePersonalAccount> DomainService
        {
            get
            {
                return Container.Resolve<IDomainService<BasePersonalAccount>>();
            }
        }

        /// <summary>
        /// Лицевой счет
        /// </summary>
        public static BasePersonalAccount Current
        {
            get
            {
                if (!ScenarioContext.Current.ContainsKey("BasePersonalAccountHelper"))
                {
                    throw new SpecFlowException(
                        string.Format("Отсутствует текущий лицевой счет. {0}", ExceptionHelper.GetExceptions()));
                }

                var current = ScenarioContext.Current.Get<dynamic>("BasePersonalAccountHelper");

                return current;
            }

            set
            {
                if (ScenarioContext.Current.ContainsKey("BasePersonalAccountHelper"))
                {
                    ScenarioContext.Current.Remove("BasePersonalAccountHelper");
                }

                ScenarioContext.Current.Add("BasePersonalAccountHelper", value);
            }
        }

        /// <summary>
        /// Фильтр реестра ЛС
        /// </summary>
        public static BaseParams FilterBaseParams
        {
            get
            {
                if (!ScenarioContext.Current.ContainsKey("BasePersonalAccountFilterBaseParams"))
                {
                    ScenarioContext.Current.Add("BasePersonalAccountFilterBaseParams", new BaseParams());
                }

                var current = ScenarioContext.Current.Get<BaseParams>("BasePersonalAccountFilterBaseParams");

                return current;
            }

            set
            {
                if (ScenarioContext.Current.ContainsKey("BasePersonalAccountFilterBaseParams"))
                {
                    ScenarioContext.Current.Remove("BasePersonalAccountFilterBaseParams");
                }

                ScenarioContext.Current.Add("BasePersonalAccountFilterBaseParams", value);
            }
        }
    }
}
