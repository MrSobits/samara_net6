namespace Bars.GkhGji.Contracts.Meta
{
    using Bars.B4.Utils;

    public class InspectingPart
    {
        [Display("Инспектируемая часть")]
        public string Name { get; set; }

        [Display("Характер и местоположение")]
        public string CharacterAndLocation { get; set; }

        [Display("Описание")]
        public string Description { get; set; }
    }
}
