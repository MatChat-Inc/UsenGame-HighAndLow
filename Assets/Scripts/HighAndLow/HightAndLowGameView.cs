using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Luna;
using Luna.UI.Navigation;
using UnityEngine.AddressableAssets;
using UnityEngine.Playables;
using UnityEngine.ResourceManagement.AsyncOperations;
using USEN;
using USEN.Games.Common;
using USEN.Games.Roulette;
using Random = UnityEngine.Random;

public class HighAndLowGameView : AbstractView, IViewOperater
{
    string m_prefabPath = "HighAndLow/HighAndLowGamePanel";
    Transform m_pokerStartTransform;
    Transform m_pokerShowTransform1;
    Transform m_pokerShowTransform2;
    GameObject m_pokerShowParticleEffect;
    GameObject m_pokerShowSmokeEffect;
    Transform m_pokerTrashTransform;
    Text m_checkRestCountLabel;
    List<GameObject> m_checkedItemList = new();
    List<int> m_pokerPool = new();
    Dictionary<int, int> m_pokerRestCount = new();
    List<int> m_checkedPokers = new();
    GameObject m_timer;
    bool m_waitTrigger;
    Text m_timeLabel;
    Button m_historyBtn;
    Button m_terminalBtn;
    Button m_rouletteBtn;
    Button m_winnerBtn;
    Button m_confirmBtn;
    Button m_startTimerBtn;
    GameObject m_pokerTemplate;
    GameObject m_resultLow;
    GameObject m_resultHigh;
    bool m_resultIsShowing;
    bool m_isGameFinished;
    GameObject m_pokersTile;
    Coroutine m_updateTimeCoroutine;

    HighAndLowRouletteView m_rouletteView;
    HighAndLowHomeView m_homeView;
    HighAndLowTerminalView m_terminalView;
    bool m_isWaitContinue;
    bool m_isWaitTimer;
    bool m_isShowTimer;
    int m_lastPoker = -1;
    
    private PlayableDirector _finishDirector;
    // private ResultPlayerDirector _resultDirector;
    
    private bool _isPopupViewShowed;
    private bool _isRouletteShowing;
    private bool _isCommendationShowing;
    private bool _isResultShowing;
    private bool _isResultAnimating;
    
    private AsyncOperationHandle<AudioClip>? _audioClipHandle;
    
    private Navigator _navigator;
    
