### How to run use this  

Clone Repo
Update connection strings in the appSettings.json file and Docker-Compose.yaml

Open solution folder in terminal.
Run the following commands:

``
dotnet ef migrations add "Initial Create"

dotnet ef database update
``
