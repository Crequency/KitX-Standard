namespace KitX.FileFormats.CSharp.ExtensionsPackage;

internal static class Header
{
    public static byte[] header =
    [
        073, // I
        116, // t
        032, //
        105, // i
        115, // s
        032, //
        097, // a
        032, //
        075, // K
        088, // X
        080, // P
        032, //
        102, // f
        105, // i
        108, // l
        101, // e

        // It is a KXP file
    ];

    public static bool IsKXP(ref byte[] src, int index = 0, int count = 16)
    {
        for (int i = index; i < index + count; ++i)
            if (src[i] != header[i])
                return false;
        return true;
    }
}
