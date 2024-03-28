namespace Bars.Gkh.Qa.Steps.WallMaterials
{
    using System;
    using Bars.B4;
    using Bars.Gkh.Domain;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Qa.Utils;
    using FluentAssertions;
    using TechTalk.SpecFlow;

    [Binding]
    public class WallMaterialsSteps : BindingBase
    {
        private IDomainService<WallMaterial> ds = Container.Resolve<IDomainService<WallMaterial>>();

        [Given(@"пользователь добавляет новый материал стен")]
        public void ДопустимПользовательДобавляетНовыйМатериалСтен()
        {
            WallMaterialsHelper.Current = new WallMaterial();
        }
        
        [Given(@"пользователь у этого материала стен заполняет поле Наименование ""(.*)""")]
        public void ДопустимПользовательУЭтогоМатериалаСтенЗаполняетПолеНаименование(string name)
        {
            WallMaterialsHelper.Current.Name = name;
        }

        [Given(@"пользователь у этого материала стен заполняет поле Наименование (.*) символов ""(.*)""")]
        public void ДопустимПользовательУЭтогоМатериалаСтенЗаполняетПолеНаименованиеСимволов(int length, char symbol)
        {
            WallMaterialsHelper.Current.Name = new string(symbol, length);
        }

        [When(@"пользователь сохраняет этот материал стен")]
        public void ЕслиПользовательСохраняетЭтотМатериалСтен()
        {
            try
            {
                ds.SaveOrUpdate(WallMaterialsHelper.Current);
            }
            catch (Exception ex)
            {
                ExceptionHelper.TestExceptions.Add(System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }
        
        [When(@"пользователь удаляет этот материал стен")]
        public void ЕслиПользовательУдаляетЭтотМатериалСтен()
        {
            try
            {
                ds.Delete(WallMaterialsHelper.Current.Id);
            }
            catch (Exception ex)
            {
                ExceptionHelper.TestExceptions.Add(System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }
        
        [Then(@"запись по этому материалу стен присутствует в справочнике материалов стен")]
        public void ТоЗаписьПоЭтомуМатериалуСтенПрисутствуетВСправочникеМатериаловСтен()
        {
            ds.Get(WallMaterialsHelper.Current.Id).Should().NotBeNull(
                    string.Format(
                        "материал стен должен присутствовать в справочнике{0}",
                        ExceptionHelper.GetExceptions()));
        }
        
        [Then(@"запись по этому материалу стен отсутствует в справочнике материалов стен")]
        public void ТоЗаписьПоЭтомуМатериалуСтенОтсутствуетВСправочникеМатериаловСтен()
        {
            ds.Get(WallMaterialsHelper.Current.Id).Should().BeNull(
                    string.Format(
                        "материал стен должен отсутствовать в справочнике{0}",
                        ExceptionHelper.GetExceptions()));
        }
    }
}
