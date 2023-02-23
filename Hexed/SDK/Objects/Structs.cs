using Hexed.Memory;
using System.Numerics;
using static Hexed.SDK.Objects.Enums;

namespace Hexed.SDK.Objects
{
    internal class Structs
    {
        public struct RecvTable
        {
            public IntPtr m_pProps;
            private int m_nProps;
            private IntPtr m_pDecoder;
            private IntPtr m_pNetTableName;

            public RecvProp[] GetProps()
            {
                return MemorySettings.Memory.ReadArray<RecvProp>(m_pProps, m_nProps);
            }

            public int GetNumProps()
            {
                return m_nProps;
            }

            public IntPtr GetDecoder()
            {
                return m_pDecoder;
            }

            public string GetTablename()
            {
                return MemorySettings.Memory.ReadString(m_pNetTableName);
            }
        }

        public struct ClientClass
        {
            private IntPtr m_pCreateFn;
            private IntPtr m_pCreateEventFn;
            private IntPtr m_pClassName;
            private IntPtr m_pRecvTable;
            private IntPtr m_pNext;
            private ClassId m_ClassID;

            public string GetClassName()
            {
                return MemorySettings.Memory.ReadString(m_pClassName);
            }

            public ClassId GetClassId()
            {
                return m_ClassID;
            }

            public RecvTable GetRecvTable()
            {
                return MemorySettings.Memory.Read<RecvTable>(m_pRecvTable);
            }

            public ClientClass GetNextClass()
            {
                return MemorySettings.Memory.Read<ClientClass>(m_pNext);
            }
        }

        public struct RecvProp
        {
            private IntPtr m_pVarName;
            private SendPropType m_RecvType;
            private int m_Flags;
            private int m_StringBufferSize;
            private bool m_bInsideArray;
            private IntPtr m_pExtraData;
            private IntPtr m_pArrayProp;
            private IntPtr m_ArrayLengthProxy;
            private IntPtr m_ProxyFn;
            private IntPtr m_DataTableProxyFn;
            private IntPtr m_pDataTable;
            private int m_Offset;
            private int m_ElementStride;
            private int m_nElements;
            private IntPtr m_pParentArrayPropName;

            public string GetVarName()
            {
                return MemorySettings.Memory.ReadString(m_pVarName);
            }

            public string GetParentPropName()
            {
                return MemorySettings.Memory.ReadString(m_pParentArrayPropName);
            }

            public SendPropType GetPropType()
            {
                return m_RecvType;
            }

            public int GetFlags()
            {
                return m_Flags;
            }

            public int GetOffset()
            {
                return m_Offset;
            }

            public int GetNumElements()
            {
                return m_nElements;
            }

            public RecvTable GetDataTable()
            {
                return MemorySettings.Memory.Read<RecvTable>(m_pDataTable);
            }
        }

        public struct BaseBone
        {
            private float pad01;
            private float unk02;
            private float unk03;
            //private fixed byte skip1 [12];
            public float X;

            private float unk04;
            private float unk05;
            private float unk06;
            //private fixed byte skip2[12];
            public float Y;

            private float unk07;
            private float unk08;
            private float unk09;
            //private fixed byte skip3[12];
            public float Z;

            public Vector3 ToVector3D()
            {
                return new Vector3 { X = X, Y = Y, Z = Z };
            }

            public static implicit operator Vector3(BaseBone val)
            {
                return new Vector3(val.X, val.Y, val.Z);
            }
        }

        public struct GlowObject
        {
            public int NextFreeSlot;
            public IntPtr Entity;
            public float R;
            public float G;
            public float B;
            public float A;

            public byte m_bGlowAlphaCappedByRenderAlpha;
            public float m_flGlowAlphaFunctionOfMaxVelocity;
            public float m_flGlowAlphaMax;
            public float m_flGlowPulseOverdrive;

            public byte m_bRenderWhenOccluded;
            public byte m_bRenderWhenUnoccluded;
            public byte m_bFullBloom;

            public int m_nFullBloomStencilTestValue;
            public int m_nRenderStyle;
            public int m_nSplitScreenSlot;

            public static implicit operator LimitedGlowObject(GlowObject other)
            {
                return new LimitedGlowObject
                {
                    R = other.R,
                    G = other.G,
                    B = other.B,
                    A = other.A,
                    m_bGlowAlphaCappedByRenderAlpha = other.m_bGlowAlphaCappedByRenderAlpha,
                    m_flGlowAlphaFunctionOfMaxVelocity = other.m_flGlowAlphaFunctionOfMaxVelocity,
                    m_flGlowAlphaMax = other.m_flGlowAlphaMax,
                    m_flGlowPulseOverdrive = other.m_flGlowPulseOverdrive,
                    m_bRenderWhenOccluded = other.m_bRenderWhenOccluded,
                    m_bRenderWhenUnoccluded = other.m_bRenderWhenUnoccluded,
                    m_bFullBloom = other.m_bFullBloom,
                    m_nFullBloomStencilTestValue = other.m_nFullBloomStencilTestValue,
                    m_nRenderStyle = other.m_nRenderStyle,
                    m_nSplitScreenSlot = other.m_nSplitScreenSlot
                };
            }
        }

        public struct LimitedGlowObject
        {
            public float R;
            public float G;
            public float B;
            public float A;

            public byte m_bGlowAlphaCappedByRenderAlpha;
            public float m_flGlowAlphaFunctionOfMaxVelocity;
            public float m_flGlowAlphaMax;
            public float m_flGlowPulseOverdrive;

            public byte m_bRenderWhenOccluded;
            public byte m_bRenderWhenUnoccluded;
            public byte m_bFullBloom;

            public int m_nFullBloomStencilTestValue;
            public int m_nRenderStyle;
            public int m_nSplitScreenSlot;
        }
    }
}
