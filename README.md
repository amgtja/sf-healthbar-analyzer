# SF Health Bar Analyzer

  A Windows desktop app that analyzes health bar data from Street Fighter 5/6 match videos, frame by frame.

  ## Features

  - **Frame-by-frame video analysis** — Scrub through any frame of a match video
  - **Automatic HP bar detection** — Click on the health bar to auto-detect its boundaries using color gradient analysis
  - **Manual mode** — Define the bar area by specifying 4 corner coordinates
  - **Dual player tracking** — Simultaneously tracks 1P and 2P health percentages
  - **Real-time visualization** — Renders a mirrored HP bar overlay on screen
  - **CSV export** — Saves per-frame HP data for further analysis
  - **SF5 / SF6 support** — Separate analysis logic for each game's bar format

  ## Tech Stack

  - **Language**: C#
  - **UI**: Windows Forms
  - **Image Processing**: OpenCvSharp
  - **Async**: async / await for non-blocking frame processing

  ## How It Works

  1. Load a match video file
  2. Click on the health bar in the first frame (auto-detect mode) or enter coordinates manually
  3. Run analysis — all frames are processed and HP % is calculated
  4. Review the HP timeline or export to CSV

  ## Screenshot
  <img width="1617" height="825" alt="image" src="https://github.com/user-attachments/assets/2137d53a-dcad-4100-8bea-eb1e2927f720" />


  ## Setup

  1. Clone this repo and switch to the `20241211-publish` branch
  2. Open `HealthBar.sln` in Visual Studio
  3. Restore NuGet packages (`OpenCvSharp4.Windows`)
  4. Build and run

  ## Notes & Future Improvements

  - Current boundary detection uses first-order color gradient; a more robust approach (e.g., Canny edge detection or template matching) could improve accuracy
  on low-contrast frames
  - SF5 and SF6 have different bar geometries — game detection is currently manual selection

  ## Background

  Built as a research tool during university.
