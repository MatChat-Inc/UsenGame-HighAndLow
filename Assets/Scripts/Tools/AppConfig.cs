// AppConfig singleton

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AppConfig
{
    public static string __REOPEN_DATA__ = "__REOPEN_GAME_DATA__";
    private static AppConfig _instance;
    public static AppConfig Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new AppConfig();
            }
            return _instance;
        }
    }

    private int _cellCount = 0;

    public int MaxCellCount {
        set
        {
            _cellCount = value;
            PreferencesStorage.SaveInt("__MAX_CELL_COUNT__", _cellCount);
        }
        get 
        {
            if (_cellCount == 0) {
                _cellCount = PreferencesStorage.ReadInt("__MAX_CELL_COUNT__", 75);
            }
            return _cellCount;
        }
    }

    private int _bgmVolume = 5;

    public int BGMVolume {
        set
        {
            _bgmVolume = value;
            PreferencesStorage.SaveInt("__BGM_VOLUME__", value);
        }
        get
        {
            _bgmVolume = PreferencesStorage.ReadInt("__BGM_VOLUME__", 5);
            return _bgmVolume;
        }
    }

    private int _effectVolume = 5;
    public int EffectVolume {
        set
        {
            _effectVolume = value;
            PreferencesStorage.SaveInt("__EFFECT_VOLUME__", value);
        }
        get
        {
            _effectVolume = PreferencesStorage.ReadInt("__EFFECT_VOLUME__", 5);
            return _effectVolume;
        }
    }

    private int _themeSelectedIdx = 0;
    public int ThemeSelectedIdx {
        set
        {
            _themeSelectedIdx = value;
            ThemeResManager.Instance.SetThemeType((EThemeTypes)value);
            PreferencesStorage.SaveInt("__THEME_SELECTED__", value);
        }
        get
        {
            _themeSelectedIdx = PreferencesStorage.ReadInt("__THEME_SELECTED__", 5);
            return _themeSelectedIdx;
        }
    }
    
    private int _commendationVideoOption = 0;
    public int CommendationVideoOption {
        set
        {
            _commendationVideoOption = value;
            ThemeResManager.Instance.SetThemeType((EThemeTypes)value);
            PreferencesStorage.SaveInt("settings.video.commendation", value);
        }
        get
        {
            _commendationVideoOption = PreferencesStorage.ReadInt("settings.video.commendation", 0);
            return _commendationVideoOption;
        }
    }

    private int _bgmSelectedIdx = 0;
    public int BgmSelectedIdx {
        set
        {
            _bgmSelectedIdx = value;
            PreferencesStorage.SaveInt("__BGM_SELECTED__", value);
        }
        get
        {
            _bgmSelectedIdx = PreferencesStorage.ReadInt("__BGM_SELECTED__", 5);
            return _bgmSelectedIdx;
        }
    }

    private int _winnerSelectedIdx = 0;
    public int WinnerSelectedIdx {
        set
        {
            _winnerSelectedIdx = value;
            PreferencesStorage.SaveInt("__WINNER_SELECTED__", value);
        }
        get
        {
            _winnerSelectedIdx = PreferencesStorage.ReadInt("__WINNER_SELECTED__", 5);
            return _winnerSelectedIdx;
        }
    }

    private GameDataHandler _gameData;

    public GameDataHandler GameData {
        set
        {
            _gameData = value;
            PreferencesStorage.SaveString(__REOPEN_DATA__, JsonSerializablity.Serialize(value));
        }
        get
        {
            if (_gameData == null) {
                var gameDataStr = PreferencesStorage.ReadString(__REOPEN_DATA__, null);
                if (gameDataStr != null && gameDataStr.Length > 0)
                    _gameData = JsonSerializablity.Deserialize<GameDataHandler>(gameDataStr);
            }
            return _gameData;
        }
    }

    public void ClearGameData() {
        AppConfig.Instance.GameData = null;
    }

    public bool HasHistoryGameData()
    {
        var saveData = PreferencesStorage.ReadString(AppConfig.__REOPEN_DATA__, null);
        return saveData != null && saveData.Length > 0;
    }

    public float rotateEaseExtraTime = 0.0f;

    private int _selectedGameIndex = 2;

    public int SelectedGameIndex {
        set
        {
            _selectedGameIndex = value;
            PreferencesStorage.SaveInt("__SELECT_GAME_INDEX__", value);
        }
        get
        {
            _selectedGameIndex = PreferencesStorage.ReadInt("__SELECT_GAME_INDEX__", 2);
            return _selectedGameIndex;
        }
    }

    public RootViewType HomeSceneRootViewType;

    public IViewOperater GetSceneRootViewType() {
        switch (HomeSceneRootViewType)
        {
            case RootViewType.HighAndLow:
                return new HighAndLowHomeView();
            // case RootViewType.Bingo:
            //     return new BingoHomeView();
            // case RootViewType.Roulette:
            //     return new RouletteHomeView();
            // case RootViewType.BingoSettings:
            //     return new BingoSettingsView();
            default:
                return new HighAndLowHomeView();
        }
    }


    private int _currentHighAndLowTimer = 10;

    public int CurrentHighAndLowTimer {
        set
        {
            _currentHighAndLowTimer = value;
            PreferencesStorage.SaveInt("__CURRENT_HIGHANDLOW_TIMER__", value);
        }
        get
        {
            _currentHighAndLowTimer = PreferencesStorage.ReadInt("__CURRENT_HIGHANDLOW_TIMER__", 10);
            return _currentHighAndLowTimer;
        }
    }

    private List<int> _checkedPokers = new List<int>();
    public List<int> CheckedPokers {
        set
        {
            _checkedPokers = value;
            var jsonStr = string.Join(",", value.ToArray());
            PreferencesStorage.SaveString("__CHECKED_POKERS__", jsonStr);
        }
        get {
            var jsonStr = PreferencesStorage.ReadString("__CHECKED_POKERS__", "");
            if (jsonStr.Length > 0) {
                string[] strArray = jsonStr.Split(",");
                _checkedPokers = strArray.ToList<string>().ConvertAll<int>((v)=>int.Parse(v));
            }
            return _checkedPokers;
        }
    }
}