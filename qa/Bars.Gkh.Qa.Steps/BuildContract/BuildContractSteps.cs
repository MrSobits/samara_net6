namespace Bars.Gkh.Qa.Steps
{
    using System;

    using Bars.Gkh.Domain;
    using Bars.Gkh.Qa.Utils;
    using Bars.GkhCr.Entities;

    using FluentAssertions;

    using TechTalk.SpecFlow;
    using TechTalk.SpecFlow.Assist;

    [Binding]
    internal class BuildContractSteps : BindingBase
    {
        private BindingBase.DomainServiceCashe<BuildContract> _cashe = new BindingBase.DomainServiceCashe<BuildContract>();

        [Given(@"пользователь добавляет новый договор подряда в текущий объект КР")]
        public void ДопустимПользовательДобавляетНовыйДоговорПодряда()
        {
            var buildContract = new BuildContract
                                    {
                                        ObjectCr = ObjectCrHelper.Current
                                    };
            
            BuildContractHelper.Current = buildContract;
        }

        [Given(@"добавлен новый договор подряда в текущий объект КР с текущей подрядной организацией")]
        public void ДопустимДобавленНовыйДоговорПодрядаВТекущийОбъектКРСТекущейПодряднойОрганизацией(Table table)
        {
            var buildContract = table.CreateInstance<BuildContract>();

            buildContract.ObjectCr = ObjectCrHelper.Current;

            buildContract.Builder = BuilderHelper.Current;

            this._cashe.Current.Save(buildContract);

            BuildContractHelper.Current = buildContract;
        }

        [Given(@"пользователь у этого договора подряда заполняет поле Документ ""(.*)""")]
        public void ДопустимПользовательУЭтогоДоговораПодрядаЗаполняетПолеДокумент(string documentName)
        {
            BuildContractHelper.Current.DocumentName = documentName;
        }

        [Given(@"пользователь у этого договора подряда заполняет поле Подрядчик")]
        public void ДопустимПользовательУЭтогоДоговораПодрядаЗаполняетПолеПодрядчик()
        {
            BuildContractHelper.Current.Builder = BuilderHelper.Current;
        }

        [When(@"пользователь сохраняет этот договор подряда")]
        public void ЕслиПользовательСохраняетЭтотДоговорПодряда()
        {
            try
            {
                this._cashe.Current.SaveOrUpdate(BuildContractHelper.Current);
            }
            catch (Exception ex)
            {
                ExceptionHelper.AddException(System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }

        [When(@"пользователь удаляет этот договор подряда")]
        public void ЕслиПользовательУдаляетЭтотДоговорПодряда()
        {
            try
            {
                this._cashe.Current.Delete(BuildContractHelper.Current.Id);
            }
            catch (Exception ex)
            {
                ExceptionHelper.AddException(System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }

        [Then(@"запись по этому договору подряда присутствует у этого объекта капитального ремонта")]
        public void ТоЗаписьПоЭтомуДоговоруПодрядаПрисутствуетУЭтогоОбъектаКапитальногоРемонта()
        {
            var builderContract = this._cashe.Current.Get(BuildContractHelper.Current.Id);

            builderContract.Should().NotBeNull(string.Format("запись по этому договору подряда должна присутствовать.{0}", ExceptionHelper.GetExceptions()));
        }

        [Then(@"запись по этому договору подряда отсутствует у этого объекта капитального ремонта")]
        public void ТоЗаписьПоЭтомуДоговоруПодрядаОтсутствуетУЭтогоОбъектаКапитальногоРемонта()
        {
            var builderContract = this._cashe.Current.Get(BuildContractHelper.Current.Id);

            builderContract.Should().BeNull(string.Format("запись по этому договору подряда должна отсутствовать.{0}", ExceptionHelper.GetExceptions()));
        }

    }
}
