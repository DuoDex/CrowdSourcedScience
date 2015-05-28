# Contributing to Crowd Sourced Science

As a prerequisite for all contributions, you require a Github account.

## Making Changes ##

There are three main types of contributions to be made to this project: science reports, support for new experiments, and support for new planets.

### Science Reports ###

Adding a new science report is fairly simple, the procedure is as follows:

1. Decide how to word your science report. This is important, as the exact text you choose will be used in the game.
  - Science reports MUST NOT exceed 300 characters, including spaces.
2. Decide the specific biome/experiment/situation combination that you would like your report to be utilized in.
3. Construct an entry using the following template:
  
  ```
  eID: <experiment>
  <planet><situation+biome> = <message>
  ```
  Where `eID` is a string identifying the experiment being run, `planet` is self-explanatory, `situation+biome` is
  a concatination of the situation and the biome (if the experiment is biome sensitive) that the message should appear in,
  and `message` is what you want displayed when the experiment is run.  For example, an atmospheric analysis landed on the 
  launch pad might be:
  
  ```
  eID: atmosphereAnalysis
  KerbinSrfLandedLaunchPad = The Atmosphere heats up exponentially. Someone must have misread the manual again.
  ```
  [A helpful list of eID tags, planets, biomes, and situations can be found here.](EID.md)

If you plan to contribute by opening an issue, you are done!  Be sure to read the "Submitting Changes" section down below.
Otherwise, do the following:

1. Navigate to the folder containing the correct experiment.
  - If this is a biome-sensitive experiment, it should be in `CrowdSourcedScience/planets/<planet>/<biome>`
  - Otherwise, it should be `CrowdSourcedScience/planets/<planet>/default`
2. Open the `.cfg` file for the experiment/mod.
3. Find the proper `EXPERIMENT_DEFINITION` block for your `eID` and insert your new message, grouped with identical situations.  e.g. Adding the message `You feel like you could get into orbit with a motorcycle and ramp.` as an EVA report from the surface in Ike's central mountain range would yield something like this:
  
  ```
  @EXPERIMENT_DEFINITION[*]:HAS[#id[evaReport]]
  {
      %RESULTS
      {
          // Science Data is available in different situations for each
          // planetary body. Please use this framework as a guide to the
          // situational science that is available for this body. You may
          // copy-paste and remove '//' to add your entry. Please add your
          // entry just before '//LAST_ENTRY' or with similar entries.
          //IkeSrfLandedCentralMountainRange = Your science report text here.
          //IkeInSpaceLowCentralMountainRange = Your science report text here.
          //IkeInSpaceHigh = Your science report text here.

          IkeSrfLandedCentralMountainRange = It seems like a hike would be nice. What might you see from the other side of this moon? Pack plenty of air!
          IkeSrfLandedCentralMountainRange = You feel like you could get into orbit with a motorcycle and ramp.
          IkeInSpaceLowCentralMountainRange = While admiring the landscape, you notice that Duna is not moving or rotating. This could mean that Ike is tidally locked, or it could mean that you forgot your glasses.
          //LAST_ENTRY
      }
  }
  ```
4. Read the "Submitting Changes" section below.

### New Experiments ###

**NOTE**: If you are adding experiments for a particular mod, it is generally permissible to combine them into one file.
All instances of `<eID>.cfg` should instead be an identifier for the mod with `.cfg` as the prefix.  The contents of the file
should be as if you had prepared them all individually and concatenated them together, ensuring there is an empty line 
between experiments.

Adding support for a new experiment is a bit more difficult and time consuming.

1. Find the ScienceDefs.cfg file for the mod/experiment.  It should be in `GameData/<mod>/Resources/ScienceDefs.cfg`
2. Find the `EXPERIMENT_DEFINITION` you are trying to add.
  - Note down the `id` field of the block.  This is your `eID`.
  - Note the `biomeMask` and `situationMask` fields.  They should be between 0 and 63.
    - If the `situationMask` field is `0`, poke around or contact the mod author(s).  Some mods define custom science experiments and biome checking.
3. Figure out which situations combinations are feasible for both `biomeMask` and `situationMask` using information in the "Situation/Biome Masks" section later in this document.
  - If the `biomeMask` field contains a situation which is NOT in the `situationMask` field, contact the mod author(s).
