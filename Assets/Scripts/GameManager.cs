using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Playables;

public class GameManager : MonoBehaviour
{
    public static Action<string> OnAnswer;
    public static Action<bool> OnValidateAnswer;
    public static Action<int> OnSetPlayer;

    public static GameManager Instance { get; private set; }
    [SerializeField] public List<PlayerController> m_players;
    [SerializeField] public int m_initialLife;

    public PlayerController ActivePlayer { get; private set; }
    [SerializeField] private PlayableDirector _director;
    public bool AllPlayerReady => m_players.All(x => x.IsReady);
    public int _nextPlayer;

    private void Awake()
    {
        Instance = this;
    }

    private void OnEnable()
    {
        OnSetPlayer += SetActivePlayer;
        OnValidateAnswer += CheckResult;
    }



    private void OnDisable()
    {
        OnSetPlayer -= SetActivePlayer;
        OnValidateAnswer -= CheckResult;
    }

    private async void Start()
    {
        foreach (var player in m_players)
        {
            player.SetLife(m_initialLife);
        }
        await Task.Delay(1000);
        OnSetPlayer?.Invoke(0);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            _director.Play();
        }
    }

    public bool GetActivePlayerName(string _name)
    {
        return ActivePlayer.Name == _name;
    }

    private void SetActivePlayer(int _arg)
    {
        ActivePlayer = m_players[_arg];
    }
    

    private async void CheckResult(bool obj)
    {
        var currentIndex = m_players.IndexOf(ActivePlayer);
        var nextIndex = (currentIndex + 1) % m_players.Count;
        var nextPlayer = m_players[nextIndex];

        var _player = obj ? nextPlayer : ActivePlayer;
        _player.Damage();
        if (_player.Life <= 0)
        {
            _player.RemoveHeart();
            Debug.Log($"End Game Winner {ActivePlayer.Name}");
            await Task.Delay(500);
            if (obj)
            {
                ActivePlayer.Finisher();
            }
            else
            {
                nextPlayer.Finisher();
            }
            return;
        }

        if (obj)
        {
            ActivePlayer.GetAttackCutscene();
        }
        else
        {
            nextPlayer.GetAttackCutscene();
        }
        _nextPlayer = nextIndex;
        
        await Task.Delay(500);
        _player.RemoveHeart();

    }

    public void Setup()
    {
        OnSetPlayer(_nextPlayer);
    }
}
