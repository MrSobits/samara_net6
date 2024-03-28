namespace Bars.Gkh.Qa.Steps
{
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4.Modules.NHibernateChangeLog;
    using Bars.B4.Utils;
    using Bars.Gkh.Qa.Utils;

    using FluentAssertions;

    using NHibernate.Dialect.Function;

    using TechTalk.SpecFlow;

    [Binding]
    internal class LogEntityPropertySteps : BindingBase
    {
        private DomainServiceCashe<LogEntityProperty> _cashe = new DomainServiceCashe<LogEntityProperty>();

        [Then(@"у этой записи в журнале действий пользователя присутствует детальная информация")]
        public void ТоУЭтойЗаписиВЖурналеДействийПользователяПрисутствуетДетальнаяИнформация()
        {
            var properties = this._cashe.Current.GetAll().Where(x => x.LogEntity.Id == LogEntityHelper.Current.Id);

            properties.Should()
                .NotBeNullOrEmpty(
                string.Format(
                "у этой записи в журнале действий пользователя должна присутствовать детальная информация. {0}",
                ExceptionHelper.GetExceptions()));
        }

        [Then(@"у этой детальной информации присутствует запись c Наименованием атрибута ""(.*)""")]
        public void ТоУЭтойДетальнойИнформацииПрисутствуетЗаписьCНаименованиемАтрибута(string propertyName)
        {
            var logEntity = LogEntityHelper.Current;

            var logEntityInfo =
                Container.Resolve<IChangeLogInfoProvider>()
                    .GetLoggedEntities()
                    .FirstOrDefault(x => x.EntityType == logEntity.EntityType);

            var property = logEntityInfo.Properties.FirstOrDefault(x => x.DisplayName == propertyName);

            if (property == null)
            {
                throw new SpecFlowException(
                    string.Format(
                        "Отсутствует атрибут с наименованием {0} для сущности {1}",
                        propertyName,
                        logEntity.EntityDescription));
            }

            var propertyCode = property.PropertyCode;

            var existingProperties = this._cashe.Current.GetAll().Where(x => x.LogEntity.Id == LogEntityHelper.Current.Id);

            var requiredProperty = existingProperties.FirstOrDefault(x => x.PropertyCode == propertyCode);

            requiredProperty.Should().NotBeNull(string.Format(
                        "у этой детальной информации должна присутствовать запись c Наименованием атрибута {0}. {1}",
                        propertyName,
                        ExceptionHelper.GetExceptions()));

            LogEntityPropertyHelper.Curent = requiredProperty;
        }

        [Then(@"у этой записи детальной информации заполнено поле Старое значение ""(.*)""")]
        public void ТоУЭтойЗаписиДетальнойИнформацииЗаполненоПолеСтароеЗначение(string oldValue)
        {
            LogEntityPropertyHelper.Curent.OldValue.Should()
                .Be(
                    oldValue,
                    string.Format("у этой записи детальной информации поле Старое значение должно быть {0}", oldValue));
        }

        [Then(@"у этой записи детальной информации заполнено поле Новое значение ""(.*)""")]
        public void ТоУЭтойЗаписиДетальнойИнформацииЗаполненоПолеНовоеЗначение(string newValue)
        {
            LogEntityPropertyHelper.Curent.NewValue.Should()
                .Be(
                    newValue,
                    string.Format("у этой записи детальной информации поле Новое значение должно быть {0}", newValue));
        }

    }
}
