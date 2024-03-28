namespace Bars.Gkh.Overhaul.Hmao.StateChange
{
    using System;
    using Bars.B4;
    using Bars.B4.Application;
    using Bars.B4.DataAccess;
    using Bars.B4.Modules.States;
    using Bars.B4.Utils;
    using Bars.Gkh.Domain;
    using Bars.Gkh.Overhaul.Hmao.Entities;

    public class PublishProgramAddDateTransferRule : IGkhRuleChangeStatus
    {
        public string Id { get { return "ovrhl_published_program_add_date_rule"; } }

        public string Name { get { return "Добавление даты включения дома в ДПКР."; } }

        public string TypeId { get { return "ovrhl_published_program"; } }

        public string Description
        {
            get
            {
                return "По данному правилу можно указать дату публикации у программы ";
            }
        }

        public ValidateResult Validate(IStatefulEntity statefulEntity, State oldState, State newState)
        {
            return  ValidateResult.Yes();
        }

        public void HandleUserParams(BaseParams baseParams, IStatefulEntity entity)
        {
            var container = ApplicationContext.Current.Container;
            var publishedProgramDomain = container.ResolveDomain<PublishedProgram>();

            try
            {
                var userParams = baseParams.Params.GetAs<DynamicDictionary>("userParams");
                var publishDate = userParams.GetAs<DateTime>("PublishDate");

                var publishProgram = entity as PublishedProgram;

                if (publishProgram != null)
                {
                    publishProgram.PublishDate = publishDate;
                }

                publishedProgramDomain.Update(publishProgram);
            }
            finally
            {
                container.Release(publishedProgramDomain);
            }
        }
    }
}