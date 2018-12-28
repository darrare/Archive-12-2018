using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace SmashRandomPicker
{
    public class Character
    {
        public static Dictionary<View, Character> CharacterViews { get; } = new Dictionary<View, Character>();

        public static Dictionary<string, Character> Characters { get; } = new Dictionary<string, Character>();

        public static string MostRecentlySelectedCharacter { get; set; }

        public int Row { set; get; }

        public int Col { set; get; }

        public string Name { get; set; }

        public int Stars { get; set; }

        public View CharacterView { private set; get; }

        public View StarView { private set; get; }

        public Character(int row, int col, string name)
        {
            Row = row;
            Col = col;
            Name = name;
            if (Application.Current.Properties.ContainsKey(name))
                Stars = int.Parse(Application.Current.Properties[name].ToString());
            else
                Stars = 0;

            CharacterView = new ContentView
            {
                Padding = new Thickness(0),
                Margin = new Thickness(0),

                Content = new Image
                {
                    Source = name + ".png",
                    Margin = new Thickness(0),
                    Aspect = Aspect.Fill,
                }
            };

            StarView = new ContentView
            {
                Padding = new Thickness(3),
                Margin = new Thickness(3),

                Content = new Image
                {
                    Source = "a" + Stars + ".png",
                    Margin = new Thickness(0),
                    Aspect = Aspect.Fill,
                }
            };

            CharacterViews.Add(CharacterView, this);
            Characters.Add(name, this);
        }
    }
}
