using System;
using System.ComponentModel;
using Bars.B4;
using Bars.Gkh.Domain;
using Bars.Gkh.Entities;
using Bars.Gkh.Qa.Utils;
using FluentAssertions;
using TechTalk.SpecFlow;

namespace Bars.Gkh.Qa.Steps.RoofingMaterials
{
    [Binding]
    public class RoofingMaterialsSteps : BindingBase
    {

        private IDomainService<RoofingMaterial> ds = Container.Resolve<IDomainService<RoofingMaterial>>();

        [Given(@"пользователь добавляет новый материал кровли")]
        public void ДопустимПользовательДобавляетНовыйМатериалКровли()
        {
            RoofingMaterialsHelper.Current = new RoofingMaterial();
        }
        
        [Given(@"пользователь у этого материала кровли заполняет поле Наименование ""(.*)""")]
        public void ДопустимПользовательУЭтогоМатериалаКровлиЗаполняетПолеНаименование(string name)
        {
            RoofingMaterialsHelper.Current.Name = name;
        }
        
        [When(@"пользователь сохраняет этот материал кровли")]
        public void ДопустимПользовательСохраняетЭтотМатериалКровли()
        {
            try
            {
                ds.SaveOrUpdate(RoofingMaterialsHelper.Current);
            }
            catch (Exception ex)
            {
                ExceptionHelper.TestExceptions.Add(System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }
        
        [When(@"пользователь удаляет этот материал кровли")]
        public void ЕслиПользовательУдаляетЭтотМатериалКровли()
        {
            try
            {
                ds.Delete(RoofingMaterialsHelper.Current.Id);
            }
            catch (Exception ex)
            {
                ExceptionHelper.TestExceptions.Add(System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }
        
        [Then(@"запись по этому материалу кровли присутствует в справочнике материалов кровли")]
        public void ТоЗаписьПоЭтомуМатериалуКровлиПрисутствуетВСправочникеМатериаловКровли()
        {
            ds.Get(RoofingMaterialsHelper.Current.Id).Should().NotBeNull();
        }
        
        [Then(@"запись по этому материалу кровли отсутствует в справочнике материалов кровли")]
        public void ТоЗаписьПоЭтомуМатериалуКровлиОтсутствуетВСправочникеМатериаловКровли()
        {
            ds.Get(RoofingMaterialsHelper.Current.Id).Should().BeNull();
        }

        [Given(@"пользователь у этого материала кровли заполняет поле Наименование (.*) символов ""(.*)""")]
        public void ДопустимПользовательУЭтогоМатериалаКровлиЗаполняетПолеНаименованиеСимволов(int length, char symbol)
        {
            RoofingMaterialsHelper.Current.Name = new string(symbol, length);
        }

    }
}
