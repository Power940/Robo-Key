# Robo-Key
A simple program to simulate key presses. Can hold keys, auto click keys, and run custom user writen scripts

uses H.InputSimulator  |  https://github.com/HavenDV/H.InputSimulator  
uses MouseKeyHook  |  https://github.com/gmamaladze/globalmousekeyhook

<h1>How to use RoboKey</h1>
The top left section is the logged keys section, use the remove and add key drop downs to add and remove keys, and use clear to remove all logged keys.
<br><br>
The top right section has the Hold Keys and Auto Click Keys controls. you can click both of the buttons and it will toggle their modes, or you can use the dropdowns to bind a key to toggle the two modes. The clear keybind buttons will un-bind the key from their respective toggle. The Auto Click button has click interval which will click keys every x milliseconds.
<br><br>
The bottom section has all of the custom key systems. you write commands in the bottom left most box and can use the help buttom for more info on writing custom commands. The toggle mode check box when on will make it so triggering the commands to run will toggle them on and off, when it's off it will run all the commands everytime they are triggered. Force Stop Commands will stop the custom commands if you need that. and the text will change based on what's written in the box to show if the script is vaild or not.
<h1>Modes</h1>
Hold Keys - will hold down all keys in the logged keys section and check every 10 milliseconds to try to hold them down again if they were pressed or release all held keys if the toggle turned off and they were held down.
<br>
Auto Click Keys - will click all logged keys every x milliseconds based on the selected click interval.
<br>
Run Commands - Will try to run keystroke commands from top to bottom.
