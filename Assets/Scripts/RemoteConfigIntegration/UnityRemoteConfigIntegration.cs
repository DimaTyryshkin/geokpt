using System;
using Unity.RemoteConfig;
using UnityEngine;
using UnityEngine.Events;

namespace Geo
{
    public class UnityRemoteConfigIntegration  
    {
        public struct UserAttributes 
        {
        }

        public struct AppAttributes 
        { 
            public string platform;
            public string appVersion;

            public AppAttributes(string platform, string appVersion)
            {
                this.platform   = platform;
                this.appVersion = appVersion;
            }
        }
        
        string log = "";

        public Version   MinAndroidVersion   { get; private set; }
        public Version   MaxAndroidVersion   { get; private set; }
        public string    GooglePlayMarketUrl { get; private set; }
        public string    GooglePlayUrl       { get; private set; }
        public string    NewVersionInfo      { get; private set; }
        public GeoConfig Config              { get; private set; }

        public event UnityAction fetchComplete;

        public UnityRemoteConfigIntegration()
        {
            MinAndroidVersion   = new Version(0, 0, 0);
            MaxAndroidVersion   = new Version(Application.version);
            GooglePlayMarketUrl = "market://details?id=com.DefaultCompany.Geo";
            GooglePlayUrl       = "https://play.google.com/store/apps/details?id=com.DefaultCompany.Geo";
            NewVersionInfo      = "Новые возможности уже рядом";
            Config              = new GeoConfig();
        }

        public void DisplayAllKeys()
        {
            var keys = ConfigManager.appConfig.GetKeys();
            string allKeys = $"Current Keys (cunt={keys.Length}):" + Environment.NewLine;
            foreach (string key in keys)
            { 
                string value = ConfigManager.appConfig.GetString(key);
                allKeys += $"    {key}={value}" + Environment.NewLine;
            }

            Debug.Log(allKeys);
        }

        public void FetchDataAsync()
        {
            ConfigManager.FetchCompleted += OnProcessResponse;

            AppAttributes appAttributes;
            #if PLATFORM_ANDROID
            appAttributes = new AppAttributes("android", Application.version);
            #endif
            
            ConfigManager.FetchConfigs<UserAttributes, AppAttributes>(new UserAttributes(), appAttributes);
        }

        void OnProcessResponse(ConfigResponse configResponse)
        {
            try
            {
                if (configResponse.status == ConfigRequestStatus.Success)
                {
                    DisplayAllKeys();

                    switch (configResponse.requestOrigin)
                    {
                        case ConfigOrigin.Default:
                            break;
                        case ConfigOrigin.Cached:
                        case ConfigOrigin.Remote:
                            Debug.Log("assignmentId="+ConfigManager.appConfig.assignmentId);
                            Debug.Log("environmentId="+ConfigManager.appConfig.environmentId);
                            
                            MinAndroidVersion   = new Version(ConfigManager.appConfig.GetString("min_android_version"));
                            MaxAndroidVersion   = new Version(ConfigManager.appConfig.GetString("max_android_version"));
                            GooglePlayMarketUrl = ConfigManager.appConfig.GetString("google_play_market_url");
                            GooglePlayUrl       = ConfigManager.appConfig.GetString("google_play_url");
                            NewVersionInfo      = ConfigManager.appConfig.GetString("new_version_info");

                            string json = ConfigManager.appConfig.GetJson("geo_config");
                            Debug.Log(json);
                            Config = JsonUtility.FromJson<GeoConfig>(json);
                            if (Config == null)
                                Config = new GeoConfig();

                            break;
                    }
                }
            }
            catch (Exception e)
            {
                Debug.LogError(e);
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