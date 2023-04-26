// Represents a damageable object with a health pool
public interface Damageable
{
    // Apply a certain amount of damage to the health pool
    void TakeDamage(float damage);

    // Get the current health of the object
    float GetHealth();

    // Is the object destroyed?
    bool IsAlive();
}
