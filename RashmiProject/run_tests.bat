@echo off
echo Running SpecFlow tests in order...

cd /d "C:\Users\sakum\source\repos\RashmiProject"


dotnet test --filter TestCategory=Order1
dotnet test --filter TestCategory=Order2
dotnet test --filter TestCategory=Order3
dotnet test --filter TestCategory=Order4

echo All tests executed successfully!
pause
