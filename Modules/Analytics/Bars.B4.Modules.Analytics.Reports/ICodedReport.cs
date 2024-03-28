namespace Bars.B4.Modules.Analytics.Reports
{
    /// <summary>
    /// 
    /// </summary>
    public interface ICodedReport : IReport
    {
        /// <summary>
        /// Ключ для регистрации.
        /// </summary>
        string Key { get; }

        /// <summary>
        /// 
        /// </summary>
        string Name { get; }

        /// <summary>
        /// 
        /// </summary>
        string Description { get; }

    }
}
