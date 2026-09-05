#if UNITY_EDITOR
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using UnityEngine.SceneManagement;
using UnityEditor.SceneManagement;

public class CurrentSceneHealthUI
{
    [InitializeOnLoadMethod]
    private static void RunOnce()
    {
        EditorApplication.delayCall += () =>
        {
            if (EditorApplication.isPlayingOrWillChangePlaymode) return;

            string key = "CurrentSceneHealthUI_Run_1";
            if (SessionState.GetBool(key, false)) return;
            SessionState.SetBool(key, true);

            Scene sc = SceneManager.GetActiveScene();
            bool modified = false;

            Canvas canvas = Object.FindObjectOfType<Canvas>();
            if (canvas == null)
            {
                GameObject canvasObj = new GameObject("Canvas");
                canvas = canvasObj.AddComponent<Canvas>();
                canvas.renderMode = RenderMode.ScreenSpaceOverlay;
                canvasObj.AddComponent<CanvasScaler>();
                canvasObj.AddComponent<GraphicRaycaster>();
                modified = true;
            }

            // Slider Player
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
            {
                HealthSystem hsPlayer = player.GetComponent<HealthSystem>();
                if (hsPlayer != null && hsPlayer.sliderVita == null)
                {
                    Transform existing = canvas.transform.Find("SliderVitaPlayer");
                    if (existing == null)
                    {
                        GameObject sliderObj = new GameObject("SliderVitaPlayer");
                        sliderObj.transform.SetParent(canvas.transform, false);
                        RectTransform rt = sliderObj.AddComponent<RectTransform>();
                        rt.sizeDelta = new Vector2(250, 20);
                        rt.anchorMin = new Vector2(0, 1);
                        rt.anchorMax = new Vector2(0, 1);
                        rt.pivot = new Vector2(0, 1);
                        rt.anchoredPosition = new Vector2(20, -20);

                        Slider slider = sliderObj.AddComponent<Slider>();
                        slider.interactable = false;
                        slider.transition = Selectable.Transition.None;

                        GameObject bgObj = new GameObject("Background");
                        bgObj.transform.SetParent(sliderObj.transform, false);
                        Image bgImg = bgObj.AddComponent<Image>();
                        bgImg.color = new Color(0.2f, 0.2f, 0.2f, 1f);
                        RectTransform bgRt = bgObj.GetComponent<RectTransform>();
                        bgRt.anchorMin = Vector2.zero;
                        bgRt.anchorMax = Vector2.one;
                        bgRt.sizeDelta = Vector2.zero;

                        GameObject fillAreaObj = new GameObject("Fill Area");
                        fillAreaObj.transform.SetParent(sliderObj.transform, false);
                        RectTransform fillAreaRt = fillAreaObj.AddComponent<RectTransform>();
                        fillAreaRt.anchorMin = Vector2.zero;
                        fillAreaRt.anchorMax = Vector2.one;
                        fillAreaRt.sizeDelta = new Vector2(-10, 0);

                        GameObject fillObj = new GameObject("Fill");
                        fillObj.transform.SetParent(fillAreaObj.transform, false);
                        Image fillImg = fillObj.AddComponent<Image>();
                        fillImg.color = Color.green;
                        RectTransform fillRt = fillObj.GetComponent<RectTransform>();
                        fillRt.sizeDelta = Vector2.zero;

                        slider.fillRect = fillRt;
                        hsPlayer.sliderVita = slider;
                        modified = true;
                    }
                }
            }

            // Slider Boss
            GameObject boss = GameObject.FindGameObjectWithTag("Boss"); // Assumendo abbia tag Boss, altrimenti lo cerco in altro modo
            if (boss == null)
            {
                // Cerco un nemico
                NemicoAI nemico = Object.FindObjectOfType<NemicoAI>();
                if (nemico != null) boss = nemico.gameObject;
            }

            if (boss != null)
            {
                HealthSystem hsBoss = boss.GetComponent<HealthSystem>();
                if (hsBoss != null && hsBoss.sliderVita == null)
                {
                    Transform existing = canvas.transform.Find("SliderVitaBoss");
                    if (existing == null)
                    {
                        GameObject sliderObj = new GameObject("SliderVitaBoss");
                        sliderObj.transform.SetParent(canvas.transform, false);
                        RectTransform rt = sliderObj.AddComponent<RectTransform>();
                        rt.sizeDelta = new Vector2(250, 20);
                        rt.anchorMin = new Vector2(1, 1);
                        rt.anchorMax = new Vector2(1, 1);
                        rt.pivot = new Vector2(1, 1);
                        rt.anchoredPosition = new Vector2(-20, -20);

                        Slider slider = sliderObj.AddComponent<Slider>();
                        slider.interactable = false;
                        slider.transition = Selectable.Transition.None;

                        GameObject bgObj = new GameObject("Background");
                        bgObj.transform.SetParent(sliderObj.transform, false);
                        Image bgImg = bgObj.AddComponent<Image>();
                        bgImg.color = new Color(0.2f, 0.2f, 0.2f, 1f);
                        RectTransform bgRt = bgObj.GetComponent<RectTransform>();
                        bgRt.anchorMin = Vector2.zero;
                        bgRt.anchorMax = Vector2.one;
                        bgRt.sizeDelta = Vector2.zero;

                        GameObject fillAreaObj = new GameObject("Fill Area");
                        fillAreaObj.transform.SetParent(sliderObj.transform, false);
                        RectTransform fillAreaRt = fillAreaObj.AddComponent<RectTransform>();
                        fillAreaRt.anchorMin = Vector2.zero;
                        fillAreaRt.anchorMax = Vector2.one;
                        fillAreaRt.sizeDelta = new Vector2(-10, 0);

                        GameObject fillObj = new GameObject("Fill");
                        fillObj.transform.SetParent(fillAreaObj.transform, false);
                        Image fillImg = fillObj.AddComponent<Image>();
                        fillImg.color = Color.red;
                        RectTransform fillRt = fillObj.GetComponent<RectTransform>();
                        fillRt.sizeDelta = Vector2.zero;

                        slider.fillRect = fillRt;
                        hsBoss.sliderVita = slider;
                        modified = true;
                    }
                }
            }

            if (modified)
            {
                EditorSceneManager.MarkSceneDirty(sc);
                EditorSceneManager.SaveScene(sc);
            }

            string thisScriptPath = "Assets/Editor/CurrentSceneHealthUI.cs";
            if (System.IO.File.Exists(thisScriptPath))
            {
                AssetDatabase.DeleteAsset(thisScriptPath);
            }
        };
    }
}
#endif