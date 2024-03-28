namespace Bars.GkhGji.Regions.Tatarstan.Entities.Resolution
{
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Enums;

    public class TatarstanResolutionPayFine : ResolutionPayFine
    {
        /// <summary>
        /// Способ поступления
        /// </summary>
        public virtual AdmissionType AdmissionType {get;set;} 
    }
}