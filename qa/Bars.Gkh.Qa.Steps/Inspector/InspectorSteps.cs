namespace Bars.Gkh.Qa.Steps
{
    using System;
    using System.Linq;
    using System.Collections.Generic;

    using Bars.B4.DataAccess;

    using TechTalk.SpecFlow;
    using TechTalk.SpecFlow.Assist;
    using FluentAssertions;

    using Bars.B4;
    using Bars.B4.Utils;

    using Bars.Gkh.Domain;
    using Bars.Gkh.DomainService;
    using Bars.Gkh.Entities;

    using Bars.Gkh.Qa.Utils;

    [Binding]
    class InspectorSteps : BindingBase
    {
        private BindingBase.DomainServiceCashe<Inspector> _cashe = new BindingBase.DomainServiceCashe<Inspector>();

        [Given(@"пользователь добавляет нового инспектора")]
        public void ДопустимПользовательДобавляетНовогоИнспектора()
        {
            InspectorHelper.Current = new Inspector();
        }

        [Given(@"пользователь у этого инспектора заполняет поле Код ""(.*)""")]
        public void ДопустимПользовательУЭтогоИнспектораЗаполняетПолеКод(string code)
        {
            InspectorHelper.Current.Code = code;
        }

        [Given(@"пользователь у этого инспектора заполняет поле Должность ""(.*)""")]
        public void ДопустимПользовательУЭтогоИнспектораЗаполняетПолеДолжность(string position)
        {
            InspectorHelper.Current.Position = position;
        }

        [Given(@"пользователь у этого инспектора заполняет поле ФИО ""(.*)""")]
        public void ДопустимПользовательУЭтогоИнспектораЗаполняетПолеФИО(string fio)
        {
            InspectorHelper.Current.Fio = fio;
        }

        [Given(@"пользователь у этого инспектора заполняет поле Фамилия И\.О\. ""(.*)""")]
        public void ДопустимПользовательУЭтогоИнспектораЗаполняетПолеФамилияИ_О_(string shortFio)
        {
            InspectorHelper.Current.ShortFio = shortFio;
        }

        [Given(@"пользователь у этого инспектора заполняет поле Телефон ""(.*)""")]
        public void ДопустимПользовательУЭтогоИнспектораЗаполняетПолеТелефон(string phone)
        {
            InspectorHelper.Current.Phone = phone;
        }

        [Given(@"пользователь у этого инспектора заполняет поле Электронная почта ""(.*)""")]
        public void ДопустимПользовательУЭтогоИнспектораЗаполняетПолеЭлектроннаяПочта(string email)
        {
            InspectorHelper.Current.Email = email;
        }

        [Given(@"пользователь у этого инспектора заполняет поле Родительный ""(.*)""")]
        public void ДопустимПользовательУЭтогоИнспектораЗаполняетПолеРодительный(string fioGenitive)
        {
            InspectorHelper.Current.FioGenitive = fioGenitive;
        }

        [Given(@"пользователь у этого инспектора заполняет поле Дательный ""(.*)""")]
        public void ДопустимПользовательУЭтогоИнспектораЗаполняетПолеДательный(string fioDative)
        {
            InspectorHelper.Current.FioDative = fioDative;
        }

        [Given(@"пользователь у этого инспектора заполняет поле Винительный ""(.*)""")]
        public void ДопустимПользовательУЭтогоИнспектораЗаполняетПолеВинительный(string fioAccusative)
        {
            InspectorHelper.Current.FioAccusative = fioAccusative;
        }

        [Given(@"пользователь у этого инспектора заполняет поле Творительный ""(.*)""")]
        public void ДопустимПользовательУЭтогоИнспектораЗаполняетПолеТворительный(string fioAblative)
        {
            InspectorHelper.Current.FioAblative = fioAblative;
        }

        [Given(@"пользователь у этого инспектора заполняет поле Предложный ""(.*)""")]
        public void ДопустимПользовательУЭтогоИнспектораЗаполняетПолеПредложный(string fioPrepositional)
        {
            InspectorHelper.Current.FioPrepositional = fioPrepositional;
        }

        [Given(@"пользователь у этого инспектора заполняет поле Код (.*) символов ""(.*)""")]
        public void ДопустимПользовательУЭтогоИнспектораЗаполняетПолеКодСимволов(int countOfSymbols, string symbol)
        {
            InspectorHelper.Current.Code =
                CommonHelper.DuplicateLine(symbol, countOfSymbols);
        }

        [Given(@"пользователь у этого инспектора заполняет поле Должность (.*) символов ""(.*)""")]
        public void ДопустимПользовательУЭтогоИнспектораЗаполняетПолеДолжностьСимволов(int countOfSymbols, string symbol)
        {
            InspectorHelper.Current.Position =
               CommonHelper.DuplicateLine(symbol, countOfSymbols);
        }

        [Given(@"пользователь у этого инспектора заполняет поле ФИО (.*) символов ""(.*)""")]
        public void ДопустимПользовательУЭтогоИнспектораЗаполняетПолеФИОСимволов(int countOfSymbols, string symbol)
        {
            InspectorHelper.Current.Fio =
              CommonHelper.DuplicateLine(symbol, countOfSymbols);
        }

        [Given(@"пользователь у этого инспектора заполняет поле Фамилия И\.О\. (.*) символов ""(.*)""")]
        public void ДопустимПользовательУЭтогоИнспектораЗаполняетПолеФамилияИ_О_Символов(int countOfSymbols, string symbol)
        {
            InspectorHelper.Current.ShortFio =
              CommonHelper.DuplicateLine(symbol, countOfSymbols);
        }

        [Given(@"пользователь у этого инспектора заполняет поле Телефон (.*) символов ""(.*)""")]
        public void ДопустимПользовательУЭтогоИнспектораЗаполняетПолеТелефонСимволов(int countOfSymbols, string symbol)
        {
            InspectorHelper.Current.Phone =
              CommonHelper.DuplicateLine(symbol, countOfSymbols);
        }

        [Given(@"пользователь у этого инспектора заполняет поле Электронная почта (.*) символов ""(.*)""")]
        public void ДопустимПользовательУЭтогоИнспектораЗаполняетПолеЭлектроннаяПочтаСимволов(int countOfSymbols, string symbol)
        {
            InspectorHelper.Current.Email =
              CommonHelper.DuplicateLine(symbol, countOfSymbols);
        }

        [Given(@"пользователь у этого инспектора в Падежи ФИО заполняет поле Родительный (.*) символов ""(.*)""")]
        public void ДопустимПользовательУЭтогоИнспектораВПадежиФИОЗаполняетПолеРодительныйСимволов(int countOfSymbols, string symbol)
        {
            InspectorHelper.Current.FioGenitive =
              CommonHelper.DuplicateLine(symbol, countOfSymbols);
        }

        [Given(@"пользователь у этого инспектора в Падежи ФИО заполняет поле Дательный (.*) символов ""(.*)""")]
        public void ДопустимПользовательУЭтогоИнспектораВПадежиФИОЗаполняетПолеДательныйСимволов(int countOfSymbols, string symbol)
        {
            InspectorHelper.Current.FioDative =
             CommonHelper.DuplicateLine(symbol, countOfSymbols);
        }

        [Given(@"пользователь у этого инспектора в Падежи ФИО заполняет поле Винительный (.*) символов ""(.*)""")]
        public void ДопустимПользовательУЭтогоИнспектораВПадежиФИОЗаполняетПолеВинительныйСимволов(int countOfSymbols, string symbol)
        {
            InspectorHelper.Current.FioAccusative =
             CommonHelper.DuplicateLine(symbol, countOfSymbols);
        }

        [Given(@"пользователь у этого инспектора в Падежи ФИО заполняет поле Творительный (.*) символов ""(.*)""")]
        public void ДопустимПользовательУЭтогоИнспектораВПадежиФИОЗаполняетПолеТворительныйСимволов(int countOfSymbols, string symbol)
        {
            InspectorHelper.Current.FioAblative =
             CommonHelper.DuplicateLine(symbol, countOfSymbols);
        }

        [Given(@"пользователь у этого инспектора в Падежи ФИО заполняет поле Предложный (.*) символов ""(.*)""")]
        public void ДопустимПользовательУЭтогоИнспектораВПадежиФИОЗаполняетПолеПредложныйСимволов(int countOfSymbols, string symbol)
        {
            InspectorHelper.Current.FioPrepositional =
             CommonHelper.DuplicateLine(symbol, countOfSymbols);
        }

        [Given(@"пользователь у этого инспектора в Падежи Должность заполняет поле Родительный (.*) символов ""(.*)""")]
        public void ДопустимПользовательУЭтогоИнспектораВПадежиДолжностьЗаполняетПолеРодительныйСимволов(int countOfSymbols, string symbol)
        {
            InspectorHelper.Current.PositionGenitive =
             CommonHelper.DuplicateLine(symbol, countOfSymbols);
        }

        [Given(@"пользователь у этого инспектора в Падежи Должность заполняет поле Дательный (.*) символов ""(.*)""")]
        public void ДопустимПользовательУЭтогоИнспектораВПадежиДолжностьЗаполняетПолеДательныйСимволов(int countOfSymbols, string symbol)
        {
            InspectorHelper.Current.PositionDative =
             CommonHelper.DuplicateLine(symbol, countOfSymbols);
        }

        [Given(@"пользователь у этого инспектора в Падежи Должность заполняет поле Винительный (.*) символов ""(.*)""")]
        public void ДопустимПользовательУЭтогоИнспектораВПадежиДолжностьЗаполняетПолеВинительныйСимволов(int countOfSymbols, string symbol)
        {
            InspectorHelper.Current.PositionAccusative =
             CommonHelper.DuplicateLine(symbol, countOfSymbols);
        }

        [Given(@"пользователь у этого инспектора в Падежи Должность заполняет поле Творительный (.*) символов ""(.*)""")]
        public void ДопустимПользовательУЭтогоИнспектораВПадежиДолжностьЗаполняетПолеТворительныйСимволов(int countOfSymbols, string symbol)
        {
            InspectorHelper.Current.PositionAblative =
             CommonHelper.DuplicateLine(symbol, countOfSymbols);
        }

        [Given(@"пользователь у этого инспектора в Падежи Должность заполняет поле Предложный (.*) символов ""(.*)""")]
        public void ДопустимПользовательУЭтогоИнспектораВПадежиДолжностьЗаполняетПолеПредложныйСимволов(int countOfSymbols, string symbol)
        {
            InspectorHelper.Current.PositionPrepositional =
             CommonHelper.DuplicateLine(symbol, countOfSymbols);
        }

        [Given(@"пользователь у этого инспектора заполняет поле Зонально жилищная инспекция ""(.*)""")]
        public void ДопустимПользовательУЭтогоИнспектораЗаполняетПолеЗональноЖилищнаяИнспекция(string zonalInspectionName)
        {
            var zonalInspection =
                _cashe.Get<ZonalInspection>().GetAll().FirstOrDefault(x => x.Name == zonalInspectionName);

            if (zonalInspection == null)
            {
                throw new SpecFlowException("Не найдено Отдела с таким именем");
            }

            InspectorHelper.AddZonalInspection(zonalInspection);
        }

        [Given(@"пользователь у этого инспектора заполняет поле Зонально жилищная инспекция")]
        public void ДопустимПользовательУЭтогоИнспектораЗаполняетПолеЗональноЖилищнаяИнспекция()
        {
            InspectorHelper.AddZonalInspection(ZonalInspectionHelper.Current);
        }

        [Given(@"пользователь у этого инспектора добавляет подписку на родительского инспектора")]
        public void ДопустимПользовательУЭтогоИнспектораДобавляетПодпискуНаРодительскогоИнспектора()
        {
            var parsedSignedInspector = new KeyValuePair<string, object>("signedInpectorId",
               InspectorHelper.Current.Id.ToString());

            var parsedInspectorToSign = new KeyValuePair<string, object>("inpectorIds", InspectorHelper.ParrentInspector.Id.ToString());

            var inspectorService = Container.Resolve<IInspectorService>();

            var paramsDict = new DynamicDictionary { parsedSignedInspector, parsedInspectorToSign };

            var baseParams = new BaseParams { Params = paramsDict };

            inspectorService.SubcribeToInspectors(baseParams);
        }


        [Given(@"добавлен инспектор")]
        public void ДопустимДобавленИнспектор(Table inspectorTable)
        {
            var inspector = inspectorTable.CreateInstance<Inspector>();

            this._cashe.Current.SaveOrUpdate(inspector);

            InspectorHelper.Current = inspector;
        }

        [When(@"пользователь сохраняет этого инспектора")]
        public void ЕслиПользовательСохраняетЭтогоИнспектора()
        {
            try
            {
                this._cashe.Current.SaveOrUpdate(InspectorHelper.Current);
            }
            catch (Exception ex)
            {
                ExceptionHelper.AddException(System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message);

                Container.Resolve<ISessionProvider>().GetCurrentSession().Clear();
            }
        }

        [When(@"пользователь удаляет этого инспектора")]
        public void ЕслиПользовательУдаляетЭтогоИнспектора()
        {
            try
            {
                this._cashe.Current.Delete(InspectorHelper.Current.Id);
            }
            catch (Exception ex)
            {
                ExceptionHelper.TestExceptions.Add(System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }

        [Then(@"запись по этому инспектору присутствует в справочнике инспекторов")]
        public void ТоЗаписьПоЭтомуИнспекторуПрисутствуетВСправочникеИнспекторов()
        {
            var inspector = this._cashe.Current.Get(InspectorHelper.Current.Id);

            inspector.Should().NotBeNull(
                string.Format("Инспектор должен присутствовать в справочнике инспекторов.{0}",
                ExceptionHelper.GetExceptions()));
        }

        [Then(@"запись по этому инспектору отсутствует в справочнике инспекторов")]
        public void ТоЗаписьПоЭтомуИнспекторуОтсутствуетВСправочникеИнспекторов()
        {
            var inspector = this._cashe.Current.Get(InspectorHelper.Current.Id);

            inspector.Should().BeNull(
                string.Format("Инспектор должен отсутствовать в справочнике инспекторов.{0}",
                ExceptionHelper.GetExceptions()));
        }
    }
}
