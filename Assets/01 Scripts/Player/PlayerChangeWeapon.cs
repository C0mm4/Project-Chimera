using UnityEngine;
using UnityEngine.Rendering.Universal;

public class PlayerChangeWeapon : MonoBehaviour
{
    //무기 생성 위치
    [SerializeField] private GameObject weaponPrefab;
    public Transform throwObjects;

    private void Awake()
    {
        //모든 무기 생성

        //검
        ObjectPoolManager.Instance.CreatePool("Pref_500000", weaponPrefab.transform);

        //활
        ObjectPoolManager.Instance.CreatePool("Pref_510000", weaponPrefab.transform);

        //테스트용
        //ChangeWeapon(WeaponTypes.Sword);
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            ChangeWeapon(WeaponTypes.Bow);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            ChangeWeapon(WeaponTypes.Sword);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            foreach (Transform transforms in weaponPrefab.transform)
            {
                ObjectPoolManager.Instance.ClearPool(transforms.name, weaponPrefab.transform);
                Debug.Log("???");
            }
        }
    }

    public void ChangeWeapon(WeaponTypes name)
    {
        string keyValue = SelectWeapon(name);

        //무기 바꾸기전 다 반환
        foreach (Transform transforms in weaponPrefab.transform)
        {
            ObjectPoolManager.Instance.ResivePool(transforms.name, transforms.gameObject, weaponPrefab.transform);
        }

        //플레이어와 같은 위치에 있는 어택에 접근
        PlayerAttack playerAttack = GetComponent<PlayerAttack>();

        //스크립트 있는지 확인
        if (playerAttack != null)
        {
            //name의 프리팹 무기 오브젝트를 활성화
            GameObject weaponGameobject = ObjectPoolManager.Instance.GetPool(keyValue, weaponPrefab.transform);

            //무기.. weapon으로 되어있어서 일단 넣 모르겠다
            BaseWeapon changeWeapon = weaponGameobject.GetComponent<BaseWeapon>();
            playerAttack.EquipNewWeapon(changeWeapon);

            
            if (name == WeaponTypes.Bow)
            {
                //플레이어 화살있는 Transform, bow를 생성하면 start에서 화살을 만들고 있음
                //throwObjects = weaponGameobject.transform;
            }
            else throwObjects = null;
            
        }
    }

    private string SelectWeapon(WeaponTypes types)
    {
        string keyValue = null;

        //추후 무기 더 생기면 넣어야함
        switch (types)
        {
            case WeaponTypes.Sword:
                return "Pref_500000";
            case WeaponTypes.Bow:
                return "Pref_510000";
        }    
        return keyValue;
    }
}


