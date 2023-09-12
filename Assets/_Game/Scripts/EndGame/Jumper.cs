using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jumper : MonoBehaviour
{
    private bool _isStart = false;

    // Update is called once per frame
    void Update()
    {
        if(this.gameObject.activeSelf==true && !_isStart)
        {
            _isStart = true;
            StartJump();
        }
    }

    private void StartJump()
    {
        GetComponentInChildren<Animator>().SetBool("Jump", true);
        GetComponent<Rigidbody2D>().AddForce(new Vector2(8, 0.0f), ForceMode2D.Impulse);
        AudioManager.Instance?.PlayOneShot(FMODEvents.Instance.MutantGrowl, transform.position);
    }
}
