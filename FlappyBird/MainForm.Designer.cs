namespace FlappyBird;

partial class MainForm
{
    private System.ComponentModel.IContainer components = null!;

    protected override void Dispose(bool disposing)
    {
        if (disposing && (components != null))
        {
            components.Dispose();
        }
        base.Dispose(disposing);
    }

    #region Windows Form Designer generated code

    private void InitializeComponent()
    {
        components = new System.ComponentModel.Container();
        gameTimer = new System.Windows.Forms.Timer(components);
        picBird = new PictureBox();
        picGround = new PictureBox();
        lblScore = new Label();
        lblInfo = new Label();
        lblMessage = new Label();
        btnNewGame = new Button();
        ((System.ComponentModel.ISupportInitialize)picBird).BeginInit();
        ((System.ComponentModel.ISupportInitialize)picGround).BeginInit();
        SuspendLayout();
        //
        // gameTimer
        //
        gameTimer.Interval = 20;
        gameTimer.Tick += GameTimer_Tick;
        //
        // picBird
        //
        picBird.BackColor = Color.Transparent;
        picBird.Location = new Point(80, 250);
        picBird.Name = "picBird";
        picBird.Size = new Size(40, 30);
        picBird.SizeMode = PictureBoxSizeMode.StretchImage;
        picBird.TabStop = false;
        //
        // picGround
        //
        picGround.BackColor = Color.Peru;
        picGround.Location = new Point(0, 540);
        picGround.Name = "picGround";
        picGround.Size = new Size(400, 60);
        picGround.TabStop = false;
        picGround.Paint += PicGround_Paint;
        //
        // lblScore
        //
        lblScore.BackColor = Color.Peru;
        lblScore.Font = new Font("Segoe UI", 16F, FontStyle.Bold);
        lblScore.ForeColor = Color.White;
        lblScore.Location = new Point(8, 554);
        lblScore.Name = "lblScore";
        lblScore.Size = new Size(120, 40);
        lblScore.Text = "Skor: 0";
        lblScore.TextAlign = ContentAlignment.MiddleLeft;
        //
        // lblInfo
        //
        lblInfo.BackColor = Color.Peru;
        lblInfo.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
        lblInfo.ForeColor = Color.White;
        lblInfo.Location = new Point(130, 554);
        lblInfo.Name = "lblInfo";
        lblInfo.Size = new Size(262, 40);
        lblInfo.Text = "Seviye: 1  Hız: 4.0  Rekor: 0";
        lblInfo.TextAlign = ContentAlignment.MiddleRight;
        //
        // lblMessage
        //
        lblMessage.BackColor = Color.Ivory;
        lblMessage.BorderStyle = BorderStyle.FixedSingle;
        lblMessage.Font = new Font("Segoe UI", 13F, FontStyle.Bold);
        lblMessage.ForeColor = Color.SaddleBrown;
        lblMessage.Location = new Point(50, 70);
        lblMessage.Name = "lblMessage";
        lblMessage.Size = new Size(300, 170);
        lblMessage.Text = "Hazır!";
        lblMessage.TextAlign = ContentAlignment.MiddleCenter;
        //
        // btnNewGame
        //
        btnNewGame.BackColor = Color.Orange;
        btnNewGame.Cursor = Cursors.Hand;
        btnNewGame.FlatStyle = FlatStyle.Flat;
        btnNewGame.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
        btnNewGame.ForeColor = Color.White;
        btnNewGame.Location = new Point(125, 255);
        btnNewGame.Name = "btnNewGame";
        btnNewGame.Size = new Size(150, 45);
        btnNewGame.Text = "Yeni Oyun";
        btnNewGame.UseVisualStyleBackColor = false;
        btnNewGame.Click += BtnNewGame_Click;
        //
        // MainForm
        //
        AutoScaleMode = AutoScaleMode.None;
        BackColor = Color.SkyBlue;
        ClientSize = new Size(400, 600);
        Controls.Add(btnNewGame);
        Controls.Add(lblMessage);
        Controls.Add(lblScore);
        Controls.Add(lblInfo);
        Controls.Add(picBird);
        Controls.Add(picGround);
        FormBorderStyle = FormBorderStyle.FixedSingle;
        KeyPreview = true;
        MaximizeBox = false;
        Name = "MainForm";
        StartPosition = FormStartPosition.CenterScreen;
        Text = "YMH219 - Flappy Bird";
        KeyDown += MainForm_KeyDown;
        KeyUp += MainForm_KeyUp;
        ((System.ComponentModel.ISupportInitialize)picBird).EndInit();
        ((System.ComponentModel.ISupportInitialize)picGround).EndInit();
        ResumeLayout(false);
    }

    #endregion

    private System.Windows.Forms.Timer gameTimer;
    private PictureBox picBird;
    private PictureBox picGround;
    private Label lblScore;
    private Label lblInfo;
    private Label lblMessage;
    private Button btnNewGame;
}
