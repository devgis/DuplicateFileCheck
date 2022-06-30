# DuplicateFileCheck

> Duplicate File Check 重复文件检查 本程序支持多目录扫描
> Dotnet core 支持各种Windows  Linux 系统

## 执行命令方式

> DuplicateFileCheck.exe -p C:\Temp\Dir1 C:\Temp\Dir2 -w true -l C:\Temp
> Linux 需要安装.net core 3.1,当然你可以下载.core 自己进行编译 或者运行以下命令执行我编译好的内容
> dotnet DuplicateFileCheck.dll -p /Home/User1/Dri1/ /Home/User1/Dir2/ -w true -l /Home/User1/Temp/


```
-p 后边跟的是需要扫描的路径。
-w 后边true 则输出扫描结果到日志文件。
-l 后边跟的是一个目录用于保存计算结果 如果路径不存在会自动创建。
```
