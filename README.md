# V1 Kanban Board
https://trello.com/b/uXNN68AP/v1-tasks
# Feasibility Study
cd BrunelUni.IntelliFarm.Data.Feasibility && dotnet run /batch <poly_multiplier> <coverage_inc> <poly_steps> <coverage_steps> <base_path> <poly_start> <coverage_start>


### Example Study Without Batch:
- edit appsettings.json to match format:
   - appsettings.example.json
- dotnet run

### Example With Batch:
- edit appsettings.json to match format:
    - appsettings.example.json
- dotnet run /batch /batch 4 25 5 3 C:\Users\roboc\TestScenes 80 100
   - which overrides files in app configuration via env vars