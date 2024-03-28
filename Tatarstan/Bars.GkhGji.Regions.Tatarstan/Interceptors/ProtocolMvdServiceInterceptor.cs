namespace Bars.GkhGji.Regions.Tatarstan.Interceptors
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Enums;
    using B4.Utils;

    using Bars.B4.DataAccess;
    using Bars.GkhGji.Entities.Dict;
    using Bars.GkhGji.Interceptors;
    using Bars.GkhGji.Regions.Tatarstan.Entities.Resolution;

    public class ProtocolMvdServiceInterceptor : ProtocolMvdServiceInterceptor<TatarstanProtocolMvd>
    {
        /// <summary>
        /// Метод вызывается перед созданием объекта
        /// </summary>
        /// <param name="service">Домен</param><param name="entity">Объект</param>
        /// <returns>
        /// Результат выполнения
        /// </returns>
        public override IDataResult BeforeCreateAction(IDomainService<TatarstanProtocolMvd> service, TatarstanProtocolMvd entity)
        {
            var citizenshipDomain = this.Container.Resolve<IDomainService<Citizenship>>();
            try
            {
                if (entity.CitizenshipType == CitizenshipType.RussianFederation && entity.Citizenship == null)
                {
                    var citizenship = citizenshipDomain.GetAll().FirstOrDefault(x => x.OksmCode == 643);

                    entity.Citizenship = citizenship;
                }
            }
            finally
            {
                this.Container.Release(citizenshipDomain);
            }

            return base.BeforeCreateAction(service, entity);
        }

        /// <summary>
        /// Метод вызывается перед обновлением объекта
        /// </summary>
        /// <param name="service">Домен</param><param name="entity">Объект</param>
        /// <returns>
        /// Результат выполнения
        /// </returns>
        public override IDataResult BeforeUpdateAction(IDomainService<TatarstanProtocolMvd> service, TatarstanProtocolMvd entity)
        {
            var protocolMvdRealityObjectService = this.Container.ResolveDomain<ProtocolMvdRealityObject>();
            var citizenshipDomain = this.Container.Resolve<IDomainService<Citizenship>>();
            try
            {
                if (entity.CitizenshipType == CitizenshipType.RussianFederation && entity.Citizenship == null)
                {
                    var citizenship = citizenshipDomain.GetAll().FirstOrDefault(x => x.OksmCode == 643);

                    entity.Citizenship = citizenship;
                }

                if (!protocolMvdRealityObjectService.GetAll().Any(x => x.ProtocolMvd.Id == entity.Id))
                {
                    return this.Failure("Следующий раздел \"Адрес правонарушения\" обязателен для заполнения");
                }

                if (entity.DateOffense.HasValue)
                {
                    var date = entity.DateOffense.ToDateTime();
                    var dateTime = DateTime.Now;

                    DateTime.TryParse(entity.TimeOffense, out dateTime);

                    entity.DateOffense = new DateTime(date.Year, date.Month, date.Day, dateTime.Hour, dateTime.Minute, 0);
                }
            }
            finally
            {
                this.Container.Release(protocolMvdRealityObjectService);
                this.Container.Release(citizenshipDomain);
            }

            return base.BeforeUpdateAction(service, entity);
        }

        /// <summary>
        /// Метод вызывается после создания объекта
        /// </summary>
        /// <param name="service">Домен</param><param name="entity">Объект</param>
        /// <returns>
        /// Результат выполнения
        /// </returns>
        public override IDataResult AfterCreateAction(IDomainService<TatarstanProtocolMvd> service, TatarstanProtocolMvd entity)
        {
            var protocolMvdArticleLawDomain = this.Container.Resolve<IDomainService<ProtocolMvdArticleLaw>>();
            var articleLawGjiDomain = this.Container.Resolve<IDomainService<ArticleLawGji>>();

            try
            {
                var articleLawGj = articleLawGjiDomain.GetAll().FirstOrDefault(x => x.Code == "15");

                var protocolMvdArticleLaw = new ProtocolMvdArticleLaw
                {
                    ProtocolMvd = entity,
                    ArticleLaw = articleLawGj
                };

                protocolMvdArticleLawDomain.Save(protocolMvdArticleLaw);
            }
            finally
            {
                this.Container.Release(protocolMvdArticleLawDomain);
                this.Container.Release(articleLawGjiDomain);
            }

            return base.AfterCreateAction(service, entity);
        }
    }
}