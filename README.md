<a id="readme-top"></a>

<br />
<div align="center">
  <a href="https://github.com/github_username/repo_name">
    <img src="images/logo.png" alt="Logo" width="80" height="80">
  </a>

<h3 align="center">MIDI2Event</h3>

  <p align="center">A lightweight C# library for synchronizing audio and code events in an artist-driven context.</p>
  <p align="center"><a href="https://github.com/cadenceblorbo/midi2event/issues">Report Bug/Request Feature</a></p>
</div>

<details>
  <summary>Table of Contents</summary>
  <ol>
    <li><a href="#about-the-project">About The Project</a></li>
    <li><a href="#getting-started">Getting Started</a></li>
    <li><a href="#usage">Usage</a></li>
    <li><a href="#usage">Current Limitations</a></li>
    <li><a href="#roadmap">Roadmap</a></li>
    <li><a href="#license">License</a></li>
    <li><a href="#contact">Contact</a></li>
  </ol>
</details>


## About The Project

Here's a blank template to get started. To avoid retyping too much info, do a search and replace with your text editor for the following: `github_username`, `repo_name`, `twitter_handle`, `linkedin_username`, `email_client`, `email`, `project_title`, `project_description`, `project_license`

<p align="right">(<a href="#readme-top">back to top</a>)</p>


## Getting Started

Download the latest source code .zip or .dll and place it in your project. Import the MIDI2Event namespace at the top of your script with a 'using' statement:
```csharp
using MIDI2Event;
```

<p align="right">(<a href="#readme-top">back to top</a>)</p>


## Usage

Use this space to show useful examples of how a project can be used. Additional screenshots, code examples and demos work well in this space. You may also link to more resources.

<p align="right">(<a href="#readme-top">back to top</a>)</p>


## Current Limitations
As this library was developed for personal use at first, it currently has limitations on the kind of MIDI data that can be used:
* MIDI data must use format 0 (consisting of a single MIDI track).
* Delta-time must be encoded using ticks per quarter note.

([This page](http://www.music.mcgill.ca/~ich/classes/mumt306/StandardMIDIfileformat.html) has more details on the MIDI file format.)

<p align="right">(<a href="#readme-top">back to top</a>)</p>


## Roadmap

- [x] Basic Features
- [ ] Expanded MIDI Support
    - [ ] More Formats
    - [ ] More Chunks
- [ ] Premade Engine Packages
    - [ ] Unity
    - [ ] Godot
- [ ] Formal Documentation

<p align="right">(<a href="#readme-top">back to top</a>)</p>


## License

Distributed under the GNU Lesser General Public License, Version 3. See `LICENSE.txt` for more information.

<p align="right">(<a href="#readme-top">back to top</a>)</p>


## Contact

Cadence Hagenauer - [devilstritone#0000](https://discord.com/users/302985879666950144) - cadence.hagenauer@gmail.com

Project Link: [https://github.com/cadenceblorbo/midi2event](https://github.com/cadenceblorbo/midi2event)

<p align="right">(<a href="#readme-top">back to top</a>)</p>

