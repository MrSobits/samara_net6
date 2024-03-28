namespace Bars.GisIntegration.UI.ViewModel.Task
{
    using System.Collections.Generic;

    using Newtonsoft.Json;

    /// <summary>
    /// Базовый узел дерева
    /// </summary>
    public class BaseNode: ITreeNode
    {
        /// <summary>
        /// Узел является "листом"
        /// </summary>
        [JsonProperty("leaf")]
        public bool Leaf { get; set; }

        /// <summary>
        /// Дочерние узлы
        /// </summary>
        [JsonProperty("children", NullValueHandling = NullValueHandling.Ignore)]
        public List<ITreeNode> Children { get; set; }

        /// <summary>
        /// Cls иконки узла
        /// </summary>
        [JsonProperty("iconCls")]
        public string IconCls { get; set; }
    }
}
