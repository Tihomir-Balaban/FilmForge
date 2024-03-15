# FilmForge

**Requirements:**
- Visual Studio 2022
- Microsoft SQL Server Manager (version >= 19.x.x.x)
- Postman

**Setup:**
1. Restore nuget
2. Build Solution
3. ConnectionString, JwtSettings, and Serilog settings should be configured in appsettings.json:
	- ConnectionString should be edited to meet your local db params
	- In JwtSettings SecretKey needs to get a string of random characters
	- In Serilog the MinimumLevel should be changed to ones needs (Informations level recomended)
4. Package Manager Console in VS should be set to FilmForge.Entities in the "Default project:" dropdown menu
5. In Package Manager Console run: Update-Database