    public void Build()
    {
        m_isGameFinished = false;
        m_mainViewGameObject = LoadViewGameObject(m_prefabPath, ViewManager.Instance.GetRootTransform());
        
        _navigator ??= Navigator.Create(m_mainViewGameObject);

        m_pokerStartTransform = m_mainViewGameObject.transform.Find("PokerStart");
        m_pokerShowTransform1 = m_mainViewGameObject.transform.Find("PokerShow1");
        m_pokerShowTransform2 = m_mainViewGameObject.transform.Find("PokerShow2");
        m_pokerShowParticleEffect = m_mainViewGameObject.transform.Find("PokerShow2/ParticleEffect").gameObject;
        m_pokerShowSmokeEffect = m_mainViewGameObject.transform.Find("PokerShow2/SmokeEffect").gameObject;
        m_pokerTrashTransform = m_mainViewGameObject.transform.Find("PokerShowTrash");

        m_checkRestCountLabel = m_mainViewGameObject.transform.Find("PokerCheckList/RestCountLabel").GetComponent<Text>();

        m_timer = m_mainViewGameObject.transform.Find("Foreground/Timer").gameObject;
        m_timeLabel = m_mainViewGameObject.transform.Find("Foreground/Timer/TimeLabel").GetComponent<Text>();

        m_historyBtn = m_mainViewGameObject.transform.Find("BottomPanel/HistoryBtn").GetComponent<Button>();
        m_historyBtn.onClick.AddListener(OnClickedHistoryButton);
        m_terminalBtn = m_mainViewGameObject.transform.Find("BottomPanel/StopBtn").GetComponent<Button>();
        m_terminalBtn.onClick.AddListener(OnClickedTerminalBtn);
        m_rouletteBtn = m_mainViewGameObject.transform.Find("BottomPanel/RouletteBtn").GetComponent<Button>();
        m_rouletteBtn.onClick.AddListener(OnClickedRouletteBtn);
        m_winnerBtn = m_mainViewGameObject.transform.Find("BottomPanel/WinnerBtn").GetComponent<Button>();
        m_winnerBtn.onClick.AddListener(OnClickedWinnerBtn);
        m_confirmBtn = m_mainViewGameObject.transform.Find("BottomPanel/ConfirmBtn").GetComponent<Button>();
        m_confirmBtn.onClick.AddListener(OnClickedConfirmBtn);
        m_startTimerBtn = m_mainViewGameObject.transform.Find("BottomPanel/StartTimerBtn").GetComponent<Button>();
        m_startTimerBtn.onClick.AddListener(OnClickedStartTimerBtn);

        m_pokerTemplate = m_mainViewGameObject.transform.Find("Poker2D").gameObject;
        m_resultLow = m_mainViewGameObject.transform.Find("Foreground/ResultLow").gameObject;
        m_resultHigh = m_mainViewGameObject.transform.Find("Foreground/ResultHigh").gameObject;
        m_pokersTile = m_mainViewGameObject.transform.Find("Bg/PokerTile").gameObject;
        
        _finishDirector = m_mainViewGameObject.transform.Find("Foreground/Finish Animation/Timeline").GetComponent<PlayableDirector>();
        // _resultDirector = m_mainViewGameObject.transform.Find("Foreground/Result Player/Timeline").GetComponent<ResultPlayerDirector>();
        
        AssetUtils.LoadAsync<CommendView>().ContinueWith(task => {
            var go = task.Result;
            var commendView = go.GetComponent<CommendView>();
            if (commendView != null) 
                _audioClipHandle = commendView.PreloadAudio();
        }, TaskScheduler.FromCurrentSynchronizationContext());

        AudioManager.Instance.keydownAudioSource.mute = true;
        
        ResumeGame();
        
        // Quick test to the last card
        if (m_pokerPool.Count > 50)
            for (int i = 0; i < 48; i++)
                GetRandomPokerFromPool();
    }

    public override void OnDestroy() {
        if (m_updateTimeCoroutine != null)
            ViewManager.Instance.StopCoroutine(m_updateTimeCoroutine);
        m_timeLabel = null;
        m_mainViewGameObject = null;
        m_checkedItemList.Clear();
        m_pokerPool.Clear();
        m_checkedPokers.Clear();
        
        AssetUtils.Unload<CommendView>();
        if (_audioClipHandle != null)
            Addressables.Release(_audioClipHandle.Value);
    }
    
    public void Hide()
    {
        m_mainViewGameObject.SetActive(false);
        AudioManager.Instance.keydownAudioSource.mute = false;
    }

    public void OnAndroidKeyDown(string keyName)
    {
        if (_isPopupViewShowed || _isRouletteShowing || _isCommendationShowing)
            return;
        
        if (keyName == "blue")
        {
            OnClickedHistoryButton();
        }

        if (keyName == "red")
        {
            OnClickedStartTimerBtn();
        }

        if (keyName == "yellow")
        {
            OnClickedWinnerBtn();
        }

        if (keyName == "green")
        {
            OnClickedRouletteBtn();
        }
    }

    public void OnThemeTypeChanged()
    {
        
    }

