namespace Bars.Gkh.Entities
{
    public class UserExternal : BaseGkhEntity
    {
        /// <summary>
        /// Id пользователя в новой 
        /// </summary>
        public virtual long UserId { get; set; }

        public virtual long UserPasswordId { get; set; }
    }
}