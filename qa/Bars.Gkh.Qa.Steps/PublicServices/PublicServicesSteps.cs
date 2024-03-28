
using FluentAssertions;

namespace Bars.Gkh.Qa.Steps
{
    using System;
    using Bars.B4;
    using Bars.Gkh.Qa.Utils;
    using TechTalk.SpecFlow;

    [Binding]
    public class PublicServicesSteps : BindingBase
    {
        private dynamic ds
        {
            get
            {
                var publicServiceType = Type.GetType("Bars.Gkh1468.Entities.PublicService, Bars.Gkh1468");
                var genericDsPublicServiceType = typeof (IDomainService<>).MakeGenericType(publicServiceType);
                return Container.Resolve(genericDsPublicServiceType);
            }
        }

        [Given(@"пользователь у этой коммунальной услуги заполняет поле Наименование (.*) символов ""(.*)""")]
        public void ДопустимПользовательУЭтойКоммунальнойУслугиЗаполняетПолеНаименованиеСимволов(int count, char ch)
        {
            PublicServicesHelper.Current.Name = new string(ch, count);
        }

        [Given(@"пользователь добавляет новую коммунальную услугу")]
        public void ДопустимПользовательДобавляетНовуюКоммунальнуюУслугу()
        {
            PublicServicesHelper.Current = Activator.CreateInstance("Bars.Gkh1468", "Bars.Gkh1468.Entities.PublicService").Unwrap();
        }
        
        [Given(@"пользователь у этой коммунальной услуги заполняет поле Наименование ""(.*)""")]
        public void ДопустимПользовательУЭтойКоммунальнойУслугиЗаполняетПолеНаименование(string name)
        {
            PublicServicesHelper.Current.Name = name;
        }

        [When(@"пользователь сохраняет эту коммунальную услугу")]
        public void ЕслиПользовательСохраняетЭтуКоммунальнуюУслугу()
        {
            try
            {
                if (PublicServicesHelper.Current.Id == 0)
                {
                    ds.Save(PublicServicesHelper.Current);
                }
                else
                {
                    ds.Update(PublicServicesHelper.Current);
                }
            }
            catch (Exception ex)
            {
                ExceptionHelper.TestExceptions.Add(System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }
        
        [When(@"пользователь удаляет эту коммунальную услугу")]
        public void ЕслиПользовательУдаляетЭтуКоммунальнуюУслугу()
        {
            try
            {
                ds.Delete(PublicServicesHelper.Current.Id);
            }
            catch (Exception ex)
            {
                ExceptionHelper.TestExceptions.Add(System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }
        
        [Then(@"запись по этой коммунальной услуге присутствует в справочнике коммунальных услуг")]
        public void ТоЗаписьПоЭтойКоммунальнойУслугеПрисутствуетВСправочникеКоммунальныхУслуг()
        {
            ((object)ds.Get(PublicServicesHelper.Current.Id)).Should().NotBeNull(string.Format("запись по этой коммунальной услуге должна присутствовать в справочнике организационно-правовых форм.{0}", ExceptionHelper.GetExceptions()));
        }
        
        [Then(@"запись по этой коммунальной услуге отсутствует в справочнике коммунальных услуг")]
        public void ТоЗаписьПоЭтойКоммунальнойУслугеОтсутствуетВСправочникеКоммунальныхУслуг()
        {
            ((object)ds.Get(PublicServicesHelper.Current.Id)).Should().BeNull(string.Format("запись по этой коммунальной услугеа должна отсутствовать в справочнике организационно-правовых форм.{0}", ExceptionHelper.GetExceptions()));
        }
    }
}
