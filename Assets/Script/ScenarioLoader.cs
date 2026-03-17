using System.IO;
using UnityEngine;

/// <summary>
/// StreamingAssetsフォルダのCSVファイルを読み込み、
/// ScenarioDataに変換するクラス。
/// </summary>
public static class ScenarioLoader
{
    /// <summary>
    /// CSVを読み込んでScenarioDataを返します。
    /// CSVはAssets/StreamingAssets/Scenarios/scenario.csvに配置してください。
    /// </summary>
    public static ScenarioData Load()
    {
        var path = Path.Combine(Application.streamingAssetsPath, "Scenarios", "scenario.csv");

        if (!File.Exists(path))
        {
            Debug.LogWarning($"[ScenarioLoader] CSVが見つかりません: {path}");
            return null;
        }

        var data = new ScenarioData();
        var lines = File.ReadAllLines(path);

        // 1行目はヘッダーなのでスキップ
        for (int i = 1; i < lines.Length; i++)
        {
            var line = ParseLine(lines[i]);
            if (line != null)
                data.lines[line.id] = line;
        }

        return data;
    }

    /// <summary>
    /// CSV1行をScenarioLineに変換します。
    /// </summary>
    private static ScenarioLine ParseLine(string csvLine)
    {
        if (string.IsNullOrWhiteSpace(csvLine)) return null;

        var cols = csvLine.Split(',');
        if (cols.Length < 18) return null;

        return new ScenarioLine
        {
            id = int.TryParse(cols[0], out var id) ? id : 0,
            character = cols[1].Trim(),
            sprite = cols[2].Trim(),
            text = cols[3].Trim(),
            choice1Text = cols[4].Trim(),
            choice1Next = int.TryParse(cols[5], out var c1n) ? c1n : 0,
            choice1Affinity = int.TryParse(cols[6], out var c1a) ? c1a : 0,
            choice2Text = cols[7].Trim(),
            choice2Next = int.TryParse(cols[8], out var c2n) ? c2n : 0,
            choice2Affinity = int.TryParse(cols[9], out var c2a) ? c2a : 0,
            bgm = cols[10].Trim(),
            se = cols[11].Trim(),
            cocktail = cols[12].Trim(),
            nextCharacter = cols[13].Trim(),
            cocktailShortNext = int.TryParse(cols[14], out var csn) ? csn : 0,
            cocktailJustNext = int.TryParse(cols[15], out var cjn) ? cjn : 0,
            cocktailLongNext = int.TryParse(cols[16], out var cln) ? cln : 0,
            cocktailMinTime = float.TryParse(cols[17], out var cmin) ? cmin : 2.5f,
            cocktailMaxTime = float.TryParse(cols[18], out var cmax) ? cmax : 3.5f,
        };
    }
}