namespace Bars.Gkh.Decisions.Nso.Domain.Impl
{
    using B4.DataAccess;
    using B4.Modules.Security;
    using B4.Utils;
    using Bars.B4;
    using Bars.B4.Modules.NHibernateChangeLog;
    using Bars.Gkh.Decisions.Nso.Entities;
    using Castle.Windsor;
    using Gkh.Entities;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using NHibernate;

    public class LogMonthlyFeeAmountDecisionServiceService : ILogMonthlyFeeAmountDecisionService
    {
        /// <summary>
        /// Провайдер сессии
        /// </summary>
        public ISessionProvider SessionProvider { get; set; }

        /// <summary>
        /// Контейнер
        /// </summary>
        public IWindsorContainer Container { get; set; }

        /// <summary>
        /// Домен сервис "Жилой дом"
        /// </summary>
        public IDomainService<RealityObject> RealityObjectServise { get; set; }

        /// <summary>
        /// Домен сервис "Логирования изменений" 
        /// </summary>
        public IDomainService<LogEntity> LogEntityServise { get; set; }

        /// <summary>
        /// Домен сервис "Детализации логирования изменений" 
        /// </summary>
        public IDomainService<LogEntityProperty> LogEntityPropertyServise { get; set; }

        /// <summary>
        /// Пользователь
        /// </summary>
        public IRepository<User> UserServise { get; set; }

        public RequestingUserInformation RequestingUserInformation { get; set; }

        /// <summary>
        /// Логирования сущности в место NHibernateChangeLog
        /// </summary>
        /// <param name="entity">Сущность после изменения</param>
        /// <param name="roId">Id жилога дома</param>
        /// <param name="type">Тип сохранения</param>
        public void Log(MonthlyFeeAmountDecision entity, long roId, ActionKind type)
        {
            var session = this.SessionProvider.GetCurrentSession();
            var flushMode = session.FlushMode;

            try
            {
                session.FlushMode = FlushMode.Never;
                var monthlyFeeAmountDecision = this.GetEntity(entity.Id);
                var roAddress = this.RealityObjectServise.GetAll().Where(x => x.Id == roId).Select(x => x.Address).FirstOrDefault();

                var oldDecision = new List<PeriodMonthlyFee>();
                var newDecision = new List<PeriodMonthlyFee>();

                if (monthlyFeeAmountDecision != null)
                {
                    oldDecision = monthlyFeeAmountDecision.Decision;
                    newDecision = entity.Decision;
                }

                if (monthlyFeeAmountDecision != null && type == ActionKind.Delete)
                {
                    this.SaveLog(oldDecision, entity, ActionKind.Delete, roAddress);
                    this.SessionProvider.GetCurrentSession().Evict(monthlyFeeAmountDecision);
                }

                //Проверка на изменения объектов в списках
                else if (monthlyFeeAmountDecision != null &&
                    !oldDecision.SequenceEqual(
                        newDecision,
                        FnEqualityComparer<PeriodMonthlyFee>.Fn((x, y) => y.Guid == x.Guid && x.Value == y.Value && x.From == y.From && x.To == y.To)))
                {
                    var insertList = newDecision.Where(x => !oldDecision.Select(y => y.Guid).Contains(x.Guid)).ToList();
                    var deleteList = oldDecision.Where(x => !newDecision.Select(y => y.Guid).Contains(x.Guid)).ToList();
                    var updateList =
                        newDecision.Where(x => oldDecision.Any(y => y.Guid == x.Guid && (x.Value != y.Value || x.From != y.From || x.To != y.To))).ToList();

                    this.SaveLog(insertList, entity, ActionKind.Insert, roAddress);
                    this.SaveLog(deleteList, entity, ActionKind.Delete, roAddress);
                    this.SaveLog(updateList, entity, ActionKind.Update, roAddress, monthlyFeeAmountDecision);

                    this.SessionProvider.GetCurrentSession().Evict(monthlyFeeAmountDecision);
                }
                else if (ActionKind.Insert == type)
                {
                    this.SaveLog(entity.Decision, entity, type, roAddress);
                }
            }
            finally
            {
                session.FlushMode = flushMode;
            }
        }

        /// <summary>
        /// Сохранения логирования сущности 
        /// </summary>
        /// <param name="elementList">Элементы с которыми происходило изменение</param>
        /// <param name="entity">Сущность после изменения</param>
        /// <param name="saveType">Тип сохранения</param>
        /// <param name="address">Адрес жилого дома</param>
        /// <param name="entitycheck">Сущность до сохранения</param>
        private void SaveLog(List<PeriodMonthlyFee> elementList, MonthlyFeeAmountDecision entity, ActionKind saveType, string address, MonthlyFeeAmountDecision entitycheck = null)
        {

            var user = this.UserServise.Get((object)this.RequestingUserInformation.UserIdentity.UserId);

                foreach (var element in elementList)
                {
                    LogEntityProperty logEntityProperty = null;

                    var logEntity = new LogEntity
                    {
                        SessionId = this.SessionProvider.CurrentSession.GetSessionImplementation().SessionId.ToString(),
                        B4SessionId = this.RequestingUserInformation.SessionId,
                        ChangeDate = DateTime.UtcNow,
                        EntityType = $"{entity.GetType().Namespace}.{entity.GetType().Name}",
                        EntityId = entity.Id,
                        ActionKind = saveType,
                        EntityDescription = address ?? string.Empty,

                        UserId = user.Id.ToString(),
                        UserIpAddress = this.RequestingUserInformation.RequestIpAddress ?? "localhost",
                        UserLogin = user.Login,
                        UserName = user.Name,
                        UserDescription = user.Name
                    };

                    LogEntityServise.Save(logEntity);

                    if (ActionKind.Update != saveType)
                    {
                        logEntityProperty = new LogEntityProperty
                        {
                            LogEntity = logEntity,
                            NewValue = this.GetStingRecord(element),
                            PropertyCode = "Decision"
                        };
                    }
                    else
                    {
                        var oldEntity = entitycheck.Decision.FirstOrDefault(x => x.Guid == element.Guid);

                        logEntityProperty = new LogEntityProperty
                        {
                            LogEntity = logEntity,
                            NewValue = this.GetStingRecord(element),
                            OldValue = this.GetStingRecord(oldEntity),
                            PropertyCode = "Decision"

                        };
                    }
                    this.LogEntityPropertyServise.Save(logEntityProperty);
                }
        }

        /// <summary>
        /// Формат информации детализации
        /// </summary>
        /// <param name="element">Элемент детализации </param>
        /// <returns></returns>
        private string GetStingRecord(PeriodMonthlyFee element)
        {
            return "С " + $"{element.From:dd.MM.yyyy}"
                + " по " + $"{element.To:dd.MM.yyyy}"
                + " сумма взноса= " + element.Value + ";";
        }

        private MonthlyFeeAmountDecision GetEntity(long id)
        {
            using (var session = this.SessionProvider.OpenStatelessSession())
            {
                return session.Get<MonthlyFeeAmountDecision>(id);
            }
        }
    }
}

