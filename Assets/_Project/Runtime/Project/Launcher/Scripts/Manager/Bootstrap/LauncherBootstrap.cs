using System.Collections;
using _Project.Runtime.Core.Bundle.Scripts;
using _Project.Runtime.Core.UI.Scripts.Manager;
using UnityEngine;
using UnityEngine.Networking;

namespace _Project.Runtime.Project.Launcher.Scripts.Manager.Bootstrap
{
    public static class ScreenKeys
    {
        public const string MenuScreen = "MenuScreen";
        public const string GameScreen = "InventoryScreen";
        public const string EndScreen = "EndScreen";

    }
    
    public static class ScreenLayerKeys
    {
        public const string Background = "BackgroundLayer";
        public const string FirstLayer = "FirstLayer";
        public const string SecondLayer = "SecondLayer";
        public const string Notifications = "Notifications";

    }
    
    public class LauncherBootstrap : MonoBehaviour
    {
        private async void Awake()
        {

            BundleModel.Instance = new BundleModel();
            var screenManager = ScreenManager.Instance;

            var menuScreen = await screenManager.OpenScreen(ScreenKeys.MenuScreen, ScreenLayerKeys.FirstLayer);
        }






    }
}
