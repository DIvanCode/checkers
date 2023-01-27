using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace Checkers.UI;

public class MainMenuForm : Form
{
    private Button? PlayButton;
    private Button? ExitButton;

    private void InitializeForm()
    {
        var resources = new ComponentResourceManager(typeof(MainMenuForm));
            
        Icon = ((Icon)(resources.GetObject("$this.Icon"))!);
        Name = "MainMenuForm";
        Text = "DIvanCheckers";
            
        AutoScaleDimensions = new SizeF(18F, 31F);
        AutoScaleMode = AutoScaleMode.Font;
        AutoSize = true;
        StartPosition = FormStartPosition.CenterScreen;
        WindowState = FormWindowState.Maximized;
            
        BackColor = SystemColors.Window;
        Font = new Font("DejaVu Serif", 16.2F, FontStyle.Regular, GraphicsUnit.Point, 204);
            
        Controls.Add(ExitButton);
        Controls.Add(PlayButton);
    }

    private void InitializeGameSettingsButton()
    {
        PlayButton!.TabStop = false;
        PlayButton.FlatStyle = FlatStyle.Flat;
        PlayButton.FlatAppearance.BorderSize = 0;
            
        PlayButton.Name = "GameSettingsButton";
            
        PlayButton.Location = new Point(500, 200);
        PlayButton.Size = new Size(200, 50);
        PlayButton.TabIndex = 0;
            
        PlayButton.BackColor = SystemColors.Menu;
        PlayButton.Text = "Играть";
        PlayButton.Cursor = Cursors.Arrow;
            
        PlayButton.Click += GameSettingsButton_Click;
    }

    private void InitializeExitButton()
    {
        ExitButton!.TabStop = false;
        ExitButton.FlatStyle = FlatStyle.Flat;
        ExitButton.FlatAppearance.BorderSize = 0;
            
        ExitButton.Name = "ExitButton";
            
        ExitButton.Location = new Point(500, 300);
        ExitButton.Size = new Size(200, 50);
        ExitButton.TabIndex = 0;
            
        ExitButton.BackColor = SystemColors.Menu;
        ExitButton.Text = "Выйти";
        ExitButton.Cursor = Cursors.Arrow;
            
        ExitButton.Click += ExitButton_Click;
    }

    private void Initialize()
    {
        SuspendLayout();
            
        PlayButton = new Button();
        InitializeGameSettingsButton();
            
        ExitButton = new Button();
        InitializeExitButton();
            
        InitializeForm();
            
        ResumeLayout();
    }
        
    public MainMenuForm()
    {
        Initialize();
        Closing += MainMenuForm_FormClosing;
    }
        
    private static void MainMenuForm_FormClosing(object? sender, CancelEventArgs cancelEventArgs)
    {
        Application.Exit();
    }

    private void GameSettingsButton_Click(object? sender, EventArgs e)
    {
        Hide();
        (new GameSettingsForm()).Show();
    }

    private void ExitButton_Click(object? sender, EventArgs e)
    {
        Application.Exit();
    }
}