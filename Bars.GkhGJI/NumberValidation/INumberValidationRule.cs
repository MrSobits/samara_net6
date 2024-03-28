namespace Bars.GkhGji.NumberValidation
{
    using Bars.B4.Modules.States;
    using Bars.GkhGji.Entities;

    /// <summary>
    /// Интерфейс проставлении номера документа гжи в соответствии с правилами региона
    /// </summary>
    public interface INumberValidationRule
    {
        ValidateResult Validate(DocumentGji document);

        string Id { get; }

        string Name { get; }
    }
}