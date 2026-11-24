namespace FlappyBird.Models;

/// <summary>Oyunun içinde bulunabileceği durumlar.</summary>
public enum GameStatus
{
    Ready,    // Başlamayı bekliyor
    Running,  // Oynanıyor
    GameOver  // Kuş çarptı
}

/// <summary>
/// Skor, seviye, hız, en yüksek skor ve oyun durumunu tek yerde tutar.
/// Form bu bilgileri doğrudan değiştirmez, bu sınıfın metotlarını çağırır.
/// </summary>
public class GameState
{
    private const float BaseSpeed = 4f;     // 1. seviyedeki engel hızı (piksel/adım)
    private const float SpeedStep = 0.5f;   // Her seviyede eklenen hız
    private const float MaxSpeed = 9f;      // Hızın üst sınırı
    private const int PointsPerLevel = 5;   // Kaç puanda bir seviye atlanır

    public GameStatus Status { get; private set; } = GameStatus.Ready;
    public int Score { get; private set; }
    public int HighScore { get; private set; }
    public bool IsNewHighScore { get; private set; }

    /// <summary>Seviye skordan hesaplanır: 0-4 → 1, 5-9 → 2, ...</summary>
    public int Level => Score / PointsPerLevel + 1;

    /// <summary>Engellerin hızı seviyeye göre artar.</summary>
    public float Speed => Math.Min(BaseSpeed + (Level - 1) * SpeedStep, MaxSpeed);

    public GameState(int highScore)
    {
        HighScore = highScore;
    }

    public void Start() => Status = GameStatus.Running;

    public void AddPoint() => Score++;

    /// <summary>Oyunu bitirir; skor rekoru geçtiyse en yüksek skoru günceller.</summary>
    public void End()
    {
        Status = GameStatus.GameOver;
        if (Score > HighScore)
        {
            HighScore = Score;
            IsNewHighScore = true;
        }
    }

    /// <summary>Yeni oyun için skoru ve durumu sıfırlar (en yüksek skor korunur).</summary>
    public void Reset()
    {
        Score = 0;
        IsNewHighScore = false;
        Status = GameStatus.Ready;
    }
}
