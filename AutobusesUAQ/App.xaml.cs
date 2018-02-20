using System;
using AutobusesUAQ.Views;
using Xamarin.Forms;

namespace AutobusesUAQ
{
    public partial class App : Application
    {
        public static bool UseMockDataStore = true;
        public static string BackendUrl = "https://localhost:5000";

        public App()
        {
            InitializeComponent();
            //MainPage = new MenuPrincipal();//Se reemplaza por las lineas siguientes porque el menu se duplicaba
            /*if (UseMockDataStore)
                DependencyService.Register<MockDataStore>();
            else
                DependencyService.Register<CloudDataStore>();

            if (Device.RuntimePlatform == Device.iOS)
                MainPage = new MapPage();
            else
                MainPage = new NavigationPage(new MapPage());*/
            MainPage = new NavigationPage(new DetalleCamion());//Se reemplaza por las lineas siguientes porque el menu se duplicaba
        }
    }
}
