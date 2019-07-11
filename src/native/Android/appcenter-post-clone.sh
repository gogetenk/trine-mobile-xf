#!usr/bin/env bash -e
    echo "**** Post Clone Script Started ***"

echo "Found Unit Test projects:"
find $APPCENTER_SOURCE_DIRECTORY -regex '.*UnitTests.*\.csproj' -exec echo {} \;
echo
echo "Run Unit Test projects"
find $APPCENTER_SOURCE_DIRECTORY -regex '.*UnitTests.*\.csproj' -exec xargs dotnet test;
