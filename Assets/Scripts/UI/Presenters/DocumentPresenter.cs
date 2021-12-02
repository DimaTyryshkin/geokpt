using System;
using System.Linq;
using Geo.KptData;
using Geo.OsIntegration;
using Geo.UI;
using SiberianWellness.NotNullValidation;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Events;

namespace Geo
{
	public class DocumentPresenter : MonoBehaviour
	{
		[SerializeField, IsntNull]
		GameObject root;
		
		[SerializeField, IsntNull]
		DocumentPopup documentPopup;
		
		[SerializeField, IsntNull]
		SaveContourPopup saveParcelDataPopup;
  
		[SerializeField, IsntNull]
		SelectContourPopup selectContourPopup;

		[SerializeField, IsntNull]
		OverlayPanel overlayPanel;

		string filePath;
		IParcel selectedParcel;
		NativeFileBrowser fileBrowser;
		AppAnalytics appAnalytics;
		
		public bool CanWakeup { get; private set; }

		public event UnityAction cancel;
		public event UnityAction<string> successLoad;
		public event UnityAction wakeUp;
		
		public event UnityAction successExport;
		public event UnityAction<Exception,string> exceptionOnLoading;

		public void ClearSuccessLoadEvent()
		{
			successLoad = null;
		}

		public void Init(NativeFileBrowser fileBrowser, AppAnalytics appAnalytics)
		{
			Assert.IsNotNull(fileBrowser);
			Assert.IsNotNull(appAnalytics);
			this.fileBrowser = fileBrowser;
			this.appAnalytics = appAnalytics;

			documentPopup.cancel += () =>
			{
				Exit();
			};

			documentPopup.exceptionOnLoading += (e) => 
			{ 
				exceptionOnLoading?.Invoke(e, filePath);
			};

			documentPopup.successLoad += (f) =>
			{
				CanWakeup = true;
				successLoad?.Invoke(f);
			};
			
			documentPopup.selectParcel += (p) =>
			{
				appAnalytics.SelectParcel(documentPopup.CurrentFilter);
				documentPopup.Close();
				
				Debug.Log(p.GetCadastralNumber());
				selectedParcel = p;

				if (p.GetContours().Count == 1)
				{
					ShowSaveParcelDataPopup(selectedParcel, selectedParcel.GetContours()[0]);
				}
				else
				{
					selectContourPopup.Show(p);
				}
			};
			
			selectContourPopup.selectContour += (contour) =>
			{ 
				selectContourPopup.Close();
				ShowSaveParcelDataPopup(selectedParcel, contour);
			};

			selectContourPopup.cancel += () =>
			{
				selectContourPopup.Close();
				documentPopup.Show();
			};

			saveParcelDataPopup.cancel += () => 
			{
				saveParcelDataPopup.Close();
				documentPopup.Show();
			};

			saveParcelDataPopup.saved += TrySave;
		}

		public void Show(string filePath)
		{
			selectContourPopup.Close();
			saveParcelDataPopup.Close();
			
			this.filePath = filePath; 
			selectedParcel  = null;
			root.SetActive(true);
			
			documentPopup.Show(filePath);
		}

		void TrySave(string filePath)
		{
			appAnalytics.TryExportContour();
#if UNITY_EDITOR
			SaveFile(filePath);
#else
			SaveFile_CatchExceptionDecorator(filePath);
#endif
		}

		void SaveFile_CatchExceptionDecorator(string filePath)
		{
			try
			{
				SaveFile(filePath);
			}
			catch (Exception e)
			{
				
				Debug.LogException(e);
				overlayPanel.Show("Ошибка!");
			}
		}
		
		void SaveFile(string filePath)
		{ 
			fileBrowser.Export(filePath, (success) =>
			{
				if (success)
				{
					appAnalytics.SuccessExportContour();
					overlayPanel.Show("Сохранен");
					successExport?.Invoke();
				}
				else
				{
					overlayPanel.Show("Не сохранено!");
				}
			}); 
		}

		/// <summary>
		/// Заново показать презентер в том состоянии в котормо он был в момент прятания
		/// </summary>
		public void Wakeup()
		{
			//TODO Вот тут надо тоже запускать туторила 
			Assert.IsTrue(CanWakeup);
			root.SetActive(true);

			if (saveParcelDataPopup.IsVisible)
				saveParcelDataPopup.ShowAndRedraw(); //перерисовываем окно, так как возможно изменились настройки формата вывода
			
			wakeUp?.Invoke();
		}
		
		/// <summary>
		/// Спрятать презентер(все что он показывает)
		/// </summary>
		public void Sleep()
		{
			root.SetActive(false);
		}

		void Exit()
		{
			cancel?.Invoke();
		}
 
		void ShowSaveParcelDataPopup(IParcel p, IContour c)
		{ 
			saveParcelDataPopup.Show(p, c, Application.temporaryCachePath);
		}
	}
}