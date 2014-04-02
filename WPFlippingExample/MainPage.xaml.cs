using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;

namespace TestWPBase
{
    public partial class MainPage : PhoneApplicationPage
    {
        // Constructor
        public MainPage()
        {
            InitializeComponent();

            List<Item> items = new List<Item>();
            items.Add(new Item() { Name = "First", ImgSource = "/Assets/ApplicationIcon.png" });
            items.Add(new Item() { Name = "Second", ImgSource = "/Assets/ApplicationIcon.png" });
            items.Add(new Item() { Name = "Third", ImgSource = "/Assets/ApplicationIcon.png" });

            FlipList.ItemsSource = items;
        }
    }

    public class Item
    {
        public string Name { get; set; }
        public string ImgSource { get; set; }
    }
}