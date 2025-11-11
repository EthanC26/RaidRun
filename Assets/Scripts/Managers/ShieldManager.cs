using UnityEngine;
using System.Collections;

public class ShieldManager : MonoBehaviour
{
    public bool isShieldActive = false;
    public float shieldDuration = 5f;

    public GameObject shield;

    private void Awake()
    {
        // Make sure shield starts disabled
        shield.SetActive(false);
    }

    public void ActivateShield()
    {
        if (!isShieldActive && shield != null)
        {
            isShieldActive = true;
            shield.SetActive(true);

            // Start timer to deactivate shield after duration
            StartCoroutine(ShieldTimer());
        }
    }

    private IEnumerator ShieldTimer()
    {
        yield return new WaitForSeconds(shieldDuration);
        DeactivateShield();
    }

    public void DeactivateShield()
    {
        if (isShieldActive && shield != null)
        {
            isShieldActive = false;
            shield.SetActive(false);
        }
    }
}
