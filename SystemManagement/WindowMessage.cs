namespace SystemManagement;

[System.Diagnostics.CodeAnalysis.SuppressMessage("Naming", "CA1720:Identifier contains type name", Justification = "<Pending>")]
[System.Diagnostics.CodeAnalysis.SuppressMessage("CodeQuality", "IDE0079:Remove unnecessary suppression", Justification = "<Pending>")]
[System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1069:Enums values should not be duplicated", Justification = "<Pending>")]
public enum WindowMessage : uint
{
    Null = 0x0000,
    Create = 0x0001,
    Destroy = 0x0002,
    Move = 0x0003,
    Size = 0x0005,
    Activate = 0x0006,
    SetFocus = 0x0007,
    KillFocus = 0x0008,
    Enable = 0x000A,
    SetRedraw = 0x000B,
    SetText = 0x000C,
    GetText = 0x000D,
    GetTextLength = 0x000E,
    Paint = 0x000F,
    Close = 0x0010,

    QueryEndSession = 0x0011,
    EndSession = 0x0016,

    Quit = 0x0012,
    EraseBackground = 0x0014,
    SysColorChange = 0x0015,

    ShowWindow = 0x0018,
    WinIniChange = 0x001A,
    SettingChange = 0x001A,

    DevModeChange = 0x001B,
    ActivateApp = 0x001C,
    FontChange = 0x001D,
    TimeChange = 0x001E,

    CancelMode = 0x001F,
    SetCursor = 0x0020,
    MouseActivate = 0x0021,
    ChildActivate = 0x0022,
    QueueSync = 0x0023,

    GetMinMaxInfo = 0x0024,

    PaintIcon = 0x0026,
    IconEraseBackground = 0x0027,
    NextDlgCtl = 0x0028,

    SpoolerStatus = 0x002A,
    DrawItem = 0x002B,
    MeasureItem = 0x002C,
    DeleteItem = 0x002D,
    VKeyToItem = 0x002E,
    CharToItem = 0x002F,

    SetFont = 0x0030,
    GetFont = 0x0031,
    SetHotkey = 0x0032,
    GetHotkey = 0x0033,

    QueryDragIcon = 0x0037,

    CompareItem = 0x0039,

    WindowPosChanging = 0x0046,
    WindowPosChanged = 0x0047,

    CopyData = 0x004A,
    CancelJournal = 0x004B,

    Notify = 0x004E,

    InputLangChangeRequest = 0x0050,
    InputLangChange = 0x0051,
    TCard = 0x0052,
    Help = 0x0053,
    UserChanged = 0x0054,

    NotifyFormat = 0x0055,

    ContextMenu = 0x007B,
    StyleChanging = 0x007C,
    StyleChanged = 0x007D,
    DisplayChange = 0x007E,
    GetIcon = 0x007F,
    SetIcon = 0x0080,

    NcCreate = 0x0081,
    NcDestroy = 0x0082,
    NcCalcSize = 0x0083,
    NcHitTest = 0x0084,
    NcPaint = 0x0085,
    NcActivate = 0x0086,
    GetDlgCode = 0x0087,
    NcMouseMove = 0x00A0,
    NcLButtonDown = 0x00A1,
    NcLButtonUp = 0x00A2,
    NcLButtonDblClk = 0x00A3,
    NcRButtonDown = 0x00A4,
    NcRButtonUp = 0x00A5,
    NcRButtonDblClk = 0x00A6,
    NcMButtonDown = 0x00A7,
    NcMButtonUp = 0x00A8,
    NcMButtonDblClk = 0x00A9,

    KeyDown = 0x0100,
    KeyUp = 0x0101,
    Char = 0x0102,
    DeadChar = 0x0103,
    SysKeyDown = 0x0104,
    SysKeyUp = 0x0105,
    SysChar = 0x0106,
    SysDeadChar = 0x0107,

    Command = 0x0111,
    SysCommand = 0x0112,

    Timer = 0x0113,

    HScroll = 0x0114,
    VScroll = 0x0115,

    InitMenu = 0x0116,
    InitMenuPopup = 0x0117,

    MenuSelect = 0x011F,
    MenuChar = 0x0120,
    EnterIdle = 0x0121,

    HotKey = 0x0312,

    Power = 0x0048,
    PowerBroadcast = 0x0218,
    DeviceChange = 0x0219,

    DdeInitiate = 0x03E0,
    DdeTerminate = 0x03E1,
    DdeAdvise = 0x03E2,
    DdeUnadvise = 0x03E3,
    DdeAck = 0x03E4,
    DdeData = 0x03E5,
    DdeRequest = 0x03E6,
    DdePoke = 0x03E7,
    DdeExecute = 0x03E8,
}
