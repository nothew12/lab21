namespace MDITextEditor;

public class MainForm : Form
{
    
    private MenuStrip _menuStrip = null!;
    private ToolStripMenuItem _menuFile = null!, _menuEdit = null!, _menuFormat = null!,
        _menuWindow = null!, _menuLang = null!;
    private ToolStripMenuItem _miNew = null!, _miOpen = null!, _miSave = null!,
        _miSaveAs = null!, _miClose = null!, _miExit = null!;
    private ToolStripMenuItem _miCut = null!, _miCopy = null!, _miPaste = null!, _miSelectAll = null!;
    private ToolStripMenuItem _miFont = null!, _miAlign = null!,
        _miAlignLeft = null!, _miAlignCenter = null!, _miAlignRight = null!;
    private ToolStripMenuItem _miCascade = null!, _miTileH = null!, _miTileV = null!, _miCloseAll = null!;
    private ToolStripMenuItem _miLangUk = null!, _miLangEn = null!, _miLangPl = null!;

    private ToolStrip _toolBar = null!;
    private StatusStrip _statusBar = null!;
    private ToolStripStatusLabel _statusLabel = null!;

    private int _docCounter = 0;

    public MainForm()
    {
        IsMdiContainer = true;
        Size = new Size(1100, 700);
        StartPosition = FormStartPosition.CenterScreen;

        BuildMenu();
        BuildToolBar();
        BuildStatusBar();
        RefreshLanguage();
    }


    private void BuildMenu()
    {
        _menuStrip = new MenuStrip();

        _menuFile = new ToolStripMenuItem();
        _miNew      = Item(Keys.Control | Keys.N, (_, _) => NewDoc());
        _miOpen     = Item(Keys.Control | Keys.O, (_, _) => OpenDoc());
        _miSave     = Item(Keys.Control | Keys.S, (_, _) => ActiveChild()?.Save());
        _miSaveAs   = Item(Keys.None,             (_, _) => ActiveChild()?.SaveAs());
        _miClose    = Item(Keys.Control | Keys.W, (_, _) => ActiveMdiChild?.Close());
        _miExit     = Item(Keys.Alt | Keys.F4,    (_, _) => Close());

        _menuFile.DropDownItems.AddRange(new ToolStripItem[]
        {
            _miNew, _miOpen, new ToolStripSeparator(),
            _miSave, _miSaveAs, new ToolStripSeparator(),
            _miClose, new ToolStripSeparator(), _miExit
        });

        _menuEdit = new ToolStripMenuItem();
        _miCut       = Item(Keys.Control | Keys.X, (_, _) => ActiveChild()?.Editor.Cut());
        _miCopy      = Item(Keys.Control | Keys.C, (_, _) => ActiveChild()?.Editor.Copy());
        _miPaste     = Item(Keys.Control | Keys.V, (_, _) => ActiveChild()?.Editor.Paste());
        _miSelectAll = Item(Keys.Control | Keys.A, (_, _) => ActiveChild()?.Editor.SelectAll());

        _menuEdit.DropDownItems.AddRange(new ToolStripItem[]
            { _miCut, _miCopy, _miPaste, new ToolStripSeparator(), _miSelectAll });

        _menuFormat = new ToolStripMenuItem();
        _miFont  = Item(Keys.None, (_, _) => ActiveChild()?.ChangeFont());
        _miAlign = new ToolStripMenuItem();

        _miAlignLeft   = AlignItem(HorizontalAlignment.Left);
        _miAlignCenter = AlignItem(HorizontalAlignment.Center);
        _miAlignRight  = AlignItem(HorizontalAlignment.Right);

        _miAlign.DropDownItems.AddRange(new ToolStripItem[]
            { _miAlignLeft, _miAlignCenter, _miAlignRight });

        _menuFormat.DropDownItems.AddRange(new ToolStripItem[] { _miFont, _miAlign });

        _menuWindow = new ToolStripMenuItem();
        _miCascade  = Item(Keys.None, (_, _) => LayoutMdi(MdiLayout.Cascade));
        _miTileH    = Item(Keys.None, (_, _) => LayoutMdi(MdiLayout.TileHorizontal));
        _miTileV    = Item(Keys.None, (_, _) => LayoutMdi(MdiLayout.TileVertical));
        _miCloseAll = Item(Keys.None, (_, _) => CloseAll());

        _menuWindow.DropDownItems.AddRange(new ToolStripItem[]
            { _miCascade, _miTileH, _miTileV, new ToolStripSeparator(), _miCloseAll });
        _menuWindow.MdiWindowListItem = true;

        _menuLang = new ToolStripMenuItem();
        _miLangUk = LangItem(AppLanguage.Ukrainian, "🇺🇦 Українська");
        _miLangEn = LangItem(AppLanguage.English,   "🇬🇧 English");
        _miLangPl = LangItem(AppLanguage.Polish,    "🇵🇱 Polski");
        _menuLang.DropDownItems.AddRange(new ToolStripItem[] { _miLangUk, _miLangEn, _miLangPl });

        _menuStrip.Items.AddRange(new ToolStripItem[]
            { _menuFile, _menuEdit, _menuFormat, _menuWindow, _menuLang });

        MainMenuStrip = _menuStrip;
        Controls.Add(_menuStrip);
    }

