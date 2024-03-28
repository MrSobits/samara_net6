namespace Bars.Gkh.Qa.Steps
{
    using System.Linq;
    using System.Collections.Generic;

    using TechTalk.SpecFlow;

    using Bars.B4.Utils;

    class ExceptionHelper
    {
        /// <summary>
        /// Ошибки текущего теста
        /// </summary>
        public static Dictionary<string, string> TestExceptions
        {
            get
            {
                if (!ScenarioContext.Current.ContainsKey("currentTestExceptions"))
                {
                    ScenarioContext.Current.Add("currentTestExceptions", new Dictionary<string, string>());
                }

                var testExceptions = ScenarioContext.Current.Get<Dictionary<string, string>>("currentTestExceptions");

                return testExceptions;
            }

            set
            {
                if (ScenarioContext.Current.ContainsKey("currentTestExceptions"))
                {
                    ScenarioContext.Current.Remove("currentTestExceptions");
                }

                ScenarioContext.Current.Add("currentTestExceptions", value);
            }
        }

        public static string GetExceptions()
        {
            var occuredExceptions = string.Join(
                ";",
                TestExceptions.Select(x => string.Format("В методе {0} выпала ошибка {1}", x.Key, x.Value)).ToArray());

            occuredExceptions = occuredExceptions.IsEmpty()
                ? string.Empty
                : string.Format("Во время выполнения теста были отловленны следующие ошибки {0}", occuredExceptions);

            return occuredExceptions;
        }

        public static void AddException(string methodName, string excMessage, int index = 0)
        {
            var indexedMethodName = methodName + index;

            if (!TestExceptions.ContainsKey(indexedMethodName))
            {
                TestExceptions.Add(indexedMethodName, excMessage);

                return;
            }

            AddException(methodName, excMessage, ++index);
        }
    }
}
