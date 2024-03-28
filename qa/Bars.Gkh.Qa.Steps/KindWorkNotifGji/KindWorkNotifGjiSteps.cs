namespace Bars.Gkh.Qa.Steps
{
    using System;

    using Bars.B4;
    
    using Bars.Gkh.Qa.Utils;

    using FluentAssertions;

    using TechTalk.SpecFlow;

    [Binding]
    internal class KindWorkNotifGjiSteps : BindingBase
    {
        private IDomainService DomainService
        {
            get
            {
                Type entityType = KindWorkNotifGjiHelper.Current.GetType();

                var t = typeof(IDomainService<>).MakeGenericType(entityType);
                return (IDomainService)Container.Resolve(t);
            }
        }

        [Given(@"пользователь добавляет новый вид работы \(уведомление\)")]
        public void ДопустимПользовательДобавляетНовыйВидРаботыУведомление()
        {
            KindWorkNotifGjiHelper.Current = Activator
                .CreateInstance("Bars.GkhGji", "Bars.GkhGji.Entities.KindWorkNotifGji").Unwrap();
        }

        [Given(@"пользователь у этого вида работы \(уведомление\) заполняет поле Наименование ""(.*)""")]
        public void ДопустимПользовательУЭтогоВидаРаботыУведомлениеЗаполняетПолеНаименование(string name)
        {
            KindWorkNotifGjiHelper.ChangeCurrent("Name", name);
        }

        [Given(@"пользователь у этого вида работы \(уведомление\) заполняет поле Код ""(.*)""")]
        public void ДопустимПользовательУЭтогоВидаРаботыУведомлениеЗаполняетПолеКод(string code)
        {
            KindWorkNotifGjiHelper.ChangeCurrent("Code", code);
        }

        [Given(@"пользователь у этого вида работы \(уведомление\) заполняет поле Наименование (.*) символов ""(.*)""")]
        public void ДопустимПользовательУЭтогоВидаРаботыУведомлениеЗаполняетПолеНаименованиеСимволов(int countOfSymbols, string symbol)
        {
            KindWorkNotifGjiHelper.ChangeCurrent("Name", CommonHelper.DuplicateLine(symbol, countOfSymbols));
        }

        [Given(@"пользователь у этого вида работы \(уведомление\) заполняет поле Код (.*) символов ""(.*)""")]
        public void ДопустимПользовательУЭтогоВидаРаботыУведомлениеЗаполняетПолеКодСимволов(int countOfSymbols, string symbol)
        {
            KindWorkNotifGjiHelper.ChangeCurrent("Code", CommonHelper.DuplicateLine(symbol, countOfSymbols));
        }

        [When(@"пользователь сохраняет этот вид работы \(уведомление\)")]
        public void ЕслиПользовательСохраняетЭтотВидРаботыУведомление()
        {
            Type entityType = KindWorkNotifGjiHelper.Current.GetType();

            var id = long.Parse(entityType.GetProperty("Id").GetValue(KindWorkNotifGjiHelper.Current).ToString());

            try
            {
                if (id == 0)
                {
                    this.DomainService.Save(KindWorkNotifGjiHelper.Current);
                }
                else
                {
                    this.DomainService.Update(KindWorkNotifGjiHelper.Current);
                }
            }
            catch (Exception ex)
            {
                ExceptionHelper.TestExceptions.Add(System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }

        [When(@"пользователь удаляет этот вид работы \(уведомление\)")]
        public void ЕслиПользовательУдаляетЭтотВидРаботыУведомление()
        {
            Type entityType = KindWorkNotifGjiHelper.Current.GetType();

            var id = long.Parse(entityType.GetProperty("Id").GetValue(KindWorkNotifGjiHelper.Current).ToString());

            try
            {
                this.DomainService.Delete(id);
            }
            catch (Exception ex)
            {
                ExceptionHelper.TestExceptions.Add(System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }

        [Then(@"запись по этому виду работы \(уведомление\) присутствует в справочнике видов работы \(уведомление\)")]
        public void ТоЗаписьПоЭтомуВидуРаботыУведомлениеПрисутствуетВСправочникеВидовРаботыУведомление()
        {
            Type entityType = KindWorkNotifGjiHelper.Current.GetType();

            var id = long.Parse(entityType.GetProperty("Id").GetValue(KindWorkNotifGjiHelper.Current).ToString());

            var kindWorkNotifGji = this.DomainService.Get(id);

            kindWorkNotifGji.Should()
                .NotBeNull(
                    string.Format(
                        "вид работ (уведомление) должен присутствовать в справочнике видов работы (уведомление).{0}",
                        ExceptionHelper.GetExceptions()));
        }

        [Then(@"запись по этому виду работы \(уведомление\) отсутствует в справочнике видов работы \(уведомление\)")]
        public void ТоЗаписьПоЭтомуВидуРаботыУведомлениеОтсутствуетВСправочникеВидовРаботыУведомление()
        {
            Type entityType = KindWorkNotifGjiHelper.Current.GetType();

            var id = long.Parse(entityType.GetProperty("Id").GetValue(KindWorkNotifGjiHelper.Current).ToString());

            var kindWorkNotifGji = this.DomainService.Get(id);

            kindWorkNotifGji.Should()
                .BeNull(
                    string.Format(
                        "вид работ (уведомление) должен отсутствовать в справочнике видов работы (уведомление).{0}",
                        ExceptionHelper.GetExceptions()));
        }
    }
}
