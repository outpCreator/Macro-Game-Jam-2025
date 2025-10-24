using UnityEngine;
using System;


namespace Utils.IDisposableUtils
{
    public class DisposableShowLoadingScreen : IDisposable
    {
        private readonly LoadingScreen _loadingScreen;

        public DisposableShowLoadingScreen(LoadingScreen loadingScreen)
        {
            _loadingScreen = loadingScreen;
            _loadingScreen.Show();
        }

        public void SetLoadingBarPercent(float percent)
        {
            _loadingScreen.SetLoadingBarPercent(percent);
        }

        public void Dispose()
        {
            _loadingScreen.Hide();
        }
    }
}
