namespace Bars.Gkh.DomainService
{
    using Bars.Gkh.Entities;
    using System.Collections.Generic;

    public interface IFieldRequirementService
    {
        IEnumerable<FieldRequirementInfo> GetAllRequirements();

        FieldRequirementInfo GetFieldRequirement(string permissionId);

        string GetFieldRequirementPath(string requirementId);
    }
}
