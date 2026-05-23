using OpenCvSharp;
using System;

namespace HealthBar {
    public static class BarColorClassifier {
        public static string DetectBarState(Vec3b color) {
            int r = color.Item2, g = color.Item1, b = color.Item0;
            if (r >= 100 && g >= 160 && b <= 200 && r >= b && g >= b) return "Yellow";
            if (r >= 100 && g >= 100 && b <= 100 && b >= 30) return "Damage";
            if (r >= 100 && r <= 250 && g <= 50 && b >= 30 && b <= 150 && r >= b && b >= g) return "1PBar";
            if (g <= 220 && g >= 50 && b >= 100 && b >= g && g >= r) return "2PBar";
            return "noize";
        }

        public static string DetectBarStateSF5(Vec3b color) {
            int r = color.Item2, g = color.Item1, b = color.Item0;
            if (r > 200 && g > 170 && b > 70 && b < 200) return "YellowBar";
            if (r > 180 && g > 150 && b > 70 && b < 220) return "YellowBar";
            if (r <= 100 && g <= 100 && b <= 100 && Math.Abs(r - g) <= 50) return "NoHealth";
            if (r > 50 && g > 180 && b < 200) return "PlayerBar";
            if (r > 150 && g > 100 && g < 220 && b > 80 && b < 150) return "PlayerBar";
            if (r < 100 && g > 220 && b > 70 && b < 120) return "PlayerBar";
            if (r > 200 && g < 120 && b < 100) return "PlayerDamagedLine";
            if (r >= 100 && g <= 100 && b <= 100) return "Damage";
            if (r >= 100 && g >= 100 && b >= 100 && r <= 150 && g <= 150 && b <= 150 && Math.Abs(r - g) <= 50 && Math.Abs(b - g) <= 50) return "TempDamage";
            if (Math.Abs(r - g) < 20 && Math.Abs(g - b) < 20) return "Edge";
            return "noize";
        }
    }
}
