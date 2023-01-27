using UnityEngine;

public class DefaultBullet : Bullets
{
    protected override void Start()
    {
        
        base.Start();
        CustomStart();
    }

    protected virtual void CustomStart()
    {
        AudioManager.GetInstance().Play("Sarbacane", this.gameObject);
        var dir = (targetPosition - startPosition).normalized;
        rb.AddForce(speed * dir, ForceMode.Impulse);
    }
}
