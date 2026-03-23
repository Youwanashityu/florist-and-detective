using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ScenarioLoader
{
    public static ScenarioData LoadedData { get; private set; }

    /// <summary>
    /// ResourcesフォルダからCSVを読み込みます。WebGL対応。
    /// </summary>
    public static IEnumerator LoadAsync(System.Action<ScenarioData> onComplete)
    {
        // ResourcesフォルダからTextAssetとして読み込む
        var textAsset = Resources.Load<TextAsset>("Scenarios/scenario");

        if (textAsset == null)
        {
            Debug.LogWarning("[ScenarioLoader] CSVが見つかりません: Resources/Scenarios/scenario");
            onComplete?.Invoke(null);
            yield break;
        }

        var data = new ScenarioData();
        var fullText = textAsset.text;
        var lines = SplitCSVLines(fullText);

        for (int i = 1; i < lines.Count; i++)
        {
            var line = ParseLine(lines[i]);
            if (line != null)
                data.lines[line.id] = line;
        }

        LoadedData = data;
        onComplete?.Invoke(data);
        yield break;
    }

    private static List<string> SplitCSVLines(string text)
    {
        var lines = new List<string>();
        var current = new System.Text.StringBuilder();
        bool inQuotes = false;

        foreach (char c in text)
        {
            if (c == '"')
            {
                inQuotes = !inQuotes;
                current.Append(c);
            }
            else if (c == '\n' && !inQuotes)
            {
                lines.Add(current.ToString().TrimEnd('\r'));
                current.Clear();
            }
            else
            {
                current.Append(c);
            }
        }

        if (current.Length > 0)
            lines.Add(current.ToString());

        return lines;
    }

    private static ScenarioLine ParseLine(string csvLine)
    {
        if (string.IsNullOrWhiteSpace(csvLine)) return null;

        var cols = SplitCSVColumns(csvLine);
        if (cols.Count < 23) return null;

        return new ScenarioLine
        {
            id = int.TryParse(cols[0], out var id) ? id : 0,
            character = cols[1].Trim(),
            sprite = cols[2].Trim(),
            text = cols[3].Trim(),
            nextId = int.TryParse(cols[4], out var nid) ? nid : 0,
            choice1Text = cols[5].Trim(),
            choice1Next = int.TryParse(cols[6], out var c1n) ? c1n : 0,
            choice1Affinity = int.TryParse(cols[7], out var c1a) ? c1a : 0,
            choice2Text = cols[8].Trim(),
            choice2Next = int.TryParse(cols[9], out var c2n) ? c2n : 0,
            choice2Affinity = int.TryParse(cols[10], out var c2a) ? c2a : 0,
            bgm = cols[11].Trim(),
            se = cols[12].Trim(),
            cocktail = cols[13].Trim(),
            nextCharacter = cols[14].Trim(),
            cocktailShortNext = int.TryParse(cols[15], out var csn) ? csn : 0,
            cocktailJustNext = int.TryParse(cols[16], out var cjn) ? cjn : 0,
            cocktailLongNext = int.TryParse(cols[17], out var cln) ? cln : 0,
            cocktailMinTime = float.TryParse(cols[18], out var cmin) ? cmin : 2.5f,
            cocktailMaxTime = float.TryParse(cols[19], out var cmax) ? cmax : 3.5f,
            affinityThreshold = int.TryParse(cols[20], out var at) ? at : 0,
            affinityNext = int.TryParse(cols[21], out var an) ? an : 0,
            nextScene = cols[22].Trim(),
        };
    }

    private static List<string> SplitCSVColumns(string line)
    {
        var cols = new List<string>();
        var current = new System.Text.StringBuilder();
        bool inQuotes = false;

        foreach (char c in line)
        {
            if (c == '"')
            {
                inQuotes = !inQuotes;
            }
            else if (c == ',' && !inQuotes)
            {
                cols.Add(current.ToString());
                current.Clear();
            }
            else
            {
                current.Append(c);
            }
        }

        cols.Add(current.ToString());
        return cols;
    }
}