    private static ToolStripMenuItem Item(Keys shortcut, EventHandler handler)
    {
        var mi = new ToolStripMenuItem { ShortcutKeys = shortcut };
        mi.Click += handler;
        return mi;
    }

    private ToolStripMenuItem AlignItem(HorizontalAlignment align)
    {
        var mi = new ToolStripMenuItem();
        mi.Click += (_, _) => ActiveChild()?.SetAlignment(align);
        return mi;
    }

    private ToolStripMenuItem LangItem(AppLanguage lang, string label)
    {
        var mi = new ToolStripMenuItem(label);
        mi.Click += (_, _) =>
        {
            LanguageManager.SetLanguage(lang);
            RefreshLanguage();
        };
        return mi;
    }


    private void BuildToolBar()
    {
        _toolBar = new ToolStrip { Dock = DockStyle.Top };

        ToolStripButton Btn(string text, string tip, Action action)
        {
            var b = new ToolStripButton(text) { ToolTipText = tip, DisplayStyle = ToolStripItemDisplayStyle.Text };
            b.Click += (_, _) => action();
            return b;
        }

        _toolBar.Items.AddRange(new ToolStripItem[]
        {
            Btn("📄", "New",  NewDoc),
            Btn("📂", "Open", OpenDoc),
            Btn("💾", "Save", () => ActiveChild()?.Save()),
            new ToolStripSeparator(),
            Btn("✂", "Cut",   () => ActiveChild()?.Editor.Cut()),
            Btn("📋", "Copy", () => ActiveChild()?.Editor.Copy()),
            Btn("📌", "Paste",() => ActiveChild()?.Editor.Paste()),
            new ToolStripSeparator(),
            Btn("◀", "Align Left",   () => ActiveChild()?.SetAlignment(HorizontalAlignment.Left)),
            Btn("●", "Align Center", () => ActiveChild()?.SetAlignment(HorizontalAlignment.Center)),
            Btn("▶", "Align Right",  () => ActiveChild()?.SetAlignment(HorizontalAlignment.Right)),
            new ToolStripSeparator(),
            Btn("🔤", "Font", () => ActiveChild()?.ChangeFont()),
        });

        Controls.Add(_toolBar);
    }


    private void BuildStatusBar()
    {
        _statusBar = new StatusStrip();
        _statusLabel = new ToolStripStatusLabel(LanguageManager.Get("status_ready"))
        {
            Spring = true,
            TextAlign = ContentAlignment.MiddleLeft
        };
        _statusBar.Items.Add(_statusLabel);
        Controls.Add(_statusBar);
    }


    public void RefreshLanguage()
    {
        Text = LanguageManager.Get("app_title");

        _menuFile.Text    = LanguageManager.Get("menu_file");
        _miNew.Text       = LanguageManager.Get("menu_new");
        _miOpen.Text      = LanguageManager.Get("menu_open");
        _miSave.Text      = LanguageManager.Get("menu_save");
        _miSaveAs.Text    = LanguageManager.Get("menu_saveas");
        _miClose.Text     = LanguageManager.Get("menu_close");
        _miExit.Text      = LanguageManager.Get("menu_exit");

        _menuEdit.Text    = LanguageManager.Get("menu_edit");
        _miCut.Text       = LanguageManager.Get("menu_cut");
        _miCopy.Text      = LanguageManager.Get("menu_copy");
        _miPaste.Text     = LanguageManager.Get("menu_paste");
        _miSelectAll.Text = LanguageManager.Get("menu_selectall");

        _menuFormat.Text      = LanguageManager.Get("menu_format");
        _miFont.Text          = LanguageManager.Get("menu_font");
        _miAlign.Text         = LanguageManager.Get("menu_align");
        _miAlignLeft.Text     = LanguageManager.Get("menu_align_left");
        _miAlignCenter.Text   = LanguageManager.Get("menu_align_center");
        _miAlignRight.Text    = LanguageManager.Get("menu_align_right");

        _menuWindow.Text  = LanguageManager.Get("menu_window");
        _miCascade.Text   = LanguageManager.Get("menu_cascade");
        _miTileH.Text     = LanguageManager.Get("menu_tile_h");
        _miTileV.Text     = LanguageManager.Get("menu_tile_v");
        _miCloseAll.Text  = LanguageManager.Get("menu_closeall");

        _menuLang.Text    = LanguageManager.Get("menu_lang");

        _statusLabel.Text = LanguageManager.Get("status_ready");
    }

    private ChildForm? ActiveChild() => ActiveMdiChild as ChildForm;

    private void NewDoc()
    {
        _docCounter++;
        var child = new ChildForm { MdiParent = this };
        child.Show();
    }

    private void OpenDoc()
    {
        using var dlg = new OpenFileDialog
        {
            Filter = LanguageManager.Get("filter_rtf"),
            FilterIndex = 1
        };
        if (dlg.ShowDialog() != DialogResult.OK) return;

        var child = new ChildForm { MdiParent = this };
        child.OpenFile(dlg.FileName);
        child.Show();
    }

    private void CloseAll()
    {
        foreach (Form child in MdiChildren)
            child.Close();
    }
}
