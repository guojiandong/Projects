using System.Collections.Generic;
using System.Linq;

namespace mtTools.Models
{
    /// <summary>
    /// 所有节点对象管理
    /// </summary>
    public class CustomNodeCache
    {

        /// <summary>
        /// 禁用构造方法
        /// </summary>
        private CustomNodeCache() { }

        /// <summary>
        /// 新实例化节点对象锁
        /// </summary>
        private static readonly object lockObj = new object();

        /// <summary>
        /// 单例模式
        /// </summary>
        private static readonly CustomNodeCache cnc = new CustomNodeCache();

        /// <summary>
        /// 快捷访问
        /// </summary>
        public static CustomNodeCache Sections { get { return cnc; } }

        /// <summary>
        /// 已实例化节点对象列表
        /// </summary>
        private static readonly List<CustomNodeSection> list = new List<CustomNodeSection>();

        /// <summary>
        /// 获取节点对象
        /// </summary>
        /// <param name="nodeName">节点名称</param>
        /// <returns></returns>
        public CustomNodeSection this[string nodeName]
        {
            get
            {
                var section = list.FirstOrDefault(p => p.NodeName == nodeName);
                if (section == null)
                {
                    lock (lockObj)
                    {
                        section = list.FirstOrDefault(p => p.NodeName == nodeName);
                        if (section == null)
                        {
                            section = new CustomNodeSection(nodeName);
                            list.Add(section);
                        }
                    }
                }
                return section;
            }
        }

    }
}
