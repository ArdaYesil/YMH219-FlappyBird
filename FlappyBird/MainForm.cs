using System.Drawing.Drawing2D;
using FlappyBird.Models;
using FlappyBird.Services;

namespace FlappyBird;

/// <summary>
/// Oyun penceresi. Klavye olaylarını yakalar, Timer ile oyun döngüsünü çalıştırır
/// ve Bird / Pipe / GameState nesnelerini birbirine bağlar.
/// </summary>
public partial class MainForm : Form
{
    private const int PipeCount = 3;      // Ekranda dönen engel sayısı
    private const int PipeSpacing = 220;  // İki engel arasındaki yatay mesafe
    private const int PipeGap = 150;      // Kuşun geçeceği boşluğun yüksekliği
    private const int BirdStartY = 250;

    private readonly Random _random = new();
    private readonly List<Pipe> _pipes = new();
    private readonly HighScoreStore _highScoreStore = new();
    private readonly Bird _bird;
    private readonly GameState _state;

    private bool _flapKeyHeld; // Tuş basılı tutulunca sürekli zıplamasın diye

    public MainForm()
    {
        InitializeComponent();
        DoubleBuffered = true; // Titremeyi azaltır

        picBird.Image = CreateBirdImage(picBird.Width, picBird.Height);
        picBird.Region = CreateBirdRegion(picBird.Width, picBird.Height); // Köşeler borunun üstünde kare görünmesin
        _bird = new Bird(picBird);
        _state = new GameState(_highScoreStore.Load());

        // Engeller çalışma zamanında oluşturulur; zeminin üst kenarı oyun alanının sınırıdır
        for (int i = 0; i < PipeCount; i++)
        {
            _pipes.Add(new Pipe(this, picGround.Top, PipeGap));
        }

        ResetGame();
    }

    // ------------------------------------------------------------------
    // Oyun akışı
    // ------------------------------------------------------------------

    /// <summary>Her şeyi başlangıç konumuna alır ve "Hazır" ekranını gösterir.</summary>
    private void ResetGame()
    {
        gameTimer.Stop();
        _state.Reset();
        _bird.Reset(BirdStartY);

        for (int i = 0; i < _pipes.Count; i++)
        {
            _pipes[i].PlaceAt(ClientSize.Width + i * PipeSpacing, _random);
        }

        btnNewGame.Visible = false;
        ShowMessage("HAZIR!\n\nBaşlamak ve zıplamak için\nSPACE veya ↑ tuşuna bas");
        UpdateHud();
    }

    private void StartGame()
    {
        _state.Start();
        lblMessage.Visible = false;
        gameTimer.Start();
    }

    private void EndGame()
    {
        gameTimer.Stop();
        _state.End();

        string record = "";
        if (_state.IsNewHighScore)
        {
            record = _highScoreStore.Save(_state.HighScore)
                ? "\nYENİ REKOR!"
                : "\nYENİ REKOR! (dosyaya kaydedilemedi)";
        }

        ShowMessage($"GAME OVER\n\nSkor: {_state.Score}\nEn Yüksek: {_state.HighScore}{record}");
        btnNewGame.Visible = true;
        btnNewGame.Focus();
        UpdateHud();
    }

    /// <summary>Oyun döngüsü: Timer her 20 ms'de bir burayı çalıştırır.</summary>
    private void GameTimer_Tick(object? sender, EventArgs e)
    {
        _bird.Update();

        // Engelleri hareket ettir; ekrandan çıkanı en sağdaki engelin arkasına taşı
        foreach (var pipe in _pipes)
        {
            pipe.Move(_state.Speed);

            if (pipe.IsOffScreen)
            {
                int rightMostX = _pipes.Max(p => p.X);
                pipe.PlaceAt(rightMostX + PipeSpacing, _random);
            }

            // Engelin sağ kenarı kuşun soluna geçtiyse engel aşılmıştır
            if (!pipe.Passed && pipe.Right < _bird.Sprite.Left)
            {
                pipe.Passed = true;
                _state.AddPoint();
            }
        }

        UpdateHud();

        if (IsColliding())
        {
            EndGame();
        }
    }

