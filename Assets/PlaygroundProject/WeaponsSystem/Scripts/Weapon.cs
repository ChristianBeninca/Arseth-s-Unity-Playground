using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    //[SerializeField] private Animator anim;
    [SerializeField] private GameObject visuals;
    public float damage;
    public float rateOfFire;
    public float range;
    public float ammoCapacity;
    public float magazineSize;

    //[SerializeField] private bool activeSway;
    [SerializeField] private float swayIntensity;
    [SerializeField] private float swaySmooth;
    private Quaternion originRotation;

    private bool onSheath = true;

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
