namespace Subtitle;

partial class ControlArea
{
    /// <summary>
    ///  Required designer variable.
    /// </summary>
    private System.ComponentModel.IContainer components = null;

    /// <summary>
    ///  Clean up any resources being used.
    /// </summary>
    /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
    protected override void Dispose(bool disposing)
    {
        if (disposing && (components != null))
        {
            components.Dispose();
        }
        base.Dispose(disposing);
    }

    #region Windows Form Designer generated code

    /// <summary>
    ///  Required method for Designer support - do not modify
    ///  the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent()
    {
        Subtitle = new Label();
        SubtitleTime = new TextBox();
        SuspendLayout();
        // 
        // Subtitle
        // 
        Subtitle.AutoSize = true;
        Subtitle.BackColor = SystemColors.ControlDark;
        Subtitle.Font = new Font("Times New Roman", 27.75F, FontStyle.Regular, GraphicsUnit.Point);
        Subtitle.Location = new Point(0, 0);
        Subtitle.Name = "Subtitle";
        Subtitle.Size = new Size(143, 42);
        Subtitle.TabIndex = 0;
        Subtitle.Text = "Subtitles";
        Subtitle.TextAlign = ContentAlignment.MiddleCenter;
        // 
        // SubtitleTime
        // 
        SubtitleTime.Location = new Point(894, 151);
        SubtitleTime.Name = "SubtitleTime";
        SubtitleTime.Size = new Size(100, 23);
        SubtitleTime.TabIndex = 1;
        // 
        // ControlArea
        // 
        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        ClientSize = new Size(1006, 177);
        Controls.Add(SubtitleTime);
        Controls.Add(Subtitle);
        Name = "ControlArea";
        Text = "Form1";
        ResumeLayout(false);
        PerformLayout();
    }

    #endregion

    private Label Subtitle;
    private TextBox SubtitleTime;
}
