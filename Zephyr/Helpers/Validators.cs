using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows;
using Zephyr.Core;
using System.Diagnostics;

namespace Zephyr.Helpers
{
    public class Validators
    {
        private JsonSetters jsonSetters;
        private JsonGetters jsonGetters;
        public RestSharpClient client;

        private const int DefaultBet = 5;
        private const int DefaultMines = 3;
        private const int DefaultTiles = 4;
        private const int MinValue = 1;
        private const int MaxValue = 24;

        private int _mines = DefaultMines;
        private int _tiles = DefaultTiles;
        private int _bet = DefaultBet;

        public Validators()
        {
            client = new RestSharpClient("http://127.0.0.1:5000");

            jsonSetters = new JsonSetters();
            jsonGetters = new JsonGetters();
        }

        public async Task<string> ValidateBet(TextBox BetEntryBox)
        {
            string authToken = jsonGetters.getAuthFromJson();

            var headers = new Dictionary<string, string>
        {
            { "x-auth-token", authToken }
        };

            var response = await client.GetAsync("wallet", headers);
            Debug.WriteLine(response);


            int wallet = (int)response["wallet"];
            Debug.WriteLine(wallet);

            if (int.TryParse(BetEntryBox.Text, out _bet))
            {
                int bet = int.Parse(BetEntryBox.Text);
                if (bet > wallet || bet < 5) { return "invalid"; }
                else { return "valid"; }
            }
            return "unable to parse";
        }

        public async Task<bool> ValidateBoxes(TextBox MinesEntryBox, TextBox TilesEntryBox, TextBox BetEntryBox)
        {
            if (!int.TryParse(MinesEntryBox.Text, out _mines) || _mines < MinValue || _mines > MaxValue)
            {
                _mines = DefaultMines;
                MinesEntryBox.Text = DefaultMines.ToString();
            }

            if (!int.TryParse(TilesEntryBox.Text, out _tiles) || _tiles < MinValue || _tiles > MaxValue)
            {
                _tiles = DefaultTiles;
                TilesEntryBox.Text = DefaultTiles.ToString();
            }

            string vBet = await ValidateBet(BetEntryBox);
            if (vBet == "unable to parse" || vBet == "invalid")
            {
                _bet = DefaultBet;
                BetEntryBox.Text = DefaultBet.ToString();

                vBet = await ValidateBet(BetEntryBox);
                if (vBet == "invalid")
                {
                    MessageBox.Show("Bet amount is lower than 5 or bet is higher than wallet amount");
                    return false;
                }
            }
            return true;
        }
    }
}
