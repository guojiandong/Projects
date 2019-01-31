using System.Collections.Generic;
using System.Collections.Specialized;
using System.Web.Configuration;

namespace mtTools.Models
{
    /// <summary>
    /// 节点对象
    /// </summary>
    public class CustomNodeSection
    {

        /// <summary>
        /// 节点根目录名称
        /// </summary>
        public string NodeName { get; set; }

        /// <summary>
        /// 缓存的节点key-value键值对
        /// </summary>
        public Dictionary<string, string> kv = new Dictionary<string, string>();

        /// <summary>
        /// 初始化并缓存其数据
        /// </summary>
        /// <param name="nodeName"></param>
        public CustomNodeSection(string nodeName)
        {
            NodeName = nodeName;

            var coll = (NameValueCollection)WebConfigurationManager.GetSection(nodeName);
            if (coll != null && coll.Count > 0)
            {
                foreach (string item in coll.AllKeys)
                {
                    kv.Add(item, coll[item]);
                }
            }
        }

        /// <summary>
        /// 获取指定key的值，key不存在则为null
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public string this[string key]
        {
            get
            {
                if (!kv.ContainsKey(key))
                    return null;
                else
                    return kv[key];
            }
        }

    }
}
