using System;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;

public class CharacterGroupController : MonoBehaviour
{
    private GameObject gameStartPanel;
    private GameObject hubPanel;

    private void Start()
    {
        gameStartPanel = transform.parent.gameObject;
        if (!gameStartPanel)
        {
            Debug.LogError("GameStart 없습니다.");
            return;
        }

        // 캔버스에서 안에서 HUD를 찾습니다.
        Canvas rootCanvas = GetComponentInParent<Canvas>();
        if (rootCanvas)
        {
            // HUD를 이름으로 찾습니다.
            Transform hudTransform = rootCanvas.transform.Find("HUD");
            if (hudTransform)
                hubPanel = hudTransform.gameObject;
            if (!hubPanel)
                Debug.LogError("HUD 없습니다.");
        }
        else { Debug.LogError("Canvas 없습니다."); }


        // GameObject(CharacterGroup)의 모든 자식에 있는 Button을 가져옵니다.
        // (true) 비활성화된 버튼도 포함
        Button[] characterButtons = GetComponentsInChildren<Button>(true);

        foreach (Button button in characterButtons)
        {
            // 버튼의 이름(예: Character0)에서 숫자 ID를 추출합니다.
            // 정규식을 사용해 이름 끝에 있는 숫자(\d+)를 찾습니다.
            Match match = Regex.Match(button.gameObject.name, @"\d+$");

            if (!match.Success)
            {
                Debug.LogWarning($"숫자가 없는 버튼: {button.gameObject.name}");
                continue;
            }
            // C# for, foreach 람다 버그 예방용
            int characterId;
            try
            {
                // 문자열(match.value)을 정수로 전환합니다.
                characterId = Int32.Parse(match.Value);
            }
            catch (Exception e)
            {
                Debug.LogError($"{button.gameObject.name} ID 파싱 중 오류: {e.Message}");
                continue;
            }
            int captureId = characterId;

            button.onClick.AddListener(() =>
            {
                Debug.Log($"ID: {captureId} 람다 실행");
                if (gameStartPanel) { gameStartPanel.SetActive(false); }
                if (hubPanel) { hubPanel.SetActive(true); }
                if (GameManager.instance) { GameManager.instance.GameStart(captureId); }
                else { Debug.LogError("GameManger 싱글턴 확인하세요"); }
            });

            // 인스펙터에 등록된 리스너 개수를 센다
            int persistentCount = button.onClick.GetPersistentEventCount();
            Debug.LogWarning($"리스너 등록 확인: '{button.gameObject.name}' " +
                $"| 인스펙터 개수: {persistentCount}");
        }
    }
}
