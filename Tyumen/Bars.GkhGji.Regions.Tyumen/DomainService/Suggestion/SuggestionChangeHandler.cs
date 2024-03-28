namespace Bars.GkhGji.Regions.Tyumen.DomainService.Suggestion
{
    using System;
    using System.Linq;
    using System.Text;
    using B4;
    using B4.DataAccess;
    using B4.Modules.Messenger;
    using B4.Utils;
    using Castle.Windsor;
    using Entities.Suggestion;
    using Gkh.DomainService;
    using Gkh.Entities.Suggestion;
    using Gkh.Repositories;
    using Gkh.Utils.EntityExtensions;

    using Microsoft.Extensions.Logging;

    public class SuggestionChangeHandler : ISuggestionChangeHandler
    {
        private readonly IWindsorContainer Container;

        private readonly ILogger LogManager;

        private readonly IDomainService<ApplicantNotification> AppNotificationDomain;

        public SuggestionChangeHandler(IWindsorContainer container, ILogger logManager, IDomainService<ApplicantNotification> appNotificationDomain)
        {
            this.Container = container;
            this.LogManager = logManager;
            this.AppNotificationDomain = appNotificationDomain;
        }

        /// <summary>
        ///     Применить изменение состояния (отправить уведомление, сохранить историю, проставить статус)
        /// </summary>
        public void ApplyChange(SuggestionComment comment, Transition transition)
        {
            var stateRepo = this.Container.Resolve<IStateRepository>();

            var startState =
                stateRepo.GetAllStates<CitizenSuggestion>()
                    .FirstOrDefault(x => x.Code == (comment.CitizenSuggestion.TestSuggestion ? "4" : "Check"));

            comment.CitizenSuggestion.State = startState;

            this.SendEmailExecutor(comment, transition);

            var notif = this.AppNotificationDomain.GetAll().FirstOrDefault(x => x.State.Id == comment.CitizenSuggestion.State.Id);
            if (notif != null)
            {
                this.SendEmailApplicant(comment, notif);
            }

            this.Container.ResolveDomain<CitizenSuggestionHistory>()
                .Save(new CitizenSuggestionHistory(comment,
                    comment.IsFirst && !comment.CitizenSuggestion.TestSuggestion ? transition.InitialExecutorType : transition.TargetExecutorType,
                    transition.ExecutorEmail));
        }

        /// <summary>
        ///     Отправить уведомление заявителю
        /// </summary>
        private void SendEmailApplicant(SuggestionComment comment, ApplicantNotification notif)
        {
            var email = comment.CitizenSuggestion.ApplicantEmail;

            if (email.IsEmpty())
            {
                return;
            }
            var executor = comment.GetExecutor(comment.GetCurrentExecutorType());

            var body = new StringBuilder(notif.EmailTemplate)
                .Replace("{Исполнитель}", executor.Return(x => x.Name) ?? string.Empty)
                .Replace("{Адрес}", comment.CitizenSuggestion.RealityObject.Return(x => x.Address) ?? string.Empty)
                .Replace("{НомерОбращения}", comment.CitizenSuggestion.Number ?? string.Empty)
                .Replace("{ДатаОбращения}", comment.CreationDate.HasValue ? comment.CreationDate.Value.ToShortDateString() : "");

            try
            {
                var sender = this.Container.Resolve<IMailSender>();
                sender.SendMessage(email, notif.EmailSubject, body.ToString());
            }
            catch (Exception e)
            {
                var message =
                    string.Format(
                        "Ошибка при отправке уведомления по обращению заявителю {0}. ExceptionMessage: {1}. StackTrace: {2}",
                        comment.CitizenSuggestion.Number, e.Message, e.StackTrace);

                this.LogManager.LogError(message);
            }
        }

        /// <summary>
        ///     Отправить уведомление исполнителю
        /// </summary>
        private void SendEmailExecutor(SuggestionComment comment, Transition transition)
        {
            var email =
                !transition.ExecutorEmail.IsEmpty()
                    ? transition.ExecutorEmail
                    : comment.GetExecutorEmail();

            if (email.IsEmpty())
            {
                return;
            }

            var body = comment.GetEmailBody(transition);

            try
            {
                var sender = this.Container.Resolve<IMailSender>();
                sender.SendMessage(email, transition.EmailSubject, body);
            }
            catch (Exception e)
            {
                var message =
                    string.Format(
                        "Ошибка при отправке уведомления по обращению исполнителю {0}. ExceptionMessage: {1}. StackTrace: {2}",
                        comment.CitizenSuggestion.Number, e.Message, e.StackTrace);

                this.Container.Resolve<ILogger>().LogError(message);
            }
        }

        /// <summary>
        ///     Отправить уведомление заявителю
        /// </summary>
        public void SendEmailApplicant(SuggestionComment comment)
        {
            var notif = this.AppNotificationDomain.GetAll().FirstOrDefault(x => x.State.Id == comment.CitizenSuggestion.State.Id);

            var email = comment.CitizenSuggestion.ApplicantEmail;

            if (email.IsEmpty() || notif == null)
            {
                return;
            }
            var executor = comment.GetExecutor(comment.GetCurrentExecutorType());

            var body = new StringBuilder(notif.EmailTemplate)
                .Replace("{Исполнитель}", executor.ReturnSafe(x => x.Name) ?? string.Empty)
                .Replace("{Адрес}", comment.CitizenSuggestion.RealityObject.Return(x => x.Address) ?? string.Empty)
                .Replace("{НомерОбращения}", comment.CitizenSuggestion.Number ?? string.Empty)
                .Replace("{ДатаОбращения}", comment.CitizenSuggestion.CreationDate.ToShortDateString());

            try
            {
                var sender = this.Container.Resolve<IMailSender>();
                sender.SendMessage(email, notif.EmailSubject, body.ToString());
            }
            catch (Exception e)
            {
                var message =
                    string.Format(
                        "Ошибка при отправке уведомления по обращению заявителю {0}. ExceptionMessage: {1}. StackTrace: {2}",
                        comment.CitizenSuggestion.Number, e.Message, e.StackTrace);

                this.LogManager.LogError(message);
            }
        }
    }
}