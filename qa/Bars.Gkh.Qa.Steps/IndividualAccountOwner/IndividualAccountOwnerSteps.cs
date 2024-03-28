namespace Bars.Gkh.Qa.Steps
{
    
    using System;
    using Bars.B4;
    using Bars.Gkh.Domain;
    using Bars.Gkh.Qa.Utils;
    using Bars.Gkh.RegOperator.Entities;

    using FluentAssertions;
    using NUnit.Framework;
    using TechTalk.SpecFlow;

    [Binding]
    public class IndividualAccountOwnerSteps : BindingBase
    {
        private IDomainService DomainService
        {
            get
            {
                Type entityType = IndividualAccountOwnerHelper.Current.GetType();

                var t = typeof(IDomainService<>).MakeGenericType(entityType);
                return (IDomainService)Container.Resolve(t);
            }
        }

        [Given(@"пользователь создает нового абонента типа Счет физического лица")]
        public void ДопустимПользовательСоздаетНовогоАбонентаТипаСчетФизическогоЛица()
        {
            IndividualAccountOwnerHelper.Current = new IndividualAccountOwner();
        }
        
        [Given(@"у этого абонента устанавливается поле Фамилия ""(.*)""")]
        public void ДопустимУЭтогоАбонентаУстанавливаетсяПолеФамилия(string surName)
        {
            IndividualAccountOwnerHelper.ChangeCurrent("Surname", surName);
        }

        [Given(@"у этого абонента устанавливается поле Имя ""(.*)""")]
        public void ДопустимУЭтогоАбонентаУстанавливаетсяПолеИмя(string firstName)
        {
            IndividualAccountOwnerHelper.ChangeCurrent("FirstName", firstName);

        }

        [Given(@"у этого абонента устанавливается поле Тип документа ""(.*)""")]
        public void ДопустимУЭтогоАбонентаУстанавливаетсяПолеТипДокумента(string identityType)
        {
            switch (identityType)
            {
                case "Паспорт":
                    IndividualAccountOwnerHelper.ChangeCurrent("IdentityType", 10);
                    break;

                case "Свидетельство о рождении":
                    IndividualAccountOwnerHelper.ChangeCurrent("IdentityType", 20);
                    break;
            }
        }

        [Given(@"у этого абонента устанавливается поле Серия документа ""(.*)""")]
        public void ДопустимУЭтогоАбонентаУстанавливаетсяПолеСерияДокумента(string identitySerial)
        {
            IndividualAccountOwnerHelper.ChangeCurrent("IdentitySerial", identitySerial);
        }
        [Given(@"у этого абонента устанавливается поле Номер документа ""(.*)""")]
        public void ДопустимУЭтогоАбонентаУстанавливаетсяПолеНомерДокумента(string identityNumber)
        {
            IndividualAccountOwnerHelper.ChangeCurrent("IdentityNumber", identityNumber);
            
        }
        [When(@"этот абонент типа Счет физ\. лица сохраняется в реестре абонентов")]
        public void ЕслиЭтотАбонентТипаСчетФиз_ЛицаСохраняетсяВРеестреАбонентов()
        {
            var id = (long)IndividualAccountOwnerHelper.GetPropertyValue("Id");

            try
            {
                if (id == 0)
                {
                    this.DomainService.Save(IndividualAccountOwnerHelper.Current);
                }
                else
                {
                    this.DomainService.Update(IndividualAccountOwnerHelper.Current);
                }
            }
            catch (Exception ex)
            {
                ExceptionHelper.TestExceptions.Add(System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }
        [When(@"пользователь удаляет абонента типа Счет физ\. лица  из реестра абонентов")]
        public void ЕслиПользовательУдаляетАбонентаТипаСчетФиз_ЛицаИзРеестраАбонентов()
        {
            var id = (long)IndividualAccountOwnerHelper.GetPropertyValue("Id");

            try
            {
                this.DomainService.Delete(id);
            }
            catch (Exception ex)
            {
                ExceptionHelper.TestExceptions.Add(System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }

        [Then(@"абонент типа Счет физ\. лица появляется в реестре абонентов")]
        public void ТоАбонентТипаСчетФиз_ЛицаПоявляетсяВРеестреАбонентов()
        {
            var id = (long)IndividualAccountOwnerHelper.GetPropertyValue("Id");

            var current = this.DomainService.Get(id);

            current.Should()
                .NotBeNull(
                    string.Format(
                        "запись по этому плану мероприятий должна присутствовать в разделе отчетных периодов{0}",
                        ExceptionHelper.GetExceptions()));
        }

        [Then(@"абонент типа Счет физ\. лица  отсутствует в реестре абонентов")]
        public void ТоАбонентТипаСчетФиз_ЛицаОтсутствуетВРеестреАбонентов()
        {
            var id = (long)IndividualAccountOwnerHelper.GetPropertyValue("Id");

            var current = this.DomainService.Get(id);

            current.Should()
                .BeNull(
                    string.Format(
                        "запись по этому плану мероприятий должна отсутствовать в разделе отчетных периодов{0}",
                        ExceptionHelper.GetExceptions()));
        }
    }
}