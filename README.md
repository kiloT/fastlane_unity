

# fastlane_unity

There is a bunch of scripts for unity project auto build and upload to Google Play and App Store.

Requirements:
 - install https://brew.sh
 - make sure your unity project builds without errors

Befor project setup run commands below:

    xcode-select --install
    brew install fastlane
    gem install dotenv

Then go to the project folder and paste files from *fastlane_unity* repository.

Open *.env.default* in *fastlane* folder file and edit all empty fields. If you need only one platform feel free to skip another platform fields.

*release_notes* used only for generate text for notification on release process complete. 
To change changelog text for stores change *CHANGELOG_TEXT* value in  *.env.default*.

**Google Play release process:**

*release_notes* used only for generate text for notification on release process complete.

Start new release for 1%
*version* possible values: major, minor, patch or AA.BB.CC(example 1.2.3 or 12.34.56 or 1.2.34)

```bash
fastlane android_release version:minor rollout:0.01 release_notes:"test"
```

Update rollout for 50%

```bash
fastlane android_update_rollout rollout:0.5 release_notes:"test"
```

Release rollout for all users (100%)

```bash
fastlane android_update_rollout rollout:1 release_notes:"test"
```

**App Store release process:**

App Store release now support only phased, automatic on approve release:

```bash
fastlane ios_release release_notes:"test"
```
