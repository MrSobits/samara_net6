namespace Bars.Gkh.Qa.Steps
{
    using Bars.Gkh.Qa.Utils;
    using Bars.GkhCr.Entities;

    using TechTalk.SpecFlow;

    internal class ProgramCrHelper : BindingBase
    {
        /// <summary>
        /// Текущая программа КР
        /// </summary>
        public static ProgramCr Current
        {
            get
            {
                if (!ScenarioContext.Current.ContainsKey("CurrentProgramCr"))
                {
                    throw new SpecFlowException("Отсутствует текущяя программа КР");
                }

                var position = ScenarioContext.Current.Get<ProgramCr>("CurrentProgramCr");

                return position;
            }

            set
            {
                if (ScenarioContext.Current.ContainsKey("CurrentProgramCr"))
                {
                    ScenarioContext.Current.Remove("CurrentProgramCr");
                }

                ScenarioContext.Current.Add("CurrentProgramCr", value);
            }
        }

        /// <summary>
        /// Текущая запись в журнале изменений
        /// </summary>
        public static ProgramCrChangeJournal CurrentProgramCrChangeJournal
        {
            get
            {
                if (!ScenarioContext.Current.ContainsKey("CurrentProgramCrChangeJournal"))
                {
                    throw new SpecFlowException("Нет текущей записи в журнале изменений");
                }

                var programCrChangeJournal = ScenarioContext.Current.Get<ProgramCrChangeJournal>("CurrentProgramCrChangeJournal");

                return programCrChangeJournal;
            }

            set
            {
                if (ScenarioContext.Current.ContainsKey("CurrentProgramCrChangeJournal"))
                {
                    ScenarioContext.Current.Remove("CurrentProgramCrChangeJournal");
                }

                ScenarioContext.Current.Add("CurrentProgramCrChangeJournal", value);
            }
        }
    }
}
