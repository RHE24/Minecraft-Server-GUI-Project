﻿using System.Globalization;
using System.Threading;
using System.Windows;
using FirstFloor.ModernUI.Presentation;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Minecraft_GUI_Project.Content
{
    /// <summary>
    /// A simple view model for configuring theme, font and accent colors.
    /// </summary>
    public class SettingsAppearanceViewModel : NotifyPropertyChanged
    {
        private const string FontSmall = "small";
        private const string FontLarge = "large";

        // 20 accent colors from Windows Phone 8
        private readonly Color[] _accentColors =
        {
            Color.FromRgb(0xa4, 0xc4, 0x00),   // lime
            Color.FromRgb(0x60, 0xa9, 0x17),   // green
            Color.FromRgb(0x00, 0x8a, 0x00),   // emerald
            Color.FromRgb(0x00, 0xab, 0xa9),   // teal
            Color.FromRgb(0x1b, 0xa1, 0xe2),   // cyan
            Color.FromRgb(0x00, 0x50, 0xef),   // cobalt
            Color.FromRgb(0x6a, 0x00, 0xff),   // indigo
            Color.FromRgb(0xaa, 0x00, 0xff),   // violet
            Color.FromRgb(0xf4, 0x72, 0xd0),   // pink
            Color.FromRgb(0xd8, 0x00, 0x73),   // magenta
            Color.FromRgb(0xa2, 0x00, 0x25),   // crimson
            Color.FromRgb(0xe5, 0x14, 0x00),   // red
            Color.FromRgb(0xfa, 0x68, 0x00),   // orange
            Color.FromRgb(0xf0, 0xa3, 0x0a),   // amber
            Color.FromRgb(0xe3, 0xc8, 0x00),   // yellow
            Color.FromRgb(0x82, 0x5a, 0x2c),   // brown
            Color.FromRgb(0x6d, 0x87, 0x64),   // olive
            Color.FromRgb(0x64, 0x76, 0x87),   // steel
            Color.FromRgb(0x76, 0x60, 0x8a),   // mauve
            Color.FromRgb(0x87, 0x79, 0x4e),   // taupe
        };

        private Color _selectedAccentColor;
        private readonly LinkCollection _themes = new LinkCollection();
        private readonly List<dynamic> _language = new List<Object>();

        private dynamic _selectedLanguage;
        private Link _selectedTheme;
        private string _selectedFontSize;

        public SettingsAppearanceViewModel()
        {
            _language.Add(new { DisplayName = "Portugues Brasil", Name = "pt-BR"});
            _language.Add(new { DisplayName = "English", Name = "en-US" });

            foreach (var lang in _language.Where(lang => lang.Name.Equals(Properties.Settings.Default.Lang)))
            {
                _selectedLanguage = lang;
            }

            // add the default themes
            _themes.Add(new Link { DisplayName = "dark", Source = AppearanceManager.DarkThemeSource });
            _themes.Add(new Link { DisplayName = "light", Source = AppearanceManager.LightThemeSource });

            SelectedFontSize = AppearanceManager.Current.FontSize == FontSize.Large ? FontLarge : FontSmall;
            SyncThemeAndColor();

            AppearanceManager.Current.PropertyChanged += OnAppearanceManagerPropertyChanged;
        }

        private void SyncThemeAndColor()
        {
            // synchronizes the selected viewmodel theme with the actual theme used by the appearance manager.
            this.SelectedTheme = this._themes.FirstOrDefault(l => l.Source.Equals(AppearanceManager.Current.ThemeSource));

            // and make sure accent color is up-to-date
            this.SelectedAccentColor = AppearanceManager.Current.AccentColor;
        }

        private void OnAppearanceManagerPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "ThemeSource" || e.PropertyName == "AccentColor")
            {
                SyncThemeAndColor();
            }
        }
        public List<dynamic> Langague
        {
            get { return this._language; }
        }

        public LinkCollection Themes
        {
            get { return this._themes; }
        }

        public string[] FontSizes
        {
            get { return new string[] { FontSmall, FontLarge }; }
        }

        public Color[] AccentColors
        {
            get { return this._accentColors; }
        }
        public dynamic SelectedLanguage
        {
            get
            {
                return this._selectedLanguage;
            }
            set
            {
                if (this._selectedLanguage == value) return;

                this._selectedLanguage = value;
                OnPropertyChanged("SelectedLanguage");

                Thread.CurrentThread.CurrentCulture = new CultureInfo(value.Name);
                Thread.CurrentThread.CurrentUICulture = new CultureInfo(value.Name);

                var old = Application.Current.MainWindow;

                Application.Current.MainWindow = new MainWindow();
                Application.Current.MainWindow.Show();

                old.Close();

                Properties.Settings.Default.Lang = value.Name;
                Properties.Settings.Default.Save();
            }
        }

        public Link SelectedTheme
        {
            get { return this._selectedTheme; }
            set
            {
                if (this._selectedTheme != value)
                {
                    this._selectedTheme = value;
                    OnPropertyChanged("SelectedTheme");

                    // and update the actual theme
                    AppearanceManager.Current.ThemeSource = value.Source;
                }
            }
        }

        public string SelectedFontSize
        {
            get { return this._selectedFontSize; }
            set
            {
                if (this._selectedFontSize != value)
                {
                    this._selectedFontSize = value;
                    OnPropertyChanged("SelectedFontSize");

                    AppearanceManager.Current.FontSize = value == FontLarge ? FontSize.Large : FontSize.Small;
                }
            }
        }

        public Color SelectedAccentColor
        {
            get { return this._selectedAccentColor; }
            set
            {
                if (this._selectedAccentColor != value)
                {
                    this._selectedAccentColor = value;
                    OnPropertyChanged("SelectedAccentColor");

                    AppearanceManager.Current.AccentColor = value;
                }
            }
        }
    }
}
