#!/usr/bin/env bash
###
# ANDROID SPECIFIC, DON'T USE THAT FOR iOS BUILDS
###

if [ "$APPCENTER_BRANCH" == "master" ];
  sed -i .bak 's/app-assistance-dev.azurewebsites.net/app-assistance.azurewebsites.net/g' ../../Bll/Trine.Mobile.Bll.Impl/Settings/AppSettings.cs
  cat ../../Bll/Trine.Mobile.Bll.Impl/Settings/AppSettings.cs
then

fi
