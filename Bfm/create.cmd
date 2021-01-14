md ..\Bfm.Diet.Core
cd ..\Bfm.Diet.Core
dotnet new classlib 

md ..\Bfm.Diet.Authorization
cd ..\Bfm.Diet.Authorization
dotnet new classlib 

md ..\Bfm.Diet.Dto
cd ..\Bfm.Diet.Dto
dotnet new classlib  

md ..\Bfm.Diet.Service
cd ..\Bfm.Diet.Service
dotnet new classlib 

md ..\Bfm.Diet.Web.Api
cd ..\Bfm.Diet.Web.Api
dotnet new webapi

cd ..
dotnet new sln 
dotnet sln add Bfm.Diet.Core/Bfm.Diet.Core.csproj 
dotnet sln add Bfm.Diet.Library/Bfm.Diet.Library.csproj 
dotnet sln add Bfm.Diet.Authorization/Bfm.Diet.Authorization.csproj 
dotnet sln add Bfm.Diet.Dto/Bfm.Diet.Dto.csproj 
dotnet sln add Bfm.Diet.Service/Bfm.Diet.Service.csproj
dotnet sln add Bfm.Diet.Service/Bfm.Diet.Web.Api.csproj