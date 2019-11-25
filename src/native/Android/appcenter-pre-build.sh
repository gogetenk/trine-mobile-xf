#!/usr/bin/env bash
###
# ANDROID SPECIFIC, DON'T USE THAT FOR iOS BUILDS
###

if [ "$APPCENTER_BRANCH" == "master" ] || [ "$APPCENTER_BRANCH" == "release" ];
  sed -i .bak 's/app-assistance-dev.azurewebsites.net/app-assistance.azurewebsites.net/g' src/Bll/Trine.Mobile.Bll.Impl/Settings/AppSettings.cs
then
  
fi
