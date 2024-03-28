namespace Bars.Gkh.Qa.Steps
{
    using System;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.Utils;
    using Bars.Gkh.Domain;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Qa.Utils;
    using Bars.GkhCr.DomainService;
    using Bars.GkhCr.Entities;
    using Bars.GkhCr.Enums;

    using FluentAssertions;

    using TechTalk.SpecFlow;

    [Binding]
    internal class ProgramCrSteps : BindingBase
    {
        private DomainServiceCashe<ProgramCr> _cashe = new DomainServiceCashe<ProgramCr>();
            
        [Given(@"пользователь добавляет новую программу капитального ремонта")]
        public void ДопустимПользовательДобавляетНовуюПрограммуКапитальногоРемонта()
        {
            ProgramCrHelper.Current = new ProgramCr();
        }

        [Given(@"пользователь у этой программы капитального ремонта заполняет поле Наименование ""(.*)""")]
        public void ДопустимПользовательУЭтойПрограммыКапитальногоРемонтаЗаполняетПолеНаименование(string name)
        {
            ProgramCrHelper.Current.Name = name;
        }

        [Given(@"пользователь у этой программы капитального ремонта заполняет поле Код ""(.*)""")]
        public void ДопустимПользовательУЭтойПрограммыКапитальногоРемонтаЗаполняетПолеКод(string code)
        {
            ProgramCrHelper.Current.Code = code;
        }

        [Given(@"пользователь у этой программы капитального ремонта заполняет поле Период")]
        public void ДопустимПользовательУЭтойПрограммыКапитальногоРемонтаЗаполняетПолеПериод()
        {
            ProgramCrHelper.Current.Period = PeriodHelper.Current;
        }

        [Given(@"пользователь у этой программы капитального ремонта заполняет поле Примечание ""(.*)""")]
        public void ДопустимПользовательУЭтойПрограммыКапитальногоРемонтаЗаполняетПолеПримечание(string description)
        {
            ProgramCrHelper.Current.Description = description;
        }

        [Given(@"пользователь у этой программы капитального ремонта заполняет поле Видимость ""(.*)""")]
        public void ДопустимПользовательУЭтойПрограммыКапитальногоРемонтаЗаполняетПолеВидимость(string visibility)
        {
            ProgramCrHelper.Current.TypeVisibilityProgramCr =
                EnumHelper.GetFromDisplayValue<TypeVisibilityProgramCr>(visibility);
        }

        [Given(@"пользователь у этой программы капитального ремонта заполняет поле Состояние ""(.*)""")]
        public void ДопустимПользовательУЭтойПрограммыКапитальногоРемонтаЗаполняетПолеСостояние(string programState)
        {
            ProgramCrHelper.Current.TypeProgramStateCr =
                EnumHelper.GetFromDisplayValue<TypeProgramStateCr>(programState);
        }

        [Given(@"пользователь у этой программы капитального ремонта заполняет поле Наименование (.*) символов ""(.*)""")]
        public void ДопустимПользовательУЭтойПрограммыКапитальногоРемонтаЗаполняетПолеНаименованиеСимволов(int countOfSymbols, string symbol)
        {
            ProgramCrHelper.Current.Name =
                CommonHelper.DuplicateLine(symbol, countOfSymbols);
        }

        [Given(@"пользователь у этой программы капитального ремонта заполняет поле Код (.*) символов ""(.*)""")]
        public void ДопустимПользовательУЭтойПрограммыКапитальногоРемонтаЗаполняетПолеКодСимволов(int countOfSymbols, string symbol)
        {
            ProgramCrHelper.Current.Code =
                CommonHelper.DuplicateLine(symbol, countOfSymbols);
        }

        [Given(@"пользователь у этой программы капитального ремонта заполняет поле Примечание (.*) символов ""(.*)""")]
        public void ДопустимПользовательУЭтойПрограммыКапитальногоРемонтаЗаполняетПолеПримечаниеСимволов(int countOfSymbols, string symbol)
        {
            ProgramCrHelper.Current.Description =
                CommonHelper.DuplicateLine(symbol, countOfSymbols);
        }

        [Given(@"добавлена программа капитального ремонта")]
        public void ДопустимДобавленаПрограммаКапитальногоРемонта(Table table)
        {
            var programCr = new ProgramCr();

            var data = table.Rows.First();

            programCr.Name = data["Name"];

            programCr.Period = this._cashe.Get<Period>().GetAll()
                .OrderByDescending(x => x.ObjectCreateDate).FirstOrDefault(x => x.Name == data["Period"]);

            this._cashe.Current.Save(programCr);

            ProgramCrHelper.Current = programCr;
        }

        [When(@"пользователь сохраняет эту программу капитального ремонта")]
        public void ЕслиПользовательСохраняетЭтуПрограммуКапитальногоРемонта()
        {
            try
            {
                this._cashe.Current.SaveOrUpdate(ProgramCrHelper.Current);
            }
            catch (Exception ex)
            {
                ExceptionHelper.TestExceptions.Add(System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }

        [When(@"пользователь удаляет эту программу капитального ремонта")]
        public void ЕслиПользовательУдаляетЭтуПрограммуКапитальногоРемонта()
        {
            try
            {
                this._cashe.Current.Delete(ProgramCrHelper.Current.Id);
            }
            catch (Exception ex)
            {
                ExceptionHelper.TestExceptions.Add(System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }

        [When(@"пользователь к этой программе капитального ремонта добавляет запись по разрезу финансирования ""(.*)""")]
        public void ДопустимПользовательКЭтойПрограммеКапитальногоРемонтаДобавляетЗаписьПоРазрезуФинансирования(string finSourceName)
        {
            var finSource = this._cashe.Get<FinanceSource>()
                .GetAll()
                .OrderByDescending(x => x.ObjectCreateDate)
                .FirstOrDefault(x => x.Name == finSourceName);

            if (finSource == null)
            {
                throw new SpecFlowException(string.Format("отсутствует разрез финансирования {0}", finSourceName));
            }

            var baseParams = new BaseParams
                                 {
                                     Params = new DynamicDictionary
                                                  {
                                                      { "programCrId", ProgramCrHelper.Current.Id },
                                                      { "objectIds", finSource.Id }
                                                  }
                                 };
            try
            {
                Container.Resolve<IProgramCrFinSourceService>().AddWorks(baseParams);
            }
            catch (Exception ex)
            {
                ExceptionHelper.AddException(System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }

        [When(@"пользователь у этой программы капитального ремонта удаляет запись по разрезу финансирования ""(.*)""")]
        public void ЕслиПользовательУЭтойПрограммеКапитальногоРемонтаУдаляетЗаписьПоРазрезуФинансирования(string finSourceName)
        {
            var programCrFinSource =
                this._cashe.Get<ProgramCrFinSource>()
                    .GetAll()
                    .FirstOrDefault(x => x.ProgramCr.Id == ProgramCrHelper.Current.Id
                        && x.FinanceSource.Name == finSourceName);

            if (programCrFinSource == null)
            {
                throw new SpecFlowException(string.Format("у этой программы капитального ремонта отсутствует запись по разрезу финансирования {}"));
            }

            try
            {
                this._cashe.Get<ProgramCrFinSource>().Delete(programCrFinSource.Id);
            }
            catch (Exception ex)
            {
                ExceptionHelper.TestExceptions.Add(System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }
       
        [Then(@"запись по этой программе капитального ремонта присутствует в разделе программ капитального ремонта")]
        public void ТоЗаписьПоЭтойПрограммеКапитальногоРемонтаПрисутствуетВРазделеПрограммКапитальногоРемонта()
        {
            var programCr = this._cashe.Current.Get(ProgramCrHelper.Current.Id);

            programCr.Should().NotBeNull(
                string.Format(
                "программа капитального ремонта должна присутствовать в разделе программ капитального ремонта.{0}",
                ExceptionHelper.GetExceptions()));
        }

        [Then(@"запись по этой программе капитального ремонта отсутствует в разделе программ капитального ремонта")]
        public void ТоЗаписьПоЭтойПрограммеКапитальногоРемонтаОтсутствуетВРазделеПрограммКапитальногоРемонта()
        {
            var programCr = this._cashe.Current.Get(ProgramCrHelper.Current.Id);

            programCr.Should().BeNull(
                string.Format(
                "программа капитального ремонта должна отсутствовать в разделе программ капитального ремонта.{0}",
                ExceptionHelper.GetExceptions()));
        }

        [Then(@"в журнале изменений присутствует запись об изменении")]
        public void ТоВЖурналеИзмененийПрисутствуетЗаписьОбИзменении()
        {
            var journal = this._cashe.Get<ProgramCrChangeJournal>().GetAll()
                .Where(x => x.ProgramCr.Id == ProgramCrHelper.Current.Id);

            var journalRecord = journal.FirstOrDefault();

            journalRecord.Should().NotBeNull(
                string.Format(
                "в журнале изменений должна быть запись об изменении.{0}",
                ExceptionHelper.GetExceptions()));

            ProgramCrHelper.CurrentProgramCrChangeJournal = journalRecord;
        }

        [Then(@"у этой записи об изменении в поле Способ формирования ""(.*)""")]
        public void ТоУЭтойЗаписиОбИзмененииВПолеСпособФормирования(string typeChangeProgramCrExternal)
        {
            var typeChangeProgramCrInternal = 
                EnumHelper
                .GetFromDisplayValue<TypeChangeProgramCr>(typeChangeProgramCrExternal);

            ProgramCrHelper.CurrentProgramCrChangeJournal.TypeChange.Should()
                .Be(
                    typeChangeProgramCrInternal,
                    string.Format(
                        "у этой записи об изменении способ формирования должен быть {0}.{1}",
                        typeChangeProgramCrExternal,
                        ExceptionHelper.GetExceptions()));
        }

        [Then(@"у этой записи об изменении в поле Дата текущая дата")]
        public void ТоУЭтойЗаписиОбИзмененииВПолеДатаТекущаяДата()
        {
            ProgramCrHelper.CurrentProgramCrChangeJournal.ChangeDate
                .Should().Be(
                    DateTime.Now.Date,
                    string.Format(
                        "у этой записи об изменении в поле Дата должна быть текущая дата.{0}",
                        ExceptionHelper.GetExceptions()));
        }

        [Then(@"записи по разрезу финансирования ""(.*)"" присутствует в этой программе капитального ремонта")]
        public void ТоЗаписиПоРазрезуФинансированияПрисутствуетВЭтойПрограммеКапитальногоРемонта(string finSourceName)
        {
            this._cashe.Get<ProgramCrFinSource>()
                .GetAll()
                .Any(x => x.ProgramCr.Id == ProgramCrHelper.Current.Id && x.FinanceSource.Name == finSourceName)
                .Should()
                .BeTrue(
                    string.Format(
                        "записи по разрезу финансирования {0} должна присутствовать в этой программе капитального ремонта",
                        finSourceName));
        }

        [Then(@"записи по разрез финансирования ""(.*)"" отсутствует в этой программе капитального ремонта")]
        public void ТоЗаписиПоРазрезФинансированияОтсутствуетВЭтойПрограммеКапитальногоРемонта(string finSourceName)
        {
            this._cashe.Get<ProgramCrFinSource>()
                .GetAll()
                .Any(x => x.ProgramCr.Id == ProgramCrHelper.Current.Id && x.FinanceSource.Name == finSourceName)
                .Should()
                .BeFalse(
                    string.Format(
                        "записи по разрезу финансирования {0} должна отсутствовать в этой программе капитального ремонта",
                        finSourceName));
        }

        [Then(@"в этой программе капитального ремонта отсутствуют дубли записи по разрезу финансирования ""(.*)""")]
        public void ТоВЭтойПрограммеКапитальногоРемонтаОтсутствуютДублиЗаписиПоРазрезуФинансирования(string finSourceName)
        {
            this._cashe.Get<ProgramCrFinSource>().GetAll()
                .Count(x => x.ProgramCr.Id == ProgramCrHelper.Current.Id && x.FinanceSource.Name == finSourceName)
                .Should()
                .Be(
                    1,
                    string.Format(
                        "в этой программе капитального ремонта должны отсутствовать дубли записи по разрезу финансирования {0}",
                        finSourceName));
        }
    }
}
