namespace Bars.B4.Modules.Analytics.Filters
{
    /// <summary>
    /// 
    /// </summary>
    public interface IMacrosContainer
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="macros"></param>
        void Register(IMacros macros);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        IMacros Get(string key);
    }
}
