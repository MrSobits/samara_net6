namespace Bars.Gkh.Overhaul.Hmao.PriorityParams
{
    using Entities;

    public interface IPriorityParams
    {
        string Id { get; }

        string Name { get; }

        TypeParam TypeParam { get; }

        object GetValue(IStage3Entity obj);
    }
}