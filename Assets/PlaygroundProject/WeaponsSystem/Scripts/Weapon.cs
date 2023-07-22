using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public enum WalkMode
    {
        Idle = 0,
        Walk = 1,
        Run = 2,
    }

    //[SerializeField] private Animator anim;
    [SerializeField] private GameObject visuals;
    [SerializeField] private Transform anchor, freeSight, ironSight;
    public float damage;
    public float rateOfFire;
    public float range;
    public float ammoCapacity;
    public float magazineSize;
    public float aimVelocity;
    [SerializeField] private float swayIntensity;
    [SerializeField] private float swaySmooth;

    //[SerializeField] private bool activeSway;
    private Quaternion originRotation;
    private Vector3 originPosition;
    private Vector3 targetBreathWeaponPosition;

    private bool onSheath = true;
    private bool aiming = false;
    private Coroutine aimAnim;
    private float idleCounter, walkCounter, runCounter;

    public virtual void Start()
    {
        SetupSway();
        SetupBreath();
        Debug.Log("<color=green>" + this.gameObject.name + " has started</color>");
    }
    public virtual void Update() { }

    public void Shoot()
    {
        //anim.SetTrigger("Shoot");
    }

    public void Sheath()
    {
        if (onSheath) return;

        //anim.SetTrigger("Sheath");
        Hide();
        onSheath = true;
    }

    public void Draw()
    {
        if (!onSheath) return;

        //anim.SetTrigger("Draw");
        Show();
        onSheath = false;
    }

    public void Aim(bool value)
    {
        aiming = value;
        if (aimAnim != null) StopCoroutine(aimAnim);
        aimAnim = StartCoroutine(AimAnim());
    }

    private IEnumerator AimAnim()
    {
        while (Vector3.Distance(anchor.position, AimPosition(aiming)) >= .001f)
        {
            anchor.position = Vector3.Lerp(anchor.position, AimPosition(aiming), Time.deltaTime * aimVelocity * 15);
            yield return new WaitForEndOfFrame();
        }
        anchor.position = AimPosition(aiming);
    }

    private Vector3 AimPosition(bool value)
    {
        if (value) return ironSight.position;
        else return freeSight.position;
    }

    public void Hide() { visuals.SetActive(false); }

    public void Show() { visuals.SetActive(true); }
    #region Weapon feeling
        #region Sway

            void SetupSway()
            {
                originRotation = anchor.localRotation;
            }

            /// <summary>
    /// This method adjusts weapon rotation to match the opost direction of the weapon, creating a natural movement feeling.
    /// </summary>
            public void UpdateSway()
            {
                float MouseX = Input.GetAxis("Mouse X");
                float MouseY = Input.GetAxis("Mouse Y");

                Quaternion swayAdjustmentX = Quaternion.AngleAxis(swayIntensity * -MouseX, Vector3.up);
                Quaternion swayAdjustmentY = Quaternion.AngleAxis(swayIntensity * MouseY, Vector3.right);
                Quaternion targetRotation = originRotation * swayAdjustmentX * swayAdjustmentY;

                anchor.localRotation = Quaternion.Lerp(anchor.localRotation, targetRotation, Time.deltaTime * swaySmooth);
            }

        #endregion

        #region Breath
            void SetupBreath()
            {
                originPosition = transform.localPosition;
            }
            
            public void UpdateBreath(WalkMode walkMode)
            {
                switch (walkMode)
                {
                    case WalkMode.Idle:
                    {
                        idleCounter += Time.deltaTime;
                        transform.localPosition = Vector3.Lerp(transform.localPosition, BreathAnimation(idleCounter, .001f, .001f), Time.deltaTime * 2);
                        break;
                    }
                    case WalkMode.Walk:
                    {
                        walkCounter += Time.deltaTime * 8;
                    transform.localPosition = Vector3.Lerp(transform.localPosition, BreathAnimation(walkCounter, .003f, .003f), Time.deltaTime * 6);
                        break;
                    }
                    case WalkMode.Run:
                    {
                        runCounter += Time.deltaTime * 16;
                        transform.localPosition = Vector3.Lerp(transform.localPosition, BreathAnimation(runCounter, .01f, .0075f), Time.deltaTime * 10);
                        break;
                    }
                }
            }
    
            public Vector3 BreathAnimation(float counter, float yInstensity, float xIntensity)
            {
                return originPosition + new Vector3(Mathf.Cos(counter) * xIntensity, Mathf.Sin(counter * 2) * yInstensity, transform.localPosition.z);
            }
        #endregion
    #endregion
}
