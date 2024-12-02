using System.Collections.Generic;
using System.Collections;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using Task = System.Threading.Tasks.Task;

public class QuestionUI : MonoBehaviour
{
    [Header("Question List")]
    [SerializeField] private QuestionSO[] m_questionSO;
    
    [Header("Question UI")]
    [SerializeField] private GameObject m_questionPanel;
    [SerializeField] private TextMeshProUGUI m_questionTMP;
    [SerializeField] private AnswerHandler[] m_answerHandlers;
    
    [Header("Answer sprite image")]
    [SerializeField] private Sprite m_einsteinAnswerImage;
    [SerializeField] private Sprite m_sabrinaAnswerImage;

    [Header("Result Banner")] 
    [SerializeField] private GameObject m_correctBanner;
    [SerializeField] private GameObject m_wrongBanner;
    
    [Header("Debug")]
    public bool isDebugMode;
    
    private List<int> _usedQuestionList = new List<int>();
    private GameManager _gameManager;

    private QuestionSO _activeQuestion;

    private int activePlayer = 0; //if 0 player 1 if 1 player 2 

    private void OnEnable()
    {
        GameManager.OnAnswer += OnAnswer;
        GameManager.OnSetPlayer += SetupQuestion;
    }

    private void Start()
    {
        _gameManager = GameManager.Instance;
        m_questionPanel.LeanScale(Vector3.zero, 0);
    }

    private void OnDisable()
    {
        GameManager.OnAnswer -= OnAnswer;
        GameManager.OnSetPlayer -= SetupQuestion;
    }
    
    private async void SetupQuestion(int _arg)
    {
        activePlayer = _arg;
        var questionIndex = await GetQuestionAsync();
        _activeQuestion = m_questionSO[questionIndex];
        m_questionTMP.text = _activeQuestion._question;
        m_questionPanel.LeanScale(Vector3.one, 0.2f);
        var _image = _arg == 0 ? m_einsteinAnswerImage : m_sabrinaAnswerImage;
        for (var i = 0; i < _activeQuestion._choices.Length; i++)
        {
            m_answerHandlers[i].SetupAnswer(_activeQuestion._choices[i], _image);
            
            // If Debug mode Show the correct answer
            if (!isDebugMode) continue;
            if (_activeQuestion.CheckAnswer(_activeQuestion._choices[i]))
            {
                m_answerHandlers[i].ShowAnswer();
            }
            else
            {
                m_answerHandlers[i].RemoveUnderLine();
            }
        }
        
        await Task.Delay(1000);
        foreach (var button in m_answerHandlers)
        {
            button.ActivateButton(true);
        }

    }
    
    private async void OnAnswer(string obj)
    {
        var result = _activeQuestion.CheckAnswer(obj);
        if (result)
        {
            //Correct
            Debug.Log("Answer is Correct");
        }
        else
        {
            //Wrong
            Debug.Log("Answer is Wrong");
        }

        foreach (var button in m_answerHandlers)
        {
            button.ActivateButton(false);
        }
        m_questionPanel.LeanScale(Vector3.zero, 0.1f);
        StartCoroutine(WaitForResultRoutine(result));
    }

    private async Task<int> GetQuestionAsync()
    {
        var index = 0;
        do
        {
            index = Random.Range(0, m_questionSO.Length);
            await Task.Delay(100);
        } while (_usedQuestionList.Contains(index));

        return index;
    }

    private IEnumerator WaitForResultRoutine(bool _result)
    {
        var _banner = GetResultBanner(_result);
        yield return new WaitUntil(() => _gameManager.AllPlayerReady);
        _banner.LeanScale(Vector3.one, 1.5f).setEaseSpring().setOnComplete(() =>
        {
            _banner.LeanScale(Vector3.zero, 0.3f).setEaseSpring();
            GameManager.OnValidateAnswer?.Invoke(_result);
        });
    }

    private GameObject GetResultBanner(bool _result)
    {
        return _result ? m_correctBanner : m_wrongBanner;
    }
}
