


# fastlane_unity

There is a bunch of scripts for unity project auto build and upload to Google Play and App Store.

Requirements:
 - MacOS
 - install https://brew.sh
 - make sure your unity project builds without errors

Befor project setup run commands below:

    xcode-select --install
    brew install fastlane
    gem install dotenv

**Preparing the project**

- Go to the project folder and paste files from *fastlane_unity* repository.
- Open *.env.default* in *fastlane* folder file and edit all empty fields. If you need only one platform feel free to skip another platform fields.

- Add to *.gitignore* files wich was added abow (optionally):

      #fastlane
      /Gemfile
      /Gemfile.lock
      [Ff]astlane/

**Google Play release process:**

***release_notes*** used only for generate text for notification on release process complete. 
***version*** possible values: major, minor, patch or AA.BB.CC(example 1.2.3 or 12.34.56 or 1.2.34)
***rollout*** can accept value from (0, 1]
To change changelog text for stores change *CHANGELOG_TEXT* value in  *.env.default*.

Start new release for 1%:

```bash
fastlane android_release version:minor rollout:0.01 release_notes:"test"
```

Update rollout for 50%:

```bash
fastlane android_update_rollout rollout:0.5 release_notes:"test"
```

Release rollout for all users (100%):

Release rollout for 100% also include merge current git brunch to MAIN_BRANCH wich configured in *.env.default*

```bash
fastlane android_update_rollout rollout:1 release_notes:"test"
```

**App Store release process:**

App Store release now support only phased, automatic on approve release:

```bash
fastlane ios_release release_notes:"test"
```
