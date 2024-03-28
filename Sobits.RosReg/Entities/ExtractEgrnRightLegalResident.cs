namespace Sobits.RosReg.Entities
{
    public class ExtractEgrnRightLegalResident : ExtractEgrnRightLegal
    {
        /// <summary>
        /// ОГРН
        /// </summary>
        public virtual long Ogrn { get; set; }

    }
}