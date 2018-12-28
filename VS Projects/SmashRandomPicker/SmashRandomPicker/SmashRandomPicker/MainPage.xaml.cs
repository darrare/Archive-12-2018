using System;
using System.Collections.ObjectModel;
using System.Windows.Input;
using Xamarin.Forms;
using System.Collections.Generic;
using System.Linq;
using Rg.Plugins.Popup.Pages;
using Rg.Plugins.Popup.Extensions;
using System.Threading;

namespace SmashRandomPicker
{
    public partial class MainPage : ContentPage
    {
        static readonly int rows = 24, columns = 3;
        Character[,] characters = new Character[rows, columns];
        string[,] characterNames = new string[,]
        {
            {"Mario", "DonkeyKong", "Link" },
            {"Samus", "DarkSamus", "Yoshi" },
            {"Kirby", "Fox", "Pikachu" },
            {"Luigi", "Ness", "CaptainFalcon" },
            {"Jigglypuff", "Peach", "Daisy" },
            {"Bowser", "IceClimbers", "Sheik" },
            {"Zelda", "DrMario", "Pichu" },
            {"Falco", "Marth", "Lucina" },
            {"YoungLink", "Ganondorf", "Mewtwo" },
            {"Roy", "Chrom", "MrGameAndWatch" },
            {"MetaKnight", "Pit", "DarkPit" },
            {"ZeroSuitSamus", "Wario", "Snake" },
            {"Ike", "PokemonTrainer", "DiddyKong" },
            {"Lucas", "Sonic", "KingDedede" },
            {"Olimar", "Lucario", "Rob" },
            {"ToonLink", "Wolf", "Villager" },
            {"MegaMan", "WiiFitTrainer", "RosalinaAndLuma" },
            {"LittleMac", "Greninja", "Palutena" },
            {"PacMan", "Robin", "Shulk" },
            {"BowserJr", "DuckHunt", "Ryu" },
            {"Ken", "Cloud", "Corrin" },
            {"Bayonetta", "Inkling", "Ridley" },
            {"Simon", "Richter", "KingKRool" },
            {"Isabelle", "Incineroar", "" },
        };

        double imageWidth, imageHeight;

        public MainPage()
        {
            InitializeComponent();
            for (int column = 0; column < columns; column++)
            {
                for (int row = 0; row < rows; row++)
                {
                    //We don't have a last slot, so don't create a character for it
                    if (column == columns - 1 && row == rows - 1)
                        return;

                    Character character = new Character(row, column, characterNames[row, column]);

                    TapGestureRecognizer tapGestureRecognizer = new TapGestureRecognizer();
                    tapGestureRecognizer.Tapped += delegate (object sender, EventArgs e)
                    {
                        OnCharacterClick(character.Name, sender);
                    };
                    character.StarView.GestureRecognizers.Add(tapGestureRecognizer);

                    characters[row, column] = character;
                    absoluteLayout.Children.Add(character.CharacterView);
                    absoluteLayout.Children.Add(character.StarView);
                }
            }
        }

        private void PickButtonClicked(object sender, EventArgs e)
        {
            List<string> pool = new List<string>();
            foreach (Character c in Character.Characters.Values)
            {
                for(int i = 0; i < c.Stars; i++)
                {
                    pool.Add(c.Name);
                }
            }

            if (pool.Count == 0)
                return;

            Character.MostRecentlySelectedCharacter = pool[new Random().Next(0, pool.Count)];
            CreateSelectionPage();
        }

        private async void CreateSelectionPage()
        {
            await Navigation.PushPopupAsync(new RandomSelectionPage());
        }

        void OnCharacterClick(string characterName, object sender)
        {
            Character clickedCharacter = Character.Characters[characterName];
            var image = (Image)((ContentView)sender).Content;
            clickedCharacter.Stars++;
            if (clickedCharacter.Stars == 4)
            {
                clickedCharacter.Stars = 0;
            }
            ((Image)((ContentView)sender).Content).Source = "a" + clickedCharacter.Stars.ToString() + ".png";
            Application.Current.Properties[clickedCharacter.Name] = clickedCharacter.Stars;
        }

        void OnContainerSizeChanged(object sender, EventArgs args)
        {
            View container = (View)sender;
            double width = container.Width;
            double height = container.Height;

            if (width <= 0 || height <= 0)
                return;

            // Orient StackLayout based on portrait/landscape mode.
            stackLayout.Orientation = (width < height) ? StackOrientation.Vertical :
                                                         StackOrientation.Horizontal;

            // Calculate tile size and position based on ContentView size.
            imageWidth = Math.Min(width, height) / columns;
            imageHeight = imageWidth * 139/240;
            absoluteLayout.WidthRequest = columns * imageWidth;
            absoluteLayout.HeightRequest = rows * imageHeight;

            foreach (View fileView in absoluteLayout.Children)
            {
                if (!Character.CharacterViews.ContainsKey(fileView))
                    continue;

                Character character = Character.CharacterViews[fileView];

                AbsoluteLayout.SetLayoutBounds(fileView, new Rectangle(character.Col * imageWidth,
                                                                        character.Row * imageHeight,
                                                                        imageWidth,
                                                                        imageHeight));

                AbsoluteLayout.SetLayoutBounds(character.StarView, new Rectangle(character.Col * imageWidth,
                                                        character.Row * imageHeight,
                                                        imageWidth,
                                                        imageHeight));
            }
        }
    }
}