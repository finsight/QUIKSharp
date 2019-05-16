del ..\artifacts\*.nupkg

dotnet restore ..\src\QuikSharp
dotnet pack ..\src\QuikSharp -c Release -o ..\artifacts -p:AutoSuffix=False

pause