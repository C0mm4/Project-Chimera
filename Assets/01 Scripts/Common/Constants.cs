public enum SceneType
{
    Intro,
    Town,
    Battle
}

public enum RoomType
{
    Up,             // 위              0
    Down,           // 아래            1
    Left,           // 왼              2
    Right,          // 오른            3
    Side,           // 왼, 오른        4
    UpAndDown,      // 위, 아래        5
    LeftUp,         // 왼, 위          6
    LeftDown,       // 왼, 아래        7
    RightUp,        // 오른, 위        8
    RightDown,      // 오른, 아래      9
    SideUp,         // 왼, 오른, 위    10
    SideDown,       // 왼, 오른, 아래  11
    UpAndDownLeft,  // 왼, 위, 아래    12
    UpAndDownRight, // 오른, 위, 아래  13
    All             // 모든방향        14
}

public enum RoomOption
{
    town,
    normal,
    boss,
    special
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