namespace Bars.GkhGji.Interceptors
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using B4;
    using B4.Modules.States;
    using Bars.B4.Application;
    using Bars.B4.DataAccess;
    using Bars.B4.IoC;
    using Bars.B4.Utils;
    using Bars.Gkh.Authentification;
    using Bars.Gkh.Entities;
    using Bars.GkhGji.Enums;
    using Contracts;
    using Contracts.Reminder;
    using Entities;

    public class AppealCitsServiceInterceptor : AppealCitsServiceInterceptor<AppealCits>
    {
    }

    public class AppealCitsServiceInterceptor<T> : EmptyDomainInterceptor<T>
        where T : AppealCits
    {
        static int CountDays(DayOfWeek day, DateTime start, DateTime end)
        {
            TimeSpan ts = end - start;                       // Total duration
            int count = (int)Math.Floor(ts.TotalDays / 7);   // Number of whole weeks
            int remainder = (int)(ts.TotalDays % 7);         // Number of remaining days
            int sinceLastDay = (int)(end.DayOfWeek - day);   // Number of days since last [day]
            if (sinceLastDay < 0) sinceLastDay += 7;         // Adjust for negative days since last [day]

            // If the days in excess of an even week are greater than or equal to the number days since the last [day], then count this one, too.
            if (remainder >= sinceLastDay) count++;

            return count;
        }

        public IGkhUserManager UserManager { get; set; }
        /// <inheritdoc />
        public override IDataResult BeforeCreateAction(IDomainService<T> service, T entity)
        {
            try
            {
                entity.Description = entity.Description.Replace("&nbsp;","");
                if (entity.QuestionStatus == null || entity.QuestionStatus == QuestionStatus.NotSet)
                {
                    entity.QuestionStatus = QuestionStatus.InWork;
                }

                var appDate = entity.DateFrom.HasValue ? entity.DateFrom.Value:DateTime.Now;
                var prodCalendarContainer = this.Container.Resolve<IDomainService<ProdCalendar>>().GetAll()
                    
                    .Where(x=> x.ProdDate>= appDate && x.ProdDate <= appDate.AddDays(38)).Select(x=> x.ProdDate).ToList();
               
                DateTime newControlDate = DateTime.Now;

                //int sartudaysCount = CountDays(DayOfWeek.Saturday, appDate.Value, appDate.Value.AddDays(28));
                //int sundaysCount = CountDays(DayOfWeek.Sunday, appDate.Value, appDate.Value.AddDays(28));
                //newControlDate = appDate.Value.AddDays(28 + sartudaysCount + sundaysCount);
                newControlDate = appDate.AddDays(27);

                //int celebrDatesCount = 0;
                //foreach (DateTime dt in prodCalendarContainer)
                //{
                //    if (dt <= newControlDate)
                //    {
                //        celebrDatesCount++;
                //    }
                //}
                // newControlDate = newControlDate.AddDays(celebrDatesCount);
                if (prodCalendarContainer.Contains(newControlDate))
                {
                    for (int i = 0; i <= prodCalendarContainer.Count; i++)
                    {
                        if (prodCalendarContainer.Contains(newControlDate))
                        {
                            newControlDate = newControlDate.AddDays(-1);
                        }
                        else
                        {
                            break;
                        }
                    }
                }               

                if (newControlDate.DayOfWeek == DayOfWeek.Saturday)
                {
                    newControlDate =  newControlDate.AddDays(-1);
                }
                else if (newControlDate.DayOfWeek == DayOfWeek.Sunday)
                {
                    newControlDate = newControlDate.AddDays(-2);
                }
                if (prodCalendarContainer.Contains(newControlDate))
                {
                    for (int i = 0; i <= prodCalendarContainer.Count; i++)
                    {
                        if (prodCalendarContainer.Contains(newControlDate))
                        {
                            newControlDate = newControlDate.AddDays(-1);
                        }
                        else
                        {
                            break;
                        }
                    }
                }
                //если стоит контрольный срок, расчетный не ставится
                if (entity.CheckTime is null)
                {
                    entity.CheckTime = newControlDate;
                }
            }
            catch(Exception e)
            {
                string str = e.Message;
            }


            if (entity.ContragentCorrespondent != null)
            {
                entity.Correspondent = entity.ContragentCorrespondent.Name;
            }

            var appSettings = ApplicationContext.Current.Configuration.AppSettings;
            var fkrApplication = appSettings.GetAs<bool>("fkrApplication");
            if (fkrApplication)
            {
                if (string.IsNullOrEmpty(entity.Number)&& string.IsNullOrEmpty(entity.DocumentNumber))
                {
                    var query = service.GetAll()
                        .Where(x => x.Number != null)
                        .Select(x => x.Number);

                    entity.Number = query.Any()
                        ? (query.AsEnumerable().Select(x => x.ToLong()).Max() + 1).ToStr()
                        : "1";
                }
                entity.NumberGji = DateTime.Now.Year.ToString().Substring(2, 2) + "-" + entity.Number;
                entity.DocumentNumber = DateTime.Now.Year.ToString().Substring(2, 2) + "-" + entity.Number;
            }

            this.SetSortableGjiNumber(entity);
            if (!entity.DateFrom.HasValue)
            {
                entity.DateFrom = DateTime.Now.Date;
            }

            if (entity.Year < 2)
            {
                entity.Year = entity.DateFrom.Value.Year;
            }
            // Перед сохранением присваиваем начальный статус 
            var servStateProvider = this.Container.Resolve<IStateProvider>();
            var servNumberRule = this.Container.Resolve<IAppealCitsNumberRule>();

            using (this.Container.Using(servStateProvider, servNumberRule))
            {
                if(!fkrApplication)
                servNumberRule.SetNumber(entity);
                if (entity.State == null)
                {
                    if (!entity.GisWork) // для обращений ГИС - свой начальный статус
                    {
                        servStateProvider.SetDefaultState(entity);
                    }
                }

                return this.Success();
            }
        }

        /// <inheritdoc />
        public override IDataResult BeforeUpdateAction(IDomainService<T> service, T entity)
        {
            var curAppeal = this.Container.Resolve<IDomainService<AppealCits>>().Get(entity.Id);
            if (entity.ContragentCorrespondent != null)
            {
                entity.Correspondent = entity.ContragentCorrespondent.Name;
            }
            this.SetSortableGjiNumber(entity);

            if (!entity.DateFrom.HasValue)
            {
                entity.DateFrom = DateTime.Now.Date;
            }

            if (entity.Year < 2)
            {
                entity.Year = entity.DateFrom.Value.Year;
            }

            this.Container.UsingForResolved<IAppealCitsNumberRule>((container, rule) =>
            {
                rule.SetNumber(entity);
            });

            if (entity.File == null && curAppeal.File != null)
            {
                entity.File = curAppeal.File;
            }
            try
            {
                var servStateProvider = this.Container.Resolve<IStateProvider>();
                using (this.Container.Using(servStateProvider))
                {
                    if (entity.State == null)
                    {
                        servStateProvider.SetDefaultState(entity);
                    }

                }
            }
            catch
            { }

            if (entity.OrderContragent == null)
            {
                var appealCitsRealityObjectDomain = Container.ResolveDomain<AppealCitsRealityObject>();
                var manOrgContractRealityObjectDomain = Container.ResolveDomain<ManOrgContractRealityObject>();
                try
                {
                    Contragent contragent = new Contragent();
                    var realityObject = appealCitsRealityObjectDomain.FirstOrDefault(x => x.AppealCits == entity);
                    if (realityObject == null)
                    {
                        return this.Success();
                    }

                    var contract = manOrgContractRealityObjectDomain.GetAll()
                       .Where(x => x.RealityObject == realityObject.RealityObject)
                       .Where(x => x.ManOrgContract.StartDate.HasValue && x.ManOrgContract.StartDate.Value <= DateTime.Now)
                       .Where(x => !x.ManOrgContract.EndDate.HasValue || x.ManOrgContract.EndDate.Value > DateTime.Now).FirstOrDefault();
                    if (contract != null)
                    {
                        if (contract.ManOrgContract.ManagingOrganization != null)
                        {
                            contragent = contract.ManOrgContract.ManagingOrganization.Contragent;
                            entity.OrderContragent = contragent;
                        }
                    }
                    return this.Success();

                }
                finally
                {
                    Container.Release(appealCitsRealityObjectDomain);
                    Container.Release(manOrgContractRealityObjectDomain);
                }
            }
            else
            {
                return this.Success();
            }
        }

        /// <inheritdoc />
        public override IDataResult BeforeDeleteAction(IDomainService<T> service, T entity)
        {
            var servAppCitsAnswer = this.Container.Resolve<IDomainService<AppealCitsAnswer>>();
            var serStatement = this.Container.Resolve<IDomainService<InspectionAppealCits>>();
            var appealCitsRealityObjectService = this.Container.Resolve<IDomainService<AppealCitsRealityObject>>();
            var appealCitsSourceService = this.Container.Resolve<IDomainService<AppealCitsSource>>();
            var appealCitsStatSubjectService = this.Container.Resolve<IDomainService<AppealCitsStatSubject>>();
            var appealCitsRequestService = this.Container.Resolve<IDomainService<AppealCitsRequest>>();
            var relatedAppealCitsService = this.Container.Resolve<IDomainService<RelatedAppealCits>>();
            var servReminder = this.Container.Resolve<IDomainService<Reminder>>();

            try
            {
                var refFuncs = new List<Func<long, string>>
                {
                    id =>
                        servAppCitsAnswer.GetAll()
                            .Any(x => x.AppealCits.Id == id)
                            ? "Ответы по обращению граждан"
                            : null,
                    id =>
                        serStatement.GetAll().Any(x => x.AppealCits.Id == id)
                            ? "Проверки по обращениям граждан"
                            : null
                };

                var refs = refFuncs.Select(x => x(entity.Id)).Where(x => x != null).ToArray();

                var message = string.Empty;

                if (refs.Length > 0)
                {
                    message = refs.Aggregate(message, (current, str) => current + string.Format(" {0}; ", str));
                    message = string.Format("Существуют связанные записи в следующих таблицах: {0}", message);
                    return this.Failure(message);
                }

                var previousAppealCitsList = service.GetAll().Where(x => x.PreviousAppealCits.Id == entity.Id).ToList();
                foreach (var value in previousAppealCitsList)
                {
                    value.PreviousAppealCits = null;
                    service.Update(value);
                }

                var appealCitsRealityObjectIds =
                    appealCitsRealityObjectService.GetAll()
                        .Where(x => x.AppealCits.Id == entity.Id)
                        .Select(x => x.Id)
                        .ToList();
                foreach (var value in appealCitsRealityObjectIds)
                {
                    appealCitsRealityObjectService.Delete(value);
                }

                var appealCitsSourceIds =
                    appealCitsSourceService.GetAll().Where(x => x.AppealCits.Id == entity.Id).Select(x => x.Id).ToList();
                foreach (var value in appealCitsSourceIds)
                {
                    appealCitsSourceService.Delete(value);
                }

                var appealCitsStatSubjectIds =
                    appealCitsStatSubjectService.GetAll()
                        .Where(x => x.AppealCits.Id == entity.Id)
                        .Select(x => x.Id)
                        .ToList();
                foreach (var value in appealCitsStatSubjectIds)
                {
                    appealCitsStatSubjectService.Delete(value);
                }

                var appealCitsRequestIds =
                    appealCitsRequestService.GetAll()
                        .Where(x => x.AppealCits.Id == entity.Id)
                        .Select(x => x.Id)
                        .ToList();
                foreach (var value in appealCitsRequestIds)
                {
                    appealCitsRequestService.Delete(value);
                }

                var relatedAppealCitsIds =
                    relatedAppealCitsService.GetAll()
                        .Where(x => x.Parent.Id == entity.Id || x.Children.Id == entity.Id)
                        .Select(x => x.Id)
                        .ToList();
                foreach (var value in relatedAppealCitsIds)
                {
                    relatedAppealCitsService.Delete(value);
                }

                var reminderIds = servReminder.GetAll()
                    .Where(x => x.AppealCits.Id == entity.Id)
                    .Select(x => x.Id)
                    .ToList();
                foreach (var value in reminderIds)
                {
                    servReminder.Delete(value);
                }
            }
            finally
            {
                this.Container.Release(servAppCitsAnswer);
                this.Container.Release(serStatement);
                this.Container.Release(appealCitsRealityObjectService);
                this.Container.Release(appealCitsSourceService);
                this.Container.Release(appealCitsStatSubjectService);
                this.Container.Release(appealCitsRequestService);
                this.Container.Release(relatedAppealCitsService);
                this.Container.Release(servReminder);
            }

            return this.Success();
        }

        /// <inheritdoc />
        public override IDataResult AfterUpdateAction(IDomainService<T> service, T entity)
        {
           
            try
            {
                Operator thisOperator = UserManager.GetActiveOperator();
                var citsInfo = this.Container.Resolve<IDomainService<AppealCitsInfo>>();
                AppealCitsInfo newRecord = new AppealCitsInfo();
                newRecord.AppealDate = entity.DateFrom.HasValue ? entity.DateFrom.Value : entity.ObjectEditDate;
                newRecord.DocumentNumber = $"{entity.DocumentNumber}/{entity.NumberGji} (id={entity.Id})";
                newRecord.ObjectCreateDate = DateTime.Now;
                newRecord.ObjectEditDate = DateTime.Now;
                newRecord.OperationDate = DateTime.Now;
                newRecord.ObjectVersion = 0;
                newRecord.OperationType = Enums.AppealOperationType.AppealEdit;
                newRecord.Operator = thisOperator.User.Login;
                citsInfo.Save(newRecord);
            }
            catch (Exception e)
            {

            }
            this.CreateReminders(entity);

            return this.Success();
        }

        /// <inheritdoc />
        public override IDataResult AfterCreateAction(IDomainService<T> service, T entity)
        {
           
            try
            {
                Operator thisOperator = UserManager.GetActiveOperator();
                var citsInfo = this.Container.Resolve<IDomainService<AppealCitsInfo>>();
                AppealCitsInfo newRecord = new AppealCitsInfo();
                newRecord.AppealDate = entity.DateFrom.HasValue ? entity.DateFrom.Value : entity.ObjectEditDate;
                newRecord.DocumentNumber = $"{entity.DocumentNumber}/{entity.NumberGji} (id={entity.Id})";
                newRecord.ObjectCreateDate = DateTime.Now;
                newRecord.ObjectEditDate = DateTime.Now;
                newRecord.OperationDate = DateTime.Now;
                newRecord.ObjectVersion = 0;
                newRecord.OperationType = Enums.AppealOperationType.AppealCreate;
                newRecord.Operator = thisOperator.User.Login;
                citsInfo.Save(newRecord);
            }
            catch (Exception e)
            {

            }
            this.CreateReminders(entity);
            return this.Success();
        }

        private void CreateReminders(T entity)
        {
            // Получаем правила формирования Напоминаний и запускаем метод создания напоминаний
            var servReminderRule = this.Container.ResolveAll<IReminderRule>();

            try
            {
                var rule = servReminderRule.FirstOrDefault(x => x.Id == "AppealCitsReminderRule");
                rule?.Create(entity);
            }
            finally
            {
                this.Container.Release(servReminderRule);
            }
        }

        protected void SetSortableGjiNumber(T entity)
        {
            entity.SortNumberGji = this.GetSortableGjiNumber(entity.NumberGji);
        }

        private long? GetSortableGjiNumber(string gjiNumber)
        {
            long sortGjiNumber;
            if (long.TryParse(gjiNumber, out sortGjiNumber))
            {
                return sortGjiNumber;
            }
            return null;
        }
    }
}