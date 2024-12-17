---
title: _Chirp!_ Project Report
subtitle: ITU BDSA 2024 Group `25`
author:
- Lukas <lukha@itu.dk>
- Carla <cmoo@itu.dk>
- Marie <mblh@itu.dk>
- Nicklas <nlje@itu.dk>
- Teis <teif@itu.dk>
numbersections: true
---

# Design and Architecture of _Chirp!_

## Domain model

Here comes a description of our domain model. ( this is just template stuff for now)

![Illustration of the _Chirp!_ data model as UML class diagram.](images/DomainModel4.drawio.png)

## Architecture — In the small
![Illustration of the _Chirp!_ ONION](images/OnionDiagram.png)
## Architecture of deployed application

## User activities
![Illustration of the _Chirp!_ User journey](images/UserJourney.drawio.png)

This illustration shows a potential user journey through the chirp application. Before login every possible user journey is represented. After login the illustration only display some of the options a user has. All functionalities are addressed at some point in the journey, but the illustration would become unreadable if it showed every possible way the user can go. For instance, it is both possible to logout and access the public timeline, no matter where the user is, so there would be a lot of crossing arrows.

## Sequence of functionality/calls trough _Chirp!_
![Illustration of the _Chirp!_ Sequence Diagram](diagrams/SequenceDiagram.png)

The illustration shows the sequence of events happening when the user opens the application.This seqeunce diagram is made relatively high-level focusing on important sequences, meaning every single operation is not shown. For example, attributes such as Like-/DislikeAmount, IsFollowing, etc. are all retrieved as a part of the fetched objects from the request and shown accordingly. 

# Process

## Build, test, release, and deployment
For Chirp! we utilize three main github workflows. The first is an automated build and test workflow that attempts to build any commit or pull request.
It also runs our basic unit and integration tests, the more advanced test suite is not automated. For azure we have a standard azure deployment workflow with a minor change:
we have added an optional dispatch, and limited what triggers the workflow. As redeploying azure on trivial changes result in extensive downtime.
Our final Workflow "chirpflow" creates a release on versions with tags matching "v.*.*.*" this is mostly used for punctiating feature implementations. The releases 
contains the program compiled for windows, linux, osx, and arm-64.
## Team work
### *** INSERT final picture of bord with unfinished tasks *** 
comment on the missing changes 

-- add comment on why playwrite does not run though workflows. 

### Introducing a new feature  
From the beginning the team agreed to, work together physically instead of remotely when ever possible, therfore a lot of task were undertaken using a combination of mob programming and co-authoring, this goes for tackling a new feature as well.

When presented with a new feature to implement, a team member creates a new issue on the project bord and adds appropriate acceptance criteria. If there is some confusion as to what that would be, the team member brings it to the team, discuss it, and create a plan for how to best approach the implementation. When a new feature is under development a team member moves it from the To-do list to the in progress list and assigns one or more members to the task.

When assigned to an issue, member(s) then create- and publish a new branch to work on the issue, occasionally creating sub-branches to try different approaches. Doing their best to keep to a GitHub Flow -style branching strategy.
When the acceptance criteria had been fulfilled, the member(s) working the branch - or someone else from the team, will then create a new sub-branch, to write the necessary test for the new methods and- or UI features.

Now the member(s) working on the issue creates a pull request to the branch, to merge it with main branch, and add a reviewer(s). Before another team member can merge preliminary workflow checks will be run, then if the new input passes workflow checks (see -- add ref to WF --) and test the reviewer can look at the code, reject if necessary, or complete the merge. The old branch can then be deleted.

![Issue from creation to merge](images/from issue creation to merge.drawio.png)

https://github.com/ITU-BDSA2024-GROUP25/Chirp/blob/main/docs/images/from%20issue%20creation%20to%20merge.drawio.png

## How to make _Chirp!_ work locally

## How to run test suite locally

# Ethics

## License
This project is licensed under the MIT License (see full License her: https://github.com/ITU-BDSA2024-GROUP25/Chirp/blob/main/LICENSE).
In the document NOTICE.MD we have provided additional information about other Licenses and copyrights for packages used, that do not fall under MIT (see https://github.com/ITU-BDSA2024-GROUP25/Chirp/blob/main/NOTICE.md).

## LLMs, ChatGPT, CoPilot, and others
During the project, ChatGPT was used sparingly and was used when we encountered problems or situations where we felt stuck and documentation sites such as StackOverflow deemed unhelpful. Another use case of ChatGPT was to avoid redundant manual tasks, such as generating colors for the dark mode stylesheet. 
The code segments developed with the help of an LLM have been clearly commented as such.  

ChatGPT was used in the following areas:
* CSS styling for dark mode and switch button.
* Database structuring and help with schema relations.
* Complex JavaScript features like "Maintain scroll" and "Switch button for color themes".
  
Generally, the use of LLMs sped up the project progress, however, in a few instances they provided wrong or buggy solutions which required extra time debugging.