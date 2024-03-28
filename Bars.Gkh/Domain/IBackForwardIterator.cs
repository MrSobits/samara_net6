namespace Bars.Gkh.Domain
{
    using Bars.B4.DataAccess;

    public interface IBackForwardIterator<T> where T : BaseEntity
    {
        T Back(T current);

        T Forward(T current);
    }
}
