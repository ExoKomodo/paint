#! /bin/bash

PROJECT=$1
shift
CONFIGURATION=$1
shift

dotnet restore ${PROJECT}
dotnet build --no-restore --configuration ${CONFIGURATION} ${PROJECT}
