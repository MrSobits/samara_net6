namespace Bars.GkhGji.Entities.Base;

using Bars.B4.DataAccess;

/// <summary>
/// <see cref="IEntity"/> с полем "Наименование"
/// </summary>
public interface IEntityWithName: IEntity
{
    /// <summary>
    /// Наименование
    /// </summary>
    string Name { get; set; }
}