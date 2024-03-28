namespace Bars.Gkh.Qa.Steps.StateDuty
{
    using Bars.B4.DataAccess;
    using Bars.Gkh.Modules.ClaimWork.Enums;
    using FluentAssertions;
    using System;
    using System.ComponentModel;
    using Bars.B4;
    using Bars.Gkh.Domain;
    using Bars.Gkh.Modules.ClaimWork.Entities;
    using Bars.Gkh.Qa.Utils;
    using TechTalk.SpecFlow;
    using TechTalk.SpecFlow.Assist;

    [Binding]
    public class StateDutySteps : BindingBase
    {
        private IDomainService<StateDuty> ds = Container.Resolve<IDomainService<StateDuty>>();

        [Given(@"добавлено заявление в суд")]
        public void ДопустимДобавленоЗаявлениеВСуд(Table table)
        {
            PetitionToCourtTypeHelper.Current = table.CreateInstance<PetitionToCourtType>();

            var ds = Container.Resolve<IDomainService<PetitionToCourtType>>();
            try
            {
                ds.SaveOrUpdate(PetitionToCourtTypeHelper.Current);
            }
            catch (Exception ex)
            {
                ExceptionHelper.TestExceptions.Add(System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }
        
        [Given(@"пользователь добавляет новую госпошлину")]
        public void ДопустимПользовательДобавляетНовуюГоспошлину()
        {
            StateDutyHelper.Current = new StateDuty();
        }
        
        [Given(@"пользователь у этой госпошлины заполняет поле Тип суда ""(.*)""")]
        public void ДопустимПользовательУЭтойГоспошлиныЗаполняетПолеТипСуда(string p0)
        {
            StateDutyHelper.Current.CourtType = EnumHelper.GetFromDisplayValue<CourtType>(p0);
        }
        
        [When(@"пользователь сохраняет эту госпошлину")]
        public void ЕслиПользовательСохраняетЭтуГоспошлину()
        {
            try
            {
                ds.SaveOrUpdate(StateDutyHelper.Current);
            }
            catch (Exception ex)
            {
                ExceptionHelper.TestExceptions.Add(System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }
        
        [When(@"пользователь удаляет эту госпошлину")]
        public void ЕслиПользовательУдаляетЭтуГоспошлину()
        {
            try
            {
                ds.Delete(StateDutyHelper.Current.Id);
            }
            catch (Exception ex)
            {
                ExceptionHelper.TestExceptions.Add(System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }
        
        [When(@"пользователь добавляет к этой госпошлине это заявление в суд")]
        public void ЕслиПользовательДобавляетКЭтойГоспошлинеЭтоЗаявлениеВСуд()
        {
            var stateDutyPetition = new StateDutyPetition
            {
                StateDuty = StateDutyHelper.Current,
                PetitionToCourtType = PetitionToCourtTypeHelper.Current
            };

            var dsStateDutyPetition = Container.Resolve<IDomainService<StateDutyPetition>>();

            try
            {
                dsStateDutyPetition.SaveOrUpdate(stateDutyPetition);
            }
            catch (Exception ex)
            {
                ExceptionHelper.TestExceptions.Add(System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message);
            }

            ScenarioContext.Current["stateDutyPetitionId"] = stateDutyPetition.Id;
        }
        
        [Then(@"запись по этой госпошлине присутствует в справочнике госпошлин")]
        public void ТоЗаписьПоЭтойГоспошлинеПрисутствуетВСправочникеГоспошлин()
        {
            ds.Get(StateDutyHelper.Current.Id).Should().NotBeNull(
                string.Format("Госпошлина должна присутствовать в справочнике.{0}",
                ExceptionHelper.GetExceptions()));
        }
        
        [Then(@"запись по этой госпошлине отсутствует в справочнике госпошлин")]
        public void ТоЗаписьПоЭтойГоспошлинеОтсутствуетВСправочникеГоспошлин()
        {
            ds.Get(StateDutyHelper.Current.Id).Should().BeNull(
                string.Format("Госпошлина должна отсутствовать в справочнике.{0}",
                ExceptionHelper.GetExceptions()));
        }
        
        [Then(@"запись по этому типу заявления притсутствует в списке заявлений этой госпошлины")]
        public void ТоЗаписьПоЭтомуТипуЗаявленияПритсутствуетВСпискеЗаявленийЭтойГоспошлины()
        {
            var dsStateDutyPetition = Container.Resolve<IDomainService<StateDutyPetition>>();

            dsStateDutyPetition.Get((long)ScenarioContext.Current["stateDutyPetitionId"]).Should().NotBeNull(
                string.Format("Тип заявления должен присутствовать в госпошлине.{0}",
                ExceptionHelper.GetExceptions()));
        }
    }
}
