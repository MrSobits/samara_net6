namespace Bars.GkhGji.Regions.Tyumen.Utils.EntityExceptions
{
    using System;
    using System.Linq;
    using System.Text;
    using B4.Application;
    using B4.DataAccess;
    using B4.IoC;
    using B4.Modules.Messenger;
    using B4.Modules.States;
    using B4.Utils;
    using Castle.Windsor;
    using Entities.Suggestion;
    using Gkh.Entities.Suggestion;
    using Gkh.Enums;
    using Gkh.Utils.EntityExtensions;

    using Microsoft.Extensions.Logging;

    internal static class CitizenSuggestionExtensions
    {
        public static IWindsorContainer Container
        {
            get { return ApplicationContext.Current.Container; }
        }

        public static void SendApplicantNotification(this CitizenSuggestion suggestion, State state)
        {
            var notificationDomain = Container.ResolveDomain<ApplicantNotification>();
            using (Container.Using(notificationDomain))
            {
                var notification = notificationDomain.GetAll().FirstOrDefault(x => x.State.Id == state.Id);
                if (notification == null)
                {
                    Container.Resolve<ILogger>().LogError("Не найден шаблон для уведомления заявителю");
                    return;
                }

                var email = suggestion.ApplicantEmail;

                if (string.IsNullOrWhiteSpace(email))
                {
                    return;
                }

                var body = suggestion.GetEmailBody(notification);

                try
                {
                    var sender = Container.Resolve<IMailSender>();
                    sender.SendMessage(email, notification.EmailSubject, body);
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
        }

        public static string GetEmailBody(this CitizenSuggestion suggestion, ApplicantNotification notification)
        {
            var commentDomain = Container.ResolveDomain<SuggestionComment>();

            using (Container.Using(commentDomain))
            {
                var comment = commentDomain.GetAll()
                    .ToArray()
                    .Select(x => new
                    {
                        sugId = x.CitizenSuggestion.Id,
                        x.Id,
                        x.CreationDate,
                        Executor =
                            x.GetCurrentExecutorType() != ExecutorType.None
                                ? x.GetExecutor(x.GetCurrentExecutorType()).Name
                                : string.Empty
                    });

                var lastComment = comment.OrderByDescending(x => x.Id).First();

                var executor = lastComment.Executor;

                var message = new StringBuilder(notification.EmailTemplate)
                    .Replace("{Исполнитель}", executor)
                    .Replace("{Адрес}", suggestion.RealityObject.Return(x => x.Address) ?? string.Empty)
                    .Replace("{НомерОбращения}", suggestion.Number ?? string.Empty)
                    .Replace("{ДатаОбращения}", suggestion.CreationDate.ToShortDateString());

                return message.ToString();
            }
        }
    }
}