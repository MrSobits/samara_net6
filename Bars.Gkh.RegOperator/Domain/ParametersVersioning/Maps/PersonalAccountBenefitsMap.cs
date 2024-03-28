namespace Bars.Gkh.RegOperator.Domain.ParametersVersioning.Maps
{
    using Entities.PersonalAccount;
    using Gkh.Domain.ParameterVersioning;

    public class PersonalAccountBenefitsMap : VersionedEntity<PersonalAccountBenefits>
    {
        public PersonalAccountBenefitsMap()
            : base(VersionedParameters.PersonalAccountBenefits)
        {
            Map(x => x.Sum, null, "Сумма начисленной льготы");
        }
    }
}