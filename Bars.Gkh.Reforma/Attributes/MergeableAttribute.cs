namespace Bars.Gkh.Reforma.Attributes
{
    using System;

    /// <summary>
    /// Указывает ClassMerger на необходимость "нежной" замены значений в агрессивном режиме
    /// </summary>
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    public class MergeableAttribute : Attribute
    {
    }
}