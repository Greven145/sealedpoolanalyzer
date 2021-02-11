using System;
using System.Collections.Generic;
using System.Drawing;
using Colorful;
using Logic.models;

namespace Console {
    internal partial class Program {
        private static readonly List<string> RatingSortOrder = new() {
            "A+",
            "A",
            "A-",
            "B+",
            "B",
            "B-",
            "C+",
            "C",
            "C-",
            "D+",
            "D",
            "D-",
            "F",
            "TBD"
        };

        private static readonly List<(string, double)> RatingToGrade = new List<(string,double)> {
            ("F ", 0 ),
            ("D-", 1 ),
            ("D ", 2 ),
            ("D+", 3 ),
            ("C-", 4 ),
            ("C ", 5 ),
            ("C+", 6 ),
            ("B-", 7 ),
            ("B ", 8 ),
            ("B+", 9 ),
            ("A-", 10),
            ("A ", 11),
            ("A+", 12)
        };

        private static readonly Color WhiteManaColor = Color.FromArgb(249, 250, 244);
        private static readonly Color BlueManaColor = Color.FromArgb(14, 104, 171);
        private static readonly Color BlackManaColor = Color.FromArgb(166, 159, 157);
        private static readonly Color RedManaColor = Color.FromArgb(211, 32, 42);
        private static readonly Color GreenManaColor = Color.FromArgb(0, 115, 62);
        private static readonly FigletFont StandardFont = FigletFont.Load(@"fonts\standard.flf");
        private static readonly FigletFont SmallFont = FigletFont.Load(@"fonts\small.flf");
    }
}