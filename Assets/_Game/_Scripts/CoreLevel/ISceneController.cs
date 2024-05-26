using System;

namespace LuisLabs.CoreLevel 
{
    public interface ISceneController
    {
        public Action OnLoadingStart { get; set; }
        public Action OnLoadingEnd { get; set; }
        public void AddScene(string scene);
        public void AddCurrentScene(string name);
        public void SwitchScene(string scene);
        public void SwitchSceneAsync(string scene);
    }
}
