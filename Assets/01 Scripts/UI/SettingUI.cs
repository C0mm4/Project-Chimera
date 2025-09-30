using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SettingUI : PopupUIBase
{
    [Header("오디오")]
    [SerializeField] private AudioMixer mainMixer;

    [Header("UI 요소 연결")]
    [SerializeField] private SlidingToggle musicToggle;
    [SerializeField] private SlidingToggle sfxToggle;
    [SerializeField] private SlidingToggle hapticsToggle; // 진동 토글
    [SerializeField] private Button backButton;


    protected override void OnOpen()
    {
        base.OnOpen();

        // UI가 열릴 때, 토글의 상태를 기본값으로 초기화합니다.
        InitializeSettings();

        // 토글의 상태가 변경될 때마다 호출될 함수를 연결합니다.
        musicToggle.OnToggleChanged.AddListener(OnMusicToggleChanged);
        sfxToggle.OnToggleChanged.AddListener(OnSFXToggleChanged);
        hapticsToggle.OnToggleChanged.AddListener(OnHapticsToggleChanged);
        backButton.onClick.AddListener(OnBackButtonClicked);
    }

    protected override void OnClose()
    {
        base.OnClose();
        // 연결했던 함수들을 깨끗하게 제거합니다.
        musicToggle.OnToggleChanged.RemoveAllListeners();
        sfxToggle.OnToggleChanged.RemoveAllListeners();
        hapticsToggle.OnToggleChanged.RemoveAllListeners();
        backButton.onClick.RemoveAllListeners();
    }

    private void InitializeSettings()
    {
        // 일단 저장 기능이 없으니 계속 On 상태로 시작하도록 설정
        musicToggle.SetState(true);
        sfxToggle.SetState(true);
        hapticsToggle.SetState(true);

        // 초기 상태에 맞춰 즉시 믹서 볼륨을 설정합니다.
        OnMusicToggleChanged(true);
        OnSFXToggleChanged(true);
        OnHapticsToggleChanged(true);
    }


    // =========== 슬라이더와 연결된 메서드들 ===========

    public void OnMusicToggleChanged(bool isOn)
    {
        // isOn 값(true/false)에 따라 믹서 볼륨을 0dB(ON) 또는 -80dB(OFF)로 설정합니다.
        mainMixer.SetFloat("BGMVolume", isOn ? 0f : -80f);
        //Debug.Log("음악 설정: " + (isOn ? "ON" : "OFF"));
    }

    public void OnSFXToggleChanged(bool isOn)
    {
        mainMixer.SetFloat("SFXVolume", isOn ? 0f : -80f);
        //Debug.Log("효과음 설정: " + (isOn ? "ON" : "OFF"));
    }

    public void OnHapticsToggleChanged(bool isOn)
    {
        // 진동 기능은 아직 구현하지 않았으므로, 상태 변경 로그만 남깁니다.
        //Debug.Log("진동 설정: " + (isOn ? "ON" : "OFF") + " (기능 미구현)");
    }

    // 뒤로가기 버튼 기능
    private void OnBackButtonClicked()
    {
        UIManager.Instance.ClosePopupUI();
    }
}
