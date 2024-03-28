namespace Bars.B4.Modules.FIAS.AutoUpdater.Utils
{
    public class FiasEntityNameAttribute : System.Attribute
    {
        public string EntityName { get; set; }
        
        public string RootName { get; set; }
        
        public FiasEntityNameAttribute(string entityName, string rootName)
        {
            EntityName = entityName;
            this.RootName = rootName;
        }
    }
}