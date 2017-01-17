using UnityEngine;
using System.Collections;
using System;

public abstract class Chessman : MonoBehaviour {
    public int CurrentX { get; set; }
    public int CurrentY { get; set; }
    public bool isWhite;

    public abstract string Annotation();

    private Quaternion originRotation;
    private float speed = 1f;
    private float incSpeed = 1.0f;

    private Animator anim;

    private bool startMove = false;


    private AudioSource _audioSource = null;
    private AudioSource AudioSource
    {
        get
        {
            if (_audioSource == null)
            {
                this.gameObject.AddComponent<AudioSource>();
                return this.gameObject.GetComponent<AudioSource>();
            }
            return _audioSource;
        }
    }
    private static AudioClip _manDeadAudio;
    private static AudioClip ManDeadAudio
    {
        get {
               if (_manDeadAudio == null)
            {
                _manDeadAudio = Resources.Load<AudioClip>("Sound/Game/dead_man");
            }
            return _manDeadAudio;
        }
    }

    private static AudioClip _femaleDeadAudio;
    private static AudioClip FemaleDeadAudio
    {
        get
        {
            if (_femaleDeadAudio == null)
            {
                _femaleDeadAudio = Resources.Load<AudioClip>("Sound/Game/dead_female");
            }
            return _femaleDeadAudio;
        }
    }

    private static AudioClip _moveSurfAudio;
    private static AudioClip MoveSurfAudio
    {
        get
        {
            if (_moveSurfAudio == null)
            {
                _moveSurfAudio = Resources.Load<AudioClip>("Sound/Game/move_surf");
            }
            return _moveSurfAudio;
        }
    }

    private static GameObject _VFX_ParticlePath;
    private static GameObject VFX_ParticlePath
    {
        get
        {
            if (_VFX_ParticlePath == null)
            {
                _VFX_ParticlePath = Resources.Load<GameObject>("Prefabs/VFX/VFX_ParticlePath");
            }
            return _VFX_ParticlePath;
        }
    }

    private GameObject _particlePathGO = null;
    private GameObject ParticlePathGO
    {
        get
        {
            if (_particlePathGO == null)
            {
                _particlePathGO = Instantiate(VFX_ParticlePath);
                _particlePathGO.transform.parent = gameObject.transform;

                if (isWhite)
                {
                    _particlePathGO.transform.localPosition = new Vector3(0, 0.5f, -0.2f);
                } else
                {
                    _particlePathGO.transform.localPosition = new Vector3(0, 0.5f, 0.2f);
                }
                
            }
            return _particlePathGO;
        }
    }


    
    private void Start()
    {
        originRotation = transform.rotation;

        //Animator[] anims = GetComponentsInChildren<Animator>();

        //anim = anims[0];

        //if (anims.Length >= 2)
        //    anim = anims[anims.Length - 1];

        //if (anim == null)
        //{
        //    Debug.Log("Chessman's animator is null");
        //}
    }

    private void Update()
    {
        if (startMove)
        {
            float step = speed * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, newPosition, step);

            speed += incSpeed;

            if (transform.position == newPosition)
            {
                newPosition = Vector3.zero;
                startMove = false;
                speed = 1f;
                HideMoveEffect();
            }
                
        }
    }

    private void HideMoveEffect()
    {
        ParticlePathGO.SetActive(false);
    }

    private void ShowMoveEffect()
    {
        ParticlePathGO.SetActive(true);
    }

    public void SetPosition(int x, int y)
    {
        CurrentX = x;
        CurrentY = y;
    }

    public virtual bool[,] PossibleMove()
    {
        return new bool[8, 8];
    }

    public virtual bool[,] PossibleEat()
    {
        return new bool[8, 8];
    }

    public bool CanGo(int x, int y)
    {
        bool[,] possible = this.PossibleMove();

        return possible[x, y];
    }

    internal void RotateEach(float seconds)
    {
        InvokeRepeating("Rotate", 0f, seconds);
    }

    private GameObject _powerEffect;
    private GameObject PowerEffect
    {
        get
        {
            if (_powerEffect == null)
            {
                _powerEffect = gameObject.transform.GetChild(2).gameObject;
            }
            return _powerEffect;
        }
    }

    public void PlayPowerEffectFor(float seconds)
    {
        if (PowerEffect == null)
        {
            Debug.Log(CurrentX + ";" + CurrentY + " " + Annotation());
        }
        PowerEffect.SetActive(true);
        Invoke("HidePowerEffect", seconds);
    }

    public void ShowPowerEffect()
    {
        PowerEffect.SetActive(true);
    }

    public void HidePowerEffect()
    {
        PowerEffect.SetActive(false);
    }

    private int t = 5;
    private int dt = 5;
    private void Rotate()
    {
        transform.Rotate(Vector3.up * t);
        t += dt;
    }

    internal void DestroyAfter(float seconds)
    {
        PlayDieSound();
        Invoke("DestroyGameObject", seconds);
    }

    private void PlayDieSound()
    {
        if (this.Annotation() != "Q")
        {
            AudioSource.PlayOneShot(ManDeadAudio);
        }
        else
        {
            AudioSource.PlayOneShot(FemaleDeadAudio);
        }
    }

    private void DestroyGameObject()
    {
        BoardManager.Instance.GetAllChessmans().Remove(gameObject);
        Destroy(gameObject);
    }

    private Vector3 newPosition = Vector3.zero;
    internal void MoveAfter(float seconds, Vector3 position)
    {
        newPosition = position;
        Invoke("Move", seconds);
    }

    private void PlayMoveSoundEffect()
    {
        AudioSource.PlayOneShot(MoveSurfAudio);
    }

    private void Move()
    {
        startMove = true;
        ShowMoveEffect();
        PlayMoveSoundEffect();
        //transform.position = newPosition;

        // stop rotation
        // CancelInvoke("Rotate");

        // restore origin rotation
        //transform.rotation = originRotation;

        // restore rotate speed
        //t = 10;
    }
}