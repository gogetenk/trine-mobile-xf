#!/usr/bin/env bash

echo "=> prebuild ${APPCENTER_BRANCH}"

if [ "$APPCENTER_BRANCH" == "master" ]
then
  sed -i .bak 's/app-assistance-dev.azurewebsites.net/app-assistance.azurewebsites.net/g' ./src/Bll/Trine.Mobile.Bll.Impl/Settings/AppSettings.cs
  
  echo "=> Définition des variables de l'application\n"
  sed -i .bak 's/android=69a27482-869f-4f32-8532-0ab77337dfc4;ios=805f888f-e673-4bfa-a1f6-78ab376c7bc5/android=9cfc99dc-15cc-4652-b794-44df21413075;ios=8a841e14-34c8-4774-b034-c8ed5991f943/g' ./src/Bootstrapper/App.xaml.cs
  # UI Tests
  sed -i .bak 's/io.trine.trineapp.dev/io.trine.trineapp/g' ./test/Trine.Mobile.UITests/AppInitializer.cs
  sed -i .bak 's/com.hellotrine.app.dev/com.hellotrine.app/g' ./test/Trine.Mobile.UITests/AppInitializer.cs
  sed -i .bak 's/s/ytocreau@trine.com/testconsultant@trine.com/g' ./test/Trine.Mobile.UITests/Consultant/HomeViewTests.cs
  sed -i .bak 's/ytocreau@trine.com/testconsultant@trine.com/g' ./src/modules/Modules.Authentication/ViewModels/LoginViewModel.cs

  echo "=> Changement du bundle id\n"
  sed -i .bak 's/io.trine.trineapp.dev/io.trine.trineapp/g' ./src/native/Android/Properties/AndroidManifest.xml
  sed -i .bak 's/com.hellotrine.app.dev/com.hellotrine.app/g' ./src/native/iOS/Info.plist
  sed -i .bak 's/Trine Dev/Trine/g' ./src/native/iOS/Info.plist
fi