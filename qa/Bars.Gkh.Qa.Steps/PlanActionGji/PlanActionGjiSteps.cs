namespace Bars.Gkh.Qa.Steps
{
    using System;

    using Bars.B4;
    using Bars.Gkh.Qa.Steps.CommonSteps;
    using Bars.Gkh.Qa.Utils;
    using Bars.GkhGji.Entities;

    using FluentAssertions;

    using TechTalk.SpecFlow;

    [Binding]
    public class PlanActionGjiSteps : BindingBase
    {
        private IDomainService DomainService
        {
            get
            {
                Type entityType = PlanActionGjiHelper.Current.GetType();

                var t = typeof(IDomainService<>).MakeGenericType(entityType);
                return (IDomainService)Container.Resolve(t);
            }
        }

        [Given(@"пользователь добавляет новый план мероприятия")]
        public void ДопустимПользовательДобавляетНовыйПланМероприятия()
        {
            PlanActionGjiHelper.Current = new PlanActionGji();
        }

        [Given(@"пользователь у этого плана мероприятия заполняет поле Наименование ""(.*)""")]
        public void ДопустимПользовательУЭтогоПланаМероприятияЗаполняетПолеНаименование(string name)
        {
            PlanActionGjiHelper.Current.Name = name;
        }

        [Given(@"пользователь у этого плана мероприятия заполняет поле Дата начала ""(.*)""")]
        public void ДопустимПользовательУЭтогоПланаМероприятияЗаполняетПолеДатаНачала(string dataStart)
        {
            PlanActionGjiHelper.Current.DateStart = dataStart.DateParse();
        }

        [Given(@"пользователь у этого плана мероприятия заполняет поле Дата окончания ""(.*)""")]
        public void ДопустимПользовательУЭтогоПланаМероприятияЗаполняетПолеДатаОкончания(string dataEnd)
        {
            PlanActionGjiHelper.Current.DateEnd = dataEnd.DateParse();
        }

        [When(@"пользователь сохраняет этот план мероприятия")]
        public void ЕслиПользовательСохраняетЭтотПланМероприятия()
        {
            var id = PlanActionGjiHelper.Current.Id;

            try
            {
                if (id == 0)
                {
                    this.DomainService.Save(PlanActionGjiHelper.Current);
                }
                else
                {
                    this.DomainService.Update(PlanActionGjiHelper.Current);
                }
            }
            catch (Exception ex)
            {
                ExceptionHelper.AddException(System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }

        [When(@"пользователь удаляет этот план мероприятия")]
        public void ЕслиПользовательУдаляетЭтотПланМероприятия()
        {
            var id = PlanActionGjiHelper.Current.Id;

            try
            {
                this.DomainService.Delete(id);
            }
            catch (Exception ex)
            {
                ExceptionHelper.AddException(System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }

        [Then(@"запись по этому плану мероприятия присутствует в справочнике планов мероприятий")]
        public void ТоЗаписьПоЭтомуПлануМероприятияПрисутствуетВСправочникеПлановМероприятий()
        {
            var id = PlanActionGjiHelper.Current.Id;

            var current = this.DomainService.Get(id);

            current.Should()
                .NotBeNull(
                    string.Format(
                        "запись по этому плану мероприятий должна присутствовать в разделе отчетных периодов{0}", 
                        ExceptionHelper.GetExceptions()));
        }

        [Then(@"запись по этому плану мероприятия отсутствует в справочнике планов мероприятий")]
        public void ТоЗаписьПоЭтомуПлануМероприятияОтсутствуетВСправочникеПлановМероприятий()
        {
            var id = PlanActionGjiHelper.Current.Id;

            var current = this.DomainService.Get(id);

            current.Should()
                .BeNull(
                    string.Format(
                        "запись по этому плану мероприятий должна отсутствовать в разделе отчетных периодов{0}", 
                        ExceptionHelper.GetExceptions()));
        }

        [Given(@"добавлен план мероприятий")]
        public void ДопустимДобавленПланМероприятий(Table table)
        {
            PlanActionGjiHelper.Current = new PlanActionGji
                                              {
                                                  Name = table.Rows[0]["Name"], 
                                                  DateStart = DateTime.Parse(table.Rows[0]["DateStart"]), 
                                                  DateEnd = DateTime.Parse(table.Rows[0]["DateEnd"])
                                              };

            var id = PlanActionGjiHelper.Current.Id;

            try
            {
                if (id == 0)
                {
                    this.DomainService.Save(PlanActionGjiHelper.Current);
                }
                else
                {
                    this.DomainService.Update(PlanActionGjiHelper.Current);
                }
            }
            catch (Exception ex)
            {
                ExceptionHelper.AddException(System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }
    }
}
