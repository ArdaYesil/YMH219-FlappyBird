namespace FlappyBird.Services;

/// <summary>
/// En yüksek skoru bir metin dosyasında saklar ve okur.
/// Dosya: %LocalAppData%\YMH219-FlappyBird\highscore.txt
/// </summary>
public class HighScoreStore
{
    private readonly string _filePath;

    public HighScoreStore()
    {
        string folder = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
            "YMH219-FlappyBird");
        _filePath = Path.Combine(folder, "highscore.txt");
    }

    /// <summary>Kayıtlı skoru döndürür. Dosya yoksa veya bozuksa 0 döner.</summary>
    public int Load()
    {
        try
        {
            if (!File.Exists(_filePath)) return 0;
            string text = File.ReadAllText(_filePath).Trim();
            return int.TryParse(text, out int score) && score >= 0 ? score : 0;
        }
        catch (Exception ex) when (ex is IOException or UnauthorizedAccessException)
        {
            return 0;
        }
    }

    /// <summary>Skoru dosyaya yazar. Başarısız olursa false döner, oyun çökmez.</summary>
    public bool Save(int score)
    {
        try
        {
            Directory.CreateDirectory(Path.GetDirectoryName(_filePath)!);
            File.WriteAllText(_filePath, score.ToString());
            return true;
        }
        catch (Exception ex) when (ex is IOException or UnauthorizedAccessException)
        {
            return false;
        }
    }
}
