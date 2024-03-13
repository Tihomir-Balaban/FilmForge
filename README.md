# FilmForge
 
Restore nuget
Build Solution
ConnectionString, JwtSettings, and Serilog settings should be configured in appsettings.json:
	- ConnectionString should be edited to meet your local db params
	- In JwtSettings SecretKey needs to get a string of random characters
	- In Serilog the MinimumLevel should be changed to ones needs
Package Manager Console in VS should be navogated to FilmForge.Entities (cd ..\FilmForge.Entities)
In Package Manager Console run: Update-Database