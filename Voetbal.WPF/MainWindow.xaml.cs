using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
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
            HashSet<string> ploegSet = new HashSet<string>();
            foreach (var speler in allSpelers)
            {
                ploegSet.Add(speler.Ploeg);
            }
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
            switch (btn.Name)
            {
                case "btnAddMinutes":
                    if (_currentlySelectedPlayer != null)
                    {
                        if (Int32.TryParse(txtMinuten.Text, out Int32 minuten))
                        {
                            _currentlySelectedPlayer.SpeelminutenToevoegen(minuten);
                            txtMinuten.Text = "";
                            UpdateLabel();
                            // no messagebox at this state
                            return;
                        }
                        else
                        {
                            msg = "Speelminuten moet een numerieke waarde zijn!";
                        }
                    }
                    break;
                case "btnSchieten":
                    if (_currentlySelectedPlayer != null)
                    {
                        if (_currentlySelectedPlayer.SchietOpDoel())
                        {
                            msg = $"{_currentlySelectedPlayer.ToString()} heeft gescoord!";
                        }
                        else
                        {
                            msg = "De bal ging er naast.";
                        }
                        UpdateLabel();
                    }
                    break;
            }

            MessageBox.Show(msg);
        }
    }
}
