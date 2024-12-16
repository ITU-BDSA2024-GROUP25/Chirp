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

![Illustration of the _Chirp!_ data model as UML class diagram.](images/DomainModel.drawio.png)

## Architecture — In the small
![Illustration of the _Chirp!_ ONION](images/ONIONDIAGRAMWIP.png)
## Architecture of deployed application

## User activities

## Sequence of functionality/calls trough _Chirp!_

# Process

## Build, test, release, and deployment

## Team work
### *** INSERT final picture of bord with unfinished tasks *** 
comment on the missing changes 

-- add comment on why playwrite does not run though workflows. 

### Introducing a new feature  
From the beginning the team agreed to, work together physically instead of remotely when ever possible, therfore a lot of task were undertaken using a combination of mob programming(ref?) and co-authoring, this goes for tackling a new feature as well.

When presented with a new feature to implement, a team member creates a new issue on the project bord and adds apropriete acceptance criteria. If there is some confusion as to what that would be, the team member brings it to the team, to desscuss it, and create a plan for how to best approch the implementation.
When a new feature is under develeopment a team member moves it from the *To-do* list to the *in progress* list and assigns one or more mebers to the task. 

When assigned to a issue, member(s) then create- and publish a new branch to work on the issue, occasionally creating sub-branches to try different approches. Doing there best to keep to a *GitHub Flow* -style branching strategy, see [session_02 slides:] (https://github.com/itu-bdsa/lecture_notes/blob/main/sessions/session_02/Slides.md)

When the acceptance criteria had been fulfilled, the member(s) working the branch - or someone else from the team, will then create a new sub-branch, to write the nessesarry test for the new methodes and- or UI features.  
Now the member(s) working on the issue creates a pull request to the branch, to merge it with main branch, and add a reviewer(s). Before another team member can merge preliminary workflow checks will be run, then if the new input passes workflow checks(see -- add ref to WF --) and test the reviewer can look at the code, reject if nessesary, or complete the merge. The old branch can then be deleted. 

  


## How to make _Chirp!_ work locally

## How to run test suite locally

# Ethics

## License
This project is licensed under the MIT License (see full License her: https://github.com/ITU-BDSA2024-GROUP25/Chirp/blob/main/LICENSE).
In the document NOTICE.MD we have provided additional information about other Licenses and copyrights for packages used, that do not fall under MIT (see https://github.com/ITU-BDSA2024-GROUP25/Chirp/blob/main/NOTICE.md).
## LLMs, ChatGPT, CoPilot, and others
