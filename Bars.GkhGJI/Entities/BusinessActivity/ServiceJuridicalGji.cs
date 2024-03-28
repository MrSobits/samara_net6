namespace Bars.GkhGji.Entities
{
    using Bars.Gkh.Entities;

    /// <summary>
    /// Услуги оказываемые юридическим лицом
    /// </summary>
    public class ServiceJuridicalGji : BaseGkhEntity 
    {
        /// <summary>
        /// Уведомление о начале предпринимательской деятельности
        /// </summary>
        public virtual BusinessActivity BusinessActivityNotif { get; set; }

        /// <summary>
        /// Вид работ уведомлений
        /// </summary>
        public virtual KindWorkNotifGji KindWorkNotif { get; set; }
    }
}