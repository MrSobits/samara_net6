namespace Bars.Gkh.Qa.Steps
{
    using System;
    using System.Reflection;

    using Bars.Gkh.Domain;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Enums;
    using Bars.Gkh.Qa.Utils;

    using FluentAssertions;

    using TechTalk.SpecFlow;

    [Binding]
    internal class WorkKindCurrentRepairSteps : BindingBase
    {
        private DomainServiceCashe<WorkKindCurrentRepair> _cashe = new DomainServiceCashe<WorkKindCurrentRepair>();

        [Given(@"пользователь добавляет новый вид работ текущего ремонта")]
        public void ДопустимПользовательДобавляетНовыйВидРаботТекущегоРемонта()
        {
            WorkKindCurrentRepairHelper.Current = new WorkKindCurrentRepair();
        }

        [Given(@"пользователь у этого вида работ текущего ремонта заполняет поле Код ""(.*)""")]
        public void ДопустимПользовательУЭтогоВидаРаботТекущегоРемонтаЗаполняетПолеКод(string code)
        {
            WorkKindCurrentRepairHelper.Current.Code = code;
        }

        [Given(@"пользователь у этого вида работ текущего ремонта заполняет поле Наименование ""(.*)""")]
        public void ДопустимПользовательУЭтогоВидаРаботТекущегоРемонтаЗаполняетПолеНаименование(string name)
        {
            WorkKindCurrentRepairHelper.Current.Name = name;
        }

        [Given(@"пользователь у этого вида работ текущего ремонта заполняет поле Ед\. измерения")]
        public void ДопустимПользовательУЭтогоВидаРаботТекущегоРемонтаЗаполняетПолеЕд_Измерения()
        {
            WorkKindCurrentRepairHelper.Current.UnitMeasure = UnitMeasureHelper.Current;
        }

        [Given(@"пользователь у этого вида работ текущего ремонта заполняет поле Тип работ ""(.*)""")]
        public void ДопустимПользовательУЭтогоВидаРаботТекущегоРемонтаЗаполняетПолеТипРабот(string workTypeName)
        {
            TypeWork workType;

            switch (workTypeName)
            {
                case "Работа":
                    {
                        workType = TypeWork.Work;
                        break;
                    }

                case "Услуга":
                    {
                        workType = TypeWork.Service;
                        break;
                    }

                default:
                    {
                        throw new SpecFlowException(string.Format("Типа работ {0} не существует", workTypeName));
                    }
            }

            WorkKindCurrentRepairHelper.Current.TypeWork = workType;
        }

        [Given(@"пользователь у этого вида работ текущего ремонта заполняет поле Код (.*) символов ""(.*)""")]
        public void ДопустимПользовательУЭтогоВидаРаботТекущегоРемонтаЗаполняетПолеКодСимволов(int countOfSymbols, string symbol)
        {
            WorkKindCurrentRepairHelper.Current.Code =
               CommonHelper.DuplicateLine(symbol, countOfSymbols);
        }

        [Given(@"пользователь у этого вида работ текущего ремонта заполняет поле Наименование (.*) символов ""(.*)""")]
        public void ДопустимПользовательУЭтогоВидаРаботТекущегоРемонтаЗаполняетПолеНаименованиеСимволов(int countOfSymbols, string symbol)
        {
            WorkKindCurrentRepairHelper.Current.Name =
               CommonHelper.DuplicateLine(symbol, countOfSymbols);
        }


        [When(@"пользователь сохраняет этот вид работ текущего ремонта")]
        public void ЕслиПользовательСохраняетЭтотВидРаботТекущегоРемонта()
        {
            try
            {
                this._cashe.Current.SaveOrUpdate(WorkKindCurrentRepairHelper.Current);
            }
            catch (Exception ex)
            {
                ExceptionHelper.TestExceptions.Add(MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }

        [When(@"пользователь удаляет этот вид работ текущего ремонта")]
        public void ЕслиПользовательУдаляетЭтотВидРаботТекущегоРемонта()
        {
            try
            {
                this._cashe.Current.Delete(WorkKindCurrentRepairHelper.Current.Id);
            }
            catch (Exception ex)
            {
                ExceptionHelper.TestExceptions.Add(MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }

        [Then(@"запись по этому виду работ текущего ремонта присутствует в справочнике видов работ текущего ремонта")]
        public void ТоЗаписьПоЭтомуВидуРаботТекущегоРемонтаПрисутствуетВСправочникеВидовРаботТекущегоРемонта()
        {
            var workKindCurrentRepair = this._cashe.Current.Get(WorkKindCurrentRepairHelper.Current.Id);

            workKindCurrentRepair.Should()
                .NotBeNull(
                    string.Format(
                        "запись по виду работ текущего ремонта должна присутствовать в справочнике работ текущего ремонта.{0}",
                        ExceptionHelper.GetExceptions()));
        }


        [Then(@"запись по этому виду работ текущего ремонта отсутствует в справочнике видов работ текущего ремонта")]
        public void ТоЗаписьПоЭтомуВидуРаботТекущегоРемонтаОтсутствуетВСправочникеВидовРаботТекущегоРемонта()
        {
            var workKindCurrentRepair = this._cashe.Current.Get(WorkKindCurrentRepairHelper.Current.Id);

            workKindCurrentRepair.Should()
                .BeNull(
                    string.Format(
                        "запись по виду работ текущего ремонта должна отсутствовать в справочнике работ текущего ремонта.{0}",
                        ExceptionHelper.GetExceptions()));
        }

    }
}
