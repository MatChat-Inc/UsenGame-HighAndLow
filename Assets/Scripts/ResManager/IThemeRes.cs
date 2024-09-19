using UnityEngine;

public interface IThemeRes
{
    //******************** 贴图 ***********************//
    // 游戏的主题背景
    public string GetThemeBgTexturePath();

    // 首页的背景装饰
    public string GetThemeBgDecorateTexturePath();

    // 游戏页的顶部装饰
    public string GetThemeGameViewTopDecorateTexturePaht();

    // 数字贴图
    public string GetNumberTexturePath();

    // 数字默认背景贴图
    public string GetNumberCellBgDefaultTexturePath();

    // 数字选中效果背景贴图
    public string GetNumberCellBgHighlightTexturePath();


    //******************** spine ***********************//

    // 首页的spine动画
    public string GetHomeSpinePrefabPath();

    // bingo和reach的spine动画
    public string GetBingoSpinePrefabPath();

    // 转盘的背景spine动画
    public string GetRotateBgSpinePrefabPath();







    //******************** 音频 ***********************//

    public string GetNormalBgmPath();

    public string GetWillReachBgmPath();

    public string GetBingoAudioPath();

    public string GetReachAudioPath();

    public string GetNumberCheckDownAudioPath();

    public string GetNumberRotateAudioPath();

    //******************** 颜色 ***********************//
    public Color GetNumberCheckedTextColor();
}