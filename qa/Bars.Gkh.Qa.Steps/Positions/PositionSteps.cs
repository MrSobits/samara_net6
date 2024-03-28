using System;
using System.ComponentModel;
using System.Linq.Expressions;
using Bars.B4;
using Bars.Gkh.Domain;
using Bars.Gkh.Entities;
using Bars.Gkh.Qa.Utils;
using FluentAssertions;
using TechTalk.SpecFlow;

namespace Bars.Gkh.Qa.Steps.Positions
{
    [Binding]
    public class PositionSteps : BindingBase
    {

        private IDomainService<Position> ds = Container.Resolve<IDomainService<Position>>();

        [Given(@"пользователь добавляет новую должность")]
        public void ДопустимПользовательДобавляетНовуюДолжность()
        {
            PositionHelper.Current = new Position();
        }
        
        [Given(@"пользователь у этой должности заполняет поле Наименование ""(.*)""")]
        public void ДопустимПользовательУЭтойДолжностиЗаполняетПолеНаименование(string name)
        {
            PositionHelper.Current.Name = name;
        }
        
        [Given(@"пользователь у этой должности заполняет поле Родительный ""(.*)""")]
        public void ДопустимПользовательУЭтойДолжностиЗаполняетПолеРодительный(string nameGenitive)
        {
            PositionHelper.Current.NameGenitive = nameGenitive;
        }
        
        [Given(@"пользователь у этой должности заполняет поле Дательный ""(.*)""")]
        public void ДопустимПользовательУЭтойДолжностиЗаполняетПолеДательный(string nameDative)
        {
            PositionHelper.Current.NameDative = nameDative;
        }
        
        [Given(@"пользователь у этой должности заполняет поле Винительный ""(.*)""")]
        public void ДопустимПользовательУЭтойДолжностиЗаполняетПолеВинительный(string nameAccusative)
        {
            PositionHelper.Current.NameAccusative = nameAccusative;
        }
        
        [Given(@"пользователь у этой должности заполняет поле Творительный ""(.*)""")]
        public void ДопустимПользовательУЭтойДолжностиЗаполняетПолеТворительный(string nameAblative)
        {
            PositionHelper.Current.NameAblative = nameAblative;
        }
        
        [Given(@"пользователь у этой должности заполняет поле Предложный ""(.*)""")]
        public void ДопустимПользовательУЭтойДолжностиЗаполняетПолеПредложный(string namePrepositional)
        {
            PositionHelper.Current.NamePrepositional = namePrepositional;
        }
        
        [When(@"пользователь сохраняет эту должность")]
        public void ЕслиПользовательСохраняетЭтуДолжность()
        {
            try
            {
                ds.SaveOrUpdate(PositionHelper.Current);
            }
            catch (Exception ex)
            {
                ExceptionHelper.TestExceptions.Add(System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }
        
        [When(@"пользователь удаляет эту должность")]
        public void ЕслиПользовательУдаляетЭтуДолжность()
        {
            try
            {
                ds.Delete(PositionHelper.Current.Id);
            }
            catch (Exception ex)
            {
                ExceptionHelper.TestExceptions.Add(System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }
        
        [Then(@"запись по этой должности присутствует в справочнике должностей")]
        public void ТоЗаписьПоЭтойДолжностиПрисутствуетВСправочникеДолжностей()
        {
            ds.Get(PositionHelper.Current.Id).Should().NotBeNull(String.Format("должность должна присутствовать{0}", ExceptionHelper.GetExceptions()));
        }
        
        [Then(@"запись по этой должности отсутствует в справочнике должностей")]
        public void ТоЗаписьПоЭтойДолжностиОтсутствуетВСправочникеДолжностей()
        {
            ds.Get(PositionHelper.Current.Id).Should().BeNull(String.Format("должность должна отсутствовать{0}", ExceptionHelper.GetExceptions()));
        }

        [Given(@"пользователь у этой должности заполняет поле Наименование (.*) символов ""(.*)""")]
        public void ДопустимПользовательУЭтойДолжностиЗаполняетПолеНаименованиеСимволов(int length, char symbol)
        {
            PositionHelper.Current.Name = new string(symbol , length);
        }

        [Given(@"пользователь у этой должности заполняет поле Код ""(.*)""")]
        public void ДопустимПользовательУЭтойДолжностиЗаполняетПолеКод(string code)
        {
            PositionHelper.Current.Code = code;
        }

        [Given(@"пользователь у этой должности заполняет поле Код (.*) символов ""(.*)""")]
        public void ДопустимПользовательУЭтойДолжностиЗаполняетПолеКодСимволов(int length, char symbol)
        {
            PositionHelper.Current.Code = new string(symbol , length);
        }


        [Given(@"пользователь у этой должности заполняет поле Родительный (.*) символов ""(.*)""")]
        public void ДопустимПользовательУЭтойДолжностиЗаполняетПолеРодительныйСимволов(int length, char symbol)
        {
            PositionHelper.Current.NameGenitive = new string(symbol, length);
        }

        [Given(@"пользователь у этой должности заполняет поле Дательный (.*) символов ""(.*)""")]
        public void ДопустимПользовательУЭтойДолжностиЗаполняетПолеДательныйСимволов(int length, char symbol)
        {
            PositionHelper.Current.NameDative = new string(symbol, length);
        }

        [Given(@"пользователь у этой должности заполняет поле Винительный (.*) символов ""(.*)""")]
        public void ДопустимПользовательУЭтойДолжностиЗаполняетПолеВинительныйСимволов(int length, char symbol)
        {
            PositionHelper.Current.NameAccusative = new string(symbol, length);
        }

        [Given(@"пользователь у этой должности заполняет поле Творительный (.*) символов ""(.*)""")]
        public void ДопустимПользовательУЭтойДолжностиЗаполняетПолеТворительныйСимволов(int length, char symbol)
        {
            PositionHelper.Current.NameAblative = new string(symbol, length);
        }

        [Given(@"пользователь у этой должности заполняет поле Предложный (.*) символов ""(.*)""")]
        public void ДопустимПользовательУЭтойДолжностиЗаполняетПолеПредложныйСимволов(int length, char symbol)
        {
            PositionHelper.Current.NamePrepositional = new string(symbol, length);
        }


    }
}
