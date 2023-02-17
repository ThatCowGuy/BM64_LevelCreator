//#===============================================================
//#===============================================================
//#
//# this is a template ASM file based on Coockies Minimal Level ASM
//# use CajeASM to compile this file into bm64data/ovlbank3/ovl0.bin
//#
//#===============================================================
//#===============================================================
[JumpTableLocation]:    0x80044A90
//#--------------------------------------
[LevelModel]:           
[LevelCollision]:       
//#--------------------------------------
[EnemyBehaviourOne]:    0x0000
[EnemyModelOne]:        0x00DE
//#--------------------------------------
[EnemyBehaviourTwo]:    0x0000
[EnemyModelTwo]:        0x00DE
//#--------------------------------------
[EnemyBehaviourThree]:  0x0000
[EnemyModelThree]:      0x00DE
//#--------------------------------------
[EnemyBehaviourFour]:   0x0000
[EnemyModelFour]:       0x00DE
//#--------------------------------------
[EnemyBehaviourFive]:   0x0000
[EnemyModelFive]:       0x00DE
//#--------------------------------------
[EnemyBehaviourSix]:    0x0000
[EnemyModelSix]:        0x00DE
//#--------------------------------------
[SkyColourR]:           0x0000
[SkyColourG]:           0x0000
[SkyColourB]:           0x0000
//#===============================================================
//#===============================================================
.org 0x0000
LUI V0, 0x8004 
JR RA
ADDIU V0, V0, 0x3010
NOP
//#--------------------------------------
ADDIU SP, SP, 0xFFE8 // -0x18
SLTI AT, A0, 0x0052
BNE R0, AT, SKIP_CHECK_01
SW RA, 0x0014 (SP)
ADDIU AT, R0, 0x0052
BEQ A0, AT, SKIP_ALL_CHECKS
ADDIU AT, R0, 0x0053
BEQ A0, AT, SKIP_ALL_CHECKS
NOP
BEQ R0, R0, SKIP_ALL_CHECKS
NOP
//#--------------------------------------
SKIP_CHECK_01:
SLTI AT, A0, 0x0051
BNE R0, AT, SKIP_CHECK_02
ADDIU AT, R0, 0x0051
BEQ AT, A0, SKIP_ALL_CHECKS
NOP
BEQ R0, R0, SKIP_ALL_CHECKS
NOP
//#--------------------------------------
SKIP_CHECK_02:
SLTI AT, A0, 0x0012
BNE R0, AT, SKIP_CHECK_03
ADDIU T6, A0, 0xFFFF
ADDIU AT, R0, 0x0050
BEQ A0, AT, EXECUTE_ACTION_TILE
NOP
BEQ R0, R0, SKIP_ALL_CHECKS
NOP
//#--------------------------------------
SKIP_CHECK_03:
SLTIU AT, T6, 0x0011
BEQ R0, AT, SKIP_ALL_CHECKS
//SLL T6, T6, 2
.word 0x000E7080
LUI AT, @JumpTableLocation
ADDU AT, AT, T6
LW T6, @JumpTableLocation (AT)
NOP
JR T6
NOP
//#===============================================================
//#===============================================================
.org 0x0200
SKIP_ALL_CHECKS:
LW RA, 0x0014 (SP)
JR RA
ADDIU SP, SP, 0x0018
//#===============================================================
//#===============================================================
.org 0x0300 //# actionTile
EXECUTE_ACTION_TILE:
BEQ R0, R0, SKIP_ALL_CHECKS
NOP
//#===============================================================
//#===============================================================
.org 0x0600 //# init
LUI A0, 0x8004
JAL 0x8027EB5C
ADDIU A0, A0, 0x4960
//#--------------------------------------
ADDIU A0, R0, 0x0000
ADDIU A1, R0, @EnemyBehaviourOne
JAL 0x80287008
ADDIU A2, R0, @EnemyModelOne
//#--------------------------------------
ADDIU A0, R0, 0x0001
ADDIU A1, R0, @EnemyBehaviourTwo
JAL 0x80287008
ADDIU A2, R0, @EnemyModelTwo
//#--------------------------------------
ADDIU A0, R0, 0x0002
ADDIU A1, R0, @EnemyBehaviourThree
JAL 0x80287008
ADDIU A2, R0, @EnemyModelThree
//#--------------------------------------
ADDIU A0, R0, 0x0003
ADDIU A1, R0, @EnemyBehaviourFour
JAL 0x80287008
ADDIU A2, R0, @EnemyModelFour
//#--------------------------------------
ADDIU A0, R0, 0x0004
ADDIU A1, R0, @EnemyBehaviourFive
JAL 0x80287008
ADDIU A2, R0, @EnemyModelFive
//#--------------------------------------
ADDIU A0, R0, 0x0005
ADDIU A1, R0, @EnemyBehaviourSix
JAL 0x80287008
ADDIU A2, R0, @EnemyModelSix
//#--------------------------------------
JAL 0x80243BA8
ADDIU A0, R0, @LevelModel
JAL 0x8026FF64
ADDIU A0, R0, @LevelCollision
//#--------------------------------------
ADDIU A0, R0, @SkyColourR
ADDIU A1, R0, @SkyColourG
JAL 0x802276E0
ADDIU A2, R0, @SkyColourB
//#--------------------------------------
JAL 0x80245CDC
NOP
NOP
NOP
//#--------------------------------------
LUI AT, 0x4234
MTC1 AT, F0
LUI AT, 0x8004
SWC1 F0, 0x4A1C (AT)
SWC1 F0, 0x4A20 (AT)
BEQ R0, R0, SKIP_ALL_CHECKS
NOP
//#===============================================================
//#===============================================================
.org 0x0700 //# death
ADDIU A0, R0, 0x0040
BEQ R0, R0, SKIP_ALL_CHECKS
NOP
//#===============================================================
//#===============================================================
.org 0x1960 //# object table
.word 0x00000000
.word 0x00000000
.word 0x00000000
.word 0x00000000
.word 0x00000000
.word 0x00000000
//#===============================================================
//#===============================================================
.org 0x1A90 ;jump table!
.word 0x80043200
.word 0x80043600 //# init
.word 0x80043200
.word 0x80043200
.word 0x80043200
.word 0x80043200
.word 0x80043200
.word 0x80043200
.word 0x80043200
.word 0x80043200
.word 0x80043200
.word 0x80043200
.word 0x80043200
.word 0x80043200
.word 0x80043200
.word 0x80043700 //# death
.word 0x80043200
//#===============================================================
//#===============================================================