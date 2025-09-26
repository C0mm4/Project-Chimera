using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class UIManager : Singleton<UIManager>
{
    
    public const string UICommonPath = "Common/";
    public const string UIPrefabPath = "Elements/";
    
    private Transform _uiRoot;
    private EventSystem _eventSystem;
    
    private bool _isCleaning; 
    private Dictionary<string, UIBase> _uiDictionary = new Dictionary<string, UIBase>();

    // To Do List : 스택으로 PopUp 구현
    int sortOrder = 10;
    Stack<PopupUIBase> popupStack = new Stack<PopupUIBase>();
    public int PopupStackCount => popupStack.Count;



    private void OnEnable()
    {
        SceneManager.sceneUnloaded += OnSceneUnloaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneUnloaded -= OnSceneUnloaded;
    }
    
    // ================================
    // UI 관리
    // ================================
    public void OpenUI<T>() where T : UIBase
    {
        var ui = GetUI<T>();
        ui?.OpenUI();
    }

    public void OpenPopupUI<T>(UpgradeableObject targetObject) where T : PopupUIBase
    {
        var ui = GetUI<T>() as UpgradePopupUI;
        if (ui == null)
        {
            Debug.LogError("UpgradePopupUI를 찾을 수 없거나 타입이 다릅니다.");
            return;
        }

        Time.timeScale = 0f;
        Debug.Log("게임 일시정지");

        ui.Initialize(targetObject);
        ui.OpenUI();

        popupStack.Push(ui);
    }

    public void CloseUI<T>() where T : UIBase
    {

        if (IsExistUI<T>())
        {
            var ui = GetUI<T>();
            ui?.CloseUI();
        }
    }

    public void ClosePopupUI()
    {
        if (popupStack.Count == 0) return;

        var ui = popupStack.Pop();
        ui?.CloseUI();

        --sortOrder;

        if (popupStack.Count == 0)
        {
            Time.timeScale = 1f;
            Debug.Log("게임 재개");
        }

        if (ui != null)
        {
            ui?.CloseUI();
            //Destroy(ui.gameObject);
        }
        // string uiName = ui.GetType().ToString();
        // _uiDictionary.Remove(uiName);


    }

    public T GetUI<T>() where T : UIBase
    {
        if (_isCleaning) return null;
        
        string uiName = GetUIName<T>();
        
        UIBase ui;
        if (IsExistUI<T>())
            ui = _uiDictionary[uiName];
        else
            ui = CreateUI<T>();

        return ui as T;
    }

    private T CreateUI<T>() where T : UIBase
    {
        if (_isCleaning) return null;
        
        string uiName = GetUIName<T>();
        if (_uiDictionary.TryGetValue(uiName, out var prevUi) && prevUi != null)
        {
            Destroy(prevUi.gameObject);
            _uiDictionary.Remove(uiName);
        }
        
        CheckCanvas();
        CheckEventSystem();
        
        // 1. 프리팹 로드 & 생성
        string path = GetPath<T>();

        GameObject prefab = ResourceManager.Instance.CreateUI<GameObject>(path, _uiRoot);
        
        if (prefab == null)
        {
            Debug.LogError($"[UIManager] Prefab not found: {path}");
            return null;
        }
        
        // 2. 컴포넌트 획득
        T ui = prefab.GetComponent<T>();
        if (ui == null)
        {
            Debug.LogError($"[UIManager] Prefab has no component : {uiName}");
            Destroy(prefab);
            return null;
        }

        // 3. Dictionary 등록
        _uiDictionary[uiName] = ui;

        return ui;
    }
    
    public T CreateSlotUI<T>(Transform parent = null) where T : UIBase
    {
        if (_isCleaning) return null;

        string path = GetPath<T>(); 
        GameObject prefab = ResourceManager.Instance.CreateUI<GameObject>(path, parent);
        if (prefab == null)
        {
            Debug.LogError($"[UIManager] Prefab not found: {path}");
            return null;
        }

        return prefab.GetComponent<T>();
    }

    public bool IsExistUI<T>() where T : UIBase
    {
        string uiName = GetUIName<T>();
        return _uiDictionary.TryGetValue(uiName, out var ui) && ui != null;
    }
    
    

    // ================================
    // path 헬퍼
    // ================================
    private string GetPath<T>() where T : UIBase
    {
        return UIPrefabPath + GetUIName<T>();
    }
    
    private string GetUIName<T>() where T : UIBase
    {
        return typeof(T).Name;
    }
    
    
    // ================================
    // UI 필수 컴포넌트 체크
    // 기존 씬에 있는 오브젝트가 있다면 처리를 해주는 코드가 필요할지도?
    // ================================
    private void CheckCanvas()
    {
        if(_uiRoot != null)
            return;

        //string prefKey = Path.UI + UICommonPath + Prefab.Canvas;
        //GameObject canvas = ResourceManager.Instance.Create<GameObject>(prefKey);

        _uiRoot = new GameObject("@UIRoot").transform;
    }

    private void CheckEventSystem()
    {
        _eventSystem = FindObjectOfType<EventSystem>();

        if (_eventSystem == null)
        {
            GameObject eventSystemObj = new GameObject("EventSystem");
            _eventSystem = eventSystemObj.AddComponent<EventSystem>();
            eventSystemObj.AddComponent<StandaloneInputModule>();
        }
    }


    // ================================
    // 리소스 정리
    // ================================
    private void OnSceneUnloaded(Scene scene)
    {
        CleanAllUIs();
        StartCoroutine(CoUnloadUnusedAssets());
    }
    
    private void CleanAllUIs()
    {
        if (_isCleaning) return;
        _isCleaning = true;
        
        try
        {
            foreach (var ui in _uiDictionary.Values)
            {
                if (ui == null) continue;
                // Close 프로세스 추가 가능
                Destroy(ui.gameObject);
            }
            _uiDictionary.Clear();
        }
        finally
        {
            _isCleaning = false;
        }
    }
    
    
    // UI 뿐만 아니라 전체 오브젝트 관리 시스템측면에서도 있으면 좋음
    private IEnumerator CoUnloadUnusedAssets()
    {
        yield return Resources.UnloadUnusedAssets();
        System.GC.Collect();
    }

    public void InitPopupCanvas(Canvas canvas)
    {
        if (canvas == null)
            return;

        canvas.overrideSorting = true;

        canvas.sortingOrder = sortOrder;
        ++sortOrder;
    }
    
    public bool IsActiveUI<T>() where T : UIBase
    {
        if (!IsExistUI<T>()) return false;

        var ui = GetUI<T>();

        return ui.gameObject.activeInHierarchy;
        
    }
}
