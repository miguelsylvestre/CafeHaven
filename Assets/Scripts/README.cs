/*
hi !

ok so since you clearly dont understand ANYTHING ive done i made this to explain it to you
hope this helps <3


scripts im not really going to get into unless you ask, you really wont have to understand how they work but ill explain how to use them
for OpenMenus.cs, to use it, select the GameObject in the hierarchy of the appliance/item you want to add a menu to, and select the main parent of the menu

here is what the hierarchy should look like.


Menus
    Menu Parent (ex. CoffeeMachineMenu)
        Menu BG (has to go on TOP of the actual appliance part for the ordering to work) (ex. CoffeeMachineBG)
        Menu item (ex. CoffeeMachine)
        Buttons (empty parent class for simplicity sake and easy disabling for when a button has already been pressed) (ex. CoffeeMachineButtons)
            button 1
            button 2
    Menu Parent (ex. BlenderMenu)
        etc.
    Menu Parent
        etc.

reason i made it like this to start is for simplicity sake, the main Menus parent object is just there for organization, 
however the formatting of the rest of it is crucial for the layer ordering to work


ok now
scene switching:

way it works is you start in TopDown, and it initializes that world along with the player

Then, when you hit the counter, it will open up the CounterView scene on TOP of the TopView scene, and disables playerMovement through a variable in the script, 
but lets npcs keep moving and other timers running in TopDown for that logic part when we add it

Then, when you go back into TopView, we cant just delete the CounterView scene because we still need the data and the timers and what not running from coffee thats 
in the process of being made, so it runs a method that hides all of the gameObjects inside of CounterGraphics so they still exist and have their scripts running, 
but are invisible and cannot be clicked

Then this toggle will go back and forth and hide/unhide anything whenever you switch between the two scenes

yes realistically we didnt need the two scenes but for organization sakes it is MUCH better to have it like this


ok now
camera settings and canvas views:

i set EVERYTHING when i first set up the cameras to work based off of my artwork which is 192x108 at its biggest, so it reads the EXACT pixel size of my work
and everything is in 1 PPU. thats why the circle thing auto pixelates itself lol, its just matching the camera

everything should be set on 1 ppu, and pixel perfect settings, along with screen space camera for canvases

make sure everything is on the right sorting layer, all the names are pretty straight forward

also a tip, if you are working with images and not spriteRenderers with their layer component, on the hierarchy, objects that are LOWER, show up on TOP (its weird ik)


next
how i want the buttons to work and how i think it should be:

the way i made the art, i just included the buttons and everything IN THE ART, so its only one picture and whatever. 
YES, i know there are button elements in unity and i could have just made them individually on unity but no thats not how im doing it because of how i did the art.

instead, we can just use an EMPTY GAMEOBJECT with a similar script to the OpenMenus with the clicking to then sense when the button is pressed. 
Using a transparent/empty gameObject, it can still show the button from the art, but still be clicked. 

this means you do NOT need any of the button art separately, and it will look the same because it is just a transparent box with the collider being the same size.
if you have an actual reason why you need them, please explain it to me IN PERSON so i can fully understand what you are saying, because from what i got
its just useless stuff that is NOT needed at all


anything i missed lmk hopefully you can figure some of this stuff out and maybe we can finally get to grinding this out :)

*/


/*
QUESTIONS? PUT THEM DOWN HERE: (or just text me)


*/