    public void Show()
    {
        m_mainViewGameObject.SetActive(true);
        
        if (m_pokerShowTransform1.childCount == 0) 
        {
            PlayFirst();
        }
        else if (m_isWaitContinue) 
        {
            // 重连继续上一局
            if (m_pokerPool.Count != 0)
            { 
                R.Audios.SfxSendPoker.Play();
                var backFaceGO = CreatePoker(EPokers.BackFace);
                (backFaceGO.transform as RectTransform).rotation = Quaternion.Euler(0f, 180f, 60f);
                backFaceGO.transform.SetParent(m_pokerShowTransform2);
                (backFaceGO.transform as RectTransform).DOAnchorPosX(0, 1).SetLink(backFaceGO);
                var tween = (backFaceGO.transform as RectTransform).DOLocalRotate(new Vector3(0f, 180f, 0f), 1).SetLink(backFaceGO);
                tween.onComplete += WaitTimer;
                m_isWaitContinue = false; 
            }
        }
    }

    public void Update()
    {
        if (!m_mainViewGameObject.activeInHierarchy)
            return;
        
        // if (Input.GetKey(KeyCode.Alpha1))
        // {
        //     _finishDirector.Play();
        // }
        
        if (Input.GetButtonDown("Cancel")) {
            if (m_mainViewGameObject.activeInHierarchy)
                OnClickedTerminalBtn();
        }
        
        if (m_waitTrigger && !_isPopupViewShowed && !_isRouletteShowing) {
            if (Input.GetKeyDown(KeyCode.UpArrow) ||
                Input.GetKeyDown(KeyCode.DownArrow) ||
                Input.GetKeyDown(KeyCode.Return) || 
                Input.GetButtonDown("Submit")) {
                m_waitTrigger = false;
            }
        }
        
        if (m_isShowTimer && !_isPopupViewShowed && !_isRouletteShowing) {
            if (Input.GetKeyDown(KeyCode.UpArrow) ||
                Input.GetKeyDown(KeyCode.DownArrow) ||
                Input.GetKeyDown(KeyCode.Return) ||
                Input.GetButtonDown("Submit")) {
                m_isShowTimer = false;
            }
        }

        if (!m_isGameFinished && !_isPopupViewShowed && !_isRouletteShowing && !_isResultAnimating) {
            if (Input.GetKeyDown(KeyCode.Return) || Input.GetButtonDown("Submit")) {
                OnClickedConfirmBtn();
            }
        }

        _isPopupViewShowed = m_terminalView?.MainGameObject.activeInHierarchy == true;
        
        m_startTimerBtn.gameObject.SetActive(m_isWaitTimer && !_isResultShowing);
    }

    void OnClickedHistoryButton() {
        SFXManager.Play(R.Audios.SfxConfirm);
        Navigator.Push<HighLowHistoryView>(view => view.pokers = m_checkedPokers);
    }

    void OnClickedTerminalBtn() {
        SFXManager.Play(R.Audios.SfxBack);
        if (m_terminalView == null) {
            m_terminalView = new HighAndLowTerminalView();
            m_terminalView.Build(m_mainViewGameObject.transform);
        }
        // ViewManager.Instance.Push(m_terminalView);
        m_terminalView.Show();
    }

    async void OnClickedRouletteBtn() {
        SFXManager.Play(R.Audios.SfxConfirm);
        
        await Navigator.Push<RouletteGameSelectionView>((view) => {
            view.Category = RouletteManager.Instance.GetCategory("バツゲーム");
            _isRouletteShowing = true;
            // AudioManager.Instance.PauseBgm();
            R.Audios.BgmRouletteLoop.PlayAsBgm();
            
            if (RoulettePreferences.DisplayMode == RouletteDisplayMode.Random)
            { 
                Navigator.Push<USEN.Games.Roulette.RouletteGameView>(async (view) => {
                    view.RouletteData = RouletteManager.Instance.GetRandomRoulette();
                    _isRouletteShowing = true;
                });
            }
        });
        
        BgmManager.Play(R.Audios.BgmHighLow);
        // AudioManager.Instance.UnPauseBgm();
        
        await UniTask.NextFrame();
        _isRouletteShowing = false;
    }

