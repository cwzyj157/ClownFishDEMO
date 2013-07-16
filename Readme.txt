 运行ClownFishDEMO的一些注意事项
====================================

1. 请在运行示例前，检查每个项目的config文件中的连接字符串，不要打开项目就立刻运行！

2. 示例程序所需的MyNorthwind数据库的下载地址：http://files.cnblogs.com/fish-li/MyNorthwind.7z

3. 如果要运行MySql示例，请务必先配置好MySql，安装好mysql-connector-net，否则不要去试了，肯定不会成功的！
   MySql所需的数据库脚本放在 db\MyNorthwind_MySql_Dump.sql，请手工运行。

4. 如果您对IIS的配置不熟悉，请直接用Visual Studio打开项目，然后运行示例。

5. 示例项目采用Visual Studio 2008开发，如果您使用Visual Studio 2010打开，请选择【不要升级】，不要急于点击确定按钮！
   我没试过使用Visual Studio 2010打开项目，所以我不保证能兼容。



 ClownFish文件及工具说明
====================================
1. ClownFish类库以及工具放在 Tools 目录中。
2. ClownFish-API-Documentation.chm：是ClownFish类库的API文档。
3. ClownFishGenerator.exe：是ClownFish专用的代码生成器。
4. ClownFishProfilerLib.dll：用于发送数据访问消息给ClownFishSQLProfiler.exe。
5. ClownFishSQLProfiler.exe：是ClownFish专用的Profiler工具。
6. XmlCommandTool.exe：是ClownFish专用的XmlCommand管理工具。
7. ICSharpCode.TextEditor.dll：ClownFishGenerator.exe，XmlCommandTool.exe都依赖的一个类库。


 对.NET版本的支持
====================================
ClownFish本身是基于.NET 3.5开发的。目前可以支持 .NET 2.0, 3.0, 3.5, 4.0 版本。
注意：在.NET 3.5以下版本中使用ClownFish，需要引用System.Core.dll 3.5版本
