using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace Acquire.Views
{
    /// <summary>
    /// Style class for this set of game views.
    /// </summary>
    public class AquireStyle
    {
        /// <summary>
        /// Associates each active corporation with a color hex code.
        /// </summary>
        private Dictionary<ECorporation, string> activeCorpColors { get; }

        /// <summary>
        /// Associates each inactive corporation with a color hex code.
        /// </summary>
        private Dictionary<ECorporation, string> inactiveCorpColors { get; }

        /// <summary>
        /// Creates a new instance of the <see cref="AquireStyle"/> class.
        /// </summary>
        public AquireStyle()
        {
            activeCorpColors = new Dictionary<ECorporation, string>
            {
                { ECorporation.American, "#ff2478ff" },
                { ECorporation.Imperial, "#fffffb26" },
                { ECorporation.Continental, "#fffc2626" },
                { ECorporation.Festival, "#ff37fc28" },
                { ECorporation.WorldWide, "#ffb826fc" },
                { ECorporation.Sackson, "#fffaa125" },
                { ECorporation.Tower, "#ff26fcc7" }
            };

            inactiveCorpColors = new Dictionary<ECorporation, string>
            {
                { ECorporation.American, "#ff14428c" },
                { ECorporation.Imperial, "#ff8c8a14" },
                { ECorporation.Continental, "#ff8c1414" },
                { ECorporation.Festival, "#ff1c8c14" },
                { ECorporation.WorldWide, "#ff66148c" },
                { ECorporation.Sackson, "#ff8c5a14" },
                { ECorporation.Tower, "#ff148c6e" }
            };
        }

        /// <summary>
        /// Gets a color which represents the active version of the named corporation.
        /// </summary>
        /// <param name="corpName">The corporation whose color is being retrieved.</param>
        /// <returns>A <see cref="SolidColorBrush"/> of the retrieved color.</returns>
        public SolidColorBrush GetActiveCorpColor(ECorporation corpName)
        {
            return new SolidColorBrush(
                (Color)ColorConverter.ConvertFromString(activeCorpColors[corpName]));
        }

        /// <summary>
        /// Gets a color which represents the inactive version of the named corporation.
        /// </summary>
        /// <param name="corpName">The corporation whose color is being retrieved.</param>
        /// <returns>A <see cref="SolidColorBrush"/> of the retrieved color.</returns>
        public SolidColorBrush GetInactiveCorpColor(ECorporation corpName)
        {
            return new SolidColorBrush(
                (Color)ColorConverter.ConvertFromString(inactiveCorpColors[corpName]));
        }
    }
}
