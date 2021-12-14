using System;

namespace Voetbal.Models
{
    public class Speler
    {
        // constructors
        public Speler() : this(voornaam: "",
            familienaam: "",
            ploeg: "")
        { }

        public Speler(string voornaam,
            string familienaam,
            string ploeg) : this(voornaam: voornaam,
                familienaam: familienaam,
                ploeg: ploeg,
                aantalDoelpogingen: 0,
                aantalDoelpunten: 0,
                aantalSpeelminuten: 0)
        { }

        public Speler(string voornaam,
            string familienaam,
            string ploeg,
            Int32 aantalDoelpunten,
            Int32 aantalDoelpogingen,
            Int32 aantalSpeelminuten)
        {
            Voornaam = voornaam;
            Familienaam = familienaam;
            Ploeg = ploeg;
            AantalDoelPogingen = aantalDoelpogingen;
            AantalDoelPunten = aantalDoelpunten;
            AantalSpeelMinuten = aantalSpeelminuten;
        }

        // methods
        public bool SchietOpDoel()
        {
            // verhoog pogingen
            AantalDoelPogingen++;

            // kans op doelpunt
            bool IsPoint = false;
            if (_rnd.Next(0, 2) == 1)
            {
                AantalDoelPunten++;
                IsPoint = true;
            }

            return IsPoint;
        }

        public void SpeelminutenToevoegen(Int32 speelminuten)
        {
            AantalSpeelMinuten += speelminuten;
        }

        public string ToonInfo() =>
            ToString() + "\n" +
            $"Doelpogingen: {AantalDoelPogingen}\n" +
            $"Doelpunten: {AantalDoelPunten}\n" +
            $"Speelminuten: {AantalSpeelMinuten}";

        public override string ToString() =>
            $"{Voornaam} {Familienaam}";

        // public properties
        public Int32 AantalDoelPogingen
        {
            get { return _aantalDoelPogingen; }
            set { _aantalDoelPogingen = value; }
        }

        public Int32 AantalDoelPunten
        {
            get { return _aantalDoelPunten; }
            set { _aantalDoelPunten = value; }
        }

        public Int32 AantalSpeelMinuten
        {
            get => _aantalSpeelMinuten;
            set => _aantalSpeelMinuten = value > 0 ?
                value : 0;
        }

        public string Familienaam
        {
            get { return _familienaam; }
            set { _familienaam = value; }
        }

        public string Ploeg
        {
            get { return _ploeg; }
            set { _ploeg = value; }
        }

        public string Voornaam
        {
            get { return _voornaam; }
            set { _voornaam = value; }
        }

        // private fields
        private Int32 _aantalDoelPogingen;
        private Int32 _aantalDoelPunten;
        private Int32 _aantalSpeelMinuten;
        private string _familienaam;
        private string _ploeg;
        private string _voornaam;
        private readonly Random _rnd = new Random();
    }
}
