namespace Bars.Gkh.Overhaul.Nso.PriorityParams
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