namespace Bars.Gkh.DomainService
{
    using System.Collections.Generic;
    using Entities;

    public interface IFieldRequirementSource
    {
        Dictionary<string, FieldRequirementInfo> GetFieldRequirements();
    }
}