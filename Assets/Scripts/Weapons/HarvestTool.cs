namespace Foxlair.Weapons
{
    public class HarvestTool : MeleeWeapon
    {

     public HarvestToolType harvestToolType;

    }
    public enum HarvestToolType
    {
        Mining,
        Woodcutting,
        Fishing,
    }
}