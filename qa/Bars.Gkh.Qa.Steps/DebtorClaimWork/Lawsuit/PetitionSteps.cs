namespace Bars.Gkh.Qa.Steps
{
    using Bars.Gkh.Modules.ClaimWork.Entities;
    using Bars.Gkh.Qa.Utils;

    using TechTalk.SpecFlow;

    [Binding]
    internal class PetitionSteps : BindingBase
    {
        [Given(@"пользователь в это Исковое заявление добавляет запись по документу")]
        public void ДопустимПользовательВЭтоИсковоеЗаявлениеДобавляетЗаписьПоДокументу()
        {
            LawsuitClwDocumentHelper.Current = new LawsuitClwDocument { DocumentClw = PetitionHelper.Current };
        }
    }
}