    /// <summary>Kuş zemine ya da herhangi bir engele çarptı mı?</summary>
    private bool IsColliding()
    {
        Rectangle hitBox = _bird.HitBox;

        if (hitBox.Bottom >= picGround.Top) return true;

        return _pipes.Any(p => p.CollidesWith(hitBox));
    }

    // ------------------------------------------------------------------
    // Olaylar
    // ------------------------------------------------------------------

    private void MainForm_KeyDown(object? sender, KeyEventArgs e)
    {
        if (e.KeyCode != Keys.Space && e.KeyCode != Keys.Up) return;

        // Olay forma ait; odaktaki kontrole (ör. buton) gitmesin
        e.Handled = true;
        e.SuppressKeyPress = true;

        if (_flapKeyHeld) return; // Basılı tutma → otomatik tekrar engellenir
        _flapKeyHeld = true;

        switch (_state.Status)
        {
            case GameStatus.Ready:
                StartGame();
                _bird.Flap();
                break;
            case GameStatus.Running:
                _bird.Flap();
                break;
            // GameOver durumunda tuş yok sayılır; "Yeni Oyun" butonu kullanılır
        }
    }

    private void MainForm_KeyUp(object? sender, KeyEventArgs e)
    {
        if (e.KeyCode == Keys.Space || e.KeyCode == Keys.Up)
        {
            _flapKeyHeld = false;
        }
    }

    private void BtnNewGame_Click(object? sender, EventArgs e) => ResetGame();

    // ------------------------------------------------------------------
    // Arayüz yardımcıları
    // ------------------------------------------------------------------

    private void UpdateHud()
    {
        lblScore.Text = $"Skor: {_state.Score}";
        lblInfo.Text = $"Seviye: {_state.Level}  Hız: {_state.Speed:0.0}  Rekor: {_state.HighScore}";
    }

    private void ShowMessage(string text)
    {
        lblMessage.Text = text;
        lblMessage.Visible = true;
    }

    /// <summary>Zemine çim şeridi ve çizgiler çizer.</summary>
    private void PicGround_Paint(object? sender, PaintEventArgs e)
    {
        var g = e.Graphics;
        g.FillRectangle(Brushes.LimeGreen, 0, 0, picGround.Width, 12);
        using var pen = new Pen(Color.Sienna, 2);
        for (int x = -10; x < picGround.Width; x += 24)
        {
            g.DrawLine(pen, x, 18, x + 12, 30);
        }
    }

    /// <summary>PictureBox'ı kuşun şekline (gövde + gaga) kırpan bölge.</summary>
    private static Region CreateBirdRegion(int width, int height)
    {
        using var path = new GraphicsPath();
        path.AddEllipse(0, 0, width - 6, height - 1);
        path.AddPolygon(new[]
        {
            new Point(width - 11, height / 2 - 1),
            new Point(width, height / 2 + 3),
            new Point(width - 11, height / 2 + 8)
        });
        return new Region(path);
    }

    /// <summary>Kuş resmini kodla çizer (harici resim dosyası gerekmez).</summary>
    private static Bitmap CreateBirdImage(int width, int height)
    {
        var bmp = new Bitmap(width, height);
        using var g = Graphics.FromImage(bmp);
        g.SmoothingMode = SmoothingMode.AntiAlias;

        g.FillEllipse(Brushes.Gold, 1, 1, width - 8, height - 3);           // gövde
        g.DrawEllipse(Pens.DarkGoldenrod, 1, 1, width - 8, height - 3);
        g.FillEllipse(Brushes.Orange, 4, height / 2 - 2, 14, 9);           // kanat
        g.FillEllipse(Brushes.White, width - 18, 4, 11, 11);              // göz
        g.FillEllipse(Brushes.Black, width - 13, 7, 4, 5);                 // göz bebeği
        g.FillPolygon(Brushes.OrangeRed, new[]                               // gaga
        {
            new Point(width - 10, height / 2),
            new Point(width - 1, height / 2 + 3),
            new Point(width - 10, height / 2 + 7)
        });
        return bmp;
    }
}
