# TouchControls
A project developed for testing different mobile gamepads prototypes for a Teeworld-style game. The project has three main parts. A Unity App for android that is a remote gamepad for the game that sends commands to a server. A c++ application (server) that receives the gamepad commands and synthesize input events such as keystrokes and mouse movements to control the Teeworld game. And finally, Cheat Engine for having more control over Teeworld (mouse movement).

## How to use it
- Open Unity Project.
- Open Scene: Project Window> _Scenes> Testing Scene.
- Select from the hierarchy: SoldatServerConnection gameobject.
- On the Inspector> Soldat Input Client> Host: Write your local IP (to be able to connect with KeyControlServer.exe)
- Keep port number = 81 (if you change it you have to change it later in KeysControlServer.cpp and rebuild)
- Unity> Build Settings> Build and Run, on Android device.
- Go back to project folder: Run TouchControls\KeysControlServer\Debug\KeysControlServer.exe
- Run teeworlds.exe (you can download the game here https://www.teeworlds.com/)
- Select a server and join (type: CTF, map: CTF5)
- Once on the game, go back to TouchControls folder, run CheatEngine4Teeworld.EXE, or run teeworlds.CT using Cheat Engine (https://github.com/cheat-engine/cheat-engine/releases/download/6.6/CheatEngine66.exe)
- Go back to teeworlds.exe
- Start using the gamepad from your android device (Wait for a few seconds (max 20seconds) until the server is fully responding)
- In the Android App> Options: You can Calibrate gyroscope, keeping your device still on the table while calibrating.
 You can change sensivity on the X axis and Y axis for the gyroscope.
 You can also change the host IP and port (If you change the port you have to change it also in KeysControlServer.cpp)
 
