#if UNITY_EDITOR

using System.Collections.Generic;

public class ftErrorCodes
{
    static Dictionary<int, string> ftraceMap = new Dictionary<int, string>
    {
        {1, "Unknown error. See .ftracelog.txt for details."},
        {2, "Error selecting pass"},
        {5120, "Can't open lms.bin"},
        {984, "lmlod.bin doesn't match lms.bin"},
        {500, "Can't load geometry data. See .ftracelog.txt for details."},
        {501, "Can't load UVGBuffer smooth position"},
        {502, "Can't load UVGBuffer face normal"},
        {505, "Can't load trimarks.bin"},
        {5005, "Can't load sky.bin"},
        {500599, "Can't load ao.bin"},
        {5005991, "Can't load sss.bin"},
        {507, "Can't load vbtraceUV0.bin"},
        {508, "Can't load UVGBuffer tangent"},
        {550, "Can't load light data. See .ftracelog.txt for details."},
        {557, "Can't load alpha IDs. See .ftracelog.txt for details."},
        {512, "Can't load compositing data. See .ftracelog.txt for details."},
        {51298, "Can't open addao.bin"},
        {875, "Can't load heightmap. See .ftracelog.txt for details."},
        {90, "Can't load normal to compose. See .ftracelog.txt for details."},
        {91, "Can't load lightmap to compose. See .ftracelog.txt for details."},
        {910, "Can't load direction to compose. See .ftracelog.txt for details."},
        {92, "Can't load lightmap to compose. See .ftracelog.txt for details."},
        {920, "Can't load lightmap to compose. See .ftracelog.txt for details."},
        {921, "Can't load emission. See .ftracelog.txt for details."},
        {93, "Can't load lightmap to compose. See .ftracelog.txt for details."},
        {94, "Can't load lightmap. See .ftracelog.txt for details."},
        {940, "Can't read direction for GI. See .ftracelog.txt for details."},
        {95, "Can't read lightmap for GI. See .ftracelog.txt for details."},
        {510, "Can't write composed lightmap. See .ftracelog.txt for details."},
        {514, "Can't write composed lightmap. See .ftracelog.txt for details."},
        {7500, "Can't load UVGBuffer normal or position"},
        {5090, "Can't decompress UVGBuffer normal"},
        {5091, "Can't decompress UVGBuffer position"},
        {5092, "Can't decompress UVGBuffer smooth position"},
        {5093, "Can't decompress UVGBuffer face normal"},
        {5083, "Can't decompress UVGBuffer tangent"},
        {7007, "Can't load direct.bin"},
        {7771, "Can't read sky texture"},
        {7772, "Can't read light texture"},
        {888, "No texture name for cubemaplight"},
        {8008, "Can't load direct lighting."},
        {1000, "Can't read albedo for GI. See .ftracelog.txt for details."},
        {1001, "Can't read lightmap for GI. See .ftracelog.txt for details."},
        {1007, "Can't read direction for GI. See .ftracelog.txt for details."},
        {1888, "Failed to initialize"},
        {10000, "Can't load gi.bin"},
    };

    static Dictionary<int, string> combineMasksMap = new Dictionary<int, string>
    {
        {23, "Can't load texture"},
        {501, "Can't write file. See console for details."},
        {5, "Failed to save TGA file. See console for details."}
    };

    static Dictionary<int, string> denoiserMap = new Dictionary<int, string>
    {
        {2, "Incorrect arguments"},
        {3, "Incorrect tile size. Must be between 64 and 8192"},
        {500, "Can't load texture. See console for details."},
        {5001, "Can't load texture. See console for details."},
        {5002, "Can't load texture. See console for details."},
        {5003, "Can't load texture. See console for details."},
        {4, "Incorrect tile size. Must be width%tile == height%tile == 0"},
        {501, "Can't write file. See console for details."},
        {505, "Unknown error (old driver?)"}
    };

    static Dictionary<int, string> h2hMap = new Dictionary<int, string>
    {
        {23, "Can't load texture. See console for details."},
        {2, "Failed to get image data from DDS. See console for details."},
        {3, "Failed to init D3D11"},
        {4, "Failed to convert"},
        {45, "Failed to transform pixels"},
        {5, "Failed to save HDR file. See console for details."}
    };

    static Dictionary<int, string> i2tMap = new Dictionary<int, string>
    {
        {1, "Incorrect arguments"},
        {2, "Can't read file. See console for details."},
        {3, "Can't write file. See console for details."},
        {4, "IES file is not valid. See console for details."},
        {5, "IES file uses unknown symmetry mode. See console for details."}
    };

    static Dictionary<int, string> seamfixerMap = new Dictionary<int, string>
    {
        {1, "Incorrect arguments"},
        {2, "Failed to init D3D11"},
        {501, "Can't load vbtraceTex.bin"},
        {10, "Can't load lms.bin"},
        {600, "Can't load lightmap"},
        {22, "Can't create D3D11 resource"},
        {3, "Can't create D3D11 resource"},
        {4, "Can't allocate RAM texture"},
        {8, "Can't save texture. See console for details."}
    };

    static Dictionary<int, string> lmrMap = new Dictionary<int, string>
    {
        {2, "Failed to init D3D11 or create resource"},
        {3, "Can't create D3D11 resource"},
        {601, "Can't load lodmask"},
        {602, "Can't decompress lodmask (unexpected size)"},
        {32, "Can't create mip texture"},
        {33, "Can't create mip render target"},
        {34, "Can't create mip shader resource view"},
        {4, "Can't allocate RAM mip texture"},
        {8, "Can't save texture"}
    };

    public static string TranslateFtrace(int code)
    {
        string text;
        if (!ftraceMap.TryGetValue(code, out text)) text = "Unknown error";
        return text + " (" + code + ")";
    }

    public static string TranslateCombineMasks(int code)
    {
        string text;
        if (!combineMasksMap.TryGetValue(code, out text)) text = "Unknown error";
        return text + " (" + code + ")";
    }

    public static string TranslateDenoiser(int code)
    {
        string text;
        if (!denoiserMap.TryGetValue(code, out text)) text = "Unknown error";
        return text + " (" + code + ")";
    }

    public static string TranslateH2H(int code)
    {
        string text;
        if (!h2hMap.TryGetValue(code, out text)) text = "Unknown error";
        return text + " (" + code + ")";
    }

    public static string TranslateI2T(int code)
    {
        string text;
        if (!i2tMap.TryGetValue(code, out text)) text = "Unknown error";
        return text + " (" + code + ")";
    }

    public static string TranslateSeamfixer(int code)
    {
        string text;
        if (!seamfixerMap.TryGetValue(code, out text)) text = "Unknown error";
        return text + " (" + code + ")";
    }

    public static string TranslateLMRebake(int code)
    {
        string text;
        if (!lmrMap.TryGetValue(code, out text)) text = "Unknown error";
        return text + " (" + code + ")";
    }

    public static string Translate(string app, int code)
    {
        if (app == "ftrace") return TranslateFtrace(code);
        if (app == "combineMasks") return TranslateCombineMasks(code);
        if (app == "denoiseDir") return TranslateDenoiser(code);
        if (app == "denoiseMask") return TranslateDenoiser(code);
        if (app == "denoiser") return TranslateDenoiser(code);
        if (app == "denoiseSH") return TranslateDenoiser(code);
        if (app == "halffloat2hdr") return TranslateH2H(code);
        if (app == "ies2tex") return TranslateI2T(code);
        if (app == "rgba2tga") return TranslateCombineMasks(code);
        if (app == "seamfixer") return TranslateSeamfixer(code);
        return ""+code;
    }
}

#endif
