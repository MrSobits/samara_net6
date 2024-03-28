namespace Bars.Gkh.DomainService
{
    using Entities;
    using System.Collections.Generic;

    public class FieldRequirementMap : IFieldRequirementSource
    {
        private readonly Dictionary<string, FieldRequirementInfo> _requirements = new Dictionary<string, FieldRequirementInfo>();

        public Dictionary<string, FieldRequirementInfo> GetFieldRequirements()
        {
            return _requirements;
        }

        public void Requirement(string requirementId, string description)
        {
            AddItem(requirementId, description);
        }

        public void Namespace(string nsId, string description)
        {
            AddItem(nsId, description, true);
        }

        private void AddItem(string requirementId, string description, bool isNamespace = false)
        {
            var fri = new FieldRequirementInfo { RequirementId = requirementId, Description = description, IsNamespace = isNamespace };
            if (IsValidToMap(fri))
            {
                _requirements.Add(requirementId, fri);
            }           
        }

        private bool IsValidToMap(FieldRequirementInfo fri)
        {
            if (_requirements.ContainsKey(fri.RequirementId))
            {
                return false;
            }

            return true;
        }
    }
}