    async void OnClickedWinnerBtn() 
    {
        _isCommendationShowing = true;
        AudioManager.Instance.PauseBgm();
        BgmManager.Pause();
        await Navigator.Push<CommendView>();
        AudioManager.Instance.UnPauseBgm();
        BgmManager.Resume();
        SFXManager.Play(R.Audios.SfxBack);
        _isCommendationShowing = false;
    }

    void OnClickedConfirmBtn()
    {
        _isResultShowing = false;
        
        if (m_isGameFinished) {
            return;
        }
        if (m_resultIsShowing){
            // 将牌丢弃
            PlayLoop();
            HideResultButtons();
            m_resultIsShowing = false;
        } else if (m_isWaitTimer )
        {
            ShowResult();
        }
    }

    void OnClickedStartTimerBtn() {
        if (m_isWaitTimer) {
            ShowTimer();
            m_isWaitTimer = false;
        }
        else if (m_isShowTimer) {
            m_isShowTimer = false;
        }
    }

    void ShowResultButtons() {
        m_rouletteBtn.gameObject.SetActive(true);
        m_winnerBtn.gameObject.SetActive(true);
    }

    void HideResultButtons() {
        m_rouletteBtn.gameObject.SetActive(false);
        m_winnerBtn.gameObject.SetActive(false);
    }

    void PlayFirst() {
        var pokerGO = CreateRandomPokerFromPool();
        (pokerGO.transform as RectTransform).rotation = Quaternion.Euler(0f, 0f, 60f);
        pokerGO.transform.SetParent(m_pokerShowTransform1);
        (pokerGO.transform as RectTransform).DOAnchorPosX(0, 1).SetLink(pokerGO);
        (pokerGO.transform as RectTransform).DOLocalRotate(Vector3.zero, 1).SetLink(pokerGO);

        var backFaceGO = CreatePoker(EPokers.BackFace);
        (backFaceGO.transform as RectTransform).rotation = Quaternion.Euler(0f, 180f, 60f);
        backFaceGO.transform.SetParent(m_pokerShowTransform2);
        (backFaceGO.transform as RectTransform).DOAnchorPosX(0, 1).SetDelay(2).SetLink(backFaceGO);
        var tween = (backFaceGO.transform as RectTransform).DOLocalRotate(new Vector3(0f, 180f, 0f), 1).SetDelay(2).SetLink(backFaceGO);
        tween.onComplete += WaitTimer;
    }

    void PlayLoop() {
        R.Audios.SfxSendPoker.Play();
        m_resultLow.SetActive(false);
        m_resultHigh.SetActive(false);
        var leftPokerGO = m_pokerShowTransform1.GetChild(0).gameObject;
        leftPokerGO.transform.SetParent(m_pokerTrashTransform);
        var angle = Random.Range(20, 70);
        (leftPokerGO.transform as RectTransform).DOAnchorPosX(0, 1).SetLink(leftPokerGO);
        (leftPokerGO.transform as RectTransform).DOLocalRotate(new Vector3(0,0,angle), 1).SetLink(leftPokerGO);


        var rightPokerGO = m_pokerShowTransform2.GetChild(2).gameObject;
        rightPokerGO.transform.SetParent(m_pokerShowTransform1);
        (rightPokerGO.transform as RectTransform).DOAnchorPosX(0, 1).SetLink(rightPokerGO);

        var backFaceGO = CreatePoker(EPokers.BackFace);
        (backFaceGO.transform as RectTransform).rotation = Quaternion.Euler(0f, 180f, 60f);
        backFaceGO.transform.SetParent(m_pokerShowTransform2);
        (backFaceGO.transform as RectTransform).DOAnchorPosX(0, 1).SetLink(backFaceGO);
        var tween = (backFaceGO.transform as RectTransform).DOLocalRotate(new Vector3(0f, 180f, 0f), 1).SetLink(backFaceGO);
        tween.onComplete += WaitTimer;
    }

    void WaitTimer() {
        m_isWaitTimer = true;
    }

