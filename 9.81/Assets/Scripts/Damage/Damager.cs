public interface Damager
{
    void DealDamage(Damageable damageable);
    float GetDamage();
    bool CanDamagePlayer();
}
