using System;
using System.Collections;
using Geo.Tutorials;
using SiberianWellness.NotNullValidation;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Game.ScenarioSystem.GuiHighlight
{
    public class TutorialGui : MonoBehaviour
    {
        [SerializeField, IsntNull]
        TutorialFacade facade;
    		
        [SerializeField]
        float defaultDarkPadding = 50;
 
        [SerializeField, IsntNull]
        Frame blockFrame;

        [SerializeField, IsntNull]
        Frame darkFrame;

        [SerializeField, IsntNull]
        GameObject blockAllPanel;

        [SerializeField, IsntNull]
        Arrow arrow;
        
        [Header("Character")] 
        [SerializeField, IsntNull]
        TutorialCharacterSpeech tutorialCharacter;
         
        [SerializeField, IsntNull]
        RectTransform defaultTextPos;
        
        [Header("Fade")]
        [SerializeField, IsntNull]
        CanvasGroup elementsCanvasGroup;
        
        [SerializeField]
        float fadeSpeed;
        
        
        [Header("AnyClick")]
        [SerializeField, IsntNull]
        Button anyClickButton;
         
        [Header("Image")]
        [SerializeField, IsntNull]
        Button imageButton;

        [SerializeField, IsntNull]
        Image image;
        
        [SerializeField, IsntNull]
        Text imageText;

        [Header("HideContent")]
        [SerializeField, IsntNull]
        GameObject hideContentPanel;
        
        [SerializeField, IsntNull]
        CanvasGroup hideContentPanelCanvas;
        
        Coroutine fadeCoroutine;
        

        public void ShowArrow(RectTransform rectTransform, ArrowOrientation orientation)
        {
            ShowArrow(rectTransform, orientation, defaultDarkPadding);
        }

        public IEnumerator WaitForClick()
        {
            var old = facade.EnableInput;
            facade.EnableInput = true;
            anyClickButton.gameObject.SetActive(true);
            var waitForButton = new WaitForButtonClick(anyClickButton);
            yield return waitForButton;

            anyClickButton.gameObject.SetActive(false);
            facade.EnableInput = old;
        }
 
        public IEnumerator ShowSprite(string text, Sprite sprite)
        {
            Hide();
            
            image.sprite   = sprite;
            imageText.text = text;
            imageButton.gameObject.SetActive(true);
            RebuildLayout(imageButton.GetComponent<RectTransform>()); 
            yield return SetAlpha(0, 1);
            var waitForButton = new WaitForButtonClick(imageButton);
            yield return waitForButton; 
        }

        void RebuildLayout(RectTransform rect)
        {  
            var allRects = rect.gameObject.GetComponentsInChildren<RectTransform>(true);
            foreach (var r in allRects)
                LayoutRebuilder.ForceRebuildLayoutImmediate(r);
        }

        public void ShowArrow(RectTransform rectTransform, ArrowOrientation orientation, float padding)
        {
            arrow.ShowArrow(rectTransform, orientation, padding);
        }

        public void ShowFrames(RectTransform rectTransform)
        {
            ShowFrames(rectTransform, defaultDarkPadding);
        }
   
        public void HideContentImmediate()
        {
            hideContentPanelCanvas.alpha = 1;
            hideContentPanel.SetActive(true);
        }

        public IEnumerator HideContentAlpha(float from, float to)
        {
            hideContentPanelCanvas.alpha = from;  
            hideContentPanel.SetActive(true);
            yield return Fade(to, hideContentPanelCanvas);
        }

        public void ShowFrames(RectTransform rectTransform, float padding)
        {
            Hide();
            blockFrame.ShowBlackout(rectTransform, 0);
            darkFrame.ShowBlackout(rectTransform, padding);
        }

        public void ShowFramesAndArrow(RectTransform rectTransform, ArrowOrientation orientation)
        {
            ShowFramesAndArrow(rectTransform, orientation, defaultDarkPadding);
        }

        public void ShowFramesAndArrow(RectTransform rectTransform, ArrowOrientation orientation, float padding)
        {
            Hide();
            blockFrame.ShowBlackout(rectTransform, 0);
            darkFrame.ShowBlackout(rectTransform, padding);
            arrow.ShowArrow(rectTransform, orientation, padding);
        }

        public void ShowTextAndArrowFromDown(string msg, RectTransform rectTransform)
        {
            ShowFramesAndArrowAndSetCharacterPosition(rectTransform, ArrowOrientation.FromDown, 1);
            tutorialCharacter.ShowText(msg);
            RebuildLayout(tutorialCharacter.Root);
        }
        
        public void ShowTextAndArrowFromTop(int imageIndex, string msg, RectTransform rectTransform)
        {
            ShowFramesAndArrowAndSetCharacterPosition(rectTransform, ArrowOrientation.FromTop, 0);
            tutorialCharacter.ShowCharacterAndText(imageIndex, msg);
            RebuildLayout(tutorialCharacter.Root);
        }

        void ShowFramesAndArrowAndSetCharacterPosition(RectTransform rectTransform, ArrowOrientation orientation, float yPivot)
        {
            Hide();
            var textTransform = tutorialCharacter.Root;
            textTransform.pivot = new Vector2(0.5f, yPivot);
 
            ShowFramesAndArrow(rectTransform, orientation);
            Vector3 pos = textTransform.position;
            pos.y                  = arrow.TextPosition.y;
            textTransform.position = pos; 
        }

        public void ShowText(int imageId, string msg)
        {
            Hide();

            RectTransform root = tutorialCharacter.Root;
            
            tutorialCharacter.ShowCharacterAndText(imageId,msg );
            root.pivot    = new Vector2(0.5f, 0.5f);
            root.position = defaultTextPos.position;
            RebuildLayout(root);
        }

        public void BlockAll()
        {
            blockAllPanel.SetActive(true);
        }

        public void Hide()
        {
            hideContentPanel.SetActive(false);
            imageButton.gameObject.SetActive(false); 
            tutorialCharacter.Hide(); 
            blockFrame.Hide();
            darkFrame.Hide();
            arrow.Hide();
            blockAllPanel.SetActive(false);
            anyClickButton.gameObject.SetActive(false);
            
            hideContentPanelCanvas.alpha = 1;
            elementsCanvasGroup.alpha            = 1;
        }

        public IEnumerator SetAlpha(float from, float to)
        {
            elementsCanvasGroup.alpha = from;  
            yield return Fade(to, elementsCanvasGroup);
        }
         
        IEnumerator Fade(float to, CanvasGroup canvasGroup)
        {
            while (!Mathf.Approximately(canvasGroup.alpha, to))
            {
                canvasGroup.alpha = Mathf.MoveTowards(canvasGroup.alpha, to, Time.deltaTime * fadeSpeed);
                yield return null;
            }

            canvasGroup.alpha = to;
        }
    }
}