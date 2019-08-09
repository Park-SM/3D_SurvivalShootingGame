using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

[RequireComponent(typeof(CharacterController))]

public class CharacterMove : MonoBehaviour
{
    public GameObject bullet_Prefab;
    public AudioClip ShootingClip;
    public AudioClip NoBulletClip;
    public AudioClip ReloadClip_1;
    public AudioClip ReloadClip_2;
    public AudioClip ChangeMode;
    public float smoothing = 5f;
    public float movementSpeed = 5f;
    public float mouseSensitivity = 2f;
    public float upDownRange = 90;
    public float jumpSpeed = 5;
    public float downSpeed = 5;
    public float ReloadTime = 0.33f;
    public float AutoShootingSpeed = 0.5f;
    public int ShootingLightTime = 7;
    public int ShootingLightIntensity = 2;
    public int MaxOfBullet = 30;
    public int V_recoil = 5;
    public int H_recoil = 3;

    private Vector3 speed;
    private float forwardSpeed;
    private float sideSpeed;

    private float rotLeftRight;
    private float rotUpDown;
    private float verticalRotation = 0f;
    private float verticalVelocity = 0f;

    private static float Health = 1f;
    private int CurrentBulletCount;
    private int thRecoil = 0, tvRecoil = 0;
    private bool IsAutoShoot = false;
    private bool IsZoom = false;
    private bool IsReload = false;
    private float AutoShootingSpeed_frame;
    private int ShootingLight_frame = 0;
    private CharacterController cc;
    private AudioSource Adsrc;
    GameObject GameOver;
    GameObject playerHead;
    GameObject Gun_ShootingLight;
    GameObject sMode;
    GameObject FP_vp, ZoomP_vp;
    GameObject E_gage, B_gage, R_gage;

    // Use this for initialization
    void Start()
    {
        playerHead = GameObject.Find("Head");
        Gun_ShootingLight = GameObject.Find("ShootingLight");
        sMode = GameObject.Find("ShootingMode");
        FP_vp = GameObject.Find("1P_viewPoint");
        ZoomP_vp = GameObject.Find("ZoomP_viewPoint");

        GameOver = GameObject.Find("GameOver");
        E_gage = GameObject.Find("EnergyGage");
        B_gage = GameObject.Find("BulletGage");
        R_gage = GameObject.Find("ReloadGage");
        E_gage.GetComponent<Image>().fillAmount = Health;

        cc = GetComponent<CharacterController>();
        Adsrc = GetComponent<AudioSource>();

        CurrentBulletCount = MaxOfBullet;
        AutoShootingSpeed_frame = AutoShootingSpeed;
        Cursor.lockState = CursorLockMode.Locked;

    }

    void FixedUpdate() {
        if (IsZoom) Camera.main.transform.position = Vector3.Lerp(Camera.main.transform.position, ZoomP_vp.transform.position, smoothing * Time.deltaTime);
        else Camera.main.transform.position = Vector3.Lerp(Camera.main.transform.position, FP_vp.transform.position, smoothing * Time.deltaTime);
    }

    // Update is called once per frame
    void Update()
    {
        FPMove();
        FPRotate(tvRecoil, thRecoil);

        if (Input.GetKeyDown(KeyCode.B)) {
            Adsrc.clip = ChangeMode;
            Adsrc.Play();
            if (IsAutoShoot) {
                IsAutoShoot = false;
                sMode.GetComponent<Text>().text = "Single";
            } else {
                IsAutoShoot = true;
                sMode.GetComponent<Text>().text = "Auto";
            }
        }

        if (Input.GetKeyDown(KeyCode.R)) {
            if (IsReload) {
                IsReload = false;
                Adsrc.Stop();
                R_gage.GetComponent<Image>().fillAmount = 0;
            } else {
                Adsrc.clip = ReloadClip_1;
                Adsrc.Play();
                IsReload = true;
            }
        }
        if (IsReload) Reload();


        if (Input.GetMouseButtonDown(1)) {
            if (IsZoom) { IsZoom = false; movementSpeed += 3f; }
            else { IsZoom = true; movementSpeed -= 3f; }
        }
        if (!IsAutoShoot && !IsReload && Input.GetMouseButtonDown(0)) Shooting();
        if (IsAutoShoot && !IsReload && Input.GetMouseButton(0)) {
            if ((AutoShootingSpeed_frame -= Time.deltaTime) <= 0) {
                Shooting();
                AutoShootingSpeed_frame = AutoShootingSpeed;
            }
            
        }

        if (--ShootingLight_frame == 0) Gun_ShootingLight.GetComponent<Light>().intensity = 0;
    }

    public void HaveDamage(float HaveDamage)
    {
        Health -= HaveDamage * 0.01f;
        E_gage.GetComponent<Image>().fillAmount = Health;
        if (Health <= 0)
        {
            GameOver.GetComponent<GameOverScript>().gOver();
        }
    }

    void Reload() {
        if ((R_gage.GetComponent<Image>().fillAmount += ReloadTime * Time.deltaTime) > 0.99f) {
            IsReload = false;
            CurrentBulletCount = MaxOfBullet;
            R_gage.GetComponent<Image>().fillAmount = 0;
            B_gage.GetComponent<Image>().fillAmount = 1;
        }
        if (!Adsrc.GetComponent<AudioSource>().isPlaying) {
            Adsrc.clip = ReloadClip_2;
            Adsrc.Play();
        }
    }

    void Shooting()
    {
        if (--CurrentBulletCount >= 0)
        {
            GameObject tBullet = Instantiate(bullet_Prefab) as GameObject;
            playerHead.GetComponent<ParticleSystem>().Play();
            Adsrc.clip = ShootingClip;
            Adsrc.Play();
            B_gage.GetComponent<Image>().fillAmount -= 1f / MaxOfBullet;
            Gun_ShootingLight.GetComponent<Light>().intensity = ShootingLightIntensity;

            tvRecoil = V_recoil;
            thRecoil = Random.Range(-H_recoil, H_recoil);

            ShootingLight_frame = ShootingLightTime;
        } else {
            Adsrc.clip = NoBulletClip;
            Adsrc.Play();
        }
    }

    //Player의 x축, z축 움직임을 담당
    void FPMove()
    {
        forwardSpeed = Input.GetAxis("Vertical") * movementSpeed;
        sideSpeed = Input.GetAxis("Horizontal") * movementSpeed;

        if (cc.isGrounded && Input.GetButtonDown("Jump"))
            verticalVelocity = jumpSpeed;
        
        verticalVelocity += Physics.gravity.y * Time.deltaTime;

        speed = new Vector3(sideSpeed, verticalVelocity, forwardSpeed);
        speed = transform.rotation * speed;

        cc.Move(speed * Time.deltaTime);
    }

    //Player의 회전을 담당
    void FPRotate(int vRecoil, int hRecoil)
    {
        //좌우 회전
        rotLeftRight = Input.GetAxis("Mouse X") * mouseSensitivity + hRecoil;
        transform.Rotate(0f, rotLeftRight, 0f);

        //상하 회전
        verticalRotation -= Input.GetAxis("Mouse Y") * mouseSensitivity + vRecoil;
        verticalRotation = Mathf.Clamp(verticalRotation, -upDownRange, upDownRange);
        playerHead.transform.localRotation = Quaternion.Euler(verticalRotation, 0f, 0f);
        if (vRecoil != 0) { tvRecoil = 0; thRecoil = 0; }
    }
}
