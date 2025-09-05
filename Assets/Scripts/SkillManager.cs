using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class KeyValuePair
{
    public string key;
    public GameObject value;
}
public class SkillManager : MonoBehaviour
{
    [SerializeField]
    private List<KeyValuePair> bulletPrefabs = new List<KeyValuePair>();
    private GameObject bulletPrefab;
    private float skillDuration = 10f;
    private float fireRate = 0.3f;
    private float bulletSpeed = 7f;
    private bool isActive;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        isActive = false;
    }

    public void ActivateSkill()
    {
        if (!isActive)
        {
            isActive = true;
            StartCoroutine(SkillCoroutine());
        }
    }
    private IEnumerator SkillCoroutine()
    {
        float endTime = Time.time + skillDuration;
        while (Time.time < endTime)
        {
            FireBullets();
            yield return new WaitForSeconds(fireRate);
        }
        isActive = false;
    }
    private void FireBullets()
    {
        if (bulletPrefab != null)
        {
            GameObject[] obstacles = GameObject.FindGameObjectsWithTag("Obstacle");

            foreach (var obstacle in obstacles)
            {
                if (obstacle.transform.position.x < transform.position.x) continue;
                GameObject bullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity);
                Vector2 direction = (obstacle.transform.position - transform.position).normalized;
                Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
                if (rb != null)
                {
                    rb.linearVelocity = direction * bulletSpeed;
                }
                float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
                bullet.transform.rotation = Quaternion.Euler(0f, 0f, angle);
            }
        }
    }
    public void SetBulletPrefab(string bulletName)
    {
        KeyValuePair pair = bulletPrefabs.Find(x => x.key == bulletName);
        bulletPrefab = pair != null ? pair.value : null;
    }
}
