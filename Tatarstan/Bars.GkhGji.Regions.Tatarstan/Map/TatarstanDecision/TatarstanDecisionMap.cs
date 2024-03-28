namespace Bars.GkhGji.Regions.Tatarstan.Map.TatartsanDecision
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.Enums;
    using Bars.GkhGji.Regions.Tatarstan.Entities.TatarstanDecision;

    public class TatarstanDecisionMap : JoinedSubClassMap<TatarstanDecision>
    {
        public TatarstanDecisionMap()
            : base(typeof(TatarstanDecision).FullName, "GJI_DECISION")
        {
        }

        /// <inheritdoc />
        protected override void Map()
        {
            this.Property(x => x.ErknmRegistrationNumber, "Учетный номер решения в ЕРКНМ").Column("ERKNM_REGISTRATION_NUMBER");
            this.Property(x => x.ErknmRegistrationDate, "Дата присвоения учетного номера / идентификатора ЕРКНМ").Column("ERKNM_REGISTRATION_DATE");
            this.Reference(x => x.DecisionPlace, "Место вынесения решения").Column("DECISION_PLACE_ID").Fetch();
            this.Property(x => x.SubmissionDate, "Дата направления требования о предоставлении документов").Column("SUBMISSION_DATE");
            this.Property(x => x.ReceiptDate, "Дата получения документов во исполнение требования").Column("RECEIPT_DATE");
            this.Property(x => x.OrganizationErknmGuid, "Гуид ЕРКНМ для организации").Column("ORGANIZATION_ERKNM_GUID").Length(36);
            this.Property(x => x.UsingMeansRemoteInteraction, "Использование средств дистанционного взаимодействия с контролируемым лицом")
                .Column("USING_MEANS_REMOTE_INTERACTION").DefaultValue(YesNoNotSet.NotSet);
            this.Property(x => x.InfoUsingMeansRemoteInteraction, "Сведения об использовании средств дистанционного взаимодействия")
                .Column("INFO_USING_MEANS_REMOTE_INTERACTION");
        }
    }
}