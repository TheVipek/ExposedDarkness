public class BaseEnemy
{
    float baseDamage;
    float baseHitpoints;
    float baseSpeed;
    float baseDeathStateLength;
    
    
    public BaseEnemy(float baseDamage,float baseHitpoints,float baseSpeed,float baseDeathStateLength)
    {
        this.baseDamage = baseDamage;
        this.baseHitpoints = baseHitpoints;
        this.baseSpeed = baseSpeed;
        this.baseDeathStateLength = baseDeathStateLength;
    }
}