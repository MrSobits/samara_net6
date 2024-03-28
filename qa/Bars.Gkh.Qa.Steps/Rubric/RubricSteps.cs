namespace Bars.Gkh.Qa.Steps
{
    using System;

    using Bars.B4.Utils;
    using Bars.Gkh.Enums;
    using Bars.Gkh.Qa.Utils;

    using FluentAssertions;

    using TechTalk.SpecFlow;

    [Binding]
    public class RubricSteps : BindingBase
    {

        private DomainServiceCashe<Entities.Suggestion.Rubric> _dscashe =
            new DomainServiceCashe<Entities.Suggestion.Rubric>();

            [Given(@"пользователь добавляет новую Рубрику")]
        public void ДопустимПользовательДобавляетНовуюРубрику()
            {
                //конструктор перегружен
            }

        [Given(@"у этой рубрики заполняет поле Код ""(.*)""")]
        public void ДопустимУЭтойРубрикиЗаполняетПолеКод(int code)
        {
            ScenarioContext.Current["Code"] = code;
        }

        [Given(@"у этой рубрики заполняет поле Наименование ""(.*)""")]
        public void ДопустимУЭтойРубрикиЗаполняетПолеНаименование(string name)
        {
            ScenarioContext.Current["Name"] = name;
        }

        [Given(@"у этой рубрики НЕ заполняет поле Код")]
        public void ДопустимУЭтойРубрикиНЕЗаполняетПолеКод()
        {
            ScenarioContext.Current["Code"] = null;
        }

        [Given(@"у этой рубрики НЕ заполняет поле Наименование")]
        public void ДопустимУЭтойРубрикиНЕЗаполняетПолеНаименование()
        {
            ScenarioContext.Current["Name"] = null;
        }


        [Given(@"пользователь выбирает тип исполнителя ""(.*)""")]
        public void ДопустимПользовательВыбираетТипИсполнителя(string firstExecutorType)
        {
            
            ExecutorType executorType;

            switch (firstExecutorType)
            {
                case "Управляющая организация":
                    executorType = ExecutorType.Mo;
                    break;

                case "Администрация МО":
                    executorType = ExecutorType.Mu;
                    break;

                case "ГЖИ":
                    executorType = ExecutorType.Gji;
                    break;

                case "Фонд КР":
                    executorType = ExecutorType.CrFund;
                    break;

                default:
                    throw new SpecFlowException("Нет типа исполнителя " + firstExecutorType);
            }

            var code = ScenarioContext.Current["Code"].ToInt();
            var name = ScenarioContext.Current["Name"].ToString();

            RubricHelper.Current = new Entities.Suggestion.Rubric(code, name, executorType);

        }
        
        [When(@"пользователь сохраняет новую рубрику")]
        public void ЕслиПользовательСохраняетНовуюРубрику()
        {
            try
            {
                if (RubricHelper.Current.Id == 0)
                {
                    _dscashe.Current.Save(RubricHelper.Current);
                }
                else
                {
                    _dscashe.Current.Update(RubricHelper.Current);
                }
            }
            catch (Exception ex)
            {
                ExceptionHelper.TestExceptions.Add(System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }
        
        [When(@"пользователь удаляет новую рубрику")]
        public void ЕслиПользовательУдаляетНовуюРубрику()
        {
            try
            {
                _dscashe.Current.Delete(RubricHelper.Current.Id);
            }
            catch (Exception ex)
            {
                ExceptionHelper.TestExceptions.Add(System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }
        
        [Then(@"рубрика присутствует в списке Рубрики")]
        public void ТоРубрикаПрисутствуетВСпискеРубрики()
        {
            _dscashe.Current.Get(RubricHelper.Current.Id).Should().NotBeNull(string.Format("Рубрика должна присутстовать в справочнике.{0}", ExceptionHelper.GetExceptions()));
        }
        
        [Then(@"рубрика отсутствует в списке Рубрики")]
        public void ТоРубрикаОтсутствуетВСпискеРубрики()
        {
            _dscashe.Current.Get(RubricHelper.Current.Id).Should().BeNull(string.Format("Рубрика должна отсутствовать в справочнике.{0}", ExceptionHelper.GetExceptions()));
        }
    }
}
