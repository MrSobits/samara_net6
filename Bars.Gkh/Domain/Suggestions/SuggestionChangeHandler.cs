namespace Bars.Gkh.Domain.Suggestions
{
    using System;
    using System.Linq;
    using B4.Application;
    using B4.DataAccess;
    using B4.Modules.Messenger;
    using B4.Modules.States;
    using B4.Utils;

    using Castle.Windsor;
    using Entities.Suggestion;
    using Utils.EntityExtensions;
    using Bars.Gkh.Repositories;

    using Microsoft.Extensions.Logging;

    internal static class SuggestionChangeHandler
    {
        private static IWindsorContainer Container
        {
            get { return ApplicationContext.Current.Container; }
        }

        /// <summary>
        /// Применить изменение состояния (отправить уведомление, сохранить историю, проставить статус)
        /// </summary>
        public static void ApplyChange(CitizenSuggestion suggestion, Transition transition)
        {
            SendEmailExecutor(suggestion, transition);
            SendEmailApplicant(suggestion, transition);

            Container.Resolve<IStateProvider>().SetDefaultState(suggestion);

            if (suggestion.Id > 0)
            {
                Container.ResolveDomain<CitizenSuggestionHistory>()
                    .Save(new CitizenSuggestionHistory(suggestion, transition.TargetExecutorType,
                        transition.ExecutorEmail));
            }
        }

        public static void ApplyChange(SuggestionComment comment, Transition transition)
        {
            var stateRepo = Container.Resolve<IStateRepository>();

            SendEmailExecutor(comment, transition);
            SendEmailApplicant(comment, transition);

            var startState = stateRepo.GetAllStates<CitizenSuggestion>().FirstOrDefault(x => x.Code == "Check");

            comment.CitizenSuggestion.State = startState;

            Container.ResolveDomain<CitizenSuggestionHistory>()
                .Save(new CitizenSuggestionHistory(comment, comment.IsFirst ? transition.InitialExecutorType : transition.TargetExecutorType,
                        transition.ExecutorEmail));
        }

        /// <summary>
        ///     Отправить уведомление заявителю
        /// </summary>
        private static void SendEmailApplicant(CitizenSuggestion suggestion, Transition transition)
        {
            var email = suggestion.ApplicantEmail;

            if (email.IsEmpty())
            {
                return;
            }

            var body = suggestion.GetEmailBody(transition);

            try
            {
                var sender = Container.Resolve<IMailSender>();
                sender.SendMessage(email, "Назначение исполнителя по обращению", body);
            }
            catch (Exception e)
            {
                var message =
                    string.Format(
                        "Ошибка при отправке уведомления по обращению заявителю {0}. ExceptionMessage: {1}. StackTrace: {2}",
                        suggestion.Number, e.Message, e.StackTrace);

                Container.Resolve<ILogger>().LogError(message);
            }
        }

        private static void SendEmailApplicant(SuggestionComment comment, Transition transition)
        {
            var email = comment.CitizenSuggestion.ApplicantEmail;

            if (email.IsEmpty())
            {
                return;
            }

            var body = comment.GetEmailBody(transition);

            try
            {
                var sender = Container.Resolve<IMailSender>();
                sender.SendMessage(email, "Назначение исполнителя по обращению", body);
            }
            catch (Exception e)
            {
                var message =
                    string.Format(
                        "Ошибка при отправке уведомления по обращению заявителю {0}. ExceptionMessage: {1}. StackTrace: {2}",
                        comment.CitizenSuggestion.Number, e.Message, e.StackTrace);

                Container.Resolve<ILogger>().LogError(message);
            }
        }

        /// <summary>
        ///     Отправить уведомление исполнителю
        /// </summary>
        private static void SendEmailExecutor(CitizenSuggestion suggestion, Transition transition)
        {
            var email = 
                !transition.ExecutorEmail.IsEmpty()
                    ? transition.ExecutorEmail
                    : suggestion.GetExecutorEmail();

            if (email.IsEmpty())
            {
                return;
            }

            var body = suggestion.GetEmailBody(transition);

            try
            {
                var sender = Container.Resolve<IMailSender>();
                sender.SendMessage(email, transition.EmailSubject, body);
            }
            catch (Exception e)
            {
                var message =
                    string.Format(
                        "Ошибка при отправке уведомления по обращению исполнителю {0}. ExceptionMessage: {1}. StackTrace: {2}",
                        suggestion.Number, e.Message, e.StackTrace);

                Container.Resolve<ILogger>().LogError(message);
            }
        }

        private static void SendEmailExecutor(SuggestionComment comment, Transition transition)
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
                var sender = Container.Resolve<IMailSender>();
                sender.SendMessage(email, transition.EmailSubject, body);
            }
            catch (Exception e)
            {
                var message =
                    string.Format(
                        "Ошибка при отправке уведомления по обращению исполнителю {0}. ExceptionMessage: {1}. StackTrace: {2}",
                        comment.CitizenSuggestion.Number, e.Message, e.StackTrace);

                Container.Resolve<ILogger>().LogError(message);
            }
        }
    }
}