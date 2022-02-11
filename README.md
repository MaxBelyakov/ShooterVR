# Shooter3d
Simple 3d FPS game

v.1.0:
- first room;
- new textures (floor, walls, box);
- pistol asset;
- first person asset;
- shooting animation.

v.1.1:
- optimize pistol flash;
- centring pistol target;
- dot-point pistol target;
- shot sounds;
- bullet shell casting sound;
- sync pistol recoil with bullet and flash animation;
- add furniture asset.

v.1.2:
- bullets impact effects;
- bullets holes;
- change crosshair design to circle;
- bullets holes and impact depends on target material;
- casting don't destroy.

v.1.3 - 1.3.1:
- pistol reload script, animation and sound;
- bullets counter;
- nop bullets animation and sound;
- metal bullet holes and impact;
- temporary change bullets to add force to hit rigidbody objects;
- fix: bullet holes have gap from sphere surface;
- fix: ignore player body when shooting near player.

v.1.4:
- inventory (pistol ammo);
- pistol magazine object;
- put new pistol ammo in inventory;
- pistol ammo inventory limits.

v.1.5 - 1.5.1:
- add machine gun;
- machine gun casing;
- fall down pistol magazine sound;
- take ammo sound;
- increase crosshair size;
- fix: pistol casing fall sound (doublicates);
- fix: shooting while reloading.

v.1.6:
- machine gun bullets impact and holes;
- fix: audio voices limit increase to 100;
- fix: fire flash effect makes more realistic.

v.1.7 - 1.7.1:
- machine gun reloading;
- machine gun ammo;
- machine gun take ammo;
- change weapon by press numbers;
- upgrade weapon controller script and create new shoot effects class that is general both for pistol and machine gun and controll flash, impact and holes effects behavior.

v.1.8:
- add shotgun;
- shotgun buckshot;
- shotgun animations;
- shotgun fire logic;
- shotgun reloading;
- shotgun casing;
- shotgun ammo;
- shotgun sounds;
- new falling objects sounds common script;
- all weapons parametrs move to weapon controller script;
- fix: debug animation errors.

v.1.9:
- add bow;
- add arrow;
- bow shooting script and animation;
- arrow move.

v.1.9.1:
- bow move up when shoot;
- creates arrow when shoot;
- correct arrow ballistic parametrs;
- arrow stuck in wood;
- arrow impacts with stone, metal;
- bow sounds (impact, stuck in wood, bow shoot);
- switch weapon to bow;
- bow training target;
- fix: arrow stuck in wood just at first contact.

v.1.9.2:
- bow ammo (arrows pack);
- arrows put/get to inventory;
- correct bow shooting behavior, fix errors;
- fix: popup don't hide.

v.2.0 - 2.1:
- take arrows from world to inventory;
- upgrade inspect target script, capsule cast instead ray have better active radius;
- fix: change inventory popup text color;
- fix: wood holes on target object;
- fix: wood holes smaller and less small parts when shooting;
- fix: move througt casing, decrease casing weight;
- fix: shoot in world objects makes a holes, now just impact;
- fix: decline change weapon when shooting or reloading;
- fix: extra shooting makes unlimited bullets.

v.2.2:
- arrow drop sound;
- bow pickup and drop;
- pistol pickup and drop;
- shotgun pickup and drop;
- machine gun pickup and drop;
- fix: can't get arrow from wooden box;
- fix: arrow impact to wood holes.

v.2.3:
- level redesign;
- new items, objects, constructions;
- new materials, textures;
- weapons material check merge to common function in shootingeffects script.

v.2.4:
- update level design;
- update objects behavior and physics;
- holes when hit books;
- make bullet holes smaller for wood, metal and stone.

v.2.5:
- military targets with chains;
- chains can be destroyed by bullets;
- update shooting power for all weapons;
- update body mass for all objects;
- arrow give extra force to target;
- decrease player jump;
- update basket collider;
- fix: arrow makes holes in metal;
- fix: weapon get inside the walls and objects;
- fix: mesh and rigidbody debug errors.

v.2.6:
- add target dummies;
- target dummy random respawn;
- target dummy for different kind of weapons.

v.2.7 (release):
- fix: no pistol dummy respawn;
- fix: shotgun show -1 bullet;
- fix: shoot in ammo debug null error;
- fix: weapons extra reload bug;
- fix: take ammo from basket, increase inventory inspector distance;
- fix: interface correction after game build.
