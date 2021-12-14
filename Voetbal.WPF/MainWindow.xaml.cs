using System;
using System.Collections.Generic;
// using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Voetbal.Models;
using Voetbal.DAL;

namespace Voetbal.WPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>

    public partial class MainWindow : Window
    {
        private readonly FileReader _reader = new FileReader();
        private const string _fName = "voetbalspelers.txt";

        private List<Speler> _ploeg1 = new List<Speler>();
        private List<Speler> _ploeg2 = new List<Speler>();

        private Speler _currentlySelectedPlayer;

        public MainWindow()
        {
            InitializeComponent();

            InitData(_fName);

        }

        private void InitData(string fName)
        {
            // read from file
            var allSpelers = _reader.ReadSpelersToList(fName);

            // get ploegnamen
            // HashSet only accepts unique entries
            // so we end up with the two teams as they only get added once
            HashSet<string> ploegSet = new HashSet<string>();
            foreach (var speler in allSpelers)
            {
                ploegSet.Add(speler.Ploeg);
            }

            // copy HashSet to array because HashSets can't be iterated
            var ploegenArr = new string[ploegSet.Count];
            ploegSet.CopyTo(ploegenArr);

            // split into teams
            foreach (var speler in allSpelers)
            {
                if (speler.Ploeg == ploegenArr[0])
                {
                    _ploeg1.Add(speler);
                }
                else if (speler.Ploeg == ploegenArr[1])
                {
                    _ploeg2.Add(speler);
                }
            }

            // link controls
            cboPloegen.ItemsSource = ploegenArr;
        }

        public void CboPloegen_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var cbo = sender as ComboBox;

            // remove previously selected player from label
            lstSpelers.SelectedIndex = -1;
            _currentlySelectedPlayer = null;

            // load ploeg in to listbox
            lstSpelers.ItemsSource = null;
            lstSpelers.ItemsSource = cbo.SelectedIndex == 0 ?
                _ploeg1 : _ploeg2;

            UpdateLabel();
        }

        private void LstSpelers_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var lst = sender as ListBox;

            if (lst.SelectedIndex != -1)
            {
                _currentlySelectedPlayer = lst.SelectedItem as Speler;

                lblSpelersinfo.Content = _currentlySelectedPlayer.ToonInfo();
            }
            else
            {
                lblSpelersinfo.Content = null;
            }

            UpdateLabel();
        }

        private void UpdateLabel()
        {
            if (_currentlySelectedPlayer != null)
            {
                lblSpelersinfo.Content = _currentlySelectedPlayer.ToonInfo();
            }
            else
            {
                lblSpelersinfo.Content = "";
            }
        }

        private void Click_Event(object sender, RoutedEventArgs e)
        {
            string msg = "Gelieve een speler te selecteren.";
            var btn = sender as Button;

            // test for selected player
            if (_currentlySelectedPlayer != null)
            {
                // test which button was pressed
                switch (btn.Name)
                {
                    case "btnAddMinutes":
                        if (Int32.TryParse(txtMinuten.Text, out Int32 minuten))
                        {
                            _currentlySelectedPlayer.SpeelminutenToevoegen(minuten);
                            txtMinuten.Text = "";
                            UpdateLabel();
                            // no message needed at this state
                            return;
                        }
                        else
                        {
                            // set relevant error message
                            msg = "Speelminuten moet een numerieke waarde zijn!";
                        }
                        break;
                    case "btnSchieten":
                        if (_currentlySelectedPlayer.SchietOpDoel())
                        {
                            msg = $"{_currentlySelectedPlayer} heeft gescoord!";
                        }
                        else
                        {
                            msg = "De bal ging er naast.";
                        }
                        UpdateLabel();
                        break;
                }
            }
            
            // always shows except when successfully adding minutes to a player
            MessageBox.Show(msg);
        }
    }
}
