
set msbuild=C:\Program Files (x86)\MSBuild\14.0\Bin\MSBuild.exe

"%msbuild%" "NArrange.sln" /t:Build /p:Configuration=Release /nr:false

cd bin\Release
call ZipTool.exe
cd ..\..\