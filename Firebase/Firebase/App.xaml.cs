﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Plugin.Badge;
using Plugin.FirebasePushNotification;
using Xamarin.Forms;

namespace Firebase
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            MainPage = new Firebase.MainPage();
        }



        protected override void OnStart()
        {

            // Handle when your app starts
            CrossFirebasePushNotification.Current.Subscribe("general");
            CrossFirebasePushNotification.Current.OnTokenRefresh += (s, p) =>
            {
                System.Diagnostics.Debug.WriteLine($"TOKEN REC: {p.Token}");
                CrossBadge.Current.ClearBadge();
            };
            System.Diagnostics.Debug.WriteLine($"TOKEN: {CrossFirebasePushNotification.Current.Token}");
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            //CrossBadge.Current.ClearBadge();
            // Handle when your app resumes
        }
    }
}
