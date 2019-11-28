#!/usr/bin/env bash
###
# ANDROID SPECIFIC, DON'T USE THAT FOR iOS BUILDS
###

set -e # Exit immediately if a command exits with a non-zero status (failure)

    echo "Post Build Script Started"
	    echo "**** Unit Tests ****"
			echo "Found Unit Test projects:"
				find $APPCENTER_SOURCE_DIRECTORY -regex '.*UnitTests.*\.csproj' -exec echo {} \;
			echo
			echo "Run Unit Test projects"
				find $APPCENTER_SOURCE_DIRECTORY -regex '.*UnitTests.*\.csproj' -exec dotnet test {} \; 
								
	
	# We run UI tests only on master branch
	if [ "$APPCENTER_BRANCH" == "master" ];
	then
		echo "**** UI Tests ****"
			echo "APPCENTER_XAMARIN_CONFIGURATION = " $APPCENTER_XAMARIN_CONFIGURATION 

			SolutionFile=`find "$APPCENTER_SOURCE_DIRECTORY" -name Trine.Mobile.sln`
			SolutionFileFolder=`dirname $SolutionFile`

			UITestProject=`find "$APPCENTER_SOURCE_DIRECTORY" -name Trine.Mobile.UITests.csproj`

			echo SolutionFile: $SolutionFile
			echo SolutionFileFolder: $SolutionFileFolder
			echo UITestProject: $UITestProject

			chmod -R 777 $SolutionFileFolder

			msbuild "$UITestProject" /property:Configuration=$APPCENTER_XAMARIN_CONFIGURATION

			UITestDLL=`find "$APPCENTER_SOURCE_DIRECTORY" -name "Trine.Mobile.UITests.dll" | grep bin`
			echo UITestDLL: $UITestDLL

			UITestBuildDir=`dirname $UITestDLL`
			echo UITestBuildDir: $UITestBuildDir


			appCenterLoginApiToken=$AppCenterLoginForAutomatedUITests # this comes from the build environment variables
			echo $appCenterLoginApiToken
		
			APKFile=`find "$APPCENTER_SOURCE_DIRECTORY" -name *.apk | head -1`
			echo APKFile: $APKFile

			UITestVersionNumber=`grep '[0-9]' $UITestProject | grep Xamarin.UITest|grep -o '[0-9]\{1,3\}\.[0-9]\{1,3\}\.[0-9]\{1,3\}'`
			echo UITestVersionNumber: $UITestVersionNumber

			TestCloudExe=`find ~/.nuget | grep test-cloud.exe | grep $UITestVersionNumber | head -1`
			echo TestCloudExe: $TestCloudExe

			TestCloudExeDirectory=`dirname $TestCloudExe`
			echo TestCloudExeDirectory: $TestCloudExeDirectory

			echo "Logging in..."
			appcenter login --token $appCenterLoginApiToken 
			appcenter test run uitest --app "Trine-App/Trine-Android" --devices "Trine-App/alpha-testers" --app-path $APKFile  --test-series "ui-tests" --locale "fr_FR" --build-dir $UITestBuildDir --uitest-tools-dir $TestCloudExeDirectory --async
		fi