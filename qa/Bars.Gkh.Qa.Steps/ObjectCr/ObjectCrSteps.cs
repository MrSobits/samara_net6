using Bars.B4.DataAccess;

namespace Bars.Gkh.Qa.Steps
{
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System;

    using Bars.B4;
    using Bars.B4.Utils;

    using Bars.GkhCr.DomainService;
    using Bars.Gkh.Domain;
    using Bars.Gkh.Qa.Utils;
    using Bars.GkhCr.Entities;

    using FluentAssertions;

    using TechTalk.SpecFlow;

    [Binding]
    internal class ObjectCrSteps : BindingBase
    {
        private BindingBase.DomainServiceCashe<ObjectCr> _cashe = new BindingBase.DomainServiceCashe<ObjectCr>();

        [Given(@"пользователь добавляет новый объект капитального ремонта")]
        public void ДопустимПользовательДобавляетНовыйОбъектКапитальногоРемонта()
        {
            var objectCr = new ObjectCr(RealityObjectHelper.CurrentRealityObject, ProgramCrHelper.Current);

            ObjectCrHelper.Current = objectCr;
        }

        [Given(@"добавлен новый объект капитального ремонта")]
        public void ДопустимДобавленНовыйОбъектКапитальногоРемонта()
        {
            var objectCr = new ObjectCr(RealityObjectHelper.CurrentRealityObject, ProgramCrHelper.Current);

            this._cashe.Current.Save(objectCr);

            ObjectCrHelper.Current = objectCr;
        }


        [When(@"пользователь сохраняет этот объект капитального ремонта")]
        public void ЕслиПользовательСохраняетЭтотОбъектКапитальногоРемонта()
        {
            try
            {
                this._cashe.Current.SaveOrUpdate(ObjectCrHelper.Current);
            }
            catch (Exception ex)
            {
                ExceptionHelper.AddException(System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }

        [When(@"пользователь удаляет этот объект капитального ремонта")]
        public void ЕслиПользовательУдаляетЭтотОбъектКапитальногоРемонта()
        {
            var baseParams = new BaseParams
            {
                Params = new DynamicDictionary
                {
                    {"records", new List<object>{ObjectCrHelper.Current.Id}}
                }
            };

            
            try
            {
                this._cashe.Current.Delete(baseParams);
                Container.Resolve<ISessionProvider>().GetCurrentSession().Flush();
            }
            catch (Exception ex)
            {
                ExceptionHelper.AddException(System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }

        [Then(@"запись по этому объекту капитального ремонта присутствует в разделе объектов капитального ремонта")]
        public void ТоЗаписьПоЭтомуОбъектуКапитальногоРемонтаПрисутствуетВРазделеОбъектовКапитальногоРемонта()
        {
            var baseParams = new BaseParams
            {
                Params = new DynamicDictionary()
            };

            var result = Container.Resolve<IViewModel<ObjectCr>>()
                .List(Container.Resolve<IDomainService<ObjectCr>>(), baseParams).Data;
            var dataresult = (IList)result.CastAs<object>();

            ((IEnumerable<object>)dataresult)
                .Where(x => (long)x.GetType().GetProperty("Id").GetValue(x) == ObjectCrHelper.Current.Id)
                .ToList().Should().NotBeEmpty();
        }

        [Then(@"запись по этому объекту капитального ремонта отсутствует в разделе объектов капитального ремонта")]
        public void ТоЗаписьПоЭтомуОбъектуКапитальногоРемонтаОтсутствуетВРазделеОбъектовКапитальногоРемонта()
        {
            var baseParams = new BaseParams
            {
                Params = new DynamicDictionary()
            };
            var result = Container.Resolve<IViewModel<ObjectCr>>()
                .List(Container.Resolve<IDomainService<ObjectCr>>(), baseParams).Data;
            var dataresult = (IList)result.CastAs<object>();

            ((IEnumerable<object>)dataresult)
                .Where(x => (long)x.GetType().GetProperty("Id").GetValue(x) == ObjectCrHelper.Current.Id)
                .ToList().Should().BeEmpty();
            
            /*
            var currentObjectCr = this._cashe.Current.Get(ObjectCrHelper.Current.Id);

            currentObjectCr.Should()
                .BeNull(
                    string.Format(
                        "запись по этому объекту капитального ремонта должна отсутствовать в разделе объектов капитального ремонта.{0}",
                        ExceptionHelper.GetExceptions()));
             */
        }

        [Then(
            @"запись по этому объекту капитального ремонта присутствует в разделе удаленные объекты капитального ремонта"
            )]
        public void ТоЗаписьПоЭтомуОбъектуКапитальногоРемонтаПрисутствуетВРазделеУдаленныеОбъектыКапитальногоРемонта()
        {
            var baseParams = new BaseParams
            {
                Params = new DynamicDictionary
                {
                    {"deleted", true}
                }
            };

            var result = Container.Resolve<IViewModel<ObjectCr>>()
                .List(Container.Resolve<IDomainService<ObjectCr>>(), baseParams).Data;
            var dataresult = (IList)result.CastAs<object>();

            ((IEnumerable<object>)dataresult)
                .Where(x => (long)x.GetType().GetProperty("Id").GetValue(x) == ObjectCrHelper.Current.Id)
                .ToList().Should().NotBeEmpty();


        }


        [When(@"пользователь восстанавливает этот объект капитального ремонта")]
        public void ЕслиПользовательВосстанавливаетЭтотОбъектКапитальногоРемонта()
        {
            BaseParams baseParams = new BaseParams
            {
                Params = new DynamicDictionary
                {
                    {"selected", ObjectCrHelper.Current.Id}
                }
            };

            var ds = Container.Resolve<IObjectCrService>();

            ds.Recover(baseParams);

            Container.Resolve<ISessionProvider>().GetCurrentSession().Flush();
        }

    }
}
