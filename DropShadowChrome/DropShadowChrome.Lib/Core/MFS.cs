using DropShadowChrome.Lib.Core;

namespace DropShadowChrome.Lib
{
    internal enum MFS : uint
    {
        GRAYED = 0x00000003,
        DISABLED = MFS.GRAYED,
        CHECKED = MF.CHECKED,
        HILITE = MF.HILITE,
        ENABLED = MF.ENABLED,
        UNCHECKED = MF.UNCHECKED,
        UNHILITE = MF.UNHILITE,
        DEFAULT = MF.DEFAULT
    }
}