using UnityEngine;

public class WiseCatController : MonoBehaviour
{
    public Animator animator;
    public float timeSleeping = 1f;

    private void Start()
    {
        Sleeping();
    }


    // Update is called once per frame
    void Update()
    {

    }

    public void PlayAnimation(string animationName)
    {
        if (animator != null)
        {
            animator.Play(animationName);
        }
        else
        {
            Debug.LogWarning("Animator is not assigned in WiseCatController.");
        }
    }

    public void Sleeping()
    {
        PlayAnimation("WiseCat_Sleeping");
    }

    public void Stretching()
    {
        PlayAnimation("WiseCat_Stretching");
    }

    public void WakingUp()
    {
        PlayAnimation("WiseCat_WakingUp");
    }

    public void Iddle()
    {
        PlayAnimation("WiseCat_Iddle");
    }

    public void Licking()
    {
        PlayAnimation("WiseCat_Licking");
    }

    public void Sat()
    {
        PlayAnimation("WiseCat_Sat");
    }
}
