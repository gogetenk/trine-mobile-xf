#!/usr/bin/env bash
###
# ANDROID SPECIFIC, DON'T USE THAT FOR IOS BUILDS
###

echo "=> prebuild ios ${APPCENTER_BRANCH}"

if [ "$APPCENTER_BRANCH" == "master" ]
then
  sed -i .bak 's/app-assistance-dev.azurewebsites.net/app-assistance.azurewebsites.net/g' ../../Bll/Trine.Mobile.Bll.Impl/Settings/AppSettings.cs
  sed -i .bak 's/ee39ba65bc0171ea932b98e05acab1f2/2be1819819cfe4eda2b908b5bba59b73/g' ./CustomVariables.cs
  sed -i .bak 's/nb4w5hji/v4l26lv4/g' ./CustomVariables.cs
 
  echo "=> Définition des variables de l'application\n"
  sed -i .bak 's/android=69a27482-869f-4f32-8532-0ab77337dfc4;ios=805f888f-e673-4bfa-a1f6-78ab376c7bc5/android=9cfc99dc-15cc-4652-b794-44df21413075;ios=8a841e14-34c8-4774-b034-c8ed5991f943/g' ../../Bootstrapper/App.xaml.cs
  # UI Tests
  sed -i .bak 's/io.trine.trineapp.dev/io.trine.trineapp/g' ../../../test/Trine.Mobile.UITests/AppInitializer.cs
  sed -i .bak 's/com.hellotrine.app.dev/com.hellotrine.app/g' ../../../test/Trine.Mobile.UITests/AppInitializer.cs
  sed -i .bak 's/ytocreau@trine.com/uitests-consultant@trine.com/g' ../../../test/Trine.Mobile.UITests/Consultant/HomeViewTests.cs
  sed -i .bak 's/remiroycourt@trine.com/uitests-customer@trine.com/g' ../../../test/Trine.Mobile.UITests/Customer/HomeViewTests.cs
  sed -i .bak 's/ytocreau@trine.com/uitests-consultant@trine.com/g' ../../../src/modules/Modules.Authentication/ViewModels/LoginViewModel.cs

  echo "=> Changement du bundle id\n"
  sed -i .bak 's/io.trine.trineapp.dev/io.trine.trineapp/g' ./Properties/AndroidManifest.xml
fi
