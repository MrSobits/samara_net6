namespace Bars.GkhGji.Entities.Email
{
    using Bars.B4.DataAccess;

    public class EmailGjiLongText : BaseEntity
    {
        public virtual EmailGji EmailGji { get; set; }

        public virtual byte[] Content { get; set; }
    }
}
