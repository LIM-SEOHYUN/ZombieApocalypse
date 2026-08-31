using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFire : MonoBehaviour
{
    public GameObject firePosition;
    public GameObject bulletEffect;
    ParticleSystem ps;
    public int weaponPower = 5;
    public GameObject[] eff_Flash;
    public AudioClip shotSound;
    private AudioSource audioSource;

    void Start()
    {
        ps = bulletEffect.GetComponent<ParticleSystem>();
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);
            RaycastHit hitInfo = new RaycastHit();
            audioSource.PlayOneShot(shotSound);
            if (Physics.Raycast(ray, out hitInfo))
            {
                if (hitInfo.transform.gameObject.layer == LayerMask.NameToLayer("Enemy"))
                {
                    EnemyFSM eFSM = hitInfo.transform.GetComponent<EnemyFSM>();
                    eFSM.HitEnemy(weaponPower);
                }
                else
                {
                    bulletEffect.transform.position = hitInfo.point;

                    bulletEffect.transform.forward = hitInfo.normal;

                    ps.Play();
                }
            }
            StartCoroutine(ShootEffectOn(0.05f));
        }
    }
    IEnumerator ShootEffectOn(float duration)
    {
        int num = Random.Range(0, eff_Flash.Length - 1);
        eff_Flash[num].SetActive(true);
        yield return new WaitForSeconds(duration);
        eff_Flash[num].SetActive(false);
    }
}