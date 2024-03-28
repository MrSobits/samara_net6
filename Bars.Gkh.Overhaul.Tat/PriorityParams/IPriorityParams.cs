namespace Bars.Gkh.Overhaul.Tat.PriorityParams
{
    using Entities;

    public interface IPriorityParams
    {
        string Id { get; }

        string Name { get; }

        TypeParam TypeParam { get; }

        object GetValue(RealityObjectStructuralElementInProgrammStage3 obj);
    }
}