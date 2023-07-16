using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    //[SerializeField] private Animator anim;
    [SerializeField] private GameObject visuals;
    [SerializeField] private Transform anchor, freeSight, ironSight;
    public float damage;
    public float rateOfFire;
    public float range;
    public float ammoCapacity;
    public float magazineSize;
    public float aimVelocity;

    //[SerializeField] private bool activeSway;
    [SerializeField] private float swayIntensity;
    [SerializeField] private float swaySmooth;
    private Quaternion originRotation;

    private bool onSheath = true;
    private bool aiming = false;
    private Coroutine aimAnim;

    public virtual void Start()
    {
        StartSway();
    }

    public virtual void Update()
    {
        UpdateSway();
    }

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
            Debug.Log("<color=green>anchor: " + anchor.position + "</color>");
            Debug.Log("<color=green>target: " + AimPosition(aiming) + "</color>");
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

    #region Sway
    void StartSway()
    {
        originRotation = transform.localRotation;
    }

    void UpdateSway()
    {
        float MouseX = Input.GetAxis("Mouse X");
        float MouseY = Input.GetAxis("Mouse Y");

        Quaternion swayAdjustmentX = Quaternion.AngleAxis(swayIntensity * -MouseX, Vector3.up);
        Quaternion swayAdjustmentY = Quaternion.AngleAxis(swayIntensity * MouseY, Vector3.right);
        Quaternion targetRotation = originRotation * swayAdjustmentX * swayAdjustmentY;

        transform.localRotation = Quaternion.Lerp(transform.localRotation, targetRotation, Time.deltaTime * swaySmooth);
    }

    #endregion
}