    void ShowTimer() {
        AudioManager.Instance.PlayTimerStartEffect();
        m_timer.SetActive(true);
        m_waitTrigger = true;
        m_isShowTimer = true;

        ViewManager.Instance.StartCoroutine(UpdateTimeLabel());
    }

    IEnumerator<WaitForSeconds> UpdateTimeLabel() {
        var timer = AppConfig.Instance.CurrentHighAndLowTimer + 1;
        // m_isShowTimer = true;
        
        var audioClip = AppConfig.Instance.CurrentHighAndLowTimer switch {
            > 25 and <= 35 => R.Audios.Sfx30Sec,
            > 15 and <= 25 => R.Audios.Sfx20Sec,
            _ => R.Audios.Sfx10Sec,
        };
        
        audioClip.Play();
        
        while (timer-- > 0) 
        {
            if (!m_isShowTimer) {
                // 提前结束
                timer = 0;
                AudioManager.Instance.StopEffectAudio();
                SFXManager.Stop();
                break;
            }
            if (m_timeLabel != null)
                m_timeLabel.text = timer.ToString();
            yield return new WaitForSeconds(1);
        }
        if (m_mainViewGameObject != null)
            ShowResult();
    }

    void ShowResult() {
        _isResultShowing = true;
        _isResultAnimating = true;
        
        HideTimer();
        ShowResultButtons();

        var leftPoker = m_lastPoker;
        var pokerType = GetRandomPokerFromPool();
        var pokerSprite = GetSpriteWithPoker(pokerType);
        
        var backFaceGO = m_pokerShowTransform2.GetChild(2).gameObject;
        
        // Set front side of poker
        var poker = backFaceGO.GetComponent<Poker2D>();
        if (pokerType != EPokers.BackFace)
            poker.front = GetSpriteWithPoker(pokerType);

        var scale1 = backFaceGO.transform.DOScale(1.4f, 1).SetLink(backFaceGO).OnComplete(()=> {});
        
        var rotate1 = backFaceGO.transform.DORotate(Vector3.zero, 1).SetDelay(0).SetLink(backFaceGO).OnComplete(()=> {
            m_pokerShowParticleEffect.SetActive(true);
        });
        
        var a3 = poker.PlayShineAnimation().OnComplete(()=> {});
        var a4 = poker.PlayShineAnimation(0f, 0.5f).SetDelay(1f).OnComplete(()=> {});
        
        var punchScale = backFaceGO.transform.DOScale(1.6f, 0.2f).SetDelay(0).SetEase(Ease.InSine).SetLink(backFaceGO).OnComplete(()=> {});
        
        var scale2 = backFaceGO.transform
            .DOScale(1, 0.4f)
            .SetDelay(0)
            .SetEase(Ease.OutSine)
            .SetLink(backFaceGO)
            .OnComplete(()=> {
            m_pokerShowParticleEffect.SetActive(false);
            m_pokerShowSmokeEffect.SetActive(true);
        });
        
        var scale3 =backFaceGO.transform.DOScale(1, 0).SetDelay(0).SetLink(backFaceGO).OnComplete(()=> {
            UniTask.Delay(TimeSpan.FromSeconds(2)).ContinueWith(() =>
            {
                m_pokerShowSmokeEffect.SetActive(false);
            });

            // 判断输赢
            if (EPokersHelper.GetPokerValue((EPokers)leftPoker) < EPokersHelper.GetPokerValue(pokerType))
            {
                R.Audios.SfxHigh.Play();
                m_resultHigh.SetActive(true);
            }

            if (EPokersHelper.GetPokerValue((EPokers)leftPoker) > EPokersHelper.GetPokerValue(pokerType))
            {
                R.Audios.SfxLow.Play();
                m_resultLow.SetActive(true);
            }
            m_resultIsShowing = true;
            
            if (m_isGameFinished)
            {
                // AudioManager.Instance.PlayFinishEffect();
                UniTask.Delay(TimeSpan.FromSeconds(5)).ContinueWith(() =>
                {
                    m_resultHigh.SetActive(false);
                    m_resultLow.SetActive(false);
                    _finishDirector.Play();
                });
            }
            
            _isResultAnimating = false;
        });
        
        Sequence sequence = DOTween.Sequence();
        sequence.Append(scale1);
        sequence.Append(rotate1);
        sequence.Append(a3);
        sequence.Append(a4);
        sequence.Append(punchScale);
        sequence.Append(scale2);
        sequence.Append(scale3);
    }

