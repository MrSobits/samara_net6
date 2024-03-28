namespace Bars.Gkh.Qa.Steps.FeatureViolGji
{
    using System;
    using System.ComponentModel;
    using Bars.B4;
    using Bars.Gkh.Qa.Utils;
    using FluentAssertions;
    using TechTalk.SpecFlow;

    [Binding]
    public class FeatureViolGjiSteps : BindingBase
    {
        private dynamic ds
        {
            get
            {
                Type type = Type.GetType("Bars.GkhGji.Entities.FeatureViolGji, Bars.GkhGji");
                return Container.Resolve(typeof(IDomainService<>).MakeGenericType(type));
            }
        }

        [Given(@"пользователь добавляет новую Группу нарушений")]
        public void ДопустимПользовательДобавляетНовуюГруппуНарушений()
        {
            FeatureViolGjiHelper.Current = Activator.CreateInstance("Bars.GkhGji", "Bars.GkhGji.Entities.FeatureViolGji").Unwrap();
        }

        [Given(@"пользователь у этой группы нарушений заполняет наименование ""(.*)""")]
        public void ДопустимПользовательУЭтойГруппыНарушенийЗаполняетНаименование(string name)
        {
            FeatureViolGjiHelper.Current.Name = name;
        }
        
        [When(@"пользователь сохраняет эту группу нарушений")]
        public void ЕслиПользовательСохраняетЭтуГруппуНарушений()
        {
            try
            {
                if (FeatureViolGjiHelper.Current.Id == 0)
                {
                    ds.Save(FeatureViolGjiHelper.Current);
                }
                else
                {
                    ds.Update(FeatureViolGjiHelper.Current);
                }
            }
            catch (Exception ex)
            {
                ExceptionHelper.TestExceptions.Add(System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }
        
        [When(@"пользователь удаляет эту группу нарушений")]
        public void ЕслиПользовательУдаляетЭтуГруппуНарушений()
        {
            try
            {
                ds.Delete(FeatureViolGjiHelper.Current.Id);
            }
            catch (Exception ex)
            {
                ExceptionHelper.TestExceptions.Add(System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }
        
        [Then(@"группа нарушений присутствует в справочнике")]
        public void ТоГруппаНарушенийПрисутствуетВСправочнике()
        {
            ((object) ds.Get(FeatureViolGjiHelper.Current.Id)).Should()
                .NotBeNull(
                    string.Format(
                        "Группа нарушений должна присутствовать в справочнике муниципальных образований.{0}",
                        ExceptionHelper.GetExceptions()));
        }

        [Then(@"группа нарушений отсутствует в справочнике")]
        public void ТоГруппаНарушенийОтсутствуетВСправочнике()
        {
            ((object) ds.Get(FeatureViolGjiHelper.Current.Id)).Should()
                .BeNull(
                    string.Format(
                        "Группа нарушений должна отсутствовать в справочнике муниципальных образований.{0}",
                        ExceptionHelper.GetExceptions()));
        }

        [Given(@"добавлена группа нарушений")]
        public void ДопустимДобавленаГруппаНарушений(Table table)
        {
            ДопустимПользовательДобавляетНовуюГруппуНарушений();
            ДопустимПользовательУЭтойГруппыНарушенийЗаполняетНаименование(table.Rows[0]["Name"]);
            ЕслиПользовательСохраняетЭтуГруппуНарушений();
        }

    }
}
