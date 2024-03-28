namespace Bars.B4.Modules.Analytics.Reports.Domain
{
    using System.Linq;

    /// <summary>
    /// 
    /// </summary>
    public interface ICodedReportService
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        IQueryable<ICodedReport> GetAll();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        ICodedReport Get(string key);
    }
}
