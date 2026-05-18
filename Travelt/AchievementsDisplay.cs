using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Travelt
{
    public class AchievementsDisplay
    {

        public int AchievementId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string IconUrl { get; set; }

        public bool AchievementUnlocked { get; set; }
        public double Opacity => AchievementUnlocked ? 1.0 : 0.25;
    }
}
