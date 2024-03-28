namespace Bars.Gkh.Qa.Steps
{
    using System.Linq;

    using Bars.Gkh.Qa.Utils;

    using FluentAssertions;

    using TechTalk.SpecFlow;

    [Binding]
    internal class ExceptionSteps : BindingBase
    {
        [Then(@"падает ошибка с текстом ""(.*)""")]
        public void ТоПадаетОшибкаСТекстом(string exceptionText)
        {
            ExceptionHelper.TestExceptions.Values.Any(x => x.Contains(exceptionText))
                .Should()
                .BeTrue(string.Format("должна падать ошибка {0}. {1}", exceptionText, ExceptionHelper.GetExceptions()));
        }

        [Then(@"Не выпало не одной ошибки")]
        public void ТоНеВыпалоНеОднойОшибки()
        {
            ExceptionHelper.TestExceptions.Values.Should()
                .BeEmpty(
                    string.Format(
                        "Во время прохождения теста не должно выпадать ошибок, но {0}",
                        ExceptionHelper.GetExceptions()));
        }
    }
}
