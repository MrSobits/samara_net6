namespace Bars.Gkh.Integration
{
    using Bars.B4.Utils;

    public interface IIntegrationService
    {
        bool LoadData(DynamicDictionary dict, out string message);
    }
}
