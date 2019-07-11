
    echo "Post Build Script Started"
	    echo "**** Unit Tests ****"
			echo "Found Unit Test projects:"
				find $APPCENTER_SOURCE_DIRECTORY -regex '.*UnitTests.*\.csproj' -exec echo {} \;
			echo
			echo "Run Unit Test projects"
				find $APPCENTER_SOURCE_DIRECTORY -regex '.*UnitTests.*\.csproj' -exec dotnet test {} \;
				
