using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum GameState {
	MainMenu,
	PlacingObjects,
	Playing,
	SuccessfulEnd
}

[System.Serializable]
public struct ObjectData
{
    public Sprite ObjectImage;
    public GameObject ObjectPrefab;
	public int Count;
}

[System.Serializable]
public struct LevelData {
	public string SceneName;
	public ObjectData[] Objects;
}

public class GameManager : GenericSingletonBehaviour<GameManager> {

	public LevelData[] Levels;
	public ObjectPlacementSystem ObjectPlacement;
	public CamPan CamPan;
	public IngameUI IngameUI;
	public OrbitCam OrbitCamPrefab;
	public AudioSource MusicSource;
	public AudioClip PlayMusic;
	public AudioClip PlacementMusic;
	public AudioSource SFXSource;

	private GameState _currentState;
	private int _currentLevelIndex;
	private List<ObjectData> _currentObjectList = new List<ObjectData>();
	private int _currentPlacementObjIndex;

	void Start() {
		DontDestroyOnLoad(gameObject);
		ObjectPlacement.OnObjectPlaced += CurrentObjectPlaced;
		Barbie.OnBarbiesTouched += BarbiesTouched;
		IngameUI.PlayButton.onClick.AddListener(() => {
			if(_currentState == GameState.PlacingObjects){
				SetState(GameState.Playing);
			} else if(_currentState == GameState.Playing) {
				Time.timeScale = 1;
			}
		});
		IngameUI.StopButton.onClick.AddListener(() => {
			if(_currentState == GameState.Playing){
				SetState(GameState.PlacingObjects);
			}
		});
		IngameUI.PauseButton.onClick.AddListener(() => {
			if(_currentState == GameState.Playing){
				Time.timeScale = 0;
			}
		});
		SetState(GameState.MainMenu);
	} 

	void Update () {
		switch(_currentState){
			case GameState.MainMenu:
				if(Input.GetKeyDown(KeyCode.F5)){
                    LoadLevel(0);
					
				}
			break;
			case GameState.PlacingObjects:
				if(Input.GetKeyDown(KeyCode.Space)){
					SetState(GameState.Playing);
				}
			break;
			case GameState.Playing:
				if(Input.GetKeyDown(KeyCode.Space)){
					SetState(GameState.PlacingObjects);
				}
			break;
			case GameState.SuccessfulEnd:
				if(Input.GetKeyDown(KeyCode.Return)){
					if(_currentLevelIndex < Levels.Length-1){
						LoadLevel(_currentLevelIndex+1);
					} else {
						SetState(GameState.MainMenu);
					}
				}
			break;
		}
	}

	public void LoadLevel(int levelIndex){
		_currentLevelIndex = levelIndex;
        ObjectPlacement.ClearPlacedObjects();
		IngameUI.ClearList();
		_currentObjectList.Clear();
		_currentObjectList.AddRange(Levels[_currentLevelIndex].Objects);
		for(int objIndex=0; objIndex < _currentObjectList.Count; ++objIndex){
			int clickIndex = objIndex;
			IngameUI.AddObject(_currentObjectList[objIndex].ObjectImage, _currentObjectList[objIndex].Count,
				() => ObjectClicked(clickIndex));
		}
		ExecutionDelayer.Instance.ExecuteNextFrame(() => 
			UnityEngine.UI.LayoutRebuilder.ForceRebuildLayoutImmediate(IngameUI.GetComponent<RectTransform>()));
        SceneManager.LoadScene(Levels[_currentLevelIndex].SceneName, LoadSceneMode.Single);
        SceneManager.LoadScene("DynamicObjects", LoadSceneMode.Additive);
        ExecutionDelayer.Instance.ExecuteNextFrame(() =>
        {
            SceneManager.SetActiveScene(SceneManager.GetSceneByName("DynamicObjects"));
            var campos = GameObject.FindWithTag("campos");
            if (campos != null)
            {
                CamPan.transform.position = campos.transform.position;
				CamPan.transform.eulerAngles = new Vector3(CamPan.transform.eulerAngles.x, campos.transform.eulerAngles.y, 0);
            }
        });
		ExecutionDelayer.Instance.ExecuteNextFrame(() => SetState(GameState.PlacingObjects));
	}

