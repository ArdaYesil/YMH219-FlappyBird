namespace FlappyBird.Models;

/// <summary>
/// Oyuncunun kontrol ettiği kuş. Ekrandaki PictureBox'ı yönetir,
/// yer çekimi ve kanat çırpma (zıplama) fiziğini uygular.
/// </summary>
public class Bird
{
    private const float Gravity = 0.55f;       // Her timer adımında hıza eklenen yer çekimi
    private const float FlapStrength = -8.5f;  // Kanat çırpınca verilen yukarı yönlü hız
    private const float MaxFallSpeed = 10f;    // Düşüş hızının üst sınırı

    private float _y; // Hassas konum (piksel değeri küsuratlı tutulur)

    public PictureBox Sprite { get; }

    /// <summary>Dikey hız. Negatif = yukarı, pozitif = aşağı.</summary>
    public float Velocity { get; private set; }

    /// <summary>
    /// Çarpışma kutusu. Resmin kenarlarındaki boşluk yüzünden
    /// haksız çarpışma olmasın diye birkaç piksel küçültülür.
    /// </summary>
    public Rectangle HitBox => Rectangle.Inflate(Sprite.Bounds, -4, -4);

    public Bird(PictureBox sprite)
    {
        Sprite = sprite;
        _y = sprite.Top;
    }

    /// <summary>Kuşa ani bir yukarı hız verir.</summary>
    public void Flap() => Velocity = FlapStrength;

    /// <summary>Her oyun döngüsü adımında çağrılır: yer çekimini uygular ve kuşu taşır.</summary>
    public void Update()
    {
        Velocity = Math.Min(Velocity + Gravity, MaxFallSpeed);
        _y += Velocity;

        // Ekranın üstünden dışarı çıkmasın
        if (_y < 0)
        {
            _y = 0;
            Velocity = 0;
        }

        Sprite.Top = (int)_y;
    }

    /// <summary>Kuşu başlangıç konumuna alır ve hızını sıfırlar.</summary>
    public void Reset(int startY)
    {
        _y = startY;
        Velocity = 0;
        Sprite.Top = startY;
    }
}