    void HideTimer() {
        if (m_mainViewGameObject != null)
            m_timer.SetActive(false);
        m_waitTrigger = false;
    }

    EPokers GetRandomPokerFromPool() {
        var temp = new List<int>();
        if (m_lastPoker != -1) {
            var pokerValue = m_lastPoker;
            bool shoted = m_pokerPool.Remove(pokerValue%16);
            if (shoted) {
                temp.Add(pokerValue%16);
            }
            shoted = m_pokerPool.Remove(pokerValue%16 + 16);
            if (shoted) {
                temp.Add(pokerValue%16 + 16);
            }
            shoted = m_pokerPool.Remove(pokerValue%16 + 32);
            if (shoted) {
                temp.Add(pokerValue%16 + 32);
            }
            shoted = m_pokerPool.Remove(pokerValue%16 + 48);
            if (shoted) {
                temp.Add(pokerValue%16 + 48);
            }
            m_pokerRestCount[m_lastPoker % 16] = 0;
        }
        var pokerIndex = Random.Range(0, m_pokerPool.Count - 1);

        var poker = m_pokerPool.ElementAt(pokerIndex);
        if (m_pokerPool.Count < 5) {
            var maxRestCount = 0;
            int maxRestCountPoker = -1;
            foreach (var item in m_pokerRestCount)
            {
                if (item.Value > maxRestCount) {
                    maxRestCountPoker = item.Key;
                    maxRestCount = item.Value;
                }
            }
            
            if (maxRestCount > 1)
                if (m_pokerPool.Contains(maxRestCountPoker)) 
                    poker = maxRestCountPoker;
                else if (m_pokerPool.Contains(maxRestCountPoker + 16))
                    poker = maxRestCountPoker + 16;
                else if (m_pokerPool.Contains(maxRestCountPoker + 32))
                    poker = maxRestCountPoker + 32;
                else if (m_pokerPool.Contains(maxRestCountPoker + 48))
                    poker = maxRestCountPoker + 48;
        }
        foreach (var item in temp)
        {
            m_pokerPool.Add(item);
        }
        m_pokerRestCount[m_lastPoker % 16] = temp.Count();
        CheckedPoker(poker);
        m_lastPoker = poker;
        m_pokerRestCount[poker % 16]--;
        return (EPokers)poker;
    }

    GameObject CreateRandomPokerFromPool() {
        var poker = GetRandomPokerFromPool();
        return CreatePoker(poker);
    }

    Sprite GetSpriteWithPoker(EPokers poker) {
        var path = "HighAndLow/Pokers/" + EPokersHelper.GetTextureNameFromPoker(poker);
        return Load<Sprite>(path);
    }

    GameObject CreatePoker(EPokers pokerType) {
        var pokerGO = GameObject.Instantiate(m_pokerTemplate, m_pokerStartTransform, false);
        var poker = pokerGO.GetComponent<Poker2D>();
        pokerGO.SetActive(true);
        if (pokerType != EPokers.BackFace)
            poker.front = GetSpriteWithPoker(pokerType);
        else poker.transform.localRotation = Quaternion.Euler(0f, 180f, 0f);
        pokerGO.GetComponent<Image>().SetNativeSize();
        return pokerGO;
    }

