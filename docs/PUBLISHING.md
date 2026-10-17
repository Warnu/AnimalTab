# Publishing

The intended fork is https://github.com/Warnu/AnimalTab, branch `1.7`, based on the original mod's `1.5` branch. The packaged target is experimental RimWorld 1.7.

## GitHub

Push the source, tests, build scripts, metadata, and licenses. Keep generated Assemblies, bin/obj, dist, and local Workshop IDs out of Git. Run build.ps1 and package.ps1, or use the Windows CI artifact. Attach the generated ZIP to a prerelease with the CHANGELOG.md entry when ready.

## Test the standalone package

The first successful test replaced a DLL under the original package ID. This fork preserves that ID, Fluffy.AnimalTab, but the complete standalone distribution must still be tested:
1. Preserve a copy of your save and working mod list.
2. Unsubscribe from the original Animal Tab and remove other local copies, then put the staged AnimalTab-Unofficial folder in RimWorld's local Mods folder. Keep only one installation with this package ID.
3. Enable Harmony and this fork, and restart.
4. Test cows, milk/wool values, scrolling, sorting, filters, handler controls, and animal-selection shortcuts on the copied save.
5. Inspect a fresh log. A log that has reached its message cap cannot rule out later errors.
6. Check add-ons separately. Dependencies and checks for Fluffy.AnimalTab retain the original package identity, but Better Animal Info and other integrations are not yet certified for this standalone package.

## Steam

Upload the staged mod folder, not the repository. Use RimWorld's local-mod upload workflow under your Steam account to create your own item. Start with private visibility until the standalone test passes.

Use the title in About/About.xml and docs/WORKSHOP-DESCRIPTION.txt. Add Harmony as a required Workshop item, retain credits/licenses, and link original and fork sources. Review the preview and description before making the item public.

The package intentionally excludes PublishedFileId.txt and upstream's old update manifest. After upload, preserve the NEW assigned item ID in your local publishing copy for later updates. Never use upstream ID 712141500. The packaging script does not automatically copy a publishing ID into a new staging folder.

Some inherited third-party assets are credited upstream as BY-NC. This package is prepared for free, noncommercial distribution.
