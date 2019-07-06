#!/usr/bin/env bash

# Post Build Script ANDROID

set -e # Exit immediately if a command exits with a non-zero status (failure)

echo "**************************************************************************************************"
echo "Post Build Script"
echo "**************************************************************************************************"

echo "list files in APPCENTER_SOURCE_DIRECTORY"
nuget locals all -list
nuget /Users/vsts/.nuget/packages/xamarin.uitest/

##################################################
# Start UI Tests
##################################################

# variables
appCenterLoginApiToken=$AppCenterLoginForAutomatedUITests # this comes from the build environment variables
appName="Trine-App/Trine-Android"
deviceSetName="b82043e1"
testSeriesName="ui-tests"

echo ""
echo "Start Xamarin.UITest run"
echo "   App Name: $appName"
echo " Device Set: $deviceSetName"
echo "Test Series: $testSeriesName"
echo ""

echo "> Run UI test command"
# Note: must put a space after each parameter/value pair 
appcenter test run uitest --app $appName --devices $deviceSetName --app-path $APPCENTER_OUTPUT_DIRECTORY/io.trine.trineapp-Signed.apk --test-series $testSeriesName --locale "fr_FR" --build-dir $APPCENTER_SOURCE_DIRECTORY/src/modules/Modules.Authentication.UITests/bin/Release --uitest-tools-dir /Users/vsts/.nuget/packages/xamarin.uitest/3.0.0/tools --token $appCenterLoginApiToken 

echo ""
echo "**************************************************************************************************"
echo "Post Build Script complete"
echo "**************************************************************************************************"