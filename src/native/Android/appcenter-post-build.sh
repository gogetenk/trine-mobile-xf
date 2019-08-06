#!/usr/bin/env bash
#ANDROID ONLY

set -e # Exit immediately if a command exits with a non-zero status (failure)

    echo "Post Build Script Started"
	    echo "**** Unit Tests ****"
			echo "Found Unit Test projects:"
				find $APPCENTER_SOURCE_DIRECTORY -regex '.*UnitTests.*\.csproj' -exec echo {} \;
			echo
			echo "Run Unit Test projects"
				find $APPCENTER_SOURCE_DIRECTORY -regex '.*UnitTests.*\.csproj' -exec dotnet test {} \; 