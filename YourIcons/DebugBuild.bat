@echo off

rem ����һ���޸ı����sln��Url
set buildSlnUrl=".\YourIcons.sln"

@echo ������ʼ�������Ŀ�ļ�(%buildSlnUrl%)����ʼ�밴���������...
@pause >nul

C:\WINDOWS\Microsoft.NET\Framework\v4.0.30319\MSBuild.exe %buildSlnUrl% /t:Build /p:Configuration=Debug;TargetFrameworkVersion=v4.5
PAUSE