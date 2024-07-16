using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zephyr.Helpers
{
    public class RandomGeneration
    {
        public List<int> GenerateRandomPositions(int numberOfTiles, int totalGridCells)
        {
            List<int> positions = new List<int>();

            Random rand = new Random();
            while (positions.Count < numberOfTiles)
            {
                int randomPosition = rand.Next(0, totalGridCells);
                if (!positions.Contains(randomPosition))
                {
                    Debug.WriteLine("Generated random position: " + randomPosition);
                    positions.Add(randomPosition);
                }
            }

            return positions;
        }
    }
}
