namespace Bars.Gkh.Qa.Steps
{

    using System;
    using Bars.B4;
    using Bars.Gkh.Domain;
    using Bars.Gkh.Qa.Utils;
    using FluentAssertions;
    using TechTalk.SpecFlow;
    using TechTalk.SpecFlow.Assist;

    [Binding]
    public class CitizenSuggestionSteps : BindingBase
    {
        
        private IDomainService<Entities.Suggestion.CitizenSuggestion> ds =
            Container.Resolve<IDomainService<Entities.Suggestion.CitizenSuggestion>>();

        [Given(@"добавлена Рубрика")]
        public void ДопустимДобавленаРубрика(Table table)
        {
            RubricHelper.Current = table.CreateInstance<Entities.Suggestion.Rubric>();
            Container.Resolve<IDomainService<Entities.Suggestion.Rubric>>().SaveOrUpdate(RubricHelper.Current);
        }

        [Given(@"у этого обращения заполняет поле Номер телефона ""(.*)""")]
        public void ДопустимУЭтогоОбращенияЗаполняетПолеНомерТелефона(string phone)
        {
            CitizenSuggestionHelper.Current.ApplicantPhone = phone;
        }

        [Given(@"пользователь добавляет новое Обращение граждан")]
        public void ДопустимПользовательДобавляетНовоеОбращениеГраждан()
        {
            CitizenSuggestionHelper.Current = new Entities.Suggestion.CitizenSuggestion(RubricHelper.Current);
        }
        
        [Given(@"у этого обращения заполняет поле Номер обращения ""(.*)""")]
        public void ДопустимУЭтогоОбращенияЗаполняетПолеНомерОбращения(string number)
        {
            CitizenSuggestionHelper.Current.Number = number;
        }
        
        [Given(@"у этого обращения заполняет поле Дата обращения ""(.*)""")]
        public void ДопустимУЭтогоОбращенияЗаполняетПолеДатаОбращения(string date)
        {
            CitizenSuggestionHelper.Current.AnswerDate = DateTime.Parse(date);
            CitizenSuggestionHelper.Current.CreationDate = DateTime.Now;
        }
        
        [Given(@"у этого обращения заполняет поле Адрес")]
        public void ДопустимУЭтогоОбращенияЗаполняетПолеАдрес()
        {
            CitizenSuggestionHelper.Current.RealityObject = RealityObjectHelper.CurrentRealityObject;
            CitizenSuggestionHelper.Current.Address = RealityObjectHelper.CurrentRealityObject.Address;
        }

        [Given(@"у этого обращения заполняет поле Email ""(.*)""")]
        public void ДопустимУЭтогоОбращенияЗаполняетПолеEmail(string email)
        {
            CitizenSuggestionHelper.Current.ApplicantEmail = email;
        }


        [Given(@"у этого обращения заполняет поле Рубрика")]
        public void ДопустимУЭтогоОбращенияЗаполняетПолеРубрика()
        {
            //CitizenSuggestionHelper.Current.GetType().GetProperty("Rubric").SetValue(CitizenSuggestionHelper.Current, RubricHelper.Current);
            //рубрика теперь в конструктор передается
        }
        
        [Given(@"у этого обращения заполняет поле Исполнитель")]
        public void ДопустимУЭтогоОбращенияЗаполняетПолеИсполнитель()
        {
            //ExecutorManagingOrganization
            //ExecutorMunicipality
            //ExecutorZonalInspection
            //ExecutorCrFund
        }
        
        [Given(@"у этого обращения заполняет поле Заявитель ""(.*)""")]
        public void ДопустимУЭтогоОбращенияЗаполняетПолеЗаявитель(string name)
        {
            CitizenSuggestionHelper.Current.ApplicantFio = name;
        }
        
        [Given(@"у этого обращения заполняет поле Почтовый адрес заявителя ""(.*)""")]
        public void ДопустимУЭтогоОбращенияЗаполняетПолеПочтовыйАдресЗаявителя(string address)
        {
            CitizenSuggestionHelper.Current.ApplicantAddress = address;
        }
        
        [Given(@"у этого обращения заполняет поле Место проблемы")]
        public void ДопустимУЭтогоОбращенияЗаполняетПолеМестоПроблемы()
        {
            //ProblemPlace
        }

        [Given(@"у этого обращения заполняет поле Описание проблемы ""(.*)""")]
        public void ДопустимУЭтогоОбращенияЗаполняетПолеОписаниеПроблемы(string description)
        {
            CitizenSuggestionHelper.Current.Description = description;
        }
        
        [When(@"пользователь сохраняет новое Обращение граждан")]
        public void ЕслиПользовательСохраняетНовоеОбращениеГраждан()
        {
            try
            {
                ds.SaveOrUpdate(CitizenSuggestionHelper.Current);
            }
            catch (Exception ex)
            {
                ExceptionHelper.TestExceptions.Add(System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message);
            }

        }
        
        [When(@"пользователь удаляет новое Обращение граждан")]
        public void ЕслиПользовательУдаляетНовоеОбращениеГраждан()
        {
            try
            {
                ds.Delete(CitizenSuggestionHelper.Current.Id);
            }
            catch (Exception ex)
            {
                ExceptionHelper.TestExceptions.Add(System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }
        
        [Then(@"обращение граждан присутствует в списке Обращений")]
        public void ТоОбращениеГражданПрисутствуетВСпискеОбращений()
        {
            ds.Get(CitizenSuggestionHelper.Current.Id)
                .Should()
                .NotBeNull(string.Format("Обращение граждан должно присутствовать в списке.{0}",
                    ExceptionHelper.GetExceptions()));
        }
        
        [Then(@"обращение граждан отсутствует в списке Обращений")]
        public void ТоОбращениеГражданОтсутствуетВСпискеОбращений()
        {
            ds.Get(CitizenSuggestionHelper.Current.Id)
                .Should()
                .BeNull(string.Format("Обращение граждан должно отсутствовать в списке.{0}",
                    ExceptionHelper.GetExceptions()));
        }

        [Given(@"у этого обращения заполняет поле ""(.*)"" (.*) символов '(.*)'")]
        public void ДопустимУЭтогоОбращенияЗаполняетПолеСимволов(string fieldName, int count, char ch)
        {
            string field;

            switch (fieldName)
            {
                case "Номер обращения":
                    field = "Number";
                    break;
                case "Описание проблемы":
                    field = "Description";
                    break;
                default:
                    throw new Exception("для поля " + fieldName + "не описано соответствие в реализации теста");
            }

            CitizenSuggestionHelper.Current.GetType().GetProperty(field).SetValue(CitizenSuggestionHelper.Current, new string(ch, count));

        }

    }
}
