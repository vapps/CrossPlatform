using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CrossPlatform.Infrastructure
{
    public abstract class W8Utility
    {
        /// <summary>
        /// Instance
        /// </summary>
        public static W8Utility Instance { get; set; }

        /// <summary>
        /// Windows 8 SettingPane.Show
        /// </summary>
        public abstract void SettingsPaneShow();

        /// <summary>
        /// ApplicationView.Value, Snapped check
        /// </summary>
        /// <returns></returns>
        public abstract string ApplicationViewValue();

        /// <summary>
        /// SearchPane ShowOnKeyboardInput
        /// </summary>
        /// <param name="enable"></param>
        public abstract void SearchPaneShowOnKeyboardInput(bool enable);

        /// <summary>
        /// Register BackgroundTask
        /// </summary>
        public abstract void RegisterBackgroundTaskTimeTrigger(string taskName, string taskEntryPoint);

        /// <summary>
        /// Set DataRequest - Share Contract
        /// </summary>
        public abstract void SetDataRequest();

        /// <summary>
        /// Unset DataRequest
        /// </summary>
        public abstract void UnsetDataRequest();

        /// <summary>
        /// Show Share UI
        /// </summary>
        public abstract void ShowShareUI();
    }
}
