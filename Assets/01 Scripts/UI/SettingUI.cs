using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SettingUI : PopupUIBase
{
    [Header("오디오")]
    [SerializeField] private AudioMixer mainMixer;
    [SerializeField] private Slider masterSlider;
    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider sfxSlider;

    [Header("버튼")]
    [SerializeField] private Button backButton;

    protected override void OnOpen()
    {
        base.OnOpen();

        // UI가 열릴 때, 저장된 값으로 슬라이더와 믹서를 초기화
        InitializeSliders();

        // 슬라이더 값이 변경될 때마다 호출될 메서드를 연결
        masterSlider.onValueChanged.AddListener(SetMasterVolume);
        musicSlider.onValueChanged.AddListener(SetMusicVolume);
        sfxSlider.onValueChanged.AddListener(SetEffectVolume);

        backButton.onClick.AddListener(OnBackButtonClicked);
    }

    protected override void OnClose()
    {
        base.OnClose();
        // 리스너 제거
        masterSlider.onValueChanged.RemoveAllListeners();
        musicSlider.onValueChanged.RemoveAllListeners();
        sfxSlider.onValueChanged.RemoveAllListeners();
        backButton.onClick.RemoveAllListeners();
    }

    private void InitializeSliders()
    {
        masterSlider.value = 1f;
        musicSlider.value = 1f;
        sfxSlider.value = 1f;

        // 불러온 값으로 믹서 볼륨도 즉시 설정
        SetMasterVolume(masterSlider.value);
        SetMusicVolume(musicSlider.value);
        SetEffectVolume(sfxSlider.value);
    }


    // =========== 슬라이더와 연결된 메서드들 ===========

    public void SetMasterVolume(float value)
    {
        // 슬라이더 값(0.0001~1)을 믹서의 데시벨(-80~0)로 변환
        mainMixer.SetFloat("MasterVolume", Mathf.Log10(value) * 20);
    }

    public void SetMusicVolume(float value)
    {
        mainMixer.SetFloat("BGMVolume", Mathf.Log10(value) * 20);
    }

    public void SetEffectVolume(float value)
    {
        mainMixer.SetFloat("SFXVolume", Mathf.Log10(value) * 20);
    }

    private void OnBackButtonClicked()
    {
        UIManager.Instance.ClosePopupUI(); // UIManager에게 팝업을 닫아달라고 요청
    }
}
