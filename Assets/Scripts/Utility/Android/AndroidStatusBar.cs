using System;
using UnityEngine;

namespace Utility.Android
{
    public static class AndroidStatusBar
    {
        private const int MinStatusBarColorAPI = 21;

        private const int SystemUIFlagLayoutFullscreen = 0x00000400;

        private static AndroidJavaObject _activity;

        private static AndroidJavaObject Activity
        {
            get
            {
                if (_activity == null)
                {
                    AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
                    _activity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
                }
                
                return _activity;
            }
        }


        public static void EnablePermanentStatusBar(Color color)
        {
            int androidColor = ConvertColorToAndroidColor(color);
            
            RunOnUiThread(() =>
                {
                    using (AndroidJavaObject window = Window)
                    {
                        window.Call("clearFlags", SystemUIFlagLayoutFullscreen);
                        
                        if (GetApi() >= MinStatusBarColorAPI)
                        {
                            window.Call("setStatusBarColor", androidColor);
                        }
                    }
                });
        }

        private static void RunOnUiThread(Action action) => 
            Activity.Call("runOnUiThread", new AndroidJavaRunnable(action));

        private static AndroidJavaObject Window => 
            Activity.Call<AndroidJavaObject>("getWindow");

        private static int GetApi()
        {
            using (AndroidJavaClass version = new AndroidJavaClass("android.os.Build$VERSION"))
            {
                return version.GetStatic<int>("SDK_INT");
            }
        }

        private static int ConvertColorToAndroidColor(Color color)
        {
            Color32 color32 = color;
            int alpha = color32.a;
            int red = color32.r;
            int green = color32.g;
            int blue = color32.b;
            
            using (AndroidJavaClass colorClass = new AndroidJavaClass("android.graphics.Color"))
            {
                int androidColor = colorClass.CallStatic<int>("argb", alpha, red, green, blue);
                return androidColor;
            }
        }
    }
}