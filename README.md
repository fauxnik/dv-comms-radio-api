[![Contributors][contributors-shield]][contributors-url]
[![Forks][forks-shield]][forks-url]
[![Stargazers][stars-shield]][stars-url]
[![Issues][issues-shield]][issues-url]
[![MIT License][license-shield]][license-url]




<!-- PROJECT TITLE -->
<div align="center">
	<h1>Comms Radio API</h1>
	<p>
		A framework mod for <a href="http://www.derailvalley.com/">Derail Valley</a> that enables other mods to create entries in the comms radio more easily.
		<br />
		<br />
		<a href="https://github.com/fauxnik/dv-comms-radio-api/issues">Report Bug</a>
		·
		<a href="https://github.com/fauxnik/dv-comms-radio-api/issues">Request Feature</a>
	</p>
</div>




<!-- TABLE OF CONTENTS -->
<details>
	<summary>Table of Contents</summary>
	<ol>
		<li><a href="#about-the-project">About The Project</a></li>
		<li><a href="#how-to-use">How To Use</a></li>
		<li><a href="#building">Building</a></li>
		<li><a href="#packaging">Packaging</a></li>
		<li><a href="#license">License</a></li>
	</ol>
</details>




<!-- ABOUT THE PROJECT -->

## About The Project

A framework mod for <a href="http://www.derailvalley.com/">Derail Valley</a> that enables other mods to create entries in the comms radio more easily.




<!-- HOW TO USE -->

## How To Use

### Setup

Download the API archive, install it using UnityModManager Installer, and add a reference in your project file.

```xml
<Reference Include="CommsRadioAPI"/>
```

You'll likely need to add a reference path to `Directory.Build.targets` to tell the compiler where the API assembly is. Adjust it to match your Derail Valley install directory.

```
C:\Program Files (x86)\Steam\steamapps\common\Derail Valley\Mods\CommsRadioAPI\
```

### Creating a Comms Radio mode

Creating a Comms Radio mode is done via the static `CommsRadioMode.Create` method.

```csharp
CommsRadioMode mode = CommsRadioMode.Create(MyInitialStateBehaviour);
```

To change the laser beam color, pass the `laserColor` parameter.

```csharp
CommsRadioMode mode = CommsRadioMode.Create(MyInitialStateBehaviour, laserColor: Color.CornflowerBlue);
```

To specify the ordering of the Comms Radio mode, pass an `insertBefore` predicate.

```csharp
CommsRadioMode mode = CommsRadioMode.Create(MyInitialStateBehaviour, insertBefore: crm => crm == ControllerAPI.GetVanillaMode(VanillaMode.LED));
```

> [!IMPORTANT] 
> The initial state passed to `CommsRadioMode.Create` _must_ have `ButtonBehaviourType.Regular` as its button behaviour.

### Working with state, actions, and update

State, player input handling, and game update handling are encapsulated by `AStateBehaviour`, which consuming mods must extend. Transitioning from one state to another is achieved by returning a new `AStateBehaviour` from one of the handler methods. This can also be used to change the behaviour in response to player input.

```csharp
class CounterBehaviour : AStateBehaviour
{
	int number;

	public CounterBehaviour(int number) : base(new CommsRadioState(titleText: "Counter", contentText: number.ToString()))
	{
		this.number = number;
	}

	public override AStateBehaviour OnAction(CommsRadioUtility utility, InputAction action)
	{
		return action switch
		{
			// These transition to a new state with the same behaviour
			InputAction.Up => new CounterBehaviour(number + 1),
			InputAction.Down => new CounterBehaviour(number - 1),
			// This transitions to an entirely different state and behaviour
			InputAction.Activate => new MyInitialStateBehaviour(),
			_ => throw new ArgumentException(),
		};
	}
}
```

> [!NOTE]
> `OnUpdate` is allowed to return `this` if no change in state has occurred in response to the game update tick.
>
> `OnAction` is not allowed to do this. Set the state's button behaviour to `ButtonBehaviourType.Ignore` instead.

### Full API

View the entire API at https://fauxnik.github.io/dv-comms-radio-api.




<!-- BUILDING -->

## Building

Building the project requires some initial setup, after which running `dotnet build` will do a Debug build or running `dotnet build -c Release` will do a Release build.

### References Setup

After cloning the repository, some setup is required in order to successfully build the mod DLLs. You will need to create a new [Directory.Build.targets][references-url] file to specify your local reference paths. This file will be located in the main directory, next to CommsRadioAPI.sln.

Below is an example of the necessary structure. When creating your targets file, you will need to replace the reference paths with the corresponding folders on your system. Make sure to include semicolons **between** each of the paths and no semicolon after the last path. Also note that any shortcuts you might use in file explorer—such as %ProgramFiles%—won't be expanded in these paths. You have to use full, absolute paths.
```xml
<Project>
	<PropertyGroup>
		<ReferencePath>
			C:\Program Files (x86)\Steam\steamapps\common\Derail Valley\DerailValley_Data\Managed\
		</ReferencePath>
		<AssemblySearchPaths>$(AssemblySearchPaths);$(ReferencePath);</AssemblySearchPaths>
	</PropertyGroup>
</Project>
```

### Line Endings Setup

It's recommended to use Git's [autocrlf mode][autocrlf-url] on Windows. Activate this by running `git config --global core.autocrlf true`.




<!-- PACKAGING -->

## Packaging

To package a build for distribution, you can run the `package.ps1` PowerShell script in the root of the project. If no parameters are supplied, it will create a .zip file ready for distribution in the dist directory. A post build event is configured to run this automatically after each successful Release build.

Linux: `pwsh ./package.ps1`
Windows: `powershell -executionpolicy bypass .\package.ps1`


### Parameters

Some parameters are available for the packaging script.

#### -NoArchive

Leave the package contents uncompressed in the output directory.

#### -OutputDirectory

Specify a different output directory.
For instance, this can be used in conjunction with `-NoArchive` to copy the mod files into your Derail Valley installation directory.




<!-- LICENSE -->

## License

Source code is distributed under the MIT license.
See [LICENSE][license-url] for more information.




<!-- MARKDOWN LINKS & IMAGES -->
<!-- https://www.markdownguide.org/basic-syntax/#reference-style-links -->

[contributors-shield]: https://img.shields.io/github/contributors/fauxnik/dv-comms-radio-api.svg?style=for-the-badge
[contributors-url]: https://github.com/fauxnik/dv-comms-radio-api/graphs/contributors
[forks-shield]: https://img.shields.io/github/forks/fauxnik/dv-comms-radio-api.svg?style=for-the-badge
[forks-url]: https://github.com/fauxnik/dv-comms-radio-api/network/members
[stars-shield]: https://img.shields.io/github/stars/fauxnik/dv-comms-radio-api.svg?style=for-the-badge
[stars-url]: https://github.com/fauxnik/dv-comms-radio-api/stargazers
[issues-shield]: https://img.shields.io/github/issues/fauxnik/dv-comms-radio-api.svg?style=for-the-badge
[issues-url]: https://github.com/fauxnik/dv-comms-radio-api/issues
[license-shield]: https://img.shields.io/github/license/fauxnik/dv-comms-radio-api.svg?style=for-the-badge
[license-url]: https://github.com/fauxnik/dv-comms-radio-api/blob/master/LICENSE
[references-url]: https://learn.microsoft.com/en-us/visualstudio/msbuild/customize-your-build?view=vs-2022
[autocrlf-url]: https://www.git-scm.com/book/en/v2/Customizing-Git-Git-Configuration#_formatting_and_whitespace
