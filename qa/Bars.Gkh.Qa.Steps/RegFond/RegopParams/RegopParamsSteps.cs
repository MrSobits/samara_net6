namespace Bars.Gkh.Qa.Steps
{
    using System;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.Utils;
    using Bars.Gkh.Domain;
    using Bars.Gkh.Qa.Utils;

    using TechTalk.SpecFlow;

    [Binding]
    internal class RegopParamsSteps : BindingBase
    {
        private IDomainService DomainService
        {
            get
            {
                Type entityType = PeriodDiHelper.Current.GetType();

                var t = typeof(IDomainService<>).MakeGenericType(entityType);
                return (IDomainService)Container.Resolve(t);
            }
        }

        [Given(@"пользователь в настройках параметров РФ поставил галочку в (.*)")]
        public void ДопустимПользовательВНастройкахПараметровРФПроставил(string paramExternalName)
        {
            var paramInternalName = RegopParamsHelper.GetInternalName(paramExternalName);

            var param = this.DomainService.GetAll().Cast<object>().FirstOrDefault(
                x => (string)ReflectionHelper.GetPropertyValue(x, "Key") == paramInternalName);

            if (param == null)
            {
                param = Activator
                .CreateInstance("Bars.Gkh.RegOperator", "Bars.Gkh.RegOperator.Entities.RegoperatorParam").Unwrap();

                ReflectionHelper.SetPropertyValue(param, "Key", paramInternalName);
            }

            ReflectionHelper.SetPropertyValue(param, "Value", "True");

            if ((long)ReflectionHelper.GetPropertyValue(param, "Id") == 0)
            {
                this.DomainService.Save(param);
            }
            else
            {
                this.DomainService.Update(param);
            }
        }

        [Given(@"настройки параметров РФ")]
        public void ДопустимНастройкиПараметровРФ(Table table)
        {
            var paramList = table.Rows;

            paramList.ForEach(
                x =>
                    {
                        var paramInternalName = RegopParamsHelper.GetInternalName(x["parametr"]);

                        var param = this.DomainService.GetAll().Cast<object>().FirstOrDefault(
                            p => (string)ReflectionHelper.GetPropertyValue(x, "Key") == paramInternalName);

                        if (param == null)
                        {
                            param = Activator
                                .CreateInstance(
                                "Bars.Gkh.RegOperator", 
                                "Bars.Gkh.RegOperator.Entities.RegoperatorParam")
                                .Unwrap();

                            ReflectionHelper.SetPropertyValue(param, "Key", paramInternalName);
                        }

                        ReflectionHelper.SetPropertyValue(param, "Value", "True");

                        if ((long)ReflectionHelper.GetPropertyValue(param, "Id") == 0)
                        {
                            this.DomainService.Save(param);
                        }
                        else
                        {
                            this.DomainService.Update(param);
                        }
                    });
        }
    }
}
