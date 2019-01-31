using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace SpyTool
{
    [Serializable]
    public enum Statu
    {
        Picture,
        Text,
    }

    [Serializable]
    [XmlInclude(typeof(PicType))]
    [XmlInclude(typeof(TextType))]
    public abstract class ControlType
    {
        public string Name { get; set; }

        public string Status { get; set; }

        public bool IsIngnore { get; set; }

        [XmlIgnore]
        public IntPtr ParentHwnd { get; set; }

        [XmlIgnore]
        public IntPtr Hwnd { get; set; }
    }

    [XmlInclude(typeof(PicType))]
    public class PicType : ControlType
    {
        [XmlIgnore]
        public Image Image { get; set; }

        public string Color { get; set; }
    }

    [XmlInclude(typeof(TextType))]
    public class TextType : ControlType
    {
    }

    [Serializable]
    public class ColorAttributeList
    {
        public List<ControlType> CheckedList { get; set; }
        public List<ControlType> IgnoredList { get; set; }
    }

}
