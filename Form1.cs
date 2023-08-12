using System.Globalization;
using System.Text.RegularExpressions;

namespace Subtitle;

public partial class ControlArea : Form
{
    private System.Threading.Timer? _timer;
    private List<Subtitle> _subtitleList = new();
    private const int WM_NCHITTEST = 0x84;
    private const int HTCLIENT = 0x1;
    private const int HTCAPTION = 0x2;
    private string _subTitleText = "";
    private float _currentTime = -1f;
    private Subtitle? _currentSubtitle;

    protected override void WndProc(ref Message message)
    {
        base.WndProc(ref message);

        if (message.Msg == WM_NCHITTEST && (int)message.Result == HTCLIENT)
            message.Result = (IntPtr)HTCAPTION;
    }

    public ControlArea()
    {
        BackColor = Color.DarkGray;
        TransparencyKey = Color.DarkGray;
        FormBorderStyle = FormBorderStyle.None;
        InitializeComponent();
        SubtitleTime.TextChanged += SubtitleTime_TextChanged;
        foreach (var file in new[] { "../../../subtitles.srt", "./subtitles.srt" })
        {
            if (Path.Exists(file)) _subTitleText = File.ReadAllText(file);
            break;
        }
        ReadSubtitles();
        SubtitleTime.BackColor = Color.DarkGray;
        Subtitle.BackColor = Color.Transparent;
        Subtitle.ForeColor = Color.Beige;
    }

    private void ReadSubtitles()
    {
        _subtitleList = new Regex("\n\n").Split(_subTitleText.Replace("\r", "")).Select(s =>
        {
            var split = s.Split("\n");
            var text = split.Where(t => !string.IsNullOrEmpty(t) && t.Any(x => char.IsLetter(x)));
            var dateString = split.FirstOrDefault(d => d.Contains("-->"));
            if (string.IsNullOrEmpty(dateString)) return new Subtitle();
            return new Subtitle()
            {
                Text = string.Join("\n", text),
                From = TimeOnly.ParseExact(dateString.Split("-->")[0].Trim(), "hh:mm:ss,fff", CultureInfo.InvariantCulture),
                To = TimeOnly.ParseExact(dateString.Split("-->")[1].Trim(), "hh:mm:ss,fff", CultureInfo.InvariantCulture),
            };
        }).Where(s => !string.IsNullOrEmpty(s.Text)).ToList();
    }

    protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
    {
        switch (keyData)
        {
            case Keys.L:
                var fileDialog = new OpenFileDialog();
                fileDialog.Filter = "Subtitle Files|*.srt";
                fileDialog.InitialDirectory = Directory.GetCurrentDirectory();
                var result = fileDialog.ShowDialog();
                if (result == DialogResult.OK)
                {
                    _subTitleText = File.ReadAllText(fileDialog.FileName);
                    ReadSubtitles();
                }
                return true;

            case Keys.OemMinus:
            case Keys.Subtract:
                _currentTime -= 0.1f;
                return true;

            case Keys.Add:
            case Keys.Oemplus:
                _currentTime += 0.1f;
                return true;

            case Keys.Right:
                Location = new Point(Location.X + 10, Location.Y);
                _currentTime += 0.1f;
                break;

            case Keys.Left:
                Location = new Point(Location.X - 10, Location.Y);
                break;

            case Keys.Up:
                Location = new Point(Location.X, Location.Y - 10);
                break;

            case Keys.Down:
                Location = new Point(Location.X, Location.Y + 10);
                break;
        }
        return base.ProcessCmdKey(ref msg, keyData);
    }

    private void ControlArea_KeyDown(object? sender, KeyEventArgs e)
    {
    }

    private void SubtitleTime_TextChanged(object? sender, EventArgs e)
    {
        _timer?.Dispose();
        _currentTime = GetTime();
        _timer = new System.Threading.Timer(_ => IncrementTimer(), default, 0, 100);
    }

    private int GetTime()
    {
        if (!int.TryParse(SubtitleTime.Text, out var seconds)) return -1;
        return seconds;
    }

    private void IncrementTimer()
    {
        _currentTime += 0.1f;
        var now = new TimeOnly().Add(TimeSpan.FromSeconds(_currentTime));
        var newSubtitle = _subtitleList.Find(s => s.To >= now && s.From <= now);
        if (newSubtitle != null) _currentSubtitle = newSubtitle;
        if (_currentSubtitle != null &&
            (_currentSubtitle.To < now.Add(TimeSpan.FromSeconds(-1)) || _currentSubtitle.From > now.Add(TimeSpan.FromSeconds(1))))
        {
            _currentSubtitle = null;
        }
        Subtitle.Invoke(() => Subtitle.Text = _currentSubtitle?.Text ?? "");
    }

    protected override void OnPaintBackground(PaintEventArgs e)
    {
        e.Graphics.FillRectangle(Brushes.DarkGray, e.ClipRectangle);
        base.OnPaintBackground(e);
    }
}