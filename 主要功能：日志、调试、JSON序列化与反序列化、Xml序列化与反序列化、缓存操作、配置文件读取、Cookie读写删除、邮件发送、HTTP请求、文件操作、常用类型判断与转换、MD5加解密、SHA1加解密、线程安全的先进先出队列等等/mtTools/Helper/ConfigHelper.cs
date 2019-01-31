using System.Configuration;
using mtTools.Models;

namespace mtTools
{
    /// <summary>
    /// 获取配置文件（App.config或Web.config）中AppSetting配置项值或自定义节点的配置信息
    /// </summary>
    public static class ConfigHelper
    {

        /// <summary>
        /// 获取配置文件（App.config或Web.config）中，AppSetting中的值
        /// </summary>
        /// <param name="settingKey">key值</param>
        /// <param name="defaultVal">如果key值不存在，或为空，返回的默认值</param>
        /// <returns></returns>
        public static string GetAppSettingValue(string settingKey, string defaultVal = "")
        {
            try
            {
                string val = ConfigurationManager.AppSettings[settingKey].ToString();
                if (string.IsNullOrEmpty(val))
                    return defaultVal;
                else
                    return val;
            }
            catch
            {
                return defaultVal;
            }
        }

        /// <summary>
        /// 获取配置文件中自定义节点的配置信息，只支持key-value集合
        /// 节点不存在亦返回集合对象不会为null，key不存在亦可以取值返回value为null
        /// </summary>
        /// <param name="nodeName">自定义根节点名称</param>
        /// <returns></returns>
        public static CustomNodeSection GetCustomNodeSetting(string nodeName)
        {
            return CustomNodeCache.Sections[nodeName];
        }

    }
}
