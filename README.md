# ShooterVR
Simple VR FPS game

v.1.0:
- get Shooter3d game as base;
- deactivate all input listeners;
- add XR Unity plugins;
- upgrade pistol model for XR gameplay (shooting and magazine grab).

v.1.1 - 1.1.1:
- pistol bullet game counter;
- pistol magazine counts bullets;
- magazine has bullets inside if not empty and has no bullets in another case;
- add muzzle flash effect to pistol;
- add impact effect to bullet;
- add bullet hole effect;
- setting pistol muzzle flash parametrs (rotation and position);
- bullets can destroy iron chains;
- remove inspect target script (dont need in VR);
- fix: on game start magazine is out of pistol
- fix: debug error with unparrenting.

v.1.2:
- machine gun model;
- machine gun magazine model;
- grab machine gun;
- socket magazine to machine gun;
- machine gun shooting/no bullets;
- edit socket script to get just machine gun ammo.

v.1.3:
- machine gun can takes in two hands, second hand rotate the gun;
- fix: machine gun ammo collide with weapon.

v.1.4:
- add shotgun model;
- shotgun shooting / no bullets;
- shotgun take by two hands.

v.1.5:
- shotgun reloading (multi sockets for bullets);
- empty casing ammo for shotgun;
- add bow item;
- add arrow item;
- arrow hold in bow socket.

v.1.6:
- fix: can get pistol magazine when weapon out of hands;
- fix: ignore second hand exit trigger when exit first hand.

v.1.7:
- bow string and inertia.

v.1.8:
- bow shooting with arrow;
- arrow effects (impact, sounds, stick in wood);
- bow can reloaded just by arrow;
- drop string and arrow when drop the bow;
- get arrow from stick in wood objects.

v.1.9:
- inventory belt;
- inventory machine gun ammo put/get;
- inventory pistol ammo put/get.

v.2.0:
- change inventory logic - items collect to list instead destroying;
- inventory shotgun bullets and box ammo;
- inventory bow arrows and quivers;
- inventory hover only selected ammo;
- weapon hover only selected ammo.

v.2.1:
- Oculus plugins;
- Oculus full integration and settings;
- teleporting ray;
- locomotion manager (show teleportation ray only when primary button is pressed);
- military target game repair scripts;
- chain target correct physics.

v.2.1.1:
- machine gun automatic shooting;
- hands drop the objects if grab button not pressed, exclude weapons (in that case hands in sticky mode);
- fix: pistol magazine not empty with no bullets.

v.2.2:
- weapon in each hand shooting;
- hands can move objects;
- ignore teleportation layer all game objects;
- teleportation inside tube;
- fix: inventory position and rotation, item bugs;
- fix: machine gun continious shooting when dropped;
- fix: weapon can grab ammo from inventory.
