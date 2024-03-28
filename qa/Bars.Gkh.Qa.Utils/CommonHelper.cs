namespace Bars.Gkh.Qa.Utils
{
    using System;

    using Bars.B4.DataAccess;
    using Bars.B4.IoC.Lifestyles.SessionLifestyle;

    using NHibernate;

    using TechTalk.SpecFlow;

    public class CommonHelper : BindingBase
    {
        public static string DuplicateLine(string line, int count)
        {
            string finalString = string.Empty;

            for (int i = 0; i < count; i++)
            {
                finalString += line;
            }

            return finalString;
        }

        public static void CloseCurrentSession()
        {
            try
            {
                Container.Resolve<ISessionProvider>().GetCurrentSession().Flush();
            }
            finally
            {
                if (ScenarioContext.Current.ContainsKey("transaction"))
                {
                    var transaction = ScenarioContext.Current.Get<IDataTransaction>("transaction");

                    transaction.Rollback();

                    transaction.Dispose();

                    ScenarioContext.Current.Remove("transaction");
                }

                Container.Resolve<ISessionProvider>().CloseCurrentSession();
                ExplicitSessionScope.LeaveScope(null);
            }
        }

        public static bool IsNow(DateTime dateToCheck)
        {
            var elapsedTime = DateTime.Now - dateToCheck;
            if (elapsedTime < TimeSpan.FromSeconds(15))
            {
                return true;
            }

            return false;
        }
    }
}
