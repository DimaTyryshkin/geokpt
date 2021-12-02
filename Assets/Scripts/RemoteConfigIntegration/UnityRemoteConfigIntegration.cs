using System; 
using UnityEngine;
using UnityEngine.Events;

namespace Game.UnityServices.RemoteConfig
{
    public class UnityRemoteConfigIntegration  
    {
        struct userAttributes{}
        struct appAttributes{}
        
        string log = "";

        public Version MinAndroidVersion { get; private set; }
        public Version MaxAndroidVersion { get; private set; }
        public string GooglePlayMarketUrl { get; private set; }
        public string GooglePlayUrl { get; private set; }
        public string NewVersionInfo { get; private set; }

        public event UnityAction fetchComplete;

        public UnityRemoteConfigIntegration()
        {
            MinAndroidVersion = new Version(0, 0, 0);
            MaxAndroidVersion = new Version(Application.version);
            GooglePlayMarketUrl = "market://details?id=com.DefaultCompany.Geo";
            GooglePlayUrl = "https://play.google.com/store/apps/details?id=com.DefaultCompany.Geo";
            NewVersionInfo = "Новые возможности уже рядом";
        }

        public void DisplayAllKeys()
        {
            string allKeys = "Current Keys:" + Environment.NewLine;
            var keys = RemoteSettings.GetKeys();
            foreach (string key in keys)
            {
                string value = RemoteSettings.GetString(key);
                allKeys += $"    {key}={value}" + Environment.NewLine;
            }

            Debug.Log(allKeys);
        }

        public void FetchDataAsync()
        {
            //-1 Services available
            RemoteSettings.Completed += OnProcessResponse;
            RemoteSettings.ForceUpdate();
            
            //-2 Services no available
            //OnProcessResponse(false, false, 0);
        }

        void OnProcessResponse(bool wasUpdatedFromServer, bool settingsChanged, int serverResponse)
        {
            if (serverResponse == 200)
            {
                DisplayAllKeys();

                MinAndroidVersion   = new Version(GetString("min_android_version"));
                MaxAndroidVersion   = new Version(GetString("max_android_version"));
                GooglePlayMarketUrl = GetString("google_play_market_url");
                GooglePlayUrl = GetString("google_play_url");
                NewVersionInfo = GetString("new_version_info");
            }

            fetchComplete?.Invoke();
        }

        string GetString(string key)
        {
            string value = RemoteSettings.GetString(key);
            return value;
        }
    }
}