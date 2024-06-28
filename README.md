# Fugro Senior Software Developer Assessment
***
## 📄 General instructions
In order to run this project, is required to have installed the .NET 8 runtime. You can download it following [this link](https://dotnet.microsoft.com/pt-br/download/dotnet/8.0).

### ⚙️ Configurations
This project have a configuration file called ```appsettings.json```.
This file is located in ```Fugro.Assessment.Application```.
In this file, the usar can inform the location of source file required for the application run. 
This file can be found in ```bin/release/net<version>``` folder as well.
If no file was informed, the application will try to use the default file that is ```bin/release/net<version>/Assets/polyline sample.csv```.

### 👾 Solution Configurations
This project have two different solution configurations.
- **Debug**: All projects are build in order to allow technical analisys and code improvements.
- **Release**: Only application projects and it dependencies will be built in order to performance increase.

### 🧑‍💻 Running the application
When user runs the application, a console is show and is required to inform coord X and coord Y subsequently.
After it, the system will read the source file to start to generate the ```Smallest Offset``` and ```Station Segments``` in order to calculate total distance from the start point to ```Offset```, which is the end point.

In the normal case, the application will ehxibith the ```Station``` which is the sum of all full segments before the segment where we have the perpendicular nearest distance plus the last one from start to perpendicular point that touches the nearest segment.

After it, the application will print the path until hit the Offset point, describing all segment names, the type of segment (if it's a Segment, a partial segment or the offset segment), and the individual size of the segment.

### 🤝 Thank you!
Thanks you very much for letting me participate of this assessment.
The challenge was very interesting and I hope the code was great as well.
