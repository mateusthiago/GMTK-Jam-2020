using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : MonoBehaviour
{
    [SerializeField] ParticleSystem particles;
    [SerializeField] float timer;
    public bool isActive;

    // Start is called before the first frame update
    void Start()
    {
        timer = particles.main.startLifetime.constant;
    }

    // Update is called once per frame
    void Update()
    {
        timer -= Time.deltaTime;
        if (timer <= 0)
        {
            isActive = false;
            gameObject.SetActive(false);
        }
    }

    private void OnEnable()
    {
        timer = particles.main.startLifetime.constant;
        isActive = true;
    }
}
