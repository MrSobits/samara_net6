namespace Bars.Gkh.Navigation
{
    using System.Collections.Generic;
    using Newtonsoft.Json;

    public class TreeMenuItem
    {
        public TreeMenuItem(string name, string iconCls, string controllerName)
        {
            options = new Dictionary<string, object>();
            Children = new List<TreeMenuItem>();
            Text = name;
            ControllerName = controllerName;
            IconCls = iconCls;
            Expanded = true;
        }

        public TreeMenuItem(string name, string iconCls)
            : this(name, iconCls, string.Empty)
        {
        }

        [JsonProperty("text")]
        public string Text { get; set; }

        [JsonProperty("moduleScript")]
        public string ControllerName { get; set; }

        [JsonProperty("iconCls")]
        public string IconCls { get; set; }

        [JsonProperty("expanded")]
        public bool Expanded { get; set; }

        [JsonProperty("options")]
        public Dictionary<string, object> options { get; set; }

        [JsonProperty("children")]
        public IList<TreeMenuItem> Children { get; set; }

        /// <summary>
        /// Флаг: есть ли подменю у данного меню
        /// </summary>
        [JsonProperty("leaf")]
        public bool Leaf
        {
            get
            {
                return this.Children == null || this.Children.Count == 0;
            }
        }

        public void Add(TreeMenuItem item)
        {
            Children.Add(item);
        }

        public void AddParams(string key, object value)
        {
            options.Add(key, value);
        }

        public void Add(string name, string iconCls, string controllerName)
        {
            Children.Add(new TreeMenuItem(name, iconCls, controllerName));
        }

        public void Add(string name, string iconCls)
        {
            Add(name, iconCls, string.Empty);
        }
    }
}