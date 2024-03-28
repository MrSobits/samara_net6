namespace Bars.Gkh.Map.Dict
{
    using Bars.Gkh.Entities.Dicts;

    public class IdentityDocumentTypeMap :  BaseGkhDictMap<IdentityDocumentType>
    {
        /// <inheritdoc />
        public IdentityDocumentTypeMap()
            : base(typeof(IdentityDocumentType).FullName, "GKH_DICT_IDENTITY_DOC_TYPE")
        {
        }

        /// <inheritdoc />
        protected override void Map()
        {
            this.Property(x => x.Description, nameof(IdentityDocumentType.Description)).Column("DESCRIPTION");
            this.Property(x => x.Regex, nameof(IdentityDocumentType.Regex)).Column("REGEX");
            this.Property(x => x.RegexErrorMessage, nameof(IdentityDocumentType.RegexErrorMessage)).Column("REGEX_ERROR_MESSAGE");
        }
    }
}
