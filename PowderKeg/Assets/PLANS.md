# Planned Features:

### Miscellaneous
* Add explosives, e.g. gunpowder/bombs
* If particle needs to move but theres an obstacle, if obstacles weighs less, swap places

### v0.6
* Bombs set fire
* Grenades delete

### v0.7
* Menu is 3 dots (...) at the bottom of the sidebar, when opened, it pauses the game.
* Menu contains customizations such as background color, update speed.
* Menu also contains options to save as image or gifs.
* A question mark button near the bottom right, once clicked, mouse over any element or button and it will replace the PowderKeg title with the name of the element.

### v0.8
* Images save the current rendered frame into a png
* Gifs save each update frame into a gif.

### v0.9
* Need to add a static ParticleState which prevents static objects from getting erased by acid/lava


Seed:
Water value, nearby water increase value every frame, if value is above a number, spawn a plant block, add our water value to plant block, when plant block water value is high enough, that grows another plant block.