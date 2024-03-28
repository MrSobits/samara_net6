namespace Bars.Gkh.RegOperator.Sources
{
    /// <summary>
    /// Источник
    /// </summary>
    public class SourceInfo
    {
        /// <summary>
        /// Описание разрешения
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Является ли источник пространством имен
        /// </summary>
        public bool IsNamespace { get; set; }

        /// <summary>
        /// Полный идентификатор разрешения
        /// </summary>
        public string PermissionId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Description"></param>
        /// <param name="PermissionId"></param>
        /// <param name="IsNamespace"></param>
        public SourceInfo(string Description, string PermissionId, bool IsNamespace)
        {
            this.Description = Description;
            this.PermissionId = PermissionId;
            this.IsNamespace = IsNamespace;
        }
    }
}
