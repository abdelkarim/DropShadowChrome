using System;

namespace DropShadowChrome.Lib
{
    [Flags]
    internal enum MIIM : uint
    {
        ID = 0x00000002,
        STRING = 0x00000040,
        STATE = 0x00000001,
        TYPE = 0x00000010
    }
}