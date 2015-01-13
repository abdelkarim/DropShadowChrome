using DropShadowChrome.Lib.Core;

namespace DropShadowChrome.Lib
{
    internal enum MFT : uint
    {
        STRING = MF.STRING,
        BITMAP = MF.BITMAP,
        MENUBARBREAK = MF.MENUBARBREAK,
        MENUBREAK = MF.MENUBREAK,
        OWNERDRAW = MF.OWNERDRAW,
        RADIOCHECK = 0x00000200,
        SEPARATOR = MF.SEPARATOR,
        RIGHTORDER = 0x00002000,
        RIGHTJUSTIFY = MF.RIGHTJUSTIFY
    }
}