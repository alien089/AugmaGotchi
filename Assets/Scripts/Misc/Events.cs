public static class MainMenuEventList
{
    /// <summary>
    /// params: string sceneName
    /// </summary>
    public static string LOAD_SCENE = "LOAD_SCENE";
}

public static class AugmaEventList 
{
    /// <summary>
    /// params: none
    /// </summary>
    public static string SPAWN_AUGMA = "SPAWN_AUGMA"; 
    
    /// <summary>
    /// params: AugmaManager class instance
    /// </summary>
    public static string GIVE_AUGMA_TO_UI = "GIVE_AUGMA_TO_UI";
    
    /// <summary>
    /// params: AugmaStates enum
    /// </summary>
    public static string CHANGE_AUGMA_STATE = "CHANGE_AUGMA_STATE";
}

public static class CaressEventList
{    
    /// <summary>
    /// params: Stats statType, float incrementValue 
    /// </summary>
    public static string CARESS_GIVEN = "CARESS_GIVEN";
    
    /// <summary>
    /// params: none 
    /// </summary>
    public static string WANT_CARESS = "WANT_CARESS";

    /// <summary>
    /// params: none 
    /// </summary>
    public static string NOT_WANT_CARESS = "NOT_WANT_CARESS";
}

public static class FoodEventList
{
    /// <summary>
    /// params: Stats statType, float incrementValue 
    /// </summary>
    public static string FOOD_GIVEN = "FOOD_GIVEN";

    /// <summary>
    /// params: none 
    /// </summary>
    public static string RESPAWN_FOOD = "RESPAWN_FOOD";

    /// <summary>
    /// params: none 
    /// </summary>
    public static string FOOD_GRABBED = "FOOD_GRABBED";

    /// <summary>
    /// params: none 
    /// </summary>
    public static string FOOD_UNGRABBED = "FOOD_UNGRABBED";
}

public static class ToyEventList 
{
    /// <summary>
    /// params: none 
    /// </summary>
    public static string RESPAWN_TOY = "RESPAWN_TOY"; 
    
    /// <summary>
    /// params: none 
    /// </summary>
    public static string TOY_GRABBED = "TOY_GRABBED"; 
    
    /// <summary>
    /// params: none 
    /// </summary>
    public static string TOY_UNGRABBED = "TOY_UNGRABBED"; 
    
    /// <summary>
    /// params: Stats statType, float incrementValue
    /// </summary>
    public static string TOY_THROWN = "TOY_THROWN"; 
}
