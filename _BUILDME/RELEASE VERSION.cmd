
set msbuild=C:\Program Files (x86)\MSBuild\12.0\Bin\MSBuild.exe

"%msbuild%" "..\NArrange.sln" /t:Build /p:Configuration=Release /nr:false