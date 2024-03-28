namespace Bars.GkhGji.Regions.Tomsk.Enums
{
    using Bars.B4.Utils;

    public enum TypeResolutionPetitions
    {
        [Display("отводов, ходатайство делу не имеет(ют)")]
        HasNotPetitions = 10,

        [Display("Ходатайствует(ют) о:")]
        HasPetitions = 20
    }
}