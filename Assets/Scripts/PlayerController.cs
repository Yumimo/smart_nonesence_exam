using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.U2D.Animation;
using Random = UnityEngine.Random;

public class PlayerController : MonoBehaviour
{

    [Serializable]
    public class BodyPartClass
    {
        public string id;
        public GameObject part;

        public Vector2 GetPosition()
        {
            return part.transform.position;
        }
    }

    public string Name;
    
    [Header("Mechanics")]
    [SerializeField] private LifeCount m_lifeCount;
    [SerializeField] private float m_walkTime = 1;
    [SerializeField] private Transform m_battleLoc;

    [Header("Body Parts")] 
    public BodyPartClass[] bodyParts;

    private List<string> removedParts = new List<string>();
    
    private Vector3 originalPosition;
    public bool HasLefthand => removedParts.Contains("larm") && Name == "Einstein";
    public int Life
    {
        get => _life;
        private set => _life = value;
    }
    public Animator animator;
    public bool IsReady;
    private int _life;
    private static readonly int IsWalking = Animator.StringToHash("IsWalking");

    private void OnEnable()
    {
        GameManager.OnAnswer += GoToLocation;
    }

    private void OnDisable()
    {
        GameManager.OnAnswer -= GoToLocation;
    }

    private void Start()
    {
        originalPosition = transform.position;
    }

    public void SetLife(int _arg)
    {
        Life = _arg;
    }

    public void Damage()
    {
        Life--;
    }
    public void RemoveHeart()
    {
        m_lifeCount.RemoveHeart(_life);
    }
    private async Task<BodyPartClass> GetBodyPartAsync()
    {
        var part = string.Empty;
        do
        {
            part = bodyParts[Random.Range(0, bodyParts.Length)].id;
            await Task.Delay(100);
        } while (removedParts.Contains(part));

        return bodyParts.FirstOrDefault(x => x.id == part);
    }

    public async void RemoveBodyPart()
    {
        var part = await GetBodyPartAsync();
        removedParts.Add(part.id);
        var _go = Instantiate(part.part, part.GetPosition(), Quaternion.identity);
        part.part.SetActive(false);

        if (_go.TryGetComponent(out SpriteSkin _skin))
        {
            _skin.enabled = false;
        }
        var _box = _go.AddComponent<BoxCollider2D>();
        _box.sharedMaterial = new PhysicsMaterial2D
        {
            bounciness = 0.2f,
        };
        
        var _rb = _go.AddComponent<Rigidbody2D>();
        _rb.gravityScale = 1f;
        _rb.AddForce(new Vector2(5f, 10f), ForceMode2D.Impulse);
    }

    private void GoToLocation(string obj)
    {
        animator.SetBool(IsWalking, true);
        transform.LeanMoveX(m_battleLoc.transform.position.x, m_walkTime).setOnComplete(() =>
        {
            animator.SetBool(IsWalking, false);
            IsReady = true;
        });

    }
}