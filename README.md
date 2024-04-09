# New Legacy Harvest System
[ I am no way associated with Electronic Arts, Broadsword, and any other entity regarding Ultima Online ]

This is an emulated version of Ultima Online's New Legacy Resource System, used for resources related to harvesting and crafting.
Information about the commercial version of this system can be found [ here ](https://www.uo-cah.com/new-legacy/everything-we-know)

NOTE: This code is not capable of running on it's own, it's a system written to work as part of the ServUO program. Several additional edits are required to the [ ServUO ](https://github.com/ServUO/ServUO) codebase for this system to be fully functional. I have no included those edits for simplicity.

The Resource Backpack is a new layer on the Player's character. In this case, the layer is a virtual holder for a container which only harvested, or some crafted, items will be stored in. 
This system is not yet available for public testing. I have designed it based on the screen shots released for public view, the rest I have taken some creative liberty in my design choices. 


The code is designed to be quick and efficient since it needs to be iterated through every time the gump / UI is sent to the player (which is also required to do when refreshing any data on it).
For this reason, I have an array of Types used a central database. From that, I build each page using 3D arrays to indicate what Item ID should be displayed in the UI, what Type of item the system needs to search the Resource Backpack for, and what hue the item displayed on the UI should be (this is used for items which use the same Item ID but need different hues to indicate a difference)


Edits not shown in this repo are: 
- Massive edits done to the crafting system to remove ~70% of craftable options, as well as to check for needed resources in the Resource Backpack instead of the player's inventory (backpack)
- Edits to the harvest system to drop resources to the Resource Backpack and to resend the resources gump if it's opened (TODO: verify if there's a better way to handle this)
- Edits to core files to support the new Layer of 'ResourceBackpack'



Harvesting preview [ click here, or the picture below, to watch the video ](https://youtu.be/OSBR0mZ_8fI)

[![Watch the video](https://img.youtube.com/vi/OSBR0mZ_8fI/hqdefault.jpg)](https://youtu.be/OSBR0mZ_8fI)

Crafting preview [ click here, or the picture below, to watch the video ](https://youtu.be/EYVma-94P3s)

[![Watch the video](https://img.youtube.com/vi/EYVma-94P3s/hqdefault.jpg)](https://youtu.be/EYVma-94P3s)


