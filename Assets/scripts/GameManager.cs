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

public class GameManager : GenericSingletonBehaviour<GameManager> {

	public string[] LevelScenes;
	public ObjectPlacementSystem ObjectPlacement;
	public CamPan CamPan;

	private GameState _currentState;
	private int _currentLevelIndex;

	void Awake() {
		DontDestroyOnLoad(gameObject);
		SetState(GameState.MainMenu);
	} 

	public void SetGameState(GameState NewGameState) {
		
		switch(NewGameState) {
			case GameState.Playing:
				SceneManager.LoadScene(LevelScenes[_currentLevelIndex], LoadSceneMode.Single);
				SceneManager.LoadScene("DynamicObjects", LoadSceneMode.Additive);
				ExecutionDelayer.Instance.ExecuteNextFrame(() => {
					SceneManager.SetActiveScene(SceneManager.GetSceneByName("DynamicObjects"));
					SetState(GameState.PlacingObjects);
					var campos = GameObject.FindWithTag("campos");
					if (campos != null) {
						CamPan.transform.position = campos.transform.position;
					}
				});
			
			break;
		}

		_currentState = NewGameState;
	}

	void Update () {
		switch(_currentState){
			case GameState.MainMenu:
				if(Input.GetKeyDown(KeyCode.F5)){
                    SceneManager.LoadScene(LevelScenes[_currentLevelIndex], LoadSceneMode.Single);
					SceneManager.LoadScene("DynamicObjects", LoadSceneMode.Additive);
					ExecutionDelayer.Instance.ExecuteNextFrame(() => {
						SceneManager.SetActiveScene(SceneManager.GetSceneByName("DynamicObjects"));
						SetState(GameState.PlacingObjects);
                        var campos = GameObject.FindWithTag("campos");
                        if (campos != null) {
                            CamPan.transform.position = campos.transform.position;
                        }
					});
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

			break;
		}
	}


	public void SetState(GameState state){
		switch(state){
			case GameState.MainMenu:
				SceneManager.LoadScene("MainMenu", LoadSceneMode.Single);
				CamPan.DisableCamPan();
				ObjectPlacement.DisableObjectPlacement();
				ObjectPlacement.ClearPlacedObjects();
				break;
			case GameState.PlacingObjects:
				if(_currentState == GameState.Playing){
					ObjectPlacement.RestorePlacedObjectState();
                    var currentActiveLevelScene = SceneManager.GetSceneByName(LevelScenes[_currentLevelIndex]);
					SceneManager.LoadScene(LevelScenes[_currentLevelIndex], LoadSceneMode.Additive);
					ExecutionDelayer.Instance.ExecuteNextFrame(() => {
						SceneManager.UnloadScene(currentActiveLevelScene);
                        SceneManager.SetActiveScene(SceneManager.GetSceneByName("DynamicObjects"));
					});
				}
				ObjectPlacement.EnableObjectPlacement();
				CamPan.EnableCamPan();
                Time.timeScale = 0;
			break;
			case GameState.Playing:
				if(_currentState == GameState.PlacingObjects){
					ObjectPlacement.SavePlacedObjectState();
				}
				ObjectPlacement.DisableObjectPlacement();
				CamPan.EnableCamPan();
				Time.timeScale = 1;
			break;
		}
		_currentState = state;
	}

    public override string GetName() {
        return "GameManager";
    }
}
