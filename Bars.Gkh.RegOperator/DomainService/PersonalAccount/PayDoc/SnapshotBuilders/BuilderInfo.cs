namespace Bars.Gkh.RegOperator.DomainService.PersonalAccount.PayDoc.SnapshotBuilders
{
    using System.Collections.Generic;
    using System.Linq;

    internal class BuilderInfo : IBuilderInfo
    {
        public string Code { get; }

        public string Name { get; }

        public string Description { get; }

        public IEnumerable<IBuilderInfo> GetChildren()
        {
            return Enumerable.Empty<IBuilderInfo>();
        }

        public BuilderInfo(string code, string name, string description)
        {
            this.Code = code;
            this.Name = name;
            this.Description = description;
        }
    }
}
