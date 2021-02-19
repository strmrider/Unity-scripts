using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerBar : MonoBehaviour
{
    [SerializeField] Slider bar;
    [SerializeField] bool regeneration;
    [SerializeField] float regenerationStartTime;
    [SerializeField] float regenerationTime;
    [SerializeField] int regenerationAmount;
    private float startTimer;
    private float timer;
    private bool startRegenerate = false;
    private bool regenerate = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (regeneration)
        {
            if (startRegenerate)
                StartRegenerateTimer();

            if (regenerate)
            {
                RegenerateBar();
                if (bar.value == bar.maxValue)
                {
                    startRegenerate = false;
                    regenerate = false;
                    startTimer = regenerationStartTime;
                    timer = regenerationTime;
                }
            }
        }
    }

    private void StartRegenerateTimer()
    {
        if (startTimer <= 0f)
        {
            startRegenerate = false;
            regenerate = true;
            startTimer = regenerationStartTime;
        }

        startTimer -= Time.deltaTime;
    }

    private void RegenerateBar()
    {
        if (timer <= 0f)
        {
            bar.value += regenerationAmount;
            timer = regenerationTime;
        }
        timer -= Time.deltaTime;
    }

    public void SetMaxValue(int value)
    {
        bar.maxValue = value;
        bar.value = value;
    }

    public void SetValue(int value)
    {
        bar.value = value;
    }

    public void ChangeValue(int value)
    {
        bar.value += value;
        if (value < 0)
            startRegenerate = true;
    }
}
