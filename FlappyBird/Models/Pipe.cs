using System.Drawing.Drawing2D;

namespace FlappyBird.Models;

/// <summary>
/// Bir engel: üstte ve altta iki boru, ortalarında kuşun geçeceği bir boşluk.
/// Sağdan sola hareket eder; ekrandan çıkınca yeniden konumlandırılır.
/// </summary>
public class Pipe
{
    public const int Width = 60;
    private const int CapHeight = 24;   // Boru ağzının (kapak) yüksekliği
    private const int MinPipeHeight = 60;

    private readonly PictureBox _top;
    private readonly PictureBox _bottom;
    private readonly int _areaHeight;   // Oynanabilir alanın yüksekliği (zeminin üst kenarı)
    private readonly int _gapSize;      // İki boru arasındaki boşluk
    private float _x;                   // Hassas yatay konum

    /// <summary>Kuş bu engeli geçti mi? (Skorun bir kez artması için)</summary>
    public bool Passed { get; set; }

    public int X => (int)_x;
    public int Right => X + Width;
    public bool IsOffScreen => Right < 0;

    public Pipe(Control parent, int areaHeight, int gapSize)
    {
        _areaHeight = areaHeight;
        _gapSize = gapSize;

        _top = CreatePipeBox(capAtBottom: true);
        _bottom = CreatePipeBox(capAtBottom: false);

        parent.Controls.Add(_top);
        parent.Controls.Add(_bottom);
    }

    /// <summary>Engeli verilen x konumuna koyar ve boşluğun yüksekliğini rastgele seçer.</summary>
    public void PlaceAt(int newX, Random random)
    {
        _x = newX;
        Passed = false;

        int maxGapTop = _areaHeight - _gapSize - MinPipeHeight;
        int gapTop = random.Next(MinPipeHeight, maxGapTop + 1);

        _top.SetBounds(X, 0, Width, gapTop);
        _bottom.SetBounds(X, gapTop + _gapSize, Width, _areaHeight - gapTop - _gapSize);
    }

    /// <summary>Engeli sola doğru verilen hız kadar kaydırır.</summary>
    public void Move(float speed)
    {
        _x -= speed;
        _top.Left = X;
        _bottom.Left = X;
    }

    /// <summary>Verilen dikdörtgen üst ya da alt boruya değiyor mu?</summary>
    public bool CollidesWith(Rectangle rect) =>
        _top.Bounds.IntersectsWith(rect) || _bottom.Bounds.IntersectsWith(rect);

    private static PictureBox CreatePipeBox(bool capAtBottom)
    {
        var box = new PictureBox { BackColor = Color.ForestGreen };
        box.Paint += (s, e) => DrawPipe(e.Graphics, ((Control)s!).ClientRectangle, capAtBottom);
        return box;
    }

    /// <summary>Boruyu gölgeli yeşil bir silindir gibi çizer, kuşa bakan uca kapak ekler.</summary>
    private static void DrawPipe(Graphics g, Rectangle r, bool capAtBottom)
    {
        if (r.Width <= 0 || r.Height <= 0) return;

        using var body = new LinearGradientBrush(r, Color.LimeGreen, Color.DarkGreen, LinearGradientMode.Horizontal);
        g.FillRectangle(body, r);

        int capY = capAtBottom ? r.Bottom - CapHeight : 0;
        var cap = new Rectangle(0, capY, r.Width, Math.Min(CapHeight, r.Height));
        using var capBrush = new LinearGradientBrush(cap, Color.GreenYellow, Color.Green, LinearGradientMode.Horizontal);
        g.FillRectangle(capBrush, cap);

        using var pen = new Pen(Color.DarkGreen, 2);
        g.DrawRectangle(pen, 1, 1, r.Width - 2, r.Height - 2);
        g.DrawRectangle(pen, cap.X + 1, cap.Y + 1, cap.Width - 2, cap.Height - 2);
    }
}
