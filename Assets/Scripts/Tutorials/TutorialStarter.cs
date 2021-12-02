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
		DocumentPresenter documentPresenter;

		[SerializeField, IsntNull]
		TutorialGui tutorialGui;

		[SerializeField, IsntNull]
		TutorialFacade tutorialFacade;
		
		[SerializeField, IsntNull]
		TutorialsList tutorialsList;
		
		AccountDataStorage storage;

		public void Init(AccountDataStorage storage)
		{
			Assert.IsNotNull(storage); 
			this.storage      = storage;

			popupsRoot.Opened               += OnPopupOpened;
			documentPopup.successLoad       += OnDocumentPopupSuccessLoad;
			documentPresenter.successExport += OnSuccessExport;
			documentPresenter.wakeUp        += StartDocumentPopupTutorial;
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
			StartDocumentPopupTutorial();
		}

		public void OnShowStartScreen()
		{
			TryCompleteTutorial(tutorialsList.HelloTutorial(), "HelloTutorial");
		}

		void StartDocumentPopupTutorial()
		{
			if (documentPopup.ParcelsCount > 0)
				TryCompleteTutorial(tutorialsList.DocumentPopupTutorial(documentPopup), "DocumentPopupTutorial");
		}
		
		void OnSuccessExport()
		{
			TryCompleteTutorial(tutorialsList.SuccessExportTutorial(), "SuccessExport");
		}
 
		void TryCompleteTutorial(IEnumerator tutorial, string tutorialId)
		{
			List<string> completedTutorialsId = storage.GetInst().completedTutorialsId;
			if (!completedTutorialsId.Contains(tutorialId))
				StartCoroutine(RunWithCallBack(tutorial, () => OnTutorialComplete(tutorialId)));
		}

		void OnTutorialComplete(string tutorialId)
		{
			List<string> completedTutorialsId = storage.GetInst().completedTutorialsId;
			completedTutorialsId.Add(tutorialId);
			storage.Save();
		}

		IEnumerator RunWithCallBack(IEnumerator tutorial, UnityAction callBack)
		{
			yield return tutorial;
			tutorialGui.Hide();
			callBack.Invoke();
		}
	}
}