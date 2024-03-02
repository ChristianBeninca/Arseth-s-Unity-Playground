using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Weapon : MonoBehaviour
{
    public enum WalkMode
    {
        Idle = 0,
        Walk = 1,
        Run = 2,
    }

    [SerializeField] private GameObject visuals_;
    [SerializeField] private Transform anchor_;
    [SerializeField] private Transform freeSight_;
    [SerializeField] private Transform ironSight_;
    [SerializeField] private List<AudioClip> shootSounds_;
    [SerializeField] private AudioClip rechargeSound_;
    [SerializeField] private GameObject impactEffect_;
    [SerializeField] private float swayIntensity;
    [SerializeField] private float swaySmooth;
    [SerializeField] private float fireRate;
    [SerializeField] private int damage;
    [SerializeField] private float range;
    [SerializeField] private float ammoCapacity;
    [SerializeField] private float magazineSize;
    [SerializeField] private float aimVelocity;

    private Quaternion originRotation;
    private Vector3 originPosition;

    private AudioSource audioSource_;
    private bool onSheath = true;
    private bool aiming = false;
    private Coroutine aimAnim;
    private float idleCounter;
    private float walkCounter;
    private float runCounter;
    private float nextTimeToFire = 0f;

    public void Hide() { visuals_.SetActive(false); }

    public void Show() { visuals_.SetActive(true); }

    public virtual void Start()
    {
        SetupSway();
        SetupBreath();
        SetupAudioSource();
    }
    public virtual void Update() { }

    public void Shoot()
    {
        if (Time.time <= nextTimeToFire) return;
        nextTimeToFire = Time.time + (1f / fireRate);

        audioSource_.pitch = Random.Range(0.8f, 1.3f);
        audioSource_.PlayOneShot(shootSounds_[Random.Range(0, shootSounds_.Count - 1)]);

        RaycastHit hit;
        Transform cameraTansform = Camera.main.transform;

        if (Physics.Raycast(cameraTansform.position, cameraTansform.forward, out hit))
        {
            if (hit.transform.TryGetComponent(out Target target))
            {
                target.TakeDamage(damage);
            }

            if (hit.rigidbody != null)
            {
                hit.rigidbody.AddForce(-hit.normal * damage);
            }

            GameObject ImpactGo = Instantiate(impactEffect_, hit.point, Quaternion.LookRotation(hit.normal));
            Destroy(ImpactGo, 2f);
        }
    }

    public void Sheath()
    {
        if (onSheath) return;

        Hide();
        onSheath = true;
    }

    public void Draw()
    {
        if (!onSheath) return;

        Show();
        onSheath = false;
    }

    public void Aim(bool value)
    {
        aiming = value;
        if (aimAnim != null) StopCoroutine(aimAnim);
        aimAnim = StartCoroutine(AimAnim());
    }

    private Vector3 AimPosition(bool value)
    {
        if (value) return ironSight_.position;
        else return freeSight_.position;
    }

    private IEnumerator AimAnim()
    {
        while (Vector3.Distance(anchor_.position, AimPosition(aiming)) >= .001f)
        {
            anchor_.position = Vector3.Lerp(anchor_.position, AimPosition(aiming), Time.deltaTime * aimVelocity * 15);
            yield return new WaitForEndOfFrame();
        }
        anchor_.position = AimPosition(aiming);
    }

    #region Sway

        void SetupSway()
        {
            originRotation = anchor_.localRotation;
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

            anchor_.localRotation = Quaternion.Lerp(anchor_.localRotation, targetRotation, Time.deltaTime * swaySmooth);
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

    #region Sound

        void SetupAudioSource()
        {
            if (!TryGetComponent(out audioSource_))
            {
            audioSource_ = gameObject.AddComponent<AudioSource>();
            }
        }
    #endregion
}
