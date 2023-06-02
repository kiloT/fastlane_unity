
# fastlane_unity

There is a bunch of scripts for unity project auto build and upload to Google Play and App Store.

Requirements:
 - install https://brew.sh
 - make sure your unity project builds without errors

Befor project setup run commands below:

    xcode-select --install
    brew install fastlane
    gem install dotenv

Thewn go to the project folder and run commands below:

    fastlane init 
    fastlane add_plugin unity
    fastlane add_plugin file_manager

Google Play release process:


App Store release process:

