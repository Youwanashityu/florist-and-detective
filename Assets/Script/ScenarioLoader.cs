using System.Collections.Generic;
using System.IO;
using UnityEngine;

/// <summary>
/// StreamingAssetsフォルダのCSVファイルを読み込み、
/// ScenarioDataに変換するクラス。
/// </summary>
public static class ScenarioLoader
{
    /// <summary>
    /// 指定した客名のCSVを読み込んでScenarioDataを返します。
    /// CSVはAssets/StreamingAssets/Scenarios/に配置してください。
    /// </summary>
    /// <param name="characterName">客の名前（CSVのファイル名）</param>
    public static ScenarioData Load(string characterName)
    {
        var path = Path.Combine(Application.streamingAssetsPath, "Scenarios", $"{characterName}.csv");

        if (!File.Exists(path))
        {
            Debug.LogWarning($"[ScenarioLoader] CSVが見つかりません: {path}");
            return null;
        }

        var data = new ScenarioData { characterName = characterName };
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
        if (cols.Length < 13) return null;

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
        };
    }
}