using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Background;
using Windows.Storage;

namespace CrossPlatform.Infrastructure.StoreApp.Commons
{
    class BackgroundTaskUtil
    {
        //public const string BACKGROUND_TASK_ENTRYPOINT = "KTour.BackgroundTask.TileUpdateTask";
        //public const string BACKGROUND_TASK_NAME = "TileUpdateTask";

        public static IList<KeyValuePair<string, bool>> BackgroundTaskStates = new List<KeyValuePair<string, bool>>();

        /// <summary>
        /// Register a background task with the specified taskEntryPoint, name, trigger,
        /// and condition (optional).
        /// </summary>
        /// <param name="taskEntryPoint">Task entry point for the background task.</param>
        /// <param name="name">A name for the background task.</param>
        /// <param name="trigger">The trigger for the background task.</param>
        /// <param name="condition">An optional conditional event that must be true for the task to fire.</param>
        public static BackgroundTaskRegistration RegisterBackgroundTask(String taskEntryPoint, String name, IBackgroundTrigger trigger, IBackgroundCondition condition)
        {
            var builder = new BackgroundTaskBuilder();

            builder.Name = name;
            builder.TaskEntryPoint = taskEntryPoint;
            builder.SetTrigger(trigger);

            if (condition != null)
            {
                builder.AddCondition(condition);
                //
                // If the condition changes while the background task is executing then it will
                // be canceled.
                //
                builder.CancelOnConditionLoss = true;
            }

            BackgroundTaskRegistration task = builder.Register();

            UpdateBackgroundTaskStatus(name, true);

            //
            // Remove previous completion status from local settings.
            //
            var settings = ApplicationData.Current.LocalSettings;
            settings.Values.Remove(name);

            return task;
        }

        /// <summary>
        /// Unregister background tasks with specified name.
        /// </summary>
        /// <param name="name">Name of the background task to unregister.</param>
        public static void UnregisterBackgroundTasks(string name)
        {
            //
            // Loop through all background tasks and unregister any with SampleBackgroundTaskName or
            // SampleBackgroundTaskWithConditionName.
            //
            foreach (var cur in BackgroundTaskRegistration.AllTasks)
            {
                if (cur.Value.Name == name)
                {
                    cur.Value.Unregister(true);
                }
            }

            UpdateBackgroundTaskStatus(name, false);
        }

        /// <summary>
        /// Store the registration status of a background task with a given name.
        /// </summary>
        /// <param name="name">Name of background task to store registration status for.</param>
        /// <param name="registered">TRUE if registered, FALSE if unregistered.</param>
        public static void UpdateBackgroundTaskStatus(string name, bool registered)
        {
            //등록상태 관리
            var existBackgroundTask = BackgroundTaskStates.FirstOrDefault(p => p.Key.Equals(name));
            if (string.IsNullOrEmpty(existBackgroundTask.Key) == false)
            {
                BackgroundTaskStates.Remove(existBackgroundTask);
            }
            BackgroundTaskStates.Add(new KeyValuePair<string, bool>(name, registered));

            //메시지 관리
            //var settings = ApplicationData.Current.LocalSettings;
            //var existSetting = settings.Values.FirstOrDefault(p => p.Key == BACKGROUND_TASK_NAME);
            //if (string.IsNullOrEmpty(existSetting.Key) == false)
            //{
            //    settings.Values.Remove(existSetting);
            //}
            //settings.Values.Add(new KeyValuePair<string, object>(BACKGROUND_TASK_NAME, "UpdateBackgroundTaskStatus"));
        }

        /// <summary>
        /// Get the registration / completion status of the background task with
        /// given name.
        /// </summary>
        /// <param name="name">Name of background task to retreive registration status.</param>
        public static string GetBackgroundTaskStatus(string name)
        {
            var registered = false;

            var existBackgroundTask = BackgroundTaskStates.FirstOrDefault(p => p.Key.Equals(name));
            if (string.IsNullOrEmpty(existBackgroundTask.Key) == false)
            {
                registered = existBackgroundTask.Value;
            }

            var status = registered ? "Registered" : "Unregistered";

            var settings = ApplicationData.Current.LocalSettings;
            if (settings.Values.ContainsKey(name))
            {
                status += " - " + settings.Values[name].ToString();
            }

            return status;
        }

        public static bool GetIsBackgroundTask(string name)
        {
            var returnValue = false;
            var existBackgroundTask = BackgroundTaskStates.FirstOrDefault(p => p.Key.Equals(name));
            if(string.IsNullOrEmpty(existBackgroundTask.Key) == false)
            {
                returnValue = existBackgroundTask.Value;
            }
            return returnValue;
        }
    }
}
