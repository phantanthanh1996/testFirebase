using System;

using Android.App;
using Android.OS;
using Android.Runtime;
using Plugin.CurrentActivity;
using Plugin.FirebasePushNotification;
using Android.Widget;
using Firebase;
using Plugin.FirebasePushNotification.Abstractions;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Plugin.Badge;

namespace Firebase.Droid
{
    [Application]
    class MainApplication : Application, Application.IActivityLifecycleCallbacks

    {

        public MainApplication(IntPtr handle, JniHandleOwnership transer) : base(handle, transer)
        {
        }

        public override void OnCreate()
        {
            base.OnCreate();
            RegisterActivityLifecycleCallbacks(this);
            var count = 0;
           // CrossBadge.Current.SetBadge(count);
            //If debug you should reset the token each time.
#if DEBUG
            FirebasePushNotificationManager.Initialize(this, new NotificationUserCategory[]
            {
                new NotificationUserCategory("message", new List<NotificationUserAction>
                {
                    new NotificationUserAction("Reply", "Reply", NotificationActionType.Foreground),
                    new NotificationUserAction("Forward", "Forward", NotificationActionType.Foreground)

                }),
                new NotificationUserCategory("request", new List<NotificationUserAction>
                {
                    new NotificationUserAction("Accept", "Accept", NotificationActionType.Default, "check"),
                    new NotificationUserAction("Reject", "Reject", NotificationActionType.Default, "cancel")
                })

            }, true);
#else
            FirebasePushNotificationManager.Initialize(this, new NotificationUserCategory[]
            {
            new NotificationUserCategory("message",new List<NotificationUserAction> {
                new NotificationUserAction("Reply","Reply",NotificationActionType.Foreground),
                new NotificationUserAction("Forward","Forward",NotificationActionType.Foreground)

            }),
            new NotificationUserCategory("request",new List<NotificationUserAction> {
                new NotificationUserAction("Accept","Accept",NotificationActionType.Default,"check"),
                new NotificationUserAction("Reject","Reject",NotificationActionType.Default,"cancel")
            })

        }, false);
#endif

            //Handle notification when app is closed here
            CrossFirebasePushNotification.Current.OnNotificationReceived += (s, p) =>
            {
                System.Diagnostics.Debug.WriteLine("NOTIFICATION RECEIVED", p.Data);
                count++;
                CrossBadge.Current.SetBadge(count);

            };
            CrossFirebasePushNotification.Current.OnNotificationOpened += (s, p) =>
            {
                count--;
                CrossBadge.Current.SetBadge(count);
            };
        }

        public override void OnTerminate()
        {
            base.OnTerminate();
            UnregisterActivityLifecycleCallbacks(this);
        }

        public void OnActivityCreated(Activity activity, Bundle savedInstanceState)
        {
            CrossCurrentActivity.Current.Activity = activity;
            CrossBadge.Current.ClearBadge();
        }

        public void OnActivityDestroyed(Activity activity)
        {
        }

        public void OnActivityPaused(Activity activity)
        {
        }

        public void OnActivityResumed(Activity activity)
        {
            CrossCurrentActivity.Current.Activity = activity;
        }

        public void OnActivitySaveInstanceState(Activity activity, Bundle outState)
        {
        }

        public void OnActivityStarted(Activity activity)
        {
            CrossCurrentActivity.Current.Activity = activity;
        }

        public void OnActivityStopped(Activity activity)
        {
        }
    }
}