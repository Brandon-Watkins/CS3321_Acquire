# IFTeam_Aquire
This is the repository for the Idaho Falls team of the Fall 2019 Software Engineering class to make the digital version of the game _Acquire_.

## Running Acquire
You may open the Acquire project in Visual Studio to run, or run the executable located in _Acquire\bin\Release\Acquire.exe_

## Important Dates
| Date | Deliverable |
| --- | --- |
| 10/14/2019 | Project Proposal |
| 11/4/2019 | Iteration Phase 1 |
| 12/2/2019 | Iteration Retrospective |
| 12/5/2019 | Final Documentation and Project Presentation |
| 12/11/2019 | Peer Reviews |

## Help Files
XML Documentation of the code was created. Sandcastle was used to create help files which can be found at:
* _SandcastleProject\Help\Documentation.chm_
* _SandcastleProject\Help\index.html_

### Updating the help files:
* Install Sandcastle.
* Build the Acquire project with the XML file.
* Open the AcquireDocs.shfbproj project in Sandcastle.
* Ensure it references the correct XML file.
* Build the Sandcastle Project.

## Unit Tests
MSTest was used to create unit tests of the Acquire project. Some issues were discovered with the Nuget packages in the project. To fix these issues,
* Open **UnitTestAcquire.csproj** XML.
* Remove all lines that have _packages\MSTest_ in their path.
* Open the project in Visual Studio.
* Go to Tools->Nuget Package Manager->Package Manager Settings.
* Check _Allow Nuget to download missing packages_.
* Check _Automatically check for missing packages during build in Visual Studio_.
* Rebuild project.

## Further Documentation
Further documentation of the project can be found on the project wiki.
