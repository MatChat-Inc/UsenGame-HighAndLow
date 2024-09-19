

using UnityEngine;

public class DefaultThemeResConfig : IThemeRes
{
    public string GetBingoSpinePrefabPath()
    {
        return "DefaultTheme/Effects/Bingo/EffectPanel";
    }

    public string GetHomeSpinePrefabPath()
    {
        return "DefaultTheme/Effects/Home/EffectPanel";
    }

    public string GetRotateBgSpinePrefabPath()
    {
        return "DefaultTheme/Effects/Rotate/EffectPanel";
    }

    public string GetNumberCellBgDefaultTexturePath()
    {
        return "DefaultTheme/CellBg/Panel_Black";
    }

    public string GetNumberCellBgHighlightTexturePath()
    {
        return "DefaultTheme/CellBg/Panel_Yellow";
    }

    public string GetNumberTexturePath()
    {
        return "DefaultTheme/Numbers/Atari_No{0}";
    }

    public string GetThemeBgTexturePath()
    {
        return "DefaultTheme/top_bg";
    }

    public string GetThemeBgDecorateTexturePath()
    {
        return "DefaultTheme/01_Maku_01";
    }

    public string GetThemeGameViewTopDecorateTexturePaht()
    {
        return "DefaultTheme/03_IthimonMaku";
    }


    public string GetNormalBgmPath()
    {
        return "DefaultTheme/Audio/bgm";
    }

    public string GetWillReachBgmPath()
    {
        return "DefaultTheme/Audio/will_reach_sample";
    }

    public string GetBingoAudioPath()
    {
        return "DefaultTheme/Audio/bingo_sample";
    }

    public string GetReachAudioPath()
    {
        return "DefaultTheme/Audio/Reach_click_sample";
    }

    public string GetNumberCheckDownAudioPath()
    {
        return "DefaultTheme/Audio/Number_check_sample";
    }

    public string GetNumberRotateAudioPath()
    {
        return "DefaultTheme/Audio/Number_rotate_sample";
    }

    public Color GetNumberCheckedTextColor()
    {
        return Color.white;
    }
}