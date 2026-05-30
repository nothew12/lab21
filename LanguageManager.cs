namespace MDITextEditor;

public enum AppLanguage { Ukrainian, English, Polish }

public static class LanguageManager
{
    public static AppLanguage Current { get; private set; } = AppLanguage.Ukrainian;

    private static readonly Dictionary<string, Dictionary<AppLanguage, string>> Strings = new()
    {
        ["menu_file"]         = new() { [AppLanguage.Ukrainian]="Файл",      [AppLanguage.English]="File",     [AppLanguage.Polish]="Plik" },
        ["menu_new"]          = new() { [AppLanguage.Ukrainian]="Новий",     [AppLanguage.English]="New",      [AppLanguage.Polish]="Nowy" },
        ["menu_open"]         = new() { [AppLanguage.Ukrainian]="Відкрити",  [AppLanguage.English]="Open",     [AppLanguage.Polish]="Otwórz" },
        ["menu_save"]         = new() { [AppLanguage.Ukrainian]="Зберегти",  [AppLanguage.English]="Save",     [AppLanguage.Polish]="Zapisz" },
        ["menu_saveas"]       = new() { [AppLanguage.Ukrainian]="Зберегти як...", [AppLanguage.English]="Save As...", [AppLanguage.Polish]="Zapisz jako..." },
        ["menu_close"]        = new() { [AppLanguage.Ukrainian]="Закрити",   [AppLanguage.English]="Close",    [AppLanguage.Polish]="Zamknij" },
        ["menu_exit"]         = new() { [AppLanguage.Ukrainian]="Вихід",     [AppLanguage.English]="Exit",     [AppLanguage.Polish]="Wyjście" },
        ["menu_edit"]         = new() { [AppLanguage.Ukrainian]="Правка",    [AppLanguage.English]="Edit",     [AppLanguage.Polish]="Edycja" },
        ["menu_cut"]          = new() { [AppLanguage.Ukrainian]="Вирізати",  [AppLanguage.English]="Cut",      [AppLanguage.Polish]="Wytnij" },
        ["menu_copy"]         = new() { [AppLanguage.Ukrainian]="Копіювати", [AppLanguage.English]="Copy",     [AppLanguage.Polish]="Kopiuj" },
        ["menu_paste"]        = new() { [AppLanguage.Ukrainian]="Вставити",  [AppLanguage.English]="Paste",    [AppLanguage.Polish]="Wklej" },
        ["menu_selectall"]    = new() { [AppLanguage.Ukrainian]="Виділити все", [AppLanguage.English]="Select All", [AppLanguage.Polish]="Zaznacz wszystko" },
        ["menu_format"]       = new() { [AppLanguage.Ukrainian]="Формат",    [AppLanguage.English]="Format",   [AppLanguage.Polish]="Format" },
        ["menu_font"]         = new() { [AppLanguage.Ukrainian]="Шрифт...",  [AppLanguage.English]="Font...",  [AppLanguage.Polish]="Czcionka..." },
        ["menu_align"]        = new() { [AppLanguage.Ukrainian]="Вирівнювання", [AppLanguage.English]="Alignment", [AppLanguage.Polish]="Wyrównanie" },
        ["menu_align_left"]   = new() { [AppLanguage.Ukrainian]="По лівому краю",  [AppLanguage.English]="Left",   [AppLanguage.Polish]="Do lewej" },
        ["menu_align_center"] = new() { [AppLanguage.Ukrainian]="По центру",        [AppLanguage.English]="Center", [AppLanguage.Polish]="Środek" },
        ["menu_align_right"]  = new() { [AppLanguage.Ukrainian]="По правому краю", [AppLanguage.English]="Right",  [AppLanguage.Polish]="Do prawej" },
        ["menu_window"]       = new() { [AppLanguage.Ukrainian]="Вікно",     [AppLanguage.English]="Window",   [AppLanguage.Polish]="Okno" },
        ["menu_cascade"]      = new() { [AppLanguage.Ukrainian]="Каскад",    [AppLanguage.English]="Cascade",  [AppLanguage.Polish]="Kaskada" },
        ["menu_tile_h"]       = new() { [AppLanguage.Ukrainian]="Мозаїка горизонтально", [AppLanguage.English]="Tile Horizontal", [AppLanguage.Polish]="Kafelki poziomo" },
        ["menu_tile_v"]       = new() { [AppLanguage.Ukrainian]="Мозаїка вертикально",   [AppLanguage.English]="Tile Vertical",   [AppLanguage.Polish]="Kafelki pionowo" },
        ["menu_closeall"]     = new() { [AppLanguage.Ukrainian]="Закрити всі", [AppLanguage.English]="Close All", [AppLanguage.Polish]="Zamknij wszystkie" },
        ["menu_lang"]         = new() { [AppLanguage.Ukrainian]="Мова",      [AppLanguage.English]="Language", [AppLanguage.Polish]="Język" },
        ["new_doc"]           = new() { [AppLanguage.Ukrainian]="Новий документ", [AppLanguage.English]="New Document", [AppLanguage.Polish]="Nowy dokument" },
        ["save_changes"]      = new() { [AppLanguage.Ukrainian]="Зберегти зміни у ", [AppLanguage.English]="Save changes to ", [AppLanguage.Polish]="Zapisz zmiany w " },
        ["yes"]               = new() { [AppLanguage.Ukrainian]="Так",  [AppLanguage.English]="Yes",  [AppLanguage.Polish]="Tak" },
        ["no"]                = new() { [AppLanguage.Ukrainian]="Ні",   [AppLanguage.English]="No",   [AppLanguage.Polish]="Nie" },
        ["cancel"]            = new() { [AppLanguage.Ukrainian]="Скасувати", [AppLanguage.English]="Cancel", [AppLanguage.Polish]="Anuluj" },
        ["status_ready"]      = new() { [AppLanguage.Ukrainian]="Готово", [AppLanguage.English]="Ready", [AppLanguage.Polish]="Gotowy" },
        ["status_modified"]   = new() { [AppLanguage.Ukrainian]="Змінено", [AppLanguage.English]="Modified", [AppLanguage.Polish]="Zmieniono" },
        ["filter_rtf"]        = new() { [AppLanguage.Ukrainian]="RTF файли (*.rtf)|*.rtf|Текстові файли (*.txt)|*.txt|Всі файли (*.*)|*.*",
                                        [AppLanguage.English]="RTF files (*.rtf)|*.rtf|Text files (*.txt)|*.txt|All files (*.*)|*.*",
                                        [AppLanguage.Polish]="Pliki RTF (*.rtf)|*.rtf|Pliki tekstowe (*.txt)|*.txt|Wszystkie pliki (*.*)|*.*" },
        ["app_title"]         = new() { [AppLanguage.Ukrainian]="MDI Текстовий Редактор", [AppLanguage.English]="MDI Text Editor", [AppLanguage.Polish]="Edytor tekstu MDI" },
    };

    public static string Get(string key)
    {
        if (Strings.TryGetValue(key, out var dict) && dict.TryGetValue(Current, out var val))
            return val;
        return key;
    }

    public static void SetLanguage(AppLanguage lang) => Current = lang;
}
