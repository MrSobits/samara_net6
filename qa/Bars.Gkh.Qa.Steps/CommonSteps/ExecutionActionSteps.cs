namespace Bars.Gkh.Qa.Steps.CommonSteps
{
    using System;
    using System.Linq;

    using Bars.Gkh.ExecutionAction;
    using Bars.Gkh.Qa.Utils;

    using TechTalk.SpecFlow;

    [Binding]
    internal class ExecutionActionSteps : BindingBase
    {
        [Given(@"выполненно действие ""(.*)""")]
        public void ДопустимВыполненноДействие(string executionActionName)
        {
            var executionActions = Container.ResolveAll<IExecutionAction>()
                .ToList();

            if (!executionActions.Any())
            {
                throw new SpecFlowException(string.Format("Нет ни одного выполняемого действия"));
            }

            var excecutionAction = executionActions.FirstOrDefault(x => x.Name == executionActionName);

            if (excecutionAction == null)
            {
                throw new SpecFlowException(
                    string.Format("Отсутствует выполняемое действие с наименованием \"{0}\"", executionActionName));
            }

            try
            {
                excecutionAction.Action();
            }
            catch (Exception ex)
            {
                ExceptionHelper.AddException(excecutionAction.Name, ex.Message);
            }
        }
    }
}
