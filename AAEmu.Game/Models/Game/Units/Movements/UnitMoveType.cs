﻿using System.Numerics;
using AAEmu.Commons.Network;
using AAEmu.Game.Models.Game.World;

namespace AAEmu.Game.Models.Game.Units.Movements
{
    public enum EStance : sbyte
    {
        Null = -1,
        Combat = 0,
        Idle = 1,
        Swim = 2,
        Coswim = 3,
        Zerog = 4,
        Stealth = 5,
        Climb = 6,
        Prone = 7,
        Fly = 8,
        Last = 9
    }
    public enum AiAlertness : sbyte
    {
        Idle = 0,
        Alert = 1,
        Combat = 2
    }
    public enum AiFilesType
    {
        SiegeWeaponPlace = 11,
        Bomb = 12,
        Roaming = 13,
        HoldPosition = 15,
        TowerDefenseAttacker = 17,
        FlyTrap = 18,
        BigMonsterHoldPosition = 19,
        BigMonsterRoaming = 20,
        ArcherHoldPosition = 21,
        ArcherRoaming = 22,
        WildBoarHoldPosition = 23,
        WildBoarRoaming = 24,
        Dummy = 25,
        Default = 26,
        AlmightyNpc = 27
    }

    public enum ActorMoveType : ushort
    {
        StandStill = 3,
        Run = 4,
        Walk = 5,
    }
    
    public class UnitMoveType : MoveType
    {
        public sbyte[] DeltaMovement { get; set; }
        public sbyte Stance { get; set; }
        public sbyte Alertness { get; set; }
        //public byte ColliOpt { get; set; } // add in 1200 march 2015
        public byte GcFlags { get; set; }
        public ushort GcPartId { get; set; }
        public float X2 { get; set; }
        public float Y2 { get; set; }
        public float Z2 { get; set; }
        public sbyte RotationX2 { get; set; }
        public sbyte RotationY2 { get; set; }
        public sbyte RotationZ2 { get; set; }
        public uint ClimbData { get; set; }
        public uint GcId { get; set; }
        public WorldPos GcWorldPos { get; set; }
        public ushort FallVel { get; set; }
        public ushort ActorFlags { get; set; }

        public override void Read(PacketStream stream)
        {
            base.Read(stream);
            (X, Y, Z) = stream.ReadPosition();
            VelX = stream.ReadInt16();
            VelY = stream.ReadInt16();
            VelZ = stream.ReadInt16();
            RotationX = stream.ReadSByte();
            RotationY = stream.ReadSByte();
            RotationZ = stream.ReadSByte();
            DeltaMovement = new sbyte[3];
            DeltaMovement[0] = stream.ReadSByte();
            DeltaMovement[1] = stream.ReadSByte();
            DeltaMovement[2] = stream.ReadSByte();
            Stance = stream.ReadSByte();    // actor.stance
            Alertness = stream.ReadSByte(); // actor.alertness
            ActorFlags = stream.ReadUInt16(); // actor.flags in 1.2 ... 1.8 byte, in 2.0 ushort (flags(byte) + colliopt(byte))
            if ((ActorFlags & 0x80) == 0x80) // ActorFlags < 0
                FallVel = stream.ReadUInt16();   // actor.fallVel
            if ((ActorFlags & 0x20) == 0x20)
            {
                GcFlags = stream.ReadByte();          // actor.gcFlags
                GcPartId = stream.ReadUInt16();       // actor.gcPartId
                (X2, Y2, Z2) = stream.ReadPosition(); // ix, iy, iz
                RotationX2 = stream.ReadSByte();
                RotationY2 = stream.ReadSByte();
                RotationZ2 = stream.ReadSByte();
            }
            if ((ActorFlags & 0x60) != 0)
                GcId = stream.ReadUInt32(); // actor.gcId
            if ((ActorFlags & 0x40) == 0x40)
                ClimbData = stream.ReadUInt32(); // actor.climbData
        }

        public override PacketStream Write(PacketStream stream)
        {
            base.Write(stream);

            stream.WritePosition(X, Y, Z);
            stream.Write(VelX);
            stream.Write(VelY);
            stream.Write(VelZ);
            stream.Write(RotationX);
            stream.Write(RotationY);
            stream.Write(RotationZ);
            stream.Write(DeltaMovement[0]);
            stream.Write(DeltaMovement[1]);
            stream.Write(DeltaMovement[2]);
            stream.Write(Stance);
            stream.Write(Alertness);
            stream.Write(ActorFlags); // actor.flags in 1.2 ... 1.8 byte, in 2.0 ushort (flags(byte) + colliopt(byte))
            if ((ActorFlags & 0x80) == 0x80) // ActorFlags < 0
                stream.Write(FallVel);
            if ((ActorFlags & 0x20) == 0x20)
            {
                stream.Write(GcFlags);
                stream.Write(GcPartId);
                stream.WritePosition(X2, Y2, Z2);
                stream.Write(RotationX2);
                stream.Write(RotationY2);
                stream.Write(RotationZ2);
            }
            if ((ActorFlags & 0x60) != 0)
                stream.Write(GcId);
            if ((ActorFlags & 0x40) == 0x40)
                stream.Write(ClimbData);

            return stream;
        }
    }
}
