namespace Bars.Gkh.Qa.Steps
{
    using System;
    using System.Linq;
    using System.Security.Permissions;
    using Bars.B4;
    using Bars.B4.Utils;
    using Bars.Gkh.Entities.Dicts;
    using Bars.Gkh.Qa.Utils;
    using FluentAssertions;
    using TechTalk.SpecFlow;
    using TechTalk.SpecFlow.Assist;

    [Binding]
    public class violationsGjiSteps : BindingBase
    {

        private dynamic ds
        {
            get
            {
                Type type = Type.GetType("Bars.GkhGji.Entities.ViolationGji, Bars.GkhGji");
                return Container.Resolve( typeof(IDomainService<>).MakeGenericType(type) );

            }
        }

        [Given(@"добавлено мероприятие по устранению нарушений")]
        public void ДопустимДобавленоМероприятиеПоУстранениюНарушений(Table table)
        {

            Type type = Type
                .GetType("Bars.GkhGji.Entities.ActionsRemovViol, Bars.GkhGji");

            dynamic ActionsRemViolDS = Container.Resolve(typeof(IDomainService<>).MakeGenericType(type));

            var methodCreateInstance = typeof(TableHelperExtensionMethods).GetMethod("CreateInstance", new Type[] { typeof(Table) });
            var genericCreateInstance = methodCreateInstance.MakeGenericMethod(type);
            ActionsRemovViolHelper.Current = genericCreateInstance.Invoke(null, new[] { table });
            

            var id = (long)ActionsRemovViolHelper.Current.Id;

            try
            {
                if (id == 0)
                {
                    ActionsRemViolDS.Save(ActionsRemovViolHelper.Current);
                }
                else
                {
                    ActionsRemViolDS.Update(ActionsRemovViolHelper.Current);
                }
            }
            catch (Exception ex)
            {
                ExceptionHelper.TestExceptions.Add(System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }
        
        [Given(@"добавлен пункт нормативно-правового документа к этому нормативному документу")]
        public void ДопустимДобавленПунктНормативно_ПравовогоДокументаКЭтомуНормативномуДокументу(Table table)
        {
            var dsItem = Container.Resolve<IDomainService<NormativeDocItem>>();
            NormativeDocItemHelper.Current = new NormativeDocItem()
            {
                NormativeDoc = NormativeDocHelper.Current
            };
        }
        
        [Given(@"пользователь добавляет новое нарушение")]
        public void ДопустимПользовательДобавляетНовоеНарушение()
        {
            ViolationsGjiHelper.Current = Activator.CreateInstance("Bars.GkhGji", "Bars.GkhGji.Entities.ViolationGji").Unwrap();
        }
        
        [Given(@"пользователь у этого нарушения заполняет поле Текст нарушения ""(.*)""")]
        public void ДопустимПользовательУЭтогоНарушенияЗаполняетПолеТекстНарушения(string name)
        {
            ViolationsGjiHelper.Current.Name = name;
        }

        [Given(@"пользователь у этого нарушения заполняет поле Текст нарушения (.*) символов ""(.*)""")]
        public void ДопустимПользовательУЭтогоНарушенияЗаполняетПолеТекстНарушенияСимволов(int count, char ch)
        {
            ViolationsGjiHelper.Current.Name = new string(ch, count);
        }
        
        [Given(@"пользователь у этого нарушения добавляет мероприятие по устранению нарушений")]
        public void ДопустимПользовательУЭтогоНарушенияДобавляетМероприятиеПоУстранениюНарушений()
        {
            var baseParams = new BaseParams
            {
                Params = new DynamicDictionary
                {
                    {"actRemoveViolIds", ActionsRemovViolHelper.Current.Id},
                    {"violationId", ViolationsGjiHelper.Current.Id}
                }
            };

            var type = Type.GetType("Bars.GkhGji.DomainService.ViolationActionsRemovGjiService, Bars.GkhGji").GetInterface("IViolationActionsRemovGjiService");
            try
            {
                var result = ((dynamic)Container.Resolve(type)).AddViolationActionsRemov(baseParams);
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }
        
        [Given(@"пользователь у этого нарушения добавляет пункт нормативно-правового документа")]
        public void ДопустимПользовательУЭтогоНарушенияДобавляетПунктНормативно_ПравовогоДокумента()
        {
            var baseParams = new BaseParams
            {
                Params = new DynamicDictionary
                {
                    {"violationId", ViolationsGjiHelper.Current.Id},
                    {"itemsIds", NormativeDocItemHelper.Current.Id}
                }
            };

            var type = Type.GetType("Bars.GkhGji.DomainService.ViolationNormativeDocItemService, Bars.GkhGji").GetInterface("IViolationNormativeDocItemService");
            try
            {
                var result = ((dynamic)Container.Resolve(type)).SaveNormativeDocItems(baseParams);
            }
            catch (Exception ex)
            {
                
                throw new Exception(ex.Message);
            }
        }
        
        [When(@"пользователь сохраняет это нарушение")]
        public void ЕслиПользовательСохраняетЭтоНарушение()
        {
            try
            {
                if (ViolationsGjiHelper.Current.Id == 0)
                {
                    ds.Save(ViolationsGjiHelper.Current);
                }
                else
                {
                    ds.Update(ViolationsGjiHelper.Current);
                }
            }
            catch (Exception ex)
            {
                ExceptionHelper.TestExceptions.Add(System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }
        
        [When(@"пользователь удаляет это нарушение")]
        public void ЕслиПользовательУдаляетЭтоНарушение()
        {
            try
            {
                ds.Delete(ViolationsGjiHelper.Current.Id);
            }
            catch (Exception ex)
            {
                ExceptionHelper.TestExceptions.Add(System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }
        
        [Then(@"запись по этому нарушению присутствует в справочнике нарушений")]
        public void ТоЗаписьПоЭтомуНарушениюПрисутствуетВСправочникеНарушений()
        {
            ((object)ds.Get(ViolationsGjiHelper.Current.Id)).Should().NotBeNull(string.Format("Нарушение должно присутствовать в списке нарушений.{0}", ExceptionHelper.GetExceptions()));
        }
        
        [Then(@"запись по этому нарушению отсутствует в справочнике нарушений")]
        public void ТоЗаписьПоЭтомуНарушениюОтсутствуетВСправочникеНарушений()
        {
            ((object)ds.Get(ViolationsGjiHelper.Current.Id)).Should().BeNull(string.Format("Нарушение должно отсутствовать в списке нарушений.{0}", ExceptionHelper.GetExceptions()));
        }

        [Given(@"добавлено нарушение")]
        public void ДопустимДобавленоНарушение(Table table)
        {
            ViolationsGjiHelper.Current = Activator.CreateInstance("Bars.GkhGji", "Bars.GkhGji.Entities.ViolationGji").Unwrap();
            ViolationsGjiHelper.Current.Name = table.Rows[0]["Name"];
            
            try
            {
                if (ViolationsGjiHelper.Current.Id == 0)
                {
                    ds.Save(ViolationsGjiHelper.Current);
                }
                else
                {
                    ds.Update(ViolationsGjiHelper.Current);
                }
            }
            catch (Exception ex)
            {
                ExceptionHelper.TestExceptions.Add(System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }

    }
}