    void CheckedPoker(int poker, bool isNeedCache = true) {
        m_checkedPokers.Add(poker);
        var index = EPokersHelper.GetIndexOfPoker((EPokers)poker);
        m_checkedItemList.ElementAt(index).SetActive(true);
        m_pokerPool.Remove((int)poker);

        // if (m_pokerPool.Count > 2)
        // {
        //     m_pokerPool = new List<int>() { 1, 2 };
        // }
        
        m_checkRestCountLabel.text = m_pokerPool.Count.ToString();
        m_isGameFinished = m_pokerPool.Count == 0;
        m_pokersTile.SetActive(m_pokerPool.Count > 0);
        // if (m_isGameFinished)
        // {
        //     // AudioManager.Instance.PlayFinishEffect();
        //     _finishDirector.Play();
        // }
        // save
        if (isNeedCache) {
            var checkedPokerValues = m_checkedPokers.ConvertAll<int>((v)=>(int)v);
            AppConfig.Instance.CheckedPokers = checkedPokerValues;
        }
    }
    
    void UnCheckedAllPokers() {
        m_pokerPool.Clear();
        for (int i = 0; i < 4; i++) {
            for (int j = 0; j < 13; j++) {
                m_checkedItemList.ElementAt(i*13 + j).SetActive(false);
                m_pokerPool.Add(i*16+j);
            }
        }
    }

    private void ResumeGame()
    {
        var cachedPokerValues = AppConfig.Instance.CheckedPokers;
        for (int i = 0; i < 4; i++) {
            for (int j = 0; j < 13; j++) {
                if (!cachedPokerValues.Contains(i*16+j)) 
                {
                    m_pokerPool.Add(i*16+j);
                    if (m_pokerRestCount.ContainsKey(j))
                        m_pokerRestCount[j]++;
                    else m_pokerRestCount.Add(j, 1);
                }
                var itemPath = string.Format("PokerCheckList/CheckedList/{0}_{1}", i, j);
                m_checkedItemList.Add(m_mainViewGameObject.transform.Find(itemPath).gameObject);
            }
        }
        
        if (cachedPokerValues.Count() > 0) 
        {
            for (int i = 0; i < cachedPokerValues.Count; i++)
            {
                var value = cachedPokerValues[i];
                CheckedPoker(value, false);
                
                // Create cached poker
                var pokerGo = CreatePoker((EPokers)value);

                if (m_pokerPool.Count != 0)
                {
                    if (i == cachedPokerValues.Count - 1) 
                    {
                        // Last one
                        pokerGo.transform.SetParent(m_pokerShowTransform1);
                        (pokerGo.transform as RectTransform).anchoredPosition = Vector2.zero;
                        m_lastPoker = value;
                    }
                    else 
                    {
                        pokerGo.transform.SetParent(m_pokerTrashTransform);
                        (pokerGo.transform as RectTransform).anchoredPosition = Vector2.zero;
                        var angle = Random.Range(20, 70);
                        (pokerGo.transform as RectTransform).localRotation = Quaternion.Euler(new Vector3(0, 0, angle));
                    }
                }
                else
                {
                    if (i == cachedPokerValues.Count - 1) 
                    {
                        pokerGo.transform.SetParent(m_pokerShowTransform2);
                        (pokerGo.transform as RectTransform).anchoredPosition = Vector2.zero;
                        m_lastPoker = value;
                    }
                    else if (i == cachedPokerValues.Count - 2) 
                    {
                        pokerGo.transform.SetParent(m_pokerShowTransform1);
                        (pokerGo.transform as RectTransform).anchoredPosition = Vector2.zero;
                        m_lastPoker = value;
                    }
                    else 
                    {
                        pokerGo.transform.SetParent(m_pokerTrashTransform);
                        (pokerGo.transform as RectTransform).anchoredPosition = Vector2.zero;
                        var angle = Random.Range(20, 70);
                        (pokerGo.transform as RectTransform).localRotation = Quaternion.Euler(new Vector3(0, 0, angle));
                    }
                }
            }

            m_isWaitContinue = true;
        }
    }
}