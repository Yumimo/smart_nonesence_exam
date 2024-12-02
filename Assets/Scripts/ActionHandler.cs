using UnityEngine;
using UnityEngine.Playables;

public class ActionHandler : MonoBehaviour
{
    private PlayerController player;
    [SerializeField] private PlayableDirector[] cutscenes;
    private PlayableDirector currentCutscene;
    private void Start()
    {
        player = GetComponent<PlayerController>();
    }

    public void FightCutscene()
    {
        if (cutscenes == null || cutscenes.Length == 0)
        {
            Debug.LogWarning("No cutscenes are assigned.");
            return;
        }
        var randomIndex = Random.Range(0, cutscenes.Length);
        currentCutscene = cutscenes[randomIndex];
        currentCutscene.stopped += OnCutsceneEnded;
        currentCutscene.Play();
    }

    private void OnCutsceneEnded(PlayableDirector obj)
    {
        currentCutscene.stopped -= OnCutsceneEnded;
        GameManager.Instance.Setup();
    }
}
