﻿namespace stage3
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            MainPage = new AppShell();
        }

        protected override async void OnStart()
        {
            await Shell.Current.GoToAsync("//AuthPage");

            base.OnStart();
        }
    }
}
