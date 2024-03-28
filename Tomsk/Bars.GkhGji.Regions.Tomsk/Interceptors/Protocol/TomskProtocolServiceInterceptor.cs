namespace Bars.GkhGji.Regions.Tomsk.Interceptors
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using B4.Utils;
    using Bars.B4;
    using Bars.B4.IoC;
    using Bars.Gkh.Utils;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Regions.Tomsk.Controller;
    using Bars.GkhGji.Regions.Tomsk.Entities;
    using Bars.GkhGji.Regions.Tomsk.Entities.Inspection;

    /// <summary>
    /// Интерцептор для Томского протокола
    /// </summary>
    public class TomskProtocolServiceInterceptor : Bars.GkhGji.Interceptors.ProtocolServiceInterceptor<TomskProtocol>
    {
        /// <summary>
        /// Домен-сервис "детских" документов
        /// </summary>
        public IDomainService<DocumentGjiChildren> ChildrenDomain { get; set; }

        /// <summary>
        /// Домен-сервис связей требований сдокументами
        /// </summary>
        public IDomainService<RequirementDocument> ReqDocDomain { get; set; }

        /// <summary>
        /// Домен-сервис реквизитов физ. лица привязанных к документу
        /// </summary>
        public IDomainService<DocumentPhysInfo> DocumentPhysInfoDomainService { get; set; }

        /// <summary>
        /// Домен-сервис инспекторов документов ГЖИ
        /// </summary>
        public IDomainService<DocumentGjiInspector> DocumentGjiInspectorDomainService { get; set; }

        /// <summary>
        /// Домен-сервис первичных обращений проверки
        /// </summary>
        public IDomainService<PrimaryBaseStatementAppealCits> PrimaryBaseStatementAppealCitsDomainService { get; set; }

        /// <summary>
        /// Домен-сервис описаний протоколов
        /// </summary>
        public IDomainService<ProtocolDescription> DescriptionService { get; set; }

        /// <summary>
        /// Действия, выполняемые перед удалением протокола
        /// </summary>
        /// <param name="service">Домен-сервис протоколов Томска</param>
        /// <param name="entity">Протокол Томска</param>
        /// <returns>Результат выполнения запроса</returns>
        public override IDataResult BeforeDeleteAction(IDomainService<TomskProtocol> service, TomskProtocol entity)
        {
            var result = base.BeforeDeleteAction(service, entity);

            if (!result.Success)
            {
                return result;
            }

            var refFuncs = new List<Func<long, string>>
            {
                id => this.Container.Resolve<IDomainService<Requirement>>().GetAll().Any(x => x.Document.Id == id)
                        ? "Требования"
                        : null
            };

            var refs = refFuncs.Select(x => x(entity.Id)).Where(x => x != null).ToArray();

            if (refs.Length > 0)
            {
                return this.Failure(string.Format("Существуют связанные записи в следующих таблицах: {0}", refs.AggregateWithSeparator("; ")));
            }

            if (this.ChildrenDomain.GetAll().Any(x => x.Parent.Id == entity.Id))
            {
                return this.Failure("Данный документ имеет дочерние документы.");
            }

            this.ReqDocDomain.GetAll()
                .Where(x => x.Document.Id == entity.Id)
                .Select(x => x.Id)
                .ForEach(x => this.ReqDocDomain.Delete(x));

            this.DocumentPhysInfoDomainService.GetAll()
                .Where(x => x.Document.Id == entity.Id)
                .Select(x => x.Id)
                .ForEach(x => this.DocumentPhysInfoDomainService.Delete(x));

            var servReminders = this.Container.Resolve<IDomainService<Reminder>>();

            using (this.Container.Using(servReminders))
            {
                var reminders = servReminders.GetAll()
                    .Where(x => x.DocumentGji.Id == entity.Id)
                    .Select(x => x.Id).ToList();

                foreach (var id in reminders)
                {
                    servReminders.Delete(id);
                }
            }

            var description = this.DescriptionService.GetAll().FirstOrDefault(x => x.Protocol.Id == entity.Id);
            if (description != null)
            {
                this.DescriptionService.Delete(description.Id);
            }

            return result;
        }

        /// <summary>
        /// Действия, выполняемые после удаления протокола
        /// </summary>
        /// <param name="service">Домен-сервис протоколов Томска</param>
        /// <param name="entity">Протокол Томска</param>
        /// <returns>Результат выполнения запроса</returns>
        public override IDataResult AfterCreateAction(IDomainService<TomskProtocol> service, TomskProtocol entity)
        {
            var result = base.BeforeCreateAction(service, entity);

            if (!result.Success)
            {
                return result;
            }

            var primaryAppealCitsContragent = this.PrimaryBaseStatementAppealCitsDomainService.GetAll()
                .Where(x => x.BaseStatementAppealCits.Inspection.Id == entity.Inspection.Id)
                .Select(x => x.BaseStatementAppealCits.Inspection.Contragent)
                .OrderByDescending(x => x.ObjectCreateDate)
                .FirstOrDefault();

            if (primaryAppealCitsContragent != null)
            {
                entity.Contragent = primaryAppealCitsContragent;
            }

            return result;
        }
    }
}
