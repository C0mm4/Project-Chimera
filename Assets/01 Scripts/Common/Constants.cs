public enum SceneType
{
    Intro,
    Title,
    InPlay,

}

public enum WeaponTypes
{
    Sword,
    Bow
}

public enum AISearchType
{
    General,
    PlayerFirst,
    DistanceFirst,
    BaseOnly,
}



public static class Path
{
    public const string Prefab = "Prefab/";
    public const string UI = "UI/";
    public const string Character = Prefab + "Character/";
    public const string Map = Prefab + "Map/";
}
    
public static class Prefab
{
    // Character
    public const string Player = "Player";
    public const string Enemy = "Enemy";
    
    // Map
    public const string Stage = "Stage";
    public const string Town = "Town";
    
    // UI
    public const string Canvas = "Canvas";
    public const string EventSystem = "EventSystem";
}

public static class PrefKey
{
    public const string Score = "Score";
    
}

public static class CharacterAnimParam
{
    public const string Hit = "Hit";
    public const string Die = "Die";
    public const string IsMoving = "IsMoving";
    public const string IsJumping = "IsJumping";
    public const string IsFalling = "IsFalling";
    public const string IsGrounded = "IsGrounded";
}