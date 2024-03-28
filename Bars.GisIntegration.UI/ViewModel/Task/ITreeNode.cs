namespace Bars.GisIntegration.UI.ViewModel.Task
{
    using System.Collections.Generic;

    using Newtonsoft.Json;

    /// <summary>
    /// Интерфейс узла дерева
    /// </summary>
    public interface ITreeNode
    {
        /// <summary>
        /// Узел является "листом"
        /// </summary>
        [JsonProperty("leaf")]
        bool Leaf { get; set; }

        /// <summary>
        /// Дочерние узлы
        /// </summary>
        [JsonProperty("children", NullValueHandling = NullValueHandling.Ignore)]
        List<ITreeNode> Children { get; set; }

        /// <summary>
        /// Cls иконки узла
        /// </summary>
        [JsonProperty("iconCls")]
        string IconCls { get; set; }
    }
}
