using System.Collections.Generic;

namespace HealthBar {
    public class AnalysisResult {
        public List<double> HP1P { get; } = new List<double>();
        public List<double> HP2P { get; } = new List<double>();
        public List<string> Errors { get; } = new List<string>();
    }
}
