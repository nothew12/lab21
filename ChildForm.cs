using System.Text;

namespace MDITextEditor;

public class ChildForm : Form
{
    public RichTextBox Editor { get; private set; }
    private string _filePath = "";
    private bool _isModified = false;
    private static int _counter = 0;

    public string FilePath => _filePath;
    public bool IsModified => _isModified;

    public ChildForm()
    {
        _counter++;
        Text = $"{LanguageManager.Get("new_doc")} {_counter}";

        Editor = new RichTextBox
        {
            Dock = DockStyle.Fill,
            Font = new Font("Segoe UI", 12F),
            AcceptsTab = true,
            ScrollBars = RichTextBoxScrollBars.Both,
            WordWrap = true
        };

        Editor.TextChanged += (s, e) =>
        {
            if (!_isModified)
            {
                _isModified = true;
                UpdateTitle();
            }
        };

        Controls.Add(Editor);

        Size = new Size(600, 450);
    }

    private void UpdateTitle()
    {
        string name = string.IsNullOrEmpty(_filePath)
            ? $"{LanguageManager.Get("new_doc")} {_counter}"
            : Path.GetFileName(_filePath);
        Text = _isModified ? name + " *" : name;
    }

    public void NewFile()
    {
        _filePath = "";
        _isModified = false;
        Editor.Clear();
        UpdateTitle();
    }

    public void OpenFile(string path)
    {
        _filePath = path;
        if (path.EndsWith(".rtf", StringComparison.OrdinalIgnoreCase))
            Editor.LoadFile(path, RichTextBoxStreamType.RichText);
        else
            Editor.Text = File.ReadAllText(path, Encoding.UTF8);

        _isModified = false;
        UpdateTitle();
    }

    public bool Save()
    {
        if (string.IsNullOrEmpty(_filePath))
            return SaveAs();
        WriteFile(_filePath);
        return true;
    }

    public bool SaveAs()
    {
        using var dlg = new SaveFileDialog
        {
            Filter = LanguageManager.Get("filter_rtf"),
            FilterIndex = 1,
            FileName = string.IsNullOrEmpty(_filePath) ? "" : Path.GetFileName(_filePath)
        };
        if (dlg.ShowDialog() != DialogResult.OK) return false;
        _filePath = dlg.FileName;
        WriteFile(_filePath);
        return true;
    }

    private void WriteFile(string path)
    {
        if (path.EndsWith(".rtf", StringComparison.OrdinalIgnoreCase))
            Editor.SaveFile(path, RichTextBoxStreamType.RichText);
        else
            File.WriteAllText(path, Editor.Text, Encoding.UTF8);

        _isModified = false;
        UpdateTitle();
    }

    public bool PromptSaveIfModified()
    {
        if (!_isModified) return true;
        string name = string.IsNullOrEmpty(_filePath)
            ? Text.TrimEnd('*', ' ')
            : Path.GetFileName(_filePath);

        var result = MessageBox.Show(
            LanguageManager.Get("save_changes") + name + "?",
            Text,
            MessageBoxButtons.YesNoCancel,
            MessageBoxIcon.Question);

        if (result == DialogResult.Yes) return Save();
        if (result == DialogResult.No) return true;
        return false;
    }

    protected override void OnFormClosing(FormClosingEventArgs e)
    {
        if (!PromptSaveIfModified())
            e.Cancel = true;
        base.OnFormClosing(e);
    }

    public void SetAlignment(HorizontalAlignment align)
    {
        Editor.SelectionAlignment = align switch
        {
            HorizontalAlignment.Left => HorizontalAlignment.Left,
            HorizontalAlignment.Center => HorizontalAlignment.Center,
            HorizontalAlignment.Right => HorizontalAlignment.Right,
            _ => HorizontalAlignment.Left
        };
    }

    public void ChangeFont()
    {
        using var dlg = new FontDialog
        {
            Font = Editor.SelectionFont ?? Editor.Font,
            ShowColor = true,
            Color = Editor.SelectionColor
        };
        if (dlg.ShowDialog() == DialogResult.OK)
        {
            Editor.SelectionFont = dlg.Font;
            Editor.SelectionColor = dlg.Color;
        }
    }
}
