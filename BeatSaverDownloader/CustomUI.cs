﻿using HMUI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using VRUI;

namespace BeatSaverDownloader
{
    class CustomUI : MonoBehaviour
    {

        private RectTransform _mainMenuRectTransform;
        private MainMenuViewController _mainMenuViewController;

        private Button _buttonInstance;
        private Button _backButtonInstance;

        static CustomUI _instance;

        public static List<Sprite> icons = new List<Sprite>();

        public CustomViewController _beatSaverViewController;

        internal static void OnLoad()
        {
            if (_instance != null)
            {
                return;
            }
            new GameObject("Custom UI").AddComponent<CustomUI>();
            
        }

        private void Awake()
        {
            _instance = this;
            foreach (Sprite sprite in Resources.FindObjectsOfTypeAll<Sprite>())
            {
                Debug.Log(sprite.name);
                icons.Add(sprite);
            }
            Debug.Log("Trying to find buttons...");
            try
            {
                _buttonInstance = Resources.FindObjectsOfTypeAll<Button>().Where(x => (x.name == "QuitButton")).First();
                _backButtonInstance = Resources.FindObjectsOfTypeAll<Button>().Where(x => (x.name == "BackArrowButton")).First();
                _mainMenuViewController = Resources.FindObjectsOfTypeAll<MainMenuViewController>().First();
                _mainMenuRectTransform = _buttonInstance.transform.parent as RectTransform;
                Debug.Log("Buttons and main menu found!");
                

            }
            catch(Exception e)
            {
                Debug.Log("EXCEPTION ON AWAKE(TRY FIND BUTTONS): "+e);
            }

            try
            {
                CreateBeatSaverButton();

                Debug.Log("BeatSaver button created!");
            }
            catch (Exception e)
            {
                Debug.Log("EXCEPTION ON AWAKE(TRY CREATE BUTTON): " + e);
            }
        }

        private void CreateBeatSaverButton()
        {
            
            Button _beatSaverButton = CreateUIButton(_mainMenuRectTransform);
            
            try
            {
                (_beatSaverButton.transform as RectTransform).anchoredPosition = new Vector2(30f, 7f);
                (_beatSaverButton.transform as RectTransform).sizeDelta = new Vector2(28f, 10f);

                SetButtonText(ref _beatSaverButton, "BeatSaver");

                SetButtonIcon(ref _beatSaverButton, icons.Where(x => (x.name == "SettingsIcon")).First());
                
                _beatSaverButton.onClick.AddListener(delegate () {

                    try
                    {
                        Debug.Log("Created button pressed");

                        if (_beatSaverViewController == null)
                        {
                            Debug.Log("BeatSaverViewController is null. Creating new...");
                            _beatSaverViewController = CreateViewController();
                            Debug.Log("Done!");
                            

                        }
                        _mainMenuViewController.PresentModalViewController(_beatSaverViewController, null, false);
                        
                    }
                    catch (Exception e)
                    {
                        Debug.Log("EXCETPION IN BUTTON: "+e.Message);
                    }

                });

            }
            catch(Exception e)
            {
                Debug.Log("EXCEPTION: "+e.Message);
            }
            Debug.Log("Finished");

        }

       

        public Button CreateUIButton(RectTransform parent)
        {
            if (_buttonInstance == null)
            {
                return null;
            }

            Button btn = Instantiate(_buttonInstance, parent, false);
            DestroyImmediate(btn.GetComponent<GameEventOnUIButtonClick>());
            btn.onClick = new Button.ButtonClickedEvent();

            return btn;
        }

        public Button CreateBackButton(RectTransform parent)
        {
            if (_backButtonInstance == null)
            {
                return null;
            }

            Button _button = Instantiate(_backButtonInstance, parent, false);
            DestroyImmediate(_button.GetComponent<GameEventOnUIButtonClick>());
            _button.onClick = new Button.ButtonClickedEvent();

            return _button;
        }

        public CustomViewController CreateViewController()
        {
            CustomViewController vc = new GameObject("CustomViewController").AddComponent<CustomViewController>();

            vc.rectTransform.anchorMin = new Vector2(0f, 0f);
            vc.rectTransform.anchorMax = new Vector2(1f, 1f);
            vc.rectTransform.sizeDelta = new Vector2(0f, 0f);
            vc.rectTransform.anchoredPosition = new Vector2(0f, 0f);

            return vc;
        }

        public TextMeshProUGUI CreateText(RectTransform parent, string text, Vector2 position)
        {
            TextMeshProUGUI textMesh = new GameObject("TextMeshProUGUI_GO").AddComponent<TextMeshProUGUI>();
            textMesh.rectTransform.SetParent(parent, false);
            textMesh.text = text;
            textMesh.fontSize = 4;
            textMesh.color = Color.white;
            textMesh.font = Resources.Load<TMP_FontAsset>("Teko-Medium SDF No Glow");
            textMesh.rectTransform.anchorMin = new Vector2(0.5f, 1f);
            textMesh.rectTransform.anchorMax = new Vector2(1f, 1f);
            textMesh.rectTransform.sizeDelta = new Vector2(60f, 10f);
            textMesh.rectTransform.anchoredPosition = position;

            return textMesh;
        }

        public void SetButtonText(ref Button _button, string _text)
        {
            if (_button.GetComponentInChildren<TextMeshProUGUI>() != null)
            {

                _button.GetComponentInChildren<TextMeshProUGUI>().text = _text;
            }

        }

        public void SetButtonIcon(ref Button _button, Sprite _icon)
        {
            if (_button.GetComponentsInChildren<UnityEngine.UI.Image>().Count() > 1)
            {

                _button.GetComponentsInChildren<UnityEngine.UI.Image>()[1].sprite = _icon;
            }

        }

        public void SetButtonBackground(ref Button _button, Sprite _background)
        {
            if (_button.GetComponentsInChildren<UnityEngine.UI.Image>().Count() > 0)
            {

                _button.GetComponentsInChildren<UnityEngine.UI.Image>()[0].sprite = _background;
            }

        }


    }
}
