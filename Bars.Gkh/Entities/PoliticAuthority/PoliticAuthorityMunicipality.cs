namespace Bars.Gkh.Entities
{
    /// <summary>
    /// Органы государственной власти мун образований
    /// </summary>
    public class PoliticAuthorityMunicipality : BaseGkhEntity
    {
        /// <summary>
        /// Орган государственнной власти
        /// </summary>
        public virtual PoliticAuthority PoliticAuthority { get; set; }

        /// <summary>
        /// Муниципальное образование
        /// </summary>
        public virtual Municipality Municipality { get; set; }
    }
}
