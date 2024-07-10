using System;

namespace Bars.Gkh.RegOperator.Utils;

/// <summary>
/// Атрибут для указания названия колонки для файлов dbf
/// </summary>
[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
public class DbfColumnAttribute : Attribute
{
    /// <summary>
    /// Название колонки
    /// </summary>
    public readonly string Name;

    public DbfColumnAttribute(string name) => Name = name;
}