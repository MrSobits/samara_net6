namespace Bars.Gkh.RegOperator.Entities
{
    using Bars.Gkh.Entities;

    using Gkh.Entities;

    public class PersonalAccountCalculatedParameter : BaseImportableEntity
    {
        public virtual BasePersonalAccount PersonalAccount { get; set; }

        public virtual EntityLogLight LoggedEntity { get; set; }
    }
}