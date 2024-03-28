namespace Bars.Gkh.Regions.Voronezh.Entities
{
    using System;
    using B4.Modules.FileStorage;

    using Gkh.Entities;
    using Enums;
    using Bars.Gkh.Modules.ClaimWork.Entities;
    using Bars.Gkh.Modules.ClaimWork.Enums;
    using Bars.Gkh.RegOperator.Entities.Owner;

    /// <summary>
    /// Исковое зявление
    /// </summary>
    public class LawsuitOwnerRepresentative : BaseGkhEntity
    {
        public virtual LawsuitOwnerInfo Rloi { get; set; }

        public virtual RepresentativeType RepresentativeType { get; set; }

        public virtual string Surname { get; set; }

        public virtual string FirstName { get; set; }

        public virtual string Patronymic { get; set; }

        public virtual DateTime BirthDate { get; set; }

        public virtual string BirthPlace { get; set; }

        public virtual string LivePlace { get; set; }

        public virtual string Note { get; set; }

    }
}