	public void SetState(GameState state){
		switch(state){
			case GameState.MainMenu:
				SceneManager.LoadScene("MainMenu", LoadSceneMode.Single);
				CamPan.DisableCamPan();
				transform.position = Vector3.zero;
				transform.eulerAngles = new Vector3(transform.eulerAngles.x, 0, 0);
				ObjectPlacement.DisableObjectPlacement();
				ObjectPlacement.ClearPlacedObjects();
				IngameUI.ActivateElements(false, false, false, false);
				IngameUI.ClearList();
				MusicSource.clip = PlayMusic;
				MusicSource.Play();
				Time.timeScale = 1;
				break;
			case GameState.PlacingObjects:
				if(_currentState == GameState.Playing){
					foreach(var barb in GameObject.FindObjectsOfType<Barbie>()){
						Destroy(barb.gameObject);
					}
					ObjectPlacement.RestorePlacedObjectState();
                    var currentActiveLevelScene = SceneManager.GetSceneByName(Levels[_currentLevelIndex].SceneName);
					SceneManager.LoadScene(Levels[_currentLevelIndex].SceneName, LoadSceneMode.Additive);
					ExecutionDelayer.Instance.ExecuteNextFrame(() => {
						SceneManager.UnloadScene(currentActiveLevelScene);
                        SceneManager.SetActiveScene(SceneManager.GetSceneByName("DynamicObjects"));
					});
				}
				ObjectPlacement.EnableObjectPlacement();
				CamPan.EnableCamPan();
                IngameUI.ActivateElements(true, true, false, false);
                MusicSource.clip = PlacementMusic;
				MusicSource.Play();
                Time.timeScale = 0;
			break;
			case GameState.Playing:
				if(_currentState == GameState.PlacingObjects){
					ObjectPlacement.SavePlacedObjectState();
				}
				ObjectPlacement.DisableObjectPlacement();
				CamPan.EnableCamPan();
                IngameUI.ActivateElements(false, true, false, false);
                MusicSource.clip = PlayMusic;
				MusicSource.Play();
				Time.timeScale = 1;
			break;
			case GameState.SuccessfulEnd:
				Time.timeScale = 0;
                if (_currentLevelIndex < Levels.Length - 1) {
                    IngameUI.ActivateElements(false, false, true, false);
                } else {
                    IngameUI.ActivateElements(false, false, false, true);
                }
			break;
		}
		_currentState = state;
	}

	private void ObjectClicked(int objIndex){
		if(_currentState == GameState.PlacingObjects){
			if(_currentObjectList[objIndex].Count > 0){
				_currentPlacementObjIndex = objIndex;
				ObjectPlacement.StartPlacement(_currentObjectList[_currentPlacementObjIndex].ObjectPrefab);
			}
		}
	}

	private void CurrentObjectPlaced() {
		if(_currentPlacementObjIndex != -1){
			var temp = _currentObjectList[_currentPlacementObjIndex];
			temp.Count--;
            IngameUI.SetObjectCount(_currentPlacementObjIndex, temp.Count);
			_currentObjectList[_currentPlacementObjIndex] = temp;
			_currentPlacementObjIndex = -1;
		}
	}

	private void BarbiesTouched(Vector3 worldpos) {
		if(_currentState == GameState.Playing){
			SFXSource.Play();
			SetState(GameState.SuccessfulEnd);
			Instantiate(OrbitCamPrefab, worldpos, Quaternion.identity);
		}
	}

    public override string GetName() {
        return "GameManager";
    }
}
