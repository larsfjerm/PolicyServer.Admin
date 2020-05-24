dotnet build

if ($LastExitCode -ne 0) { return; }

Start-Process "dotnet" -ArgumentList "run --project src/IdentityServer --no-build"

sleep 1

Start-Process "dotnet" -ArgumentList "run --project src/PolicyServer.Admin --no-build"

sleep 1

Start-Process "dotnet" -ArgumentList "run --project src/Host --no-build"