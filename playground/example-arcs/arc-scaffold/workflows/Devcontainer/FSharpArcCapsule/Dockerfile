#FROM rocker/r-ver:3.4.4
FROM mcr.microsoft.com/dotnet/sdk:6.0
#FROM r-base
# Copy endpoint specific user settings into container to specify
# .NET Core should be used as the runtime.
COPY *.devcontainer/settings.vscode.json /root/.vscode-remote/data/Machine/settings.json

# Install git, process tools
RUN apt-get update && apt-get -y install git procps

