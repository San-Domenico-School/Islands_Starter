using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;

namespace bgustin
{
    public class UIController : MonoBehaviour
    {
        private UIDocument uiDocument;
        private Label[] teamScoresUI;
        private ProgressBar levelProgress;
        private ProgressBar gameProgress;
        private int[] teamScores = new int[4]; 

        private int levelTime = 90;


        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            // Get the UIDocument component
            uiDocument = GetComponent<UIDocument>();
            var root = uiDocument.rootVisualElement;
            
            teamScoresUI = new Label[] {
                                        root.Q<Label>("9thScore"),
                                        root.Q<Label>("10thScore"),
                                        root.Q<Label>("11thScore"),
                                        root.Q<Label>("12thScore")
                                        };
            
            levelProgress = root.Q<ProgressBar>("LevelTimeRemaining");
            gameProgress = root.Q<ProgressBar>("GameTimeRemaining");

            InvokeRepeating("TimeRemaining", 0, 1f);
        }

        // Update is called once per frame
        void Update()
        {
            ScoreUpdate();
            ProgressUpdate();
        }

        private void ScoreUpdate()
        {
            for(int i = 0; i < teamScoresUI.Length; i++)
            {
                string grade = (i + 9) + "th grade: ";
                teamScores[i] = GlobalEvents.TeamScores[i];
                teamScoresUI[i].text = grade + teamScores[i].ToString();
                Debug.Log($"Current score for team {i}: {GlobalEvents.TeamScores[i]}");
            }
        }

        private void ProgressUpdate()
        {
            levelProgress.value = levelTime;
            gameProgress.value =  GlobalEvents.GameTime;
        }

        private void TimeRemaining()
        {
            levelTime--;

            // Decrements time
            GlobalEvents.SendGameTime(); 
            SwitchScenes();                                     
        }

        private void SwitchScenes()
        {
            if(levelTime == 0)
            {
                // Increments scene index
                GlobalEvents.SendSceneIndex();

                if(GlobalEvents.SceneIndex < SceneManager.sceneCountInBuildSettings)
                { 
                    SceneManager.LoadScene(GlobalEvents.SceneIndex);
                }
                else if(GlobalEvents.GameTime == 0)
                {
                    Time.timeScale = 0;
                    CancelInvoke("TimeRemaining");
                }
            }
        }
    }
}