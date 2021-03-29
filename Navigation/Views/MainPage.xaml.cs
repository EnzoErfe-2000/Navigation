using System;
using System.ComponentModel;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Navigation.Views
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }
        async void GoToPage(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new ItemsPage());
        }
        async void GoToCalc(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new CalculatorPage());
        }
        async void GoToCustBtn(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new CustomButtonPage());
        }
    }
}