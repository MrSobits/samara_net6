namespace Bars.Gkh.Overhaul.Hmao.StateChange
{
    using Bars.B4.Application;
    using Bars.B4.DataAccess;
    using Bars.B4.Modules.States;
    using Bars.Gkh.Overhaul.Hmao.Entities;

    public class PublishProgramDeleteDateTransferRule : IRuleChangeStatus
    {
        public string Id { get { return "ovrhl_published_program_delete_date_rule"; } }

        public string Name { get { return "Удаление даты включения дома в ДПКР."; } }

        public string TypeId { get { return "ovrhl_published_program"; } }

        public string Description
        {
            get
            {
                return "Данное правило удаляет дату публикации у программы ";
            }
        }

        public ValidateResult Validate(IStatefulEntity statefulEntity, State oldState, State newState)
        {
            var container = ApplicationContext.Current.Container;
            var publishedProgramDomain = container.ResolveDomain<PublishedProgram>();

            try
            {
                var publishProgram = statefulEntity as PublishedProgram;

                if (publishProgram != null)
                {
                    publishProgram.PublishDate = null;
                }

                publishedProgramDomain.Update(publishProgram);

                return ValidateResult.Yes();
            }
            finally
            {
                container.Release(publishedProgramDomain);
            }
        }
    }
}