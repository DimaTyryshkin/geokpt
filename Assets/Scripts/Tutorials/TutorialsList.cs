﻿using System.Collections;
using Game.ScenarioSystem.GuiHighlight;
using Geo.UI;
using SiberianWellness.NotNullValidation;
using UnityEngine;

namespace Geo.Tutorials
{
	public class TutorialsList : MonoBehaviour
	{
		[SerializeField, IsntNull]
		TutorialFacade facade;
		 
		[SerializeField, IsntNull]
		TutorialGui tutorialGui;
		
		[SerializeField, IsntNull]
		RectTransform settingsButtonRect;

		public IEnumerator HelloTutorial()
		{ 
			facade.EnableInput = false;
			tutorialGui.ShowText("Привет, меня зовут Барт. \n<color=#FFC6C0>(Нажми на экран, чтобы продолжить беседу с Бартом)</color>");
			tutorialGui.HideContentImmediate();
			yield return tutorialGui.SetAlpha(0, 1);
			yield return tutorialGui.WaitForClick();

			tutorialGui.ShowText("Покажу тебе, что умеет приложение 'GeoKPT'.");
			tutorialGui.HideContentImmediate();
			yield return tutorialGui.WaitForClick();
			
			tutorialGui.ShowText("Сверху ты видишь список скачанных кадастровых планов территорий (КПТ).");
			StartCoroutine(tutorialGui.HideContentAlpha(1, 0));
			
			yield return tutorialGui.WaitForClick();

			tutorialGui.ShowText("Я добавил эти КПТ для примера, чтобы ты мог исследовать приложение.");
			yield return tutorialGui.WaitForClick();
			
			tutorialGui.ShowText("Ты можешь сам скачать другие файлы КПТ на телефон и в любой момент получить координаты нужного участка.");
			yield return tutorialGui.WaitForClick();
			
			yield return tutorialGui.SetAlpha(1, 0);
			facade.EnableInput = true;
			tutorialGui.Hide();
		}

		public IEnumerator DocumentPopupTutorial(DocumentPopup documentPopup)
		{
			facade.EnableInput = false;

			documentPopup.DeselectInput();
			//yield return new WaitForSeconds(5);

			tutorialGui.ShowText("Это список участков.");
			yield return tutorialGui.SetAlpha(0, 1);
			documentPopup.DeselectInput();
			yield return tutorialGui.WaitForClick();

			tutorialGui.ShowText("Сверху есть удобный поисковик.\n<color=#FFC6C0>(Тапни по экрану, чтобы продолжить беседу с Бартом)</color>", documentPopup.InputFieldRect, ArrowOrientation.FromDown);
			yield return tutorialGui.SetAlpha(0, 1);
			yield return tutorialGui.WaitForClick();

			tutorialGui.ShowText("Можно искать участки по части кадастрового номера или адреса.", documentPopup.InputFieldRect, ArrowOrientation.FromDown);
			yield return tutorialGui.WaitForClick();

			tutorialGui.ShowText("Выбери любой участок, чтобы увидеть его координаты.");
			yield return tutorialGui.SetAlpha(0, 1);
			yield return tutorialGui.WaitForClick();

			yield return tutorialGui.SetAlpha(1, 0);
			facade.EnableInput = true;
			tutorialGui.Hide();
		}

		public IEnumerator SettingsPopupTutorial()
		{
			facade.EnableInput = false;
			tutorialGui.ShowText("Настрой формат txt файла под свой GPS приемник.");
			yield return tutorialGui.SetAlpha(0, 1);
			yield return tutorialGui.WaitForClick();

			yield return tutorialGui.SetAlpha(1, 0);
			facade.EnableInput = true;
			tutorialGui.Hide();
		}
		
		public IEnumerator UserFormatTutorial()
		{
			facade.EnableInput = false;
			tutorialGui.ShowText("Создай формат координат для своего GPS приемника.");
			yield return tutorialGui.SetAlpha(0, 1);
			yield return tutorialGui.WaitForClick();
			
			tutorialGui.ShowText("Чтобы создать формат координат, введи строку формата в поле. Иcпользуй переменные.");
			yield return tutorialGui.WaitForClick();
			
			tutorialGui.ShowText("Вводи имена переменных вместе с фигурными скобками.");
			yield return tutorialGui.WaitForClick();

			yield return tutorialGui.SetAlpha(1, 0);
			facade.EnableInput = true;
			tutorialGui.Hide();
		}

		public IEnumerator SaveContourPopupTutorial(SaveContourPopup saveContourPopup)
		{
			facade.EnableInput = false;
			tutorialGui.ShowText("Видно координаты участка и другую информацию.");
			yield return tutorialGui.SetAlpha(0, 1); 
			yield return tutorialGui.WaitForClick();
			
			//Показываем кнопку сохранить
			RectTransform buttonRect = saveContourPopup.SaveToFileButton.GetComponent<RectTransform>();
			tutorialGui.ShowText("Можешь сохраним координаты в txt файл.", buttonRect , ArrowOrientation.FromTop);
			yield return tutorialGui.SetAlpha(0, 1);
			yield return tutorialGui.WaitForClick();
			
			//---Показываем настройки
			tutorialGui.ShowText("Возможно, ты хочешь изменить формат координат.");
			yield return tutorialGui.SetAlpha(0, 1);
			yield return tutorialGui.WaitForClick();
			  
			tutorialGui.ShowText("Формат изменяется в настройках", settingsButtonRect, ArrowOrientation.FromTop);
			yield return tutorialGui.SetAlpha(0, 1);
			yield return tutorialGui.WaitForClick();
			 
			yield return tutorialGui.SetAlpha(1, 0);
			facade.EnableInput = true;
			tutorialGui.Hide();
		}

		public IEnumerator SuccessExportTutorial()
		{
			facade.EnableInput = false;
			yield return new WaitForSeconds(0.5f);
			tutorialGui.ShowText("Теперь можешь открыть файл в приложении, которым ты управляешь приёмником.\nУдачи.");
			yield return tutorialGui.SetAlpha(0, 1); 
			yield return tutorialGui.WaitForClick();
			 
			yield return tutorialGui.SetAlpha(1, 0);
			facade.EnableInput = true;
			tutorialGui.Hide();
		}
	}
}
