using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using TMPro;
public class WorldEnded : MonoBehaviour
{
    [SerializeField] private float cooldown = 90f;
    [SerializeField] private Button UI;
    [SerializeField] private TextMeshProUGUI cooldownText; // Text to display cooldown time
    [SerializeField] private Image cooldownOverlay; // Image overlay (radial fill)
    [SerializeField] private GameObject player;
    [SerializeField] private float duration = 7f;
    [SerializeField] private float lifeSteal = 0.2f;
    [SerializeField] private float fireRateBoost = 0.5f;
    [SerializeField] private float damageBoost = 0.3f;

    public AudioClip clip;

    private AudioSource audioSource;
    private PlayerWeapon playerWeapon;
    private bool isOnCooldown = false;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        playerWeapon = player.GetComponent<PlayerWeapon>();
    }
    IEnumerator ApplyEffectCoroutine()
    {
        //hide cooldown text
        cooldownText.text = "";
        audioSource.PlayOneShot(clip);
        playerWeapon.CurrentWeapon.lifeSteal = lifeSteal;
        playerWeapon.CurrentWeapon.fireRate *= (1+fireRateBoost);
        playerWeapon.CurrentWeapon.damage *= (1 + damageBoost);

        StartCoroutine(StartCooldown());
        yield return new WaitForSeconds(duration);
        
        playerWeapon.CurrentWeapon.lifeSteal = 0.0f;
        playerWeapon.CurrentWeapon.fireRate /= (1 + fireRateBoost);
        playerWeapon.CurrentWeapon.damage /= (1 + damageBoost);
    }
    IEnumerator StartCooldown()
    {
        isOnCooldown = true;
        UI.interactable = false;
        cooldownOverlay.fillAmount = 1f;

        float timer = cooldown;
        while (timer > 0)
        {
            timer -= Time.deltaTime;
            cooldownText.text = Mathf.Ceil(timer).ToString(); // Update cooldown text
            cooldownOverlay.fillAmount = timer / cooldown;
            yield return null;
        }

        UI.interactable = true;
        cooldownOverlay.fillAmount = 0f;
        isOnCooldown = false;
    }
    public void ApplyEffect()
    {
        StartCoroutine(ApplyEffectCoroutine());

    }
}
    

