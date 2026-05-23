# HealthBar Analyzer

格闘ゲーム動画から体力バーのHP割合をフレーム単位で抽出し、CSVに出力するC# WinFormsツール。

## 対応ゲーム

- Street Fighter 6（デフォルト）
- Street Fighter 5

## クラス構成（Phase 1〜4 リファクタリング済み）

```
HPBarForm           UI・イベントハンドラ。他クラスを保持するエントリポイント。
├── VideoLoader     IFrameReader実装。OpenCvSharpでフレームを読み取り、表示サイズにスケール。
├── Boundary        バー境界の検出・HP計算・CSV出力。HPBarForm非依存（IFrameReader経由のみ）。
├── Calculate       輝度グラデーション計算（SF6用境界自動検出の前処理）。HPBarForm非依存。
├── BarColorClassifier  SF6/SF5のバー色状態を判定するstatic純関数クラス。
└── AnalysisResult  解析結果DTO（HP1P / HP2P / Errors リスト）。

IFrameReader        VideoLoaderを抽象化するインターフェース（GetFrameRead / TotalFrames）。
```

## 検出アルゴリズム概要

### Step 1: バー位置の特定（初期1回）

**SF6**: 動画フレームの1ライン（Y座標）でRGB輝度の水平勾配（1次微分）を計算。
輝度変化が小さい領域が `Sf6MinBarWidthPixels`(250px) 以上続く → バーの平坦部分として検出。

**SF5**: 1P/2PバーのX座標が画面上ほぼ固定のため、定数値（`Sf5Bar1PSearchOrigin`=200）
からRチャンネル閾値(`Sf5ColorThreshold`=150)で境界を走査。

手動指定モード: 「手動座標設定」ボタンで4点をクリック指定することも可能。

### Step 2: フレームごとのHP計算

基準フレームで最大HP時の境界X座標を記録。
各フレームで現在のバー端位置を検出し、`(現在幅 / 最大幅) × 100` でHP割合を算出。

**色状態の分類**（`BarColorClassifier.DetectBarState` / `DetectBarStateSF5`）:
- 通常バー: 1PはRed寄り、2PはBlue寄り
- Yellow: HP25%以下で変色
- Damage: ダメージ表示（オレンジ）
- noize: バー以外の領域

**ノイズ対策**: `gradientCount` がしきい値(`thresholdGradient`=4)を超えたフレームは
前フレームの値を引き継ぐ（`temp1P` / `temp2P`）。

**KO判定**: 画面中央(x=200-250)が白飛び(RGB=200-250) かつ バー端がnoize → HP=0。

### Step 3: CSV出力

`FrameNumber, LeftHP[%], RightHP[%], ErrorInformation` の形式で保存。
ErrorInformation: 0=正常, 1=1P検出不安定, 2=2P検出不安定, 3=両方不安定。

## 定数一覧（Boundary.cs）

| 定数 | 値 | 用途 |
|------|----|------|
| `Sf6GradientThreshold` | 20 | SF6: 輝度差がこれ以下なら平坦とみなす |
| `Sf6MinBarWidthPixels` | 250 | SF6: バー平坦部の最小幅（px） |
| `Sf5Bar1PSearchOrigin` | 200 | SF5: 1Pバーの走査開始X（1280×720基準） |
| `Sf5Bar1PRightEdge` | 700 | SF5: 1Pバーの右端X |
| `Sf5Bar2PSearchOrigin` | 800 | SF5: 2Pバーの走査開始X |
| `Sf5Bar2PRightEdge` | 1200 | SF5: 2Pバーの右端X |
| `Sf5ColorThreshold` | 150 | SF5: Rチャンネル境界判定しきい値 |
| `KoTextRegionStart` | 200 | KO判定: 白飛び確認X範囲（開始） |
| `KoTextRegionEnd` | 250 | KO判定: 白飛び確認X範囲（終了） |
| `KoPixelIntensityMin` | 200 | KO判定: 白飛びRGB最小値 |
| `KoPixelIntensityMax` | 250 | KO判定: 白飛びRGB最大値 |

## 推奨動作環境

- 解像度: 720p以上を推奨（360p以下はバー色の圧縮劣化により検出精度が低下する）
- SF5定数（`Sf5Bar*`）は1280×720解像度に依存。他解像度では要調整

## 開発経緯

`20241211-midterm3` ブランチで輝度・RGB・勾配の1次元グラフ可視化（`Charts.cs`）と
グレースケール差分アプローチ（`hakaba.cs`）を試作。
最終的に「RGBグラデーション+色状態分類」の組み合わせを採用（`20241211-publish`）。

Phase 1〜4 でコードの品質改善を実施（タイポ修正・定数化・依存関係逆転・メモリリーク修正）。

## 既知の制限・残課題

- SF5定数（`Sf5Bar*`）は1280×720解像度に依存。他解像度では要調整
- `SetBaseBoundaries`: `boundaries[2]` と `[3]` が2PバーではX順が逆転（ソート済みリストのため）
- `VideoLoader` がまだ `HPBarForm.pictureBoxFrame.Width` に直接依存（表示スケーリング用）
- 解析中断機能（`cancellationTokenSource`）が宣言済みだが未接続
- HPBarForm に不要な using 文と public メンバーが残存
- `Boundary` の一部フィールド（`boundary1P`, `boundary2P`）が未使用のまま残存
