namespace Bars.Gkh.Domain.RoleFilterRestriction.Impl
{
    /// <summary>
    /// Имя роли "Поставщика услуг"
    /// </summary>
    public class ProviderServiceNoServiceFilterRoleName : INoServiceFilterRole
    {
        public string RoleName
        {
            get
            {
                return "Поставщик услуг";
            }
        }
    }
}
