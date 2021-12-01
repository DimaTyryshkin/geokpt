using System.Collections;
using System.Collections.Generic;
using Game.ScenarioSystem.GuiHighlight;
using Geo.Data;
using Geo.UI;
using SiberianWellness.NotNullValidation;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Events;

namespace Geo.Tutorials
{
	public class TutorialStarter : MonoBehaviour
	{
		[SerializeField, IsntNull]
		PopupsRoot popupsRoot;

		[SerializeField, IsntNull]
		MainPresenter mainPresenter;

		[SerializeField, IsntNull]
		DocumentPopup documentPopup;

		[SerializeField, IsntNull]
		TutorialGui tutorialGui;

		[SerializeField, IsntNull]
		TutorialFacade tutorialFacade;
		
		[SerializeField, IsntNull]
		TutorialsList tutorialsList;
		
		AppAnalytics       appAnalytics;
		AccountDataStorage storage;

		public void Init(AccountDataStorage storage, AppAnalytics appAnalytics)
		{
			Assert.IsNotNull(storage);
			Assert.IsNotNull(appAnalytics);
			this.storage      = storage;
			this.appAnalytics = appAnalytics;
			
			mainPresenter.ShowStartScreen += OnShowStartScreen;
			documentPopup.successLoad     += OnDocumentPopupSuccessLoad;
			popupsRoot.Opened             += OnPopupOpened;
		}

		void OnPopupOpened(Popup p)
		{
			StopAllCoroutines();
			tutorialGui.Hide();
			tutorialFacade.SetEnableInput(true);

			if (p is SaveContourPopup saveContourPopup)
			{
				if (saveContourPopup.PointCount > 0)
					TryCompleteTutorial(tutorialsList.SaveContourPopupTutorial(saveContourPopup), "SaveContourPopupTutorial");
			}

			if (p is SettingsPopup)
			{
				TryCompleteTutorial(tutorialsList.SettingsPopupTutorial(), "SettingsPopupTutorial");
			}
		}
 
		void OnDocumentPopupSuccessLoad(string _)
		{
			if (documentPopup.ParcelsCount > 0)
				TryCompleteTutorial(tutorialsList.DocumentPopupTutorial(documentPopup), "DocumentPopupTutorial");
		}

		void OnShowStartScreen()
		{
			TryCompleteTutorial(tutorialsList.HelloTutorial(), "HelloTutorial");
		}

		void TryCompleteTutorial(IEnumerator tutorial, string tutorialId)
		{
			List<string> completedTutorialsId = storage.GetInst().completedTutorialsId;
			if (!completedTutorialsId.Contains(tutorialId))
				StartCoroutine(RunWithCallBack(tutorial, () => completedTutorialsId.Add(tutorialId)));
		}

		IEnumerator RunWithCallBack(IEnumerator tutorial, UnityAction callBack)
		{
			yield return tutorial;
			tutorialGui.Hide();
			callBack.Invoke();
		}
	}
}