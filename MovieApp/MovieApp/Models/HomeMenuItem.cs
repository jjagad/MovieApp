using System;
using System.Collections.Generic;
using System.Text;

namespace MovieApp.Models
{
    public enum MenuItemType
    {
        Popcorn,
        About
    }
    public class HomeMenuItem
    {
        public MenuItemType Id { get; set; }

        public string Title { get; set; }
    }
}
