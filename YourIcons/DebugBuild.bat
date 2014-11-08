@echo off

rem 在下一行修改编译的sln的Url
set buildSlnUrl=".\YourIcons.sln"

@echo 即将开始编译该项目文件(%buildSlnUrl%)，开始请按任意键继续...
@pause >nul

C:\WINDOWS\Microsoft.NET\Framework\v4.0.30319\MSBuild.exe %buildSlnUrl% /t:Build /p:Configuration=Debug;TargetFrameworkVersion=v4.5
PAUSE