using UnityEngine;

public class PlayerChangeWeapon : MonoBehaviour
{
    //무기 생성 위치
    [SerializeField] private GameObject weaponPrefab;

    private void Awake()
    {
        //모든 무기 생성

        //검
        ObjectPoolManager.Instance.CreatePool("Pref_500000", weaponPrefab.transform);

        //활
        ObjectPoolManager.Instance.CreatePool("Pref_510000", weaponPrefab.transform);

        //테스트용
        ChangeWeapon(WeaponTypes.Bow);
    }

    public void ChangeWeapon(WeaponTypes name)
    {
        string keyValue = SelectWeapon(name);

        //무기 바꾸기전 다 반환
        foreach (Transform transforms in weaponPrefab.transform)
        {
            ObjectPoolManager.Instance.ResivePool(transforms.name, transforms.gameObject);
        }
        
        //플레이어와 같은 위치에 있는 어택에 접근
        PlayerAttack playerAttack = GetComponent<PlayerAttack>();

        //스크립트 있는지 확인
        if (playerAttack != null)
        {
            //name의 프리팹 무기 오브젝트를 활성화
            GameObject weaponGameobject = ObjectPoolManager.Instance.GetPool(keyValue);

            //무기.. weapon으로 되어있어서 일단 넣 모르겠다
            BaseWeapon changeWeapon = weaponGameobject.GetComponent<BaseWeapon>();
            playerAttack.EquipNewWeapon(changeWeapon);
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


