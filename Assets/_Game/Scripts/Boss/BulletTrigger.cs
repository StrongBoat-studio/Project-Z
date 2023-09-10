using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletTrigger : MonoBehaviour
{
    [SerializeField] private GameObject _blood;


    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("bullet");
        GameObject prefab = Instantiate(_blood, collision.transform.position, Quaternion.identity);

        new WaitForSeconds(1f);
        Destroy(prefab);
    }
}