4. Figure out the `form` of the situations.
  - If the situation is in both the `biomeMask` and `situationMask` fields, then the `form` will be `<situation><biome>`.
  - Otherwise, if the situation is only in the `situationMask` field, then the `form will be `<situation>`
5. Create a new file called `<eID>.cfg` containing the following:
  
  ```
  @EXPERIMENT_DEFINITION[*]:HAS[#id[<eID>]]
  {
      %RESULTS
      {
          // Science Data is available in different situations for each
          // planetary body. Please use this framework as a guide to the
          // situational science that is available for this body. You may
          // copy-paste and remove '//' to add your entry. Please add your
          // entry just before '//LAST_ENTRY' or with similar entries.
  <situations>
          //LAST_ENTRY
      }
  }
  ```
6. Replace `<eID>` with the `eID` of the experiment
7. Replace `<situations>` with each situation, one after the other grouped by `form`:
  
  ```
         //Yourplanet<form> = Your science report text here.
  ```
  e.g. with a `situationMask` of 63 and a `biomeMask` of 13, `<situations>` would be:
  
  ```
          //YourplanetInSpaceHigh = Your science report text here.
          //YourplanetInSpaceLow = Your science report text here.
          //YourplanetSrfSplashed = Your science report text here.
          //YourplanetFlyingHighBiome = Your science report text here.
          //YourplanetFlyingLowBiome = Your science report text here.
          //YourplanetSrfLandedBiome = Your science report text here.
  ```
8. Put a copy of this file into every `default` folder of every planet in the `CrowdSourcedScience/planets` folder, replacing the `Yourplanet` portion with the name of the planet.  (case-sensitive search and replace is very helpful, here)
9. If the experiment is biome-sensitive, do the same for every biome folder of every planet folder.
  - The exceptions being biome sensitivity while flying for an airless planet or biome sensitivity while in the ocean on a planet without oceans.
    - e.g. if considering Minmus and the experiment is only sensitive biomes when in the atmosphere, you should just put it in the default folder.
  - Additionally, if the experiment is biome sensitive when landed on the surface, place the line
    
    ```
            //There are additional situations listed in 'Location biomes.txt'
    ```
    in above the `situations` section of the file you place in the `CrowdSourcedScience/planets/kerbin/ksc` folder.
10. Add a copy to the `000_default` folder in `CrowdSourcedScience/planets`, removing all comments (lines prefixed with `//`)
    except the situations and `//LAST_ENTRY` line.  Add the following at the beginning of the file:
    
    ```
    // This file adds support for the '<eID>' experiment from '<mod>' by '<Author>'.
    // http://forum.kerbalspaceprogram.com/threads/<threadID>
    //
    // Additional files supporting this experiment are located throughout as <eID>.cfg.
    ```
    With `<eID>` being the `eID` of the experiment, `<mod>`, `<Author>`, and `<threadID>` are fairly self explainatory.
  - If you are adding a number of experiments from the same mod, use the following instead:
    ```
    // This file adds support for the '<mod>' by '<Author>'.
    // http://forum.kerbalspaceprogram.com/threads/<threadID>
    //
    // There are <##> experiments available. Additional files supporting this mod are
    // located throughout as <mod>.cfg.
    ```
    Where `<##>` is the number of added experiments.
11. Note the `eID`(s) and the mod it is attached to in CrowdSourcedScience/Modlist.txt (it helps us track it).
12. (Optional) Add messages for your newly added experiment as specified in the "Science Reports" section above.

#### Situation/Biome Masks ####

 **Situation** | InSpaceHigh | InSpaceLow | FlyingHigh | FlyingLow | SrfSplashed | SrfLanded 
 :-------------|:-----------:|:----------:|:----------:|:---------:|:-----------:|:---------:
   **Binary**  |    100000   |   010000   |   001000   |   000100  |    000010   |   000001  
  **Decimal**  |      32     |     16     |      8     |     4     |      2      |     1     

If you can convert the values to binary, the comparison is simple.  Otherwise, repeatedly subtract the largest value
not greater than the field until the field equals zero, noting the situation each represents. e.g. if the situationMask equals 24:
  
```
The largest value which is not greater than 24 is 16, which represents InSpaceLow.
We then subtract 16 from 24, giving 8.
The largest value which is not greater than 8 is 8, which represents FlyingHigh.
As 8 subtracted from 8 equals 0, we are done.
The situations are InSpaceLow and FlyingHigh.
```

### Adding a New Planet ###

This is, ironically, less work than adding a brand new experiment.  The procedure is as follows:

1. Copy one of the planet folders in the `CrowdSourcedScience/planets` folder and rename it to the lowercase name of your
   new planet.
  - Try to copy a planet that is superficially similar to the one you are adding.  For instance, don't copy the airless,
    oceanless Ike if you want something more like Laythe or Kerbin.
2. Remove all the biome folders except one... unless your planets has oceans, in which case leave a one biome folder for land and one for ocean.
3. In every file in the remaining folders, do a search and replace from the copied planets name to the name of your new planet. (Case-sensitive matching is suggested)
4. For each biome on the new planet, copy the apporpriate biome folder (ocean biome for an ocean biome, etc.) and do the following:
  - Rename the folder to a fully lowercase version of the new biome name.
  - Do a search and replace (again, case-sensitivity is recommended) from the old biome name to the new biome name.
5. Note the planet(s) and the mod it is attached to in CrowdSourcedScience/Modlist.txt (it helps us track it).
6. (Optional) Specialise the copied messages for your new planet as specified in the "Science Reports" section above.

## Submitting Changes ##

**By contributing, you are licensing your work under the [Creative Commons CC0 1.0 License ![][shield:CC01]][License:1.0] and are waiving all rights to it.**

(This is so that we can continue to offer CrowdSourcedScience openly under the [Creative Commons CC BY-NC-SA 4.0 License ![][shield:CCO4]][License:4.0].)

If you are contributing a small number (three or less, generally) of science reports, you may open an issue on the repository containing your contribution.
By doing so, you are still dedicating your work to the public domain under the above linked CC0 license.
To be included, your contribution MUST (if it does not use the format specified above) contain the following:
- The text of the report, which MUST NOT exceed 300 characters, including spaces
- The experiment being performed
- The planet associated with the experiment
- The situation associated with the experiment
- The biome, if it is required by the experiment/situation.

Otherwise, you should [fork the repository](https://help.github.com/articles/fork-a-repo/),
make your changes (as described above) in your fork, and [open a pull request](https://help.github.com/articles/creating-a-pull-request/).

In either case, a friendly moderator or repository maintainer will come along and evaluate your proposal, praise Octocat! ([Github's mascot](https://octodex.github.com/))

[License:4.0]: http://creativecommons.org/licenses/by-nc-sa/4.0/legalcode
[License:1.0]: https://creativecommons.org/publicdomain/zero/1.0/
[shield:CCO4]: http://img.shields.io/badge/License-CC%20BY--NC--SA%204.0-blue.svg
[shield:CC01]: http://img.shields.io/badge/License-CC0%201.0-blue.svg
