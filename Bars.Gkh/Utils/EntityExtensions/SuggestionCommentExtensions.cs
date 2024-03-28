namespace Bars.Gkh.Utils.EntityExtensions
{
    using System.Linq;
    using System.Text;
    using B4.Application;
    using B4.DataAccess;
    using B4.Utils;
    using B4.Utils.Annotations;
    using Castle.Windsor;
    using Entities;
    using Entities.Suggestion;
    using Enums;

    public static class SuggestionCommentExtensions
    {
        public static IWindsorContainer Container
        {
            get { return ApplicationContext.Current.Container; }
        }

        public static ExecutorProxy GetExecutor(this SuggestionComment value, ExecutorType executorType)
        {
            if (executorType == ExecutorType.Mo && value.ExecutorManagingOrganization != null)
            {
                return new ExecutorProxy
                {
                    Id = value.ExecutorManagingOrganization.Id,
                    Name = value.ExecutorManagingOrganization.Contragent.Name
                };
            }

            if (executorType == ExecutorType.Mu && value.ExecutorMunicipality != null)
            {
                var name = GetMunicipalityExecutor(value).Return(x => x.Contragent).Return(x => x.Name)
                           ?? value.ExecutorMunicipality.Name;

                return new ExecutorProxy { Id = value.ExecutorMunicipality.Id, Name = name };
            }

            if (executorType == ExecutorType.Gji && value.ExecutorZonalInspection != null)
            {
                return new ExecutorProxy
                {
                    Id = value.ExecutorZonalInspection.Id,
                    Name = value.ExecutorZonalInspection.ZoneName
                };
            }

            if (executorType == ExecutorType.CrFund && value.ExecutorCrFund != null)
            {
                return new ExecutorProxy
                {
                    Id = value.ExecutorCrFund.Id,
                    Name = string.Format("{0}, {1}",
                        value.ExecutorCrFund.FullName,
                        value.ExecutorCrFund.Position.Name)
                };
            }

            return null;
        }

        public static string GetExecutorEmail(this SuggestionComment comment)
        {
            ArgumentChecker.NotNull(comment, "suggestion");

            string email = null;

            switch (comment.GetCurrentExecutorType())
            {
                case ExecutorType.Gji:
                    email = comment.ExecutorZonalInspection.Return(x => x.Email);
                    break;
                case ExecutorType.Mu:
                    email = GetMunicipalityExecutor(comment).Return(x => x.Contragent).Return(x => x.Email);
                    break;
                case ExecutorType.Mo:
                    email = comment.ExecutorManagingOrganization.Return(x => x.Contragent).Return(x => x.Email);
                    break;
            }

            return email;
        }

        public static string GetEmailBody(this SuggestionComment comment, Transition transition)
        {
            var executor = comment.GetExecutor(comment.GetCurrentExecutorType());

            var message = new StringBuilder(transition.EmailTemplate)
                .Replace("{Исполнитель}", executor.Return(x => x.Name) ?? string.Empty)
                .Replace("{Адрес}", comment.CitizenSuggestion.RealityObject.Return(x => x.Address) ?? string.Empty)
                .Replace("{НомерОбращения}", comment.CitizenSuggestion.Number ?? string.Empty)
                .Replace("{ДатаОбращения}", comment.CreationDate.ToDateString());

            return message.ToString();
        }

        private static LocalGovernment GetMunicipalityExecutor(SuggestionComment comment)
        {
            if (comment.ExecutorMunicipality == null)
            {
                return null;
            }

            return Container.ResolveDomain<LocalGovernmentMunicipality>().GetAll()
                .Where(x => x.Municipality.Id == comment.ExecutorMunicipality.Id)
                .Where(x => x.LocalGovernment != null)
                .Select(x => x.LocalGovernment)
                .FirstOrDefault();
        }

        /// <summary>
        /// Прокси исполнителя.
        /// </summary>
        public sealed class ExecutorProxy
        {
            /// <summary>
            /// ИД.
            /// </summary>
            public long Id { get; set; }

            /// <summary>
            /// Наименование
            /// </summary>
            /// <remarks>
            /// Для типа исполнителя MU это имя контрагента органа местного самоуправления (ОМС) связанного
            /// с муниципальным образованием. При нескольких ОМС берется первое попавшееся.
            /// </remarks>
            public string Name { get; set; }
        }
    }
}