# mtTools

# 简介
mtTools为C#基础类库，无任何第三方依赖。由本人为国内一知名企业设计的内部通用公共类库中提取的一部分常用工能所组成。主要功能已经经过使用验证。

<span style="color:#0000ee;">如仅需dll文件请直接于附件中下载即可。<span>

# 主要功能
主要功能：日志、调试、JSON序列化与反序列化、Xml序列化与反序列化、缓存操作、配置文件读取、Cookie读写删除、邮件发送、HTTP请求、文件操作、常用类型判断与转换、MD5加解密、SHA1加解密、线程安全的先进先出队列等等。
![mtTools内容图示](https://gitee.com/uploads/images/2018/0525/073146_40043b2e_1683949.png "mtTools.png")

# 问题反馈
使用中有任何BUG，欢迎反馈给我，请发往邮箱：mkwuji@yeah.net，本人不能保证及时看到并处理。

# 不接受捐赠
此项目不接受捐赠，更不接受捐赠为名的新功能增加定制等需要。此项目中功能久经验证，可放心使用。为保证后期可能的性能、使用升级，功能不会乱动，并尽量保证兼容。

# 开源说明
可直接引用、或移植部分代码，引用或移植后可闭包、可商用，可以以本人为名进行宣传。保留版本信息可利于后期代码升级及查询兼容性。


# 使用示例
此项目仅为公共类库，后续项目之辅助，功能简明易懂，代码调用示例位于/Demos v1.0.0.0/Demo.cs中，有兴趣可下载研究，文后仅稍作功能使用展示。

-日志操作类
```
  <!--日志配置可不配置，未配置时此为默认值-->
  <configSections>
    ... ...
    <section name="toolLog" type="System.Configuration.NameValueSectionHandler"/>
  </configSections>
  <!--LogHelper配置-->
  <toolLog>
    <!--日志记录路径：绝对地址D:\Logs\或应用程序下目录Logs/,留空为应用程序根目录下Logs文件夹-->
    <add key="LogPath" value="" />
    <!--日志内容格式：@Now-日期;@Message-消息内容-->
    <add key="LogMessage" value="【@Now】@Message" />
  </toolLog>

    for(var i = 0;i<100;i++)
    {//日志按调用写入的先后顺序有序写入
        LogHelper.Logs["Test"].WriteLine("Line:" + i.ToString() + ".");//Test/yyyyMMdd.txt

        LogHelper.Logs["Test2", "Test53"].WriteLine("Line:" + i.ToString() + ".");//Test2/Test53.txt
        LogHelper.Logs["Test2", "Test54"].WriteLine("Line:" + i.ToString() + ".");//Test2/Test54.txt

        LogHelper.Logs["Test" + DateTime.Now.ToString("yyyyMMdd"), "Test53"].WriteLine("Line:" + i.ToString() + ".");//TestyyyyMMdd/Test53.txt
        LogHelper.Logs["Test" + DateTime.Now.ToString("yyyyMMdd"), "Test54"].WriteLine("Line:" + i.ToString() + ".");//TestyyyyMMdd/Test54.txt

        LogHelper.Logs["Test3", "Test2/roomid54"].WriteLine("Line:" + i.ToString() + ".");//Test3/Test2/roomid54.txt
        LogHelper.Logs["Test4", "Test2\\roomid54"].WriteLine("Line:" + i.ToString() + ".");//Test4/Test2/roomid54.txt
    }
```
-配置读取类
```
  <configSections>
    ... ...
    <section name="toolDebug" type="System.Configuration.NameValueSectionHandler"/>
  </configSections>
  <!--DebugHelper配置-->
  <toolDebug>
    <!--是否输出调试信息-->
    <add key="IsDebug" value="true"/>
    <!--调试信息内容格式：@Now-日期;@CodeLocal-出错位置;@Message-消息内容-->
    <add key="DebugMessage" value="【@Now】【@CodeLocal】@Message"/>
  </toolDebug>

    var cf = ConfigHelper.GetCustomNodeSetting("toolDebug");
    var isDebug = cf["IsDebug"];
    var isDebug2 = cf["tIsDebug"];//tIsDebug不存在：值为null
    var cf2 = ConfigHelper.GetCustomNodeSetting("toolDebug");//thuanrongDebug不存在：值为CustomNodeSection，只是其中kv.Count==0
    var tDebug = cf2["tDebug"];//tDebug不存在：值为null
```
