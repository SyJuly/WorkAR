# WorkAR

Prototypical development of an AR work environment in combination of smart devices. 

## WorkAR in context

The goal of the thesis is to develop a framework describing basic theories which advantages AR offers in context of digital working and how these can be used in an work environment. WorkAR is the prototype according to these guidelines.

#### AR enhances work processes
 - three-dimensional visualisation
 - spacial information storage
 - natural movements
 - natural interactions
 - reality-integration

### Prototypical application
  
The prototype uses spatial information storage in terms of information of a context (i.e. a clock or a calendar) being placed according to the room the user is standing in. These information contextes are called widgets, reminding of those you find on a smartphone. Five widgets/ applications have been implemented in this prototype:

   - clock
   - calendar
   - board of notes
   - 3D-model loader
   - 3D-model viewer/modifier
   
   
#### AR in combinationen with other smart devices

More komplex widgets communicate with APIs such as the Google Calendar and the Trello API. The widgets are programmed to sync automatically, so it's possible to combine work processes with other smart devices.

## Build & Deployment

This application was developed for the Microsoft Hololens, using the Unity Engine.

**Unity** version: 2018.3.0f2 (requiring UWP Build Support, Windows BuildSupport, Vuforia Augmented Reality Support)
**Microsoft's [Mixed Reality Toolkit](https://github.com/Microsoft/MixedRealityToolkit-Unity/releases)**

When the Holotoolkit is integrated, you can apply all UWP project settings. For this application use the .NET scripting backend. The following **capabilites** must be applied:

- InternetClient
- PicturesLibrary
- WebCam
- Microphone
- SpatialPerception

Build the Unity Project in the Build Window as well as the APPX-Package. You can deploy the APPX-file via the Hololens Web Portal including the dependencies(these are generated inside the build folder inside x86) which you upload under optional packages.


## Licence
MIT Licence
