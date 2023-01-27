using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace Checkers.UI;

public class GameSettingsForm : Form
{   
    private Label Player1Label = new();
    private Label Player2Label = new();
        
    private TextBox Player1NicknameTextBox = new();
    private TextBox Player2NicknameTextBox = new();
        
    private Button PlayButton = new();
        
    private void InitializeForm()
    {
        var resources = new ComponentResourceManager(typeof(GameSettingsForm));
            
        Icon = ((Icon)(resources.GetObject("$this.Icon"))!);
        Name = "GameSettingsForm";
        Text = "DIvanCheckers";
            
        AutoScaleDimensions = new SizeF(18F, 31F);
        AutoScaleMode = AutoScaleMode.Font;
        AutoSize = true;
        StartPosition = FormStartPosition.CenterScreen;
        WindowState = FormWindowState.Maximized;
            
        BackColor = SystemColors.Window;
        Font = new Font("DejaVu Serif", 16.2F, FontStyle.Regular, GraphicsUnit.Point, 204);
            
        Controls.Add(Player1Label);
        Controls.Add(Player2Label);
        Controls.Add(Player1NicknameTextBox);
        Controls.Add(Player2NicknameTextBox);
        Controls.Add(PlayButton);
    }

    private void InitializePlayer1Label()
    {
        Player1Label.Name = "Player1Label";
            
        Player1Label.Location = new Point(50, 100);
        Player1Label.Size = new Size(200, 40);
        Player1Label.TabIndex = 0;
            
        Player1Label.BackColor = SystemColors.Window;
        Player1Label.Text = "Первый игрок";
    }
        
    private void InitializePlayer2Label()
    {
        Player2Label.Name = "Player2Label";
            
        Player2Label.Location = new Point(700, 100);
        Player2Label.Size = new Size(200, 40);
        Player2Label.TabIndex = 0;
            
        Player2Label.BackColor = SystemColors.Window;
        Player2Label.Text = "Второй игрок";
    }

    private void InitializePlayer1NicknameTextBox()
    {
        Player1NicknameTextBox.Name = "Player1NicknameTextBox";
            
        Player1NicknameTextBox.Location = new Point(250, 100);
        Player1NicknameTextBox.Size = new Size(200, 40);
        Player1NicknameTextBox.TabIndex = 0;
    }
        
    private void InitializePlayer2NicknameTextBox()
    {
        Player2NicknameTextBox.Name = "Player2NicknameTextBox";
            
        Player2NicknameTextBox.Location = new Point(900, 100);
        Player2NicknameTextBox.Size = new Size(200, 40);
        Player2NicknameTextBox.TabIndex = 0;
    }

    private void InitializePlayButton()
    {
        PlayButton.Name = "PlayButton";
            
        PlayButton.Location = new Point(500, 300);
        PlayButton.Size = new Size(200, 50);
        PlayButton.TabIndex = 0;
            
        PlayButton.BackColor = SystemColors.Menu;
        PlayButton.Text = "Играть";
        PlayButton.Cursor = Cursors.Arrow;
            
        PlayButton.Click += PlayButton_Click;
    }
        
    private void Initialize()
    {
        SuspendLayout();
            
        InitializePlayer1Label();
            
        InitializePlayer2Label();
            
        InitializePlayer1NicknameTextBox();
            
        InitializePlayer2NicknameTextBox();
            
        InitializePlayButton();
            
        InitializeForm();
            
        ResumeLayout();
    }
        
    public GameSettingsForm()
    {
        Initialize();
        Closing += GameSettingsForm_FormClosing;
    }
        
    private static void GameSettingsForm_FormClosing(object? sender, CancelEventArgs cancelEventArgs)
    {
        Application.Exit();
    }

    private void PlayButton_Click(object? sender, EventArgs e)
    {
        Hide();
        (new GameForm(Player1NicknameTextBox.Text, Player2NicknameTextBox.Text)).Show();
    }
}