﻿using Navigation.ViewModels;
using System.ComponentModel;
using Xamarin.Forms;

namespace Navigation.Views
{
    public partial class ItemDetailPage : ContentPage
    {
        public ItemDetailPage()
        {
            InitializeComponent();
            BindingContext = new ItemDetailViewModel();
        }
    }
}