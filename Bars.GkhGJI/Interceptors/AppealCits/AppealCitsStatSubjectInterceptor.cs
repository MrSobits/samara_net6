namespace Bars.GkhGji.Interceptors
{
    using B4;
    using B4.Modules.States;
    using Bars.B4.DataAccess;
    using Bars.GkhGji.DomainService;
    using Bars.GkhGji.Enums;
    using Entities;
    using System.Collections.Generic;
    using System.Linq;

    public class AppealCitsStatSubjectInterceptor : EmptyDomainInterceptor<AppealCitsStatSubject>
    {
        //public override IDataResult AfterCreateAction(IDomainService<AppealCitsStatSubject> service, AppealCitsStatSubject entity)
        //{
        //    var appealCitsContainer = Container.Resolve<IDomainService<AppealCits>>();
        //    var appealCitsStatSubjectContainer = Container.Resolve<IDomainService<AppealCitsStatSubject>>();
        //    var statSubjectGjiContainer = Container.Resolve<IDomainService<StatSubjectGji>>();
        //    try
        //    {
        //        var statSubjectsList = appealCitsStatSubjectContainer.GetAll()
        //            .Where(x=> entity.AppealCits.Id==x.AppealCits.Id)
        //            .Select(x => x.Subject.Name).ToList()
        //            ;
        //        AppealCits appeal = entity.AppealCits;
        //        string statSubjects = statSubjectGjiContainer.Get(entity.Subject.Id).Name;
        //        foreach(string subject in statSubjectsList)
        //        {
        //            if (statSubjects != "")
        //                statSubjects += ", " + subject;
        //            else statSubjects = subject;
        //        }
        //        appeal.StatementSubjects = statSubjects;
        //        appealCitsContainer.Update(appeal);
        //    }
        //    finally
        //    {
           
        //    }

        //    return this.Success();
            
        //}
        public override IDataResult BeforeUpdateAction(IDomainService<AppealCitsStatSubject> service, AppealCitsStatSubject entity)
        {
            var appealCitsContainer = Container.Resolve<IDomainService<AppealCits>>();
            var appealCitsStatSubjectContainer = Container.Resolve<IDomainService<AppealCitsStatSubject>>();
            try
            {
                var statSubjectsList = appealCitsStatSubjectContainer.GetAll()
                    .Where(x => entity.AppealCits.Id == x.AppealCits.Id)
                    .Select(x => x.Subject.Name).ToList()
                    ;
                AppealCits appeal = appealCitsContainer.Get(entity.AppealCits.Id);
                string statSubjects = "";
                foreach (string subject in statSubjectsList)
                {
                    if (statSubjects != "")
                        statSubjects += ", " + subject;
                    else statSubjects = subject;
                }
                appeal.StatementSubjects = statSubjects;
                appealCitsContainer.Update(appeal);
            }
            finally
            {

            }

            return this.Success();

        }

        public override IDataResult AfterCreateAction(IDomainService<AppealCitsStatSubject> service, AppealCitsStatSubject entity)
        {
            var logEntityHistoryService = this.Container.Resolve<ILogEntityHistoryService>();
            var subjDomain = Container.ResolveDomain<StatSubjectGji>();
            try
            {
                logEntityHistoryService.UpdateLog(entity, TypeEntityLogging.AppealCitsStatSubject, entity.Id, entity.GetType(), GetPropertyValues(), entity.AppealCits.Id.ToString() + " " + subjDomain.Get(entity.Subject.Id).Name);
            }
            catch
            {

            }
            return this.Success();
        }
        public override IDataResult AfterUpdateAction(IDomainService<AppealCitsStatSubject> service, AppealCitsStatSubject entity)
        {
            var logEntityHistoryService = this.Container.Resolve<ILogEntityHistoryService>();
            try
            {
                logEntityHistoryService.UpdateLog(entity, TypeEntityLogging.AppealCitsStatSubject, entity.Id, entity.GetType(), GetPropertyValues(), entity.AppealCits.Id.ToString() + " " + entity.Subject.Name);
            }
            catch
            {

            }
            return this.Success();
        }

        public override IDataResult AfterDeleteAction(IDomainService<AppealCitsStatSubject> service, AppealCitsStatSubject entity)
        {
            var logEntityHistoryService = this.Container.Resolve<ILogEntityHistoryService>();
            try
            {
                logEntityHistoryService.UpdateLog(null, TypeEntityLogging.AppealCitsStatSubject, entity.Id, entity.GetType(), GetPropertyValues(), entity.AppealCits.Id.ToString() + " " + entity.Subject.Name);
            }
            catch
            {

            }
            return this.Success();
        }

        private Dictionary<string, string> GetPropertyValues()
        {
            var result = new Dictionary<string, string>
            {
                { "AppealCits", "Обращение граждан" },
                { "Subject", "Тематика" },
                { "Subsubject", "Подтематика" },
                { "Feature", "Характеристика" }
            };
            return result;
        }
    }
}
