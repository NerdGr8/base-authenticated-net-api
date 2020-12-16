### How to run this 

Update connection strings in the appSettings.json file. 

open folder in terminal.
Run the following commands:

``
dotnet ef migrations add "Initial Create" 
dotnet ef database update
``