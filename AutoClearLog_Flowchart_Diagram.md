# AutoClearLog - Flowchart Diagram

## 📊 Main Process Flow

```mermaid
flowchart TD
    Start([🎯 START]) --> ReadPath["📖 อ่านไฟล์ path.txt"]
    ReadPath --> CheckPath{path.txt<br/>มีอยู่?}
    
    CheckPath -->|❌ NO| ErrorPath["❌ Error: path.txt not found"]
    ErrorPath --> End1([🛑 END])
    
    CheckPath -->|✅ YES| ReadFolders["📖 อ่านเส้นทาง folder"]
    ReadFolders --> LoopFolder["🔄 วนลูปสำหรับแต่ละโฟลเดอร์"]
    
    LoopFolder --> CheckFolder{โฟลเดอร์<br/>มีอยู่?}
    CheckFolder -->|❌ NO| SkipFolder["⏭️ Skip folder"]
    SkipFolder --> MoreFolders{โฟลเดอร์<br/>เหลือ?}
    
    CheckFolder -->|✅ YES| CreateZip["📁 สร้าง Zipfile folder"]
    CreateZip --> FindLogs["🔍 ค้นหาไฟล์ .log"]
    FindLogs --> LoopLog["🔄 วนลูปสำหรับแต่ละไฟล์"]
    
    LoopLog --> CheckExt{นามสกุล<br/>.log?}
    CheckExt -->|❌ NO| SkipExt["⏭️ Skip file"]
    SkipExt --> MoreLogs{ไฟล์<br/>เหลือ?}
    
    CheckExt -->|✅ YES| ExtractDate["📅 ดึงวันที่จากชื่อไฟล์"]
    ExtractDate --> CheckDate{วันที่<br/>ถูก?}
    CheckDate -->|❌ NO| SkipDate["⏭️ Skip file"]
    SkipDate --> MoreLogs
    
    CheckDate -->|✅ YES| CompareDate{ไฟล์เก่ากว่า<br/>daysThreshold?}
    CompareDate -->|❌ NO| SkipOld["⏭️ Skip file"]
    SkipOld --> MoreLogs
    
    CompareDate -->|✅ YES| AddZip["📦 เพิ่มลงใน ZIP"]
    AddZip --> DeleteFile["🗑️ ลบไฟล์ต้นฉบับ"]
    DeleteFile --> MoreLogs{ไฟล์<br/>เหลือ?}
    
    MoreLogs -->|YES| LoopLog
    MoreLogs -->|NO| MoreFolders{โฟลเดอร์<br/>เหลือ?}
    
    MoreFolders -->|YES| LoopFolder
    MoreFolders -->|NO| WriteLog["📝 บันทึกลง log file"]
    WriteLog --> CloseLogger["🔐 ปิด Logger"]
    CloseLogger --> End2([✅ END])
```

---

## 🔀 Decision Tree

```mermaid
graph TD
    A["🎯 START:<br/>Read path.txt"] --> B{File<br/>exists?}
    
    B -->|NO| C["❌ Error:<br/>File not found"]
    C --> END1["🛑 END"]
    
    B -->|YES| D["📂 Loop:<br/>For each folder"]
    
    D --> E{Folder<br/>exists?}
    E -->|NO| F["⏭️ Skip:<br/>Folder not found"]
    
    E -->|YES| G["📁 Create:<br/>Zipfile folder"]
    G --> H["🔍 Find:<br/>*.log files"]
    
    H --> I["📄 Loop:<br/>For each .log file"]
    
    I --> J{Is .log<br/>file?}
    J -->|NO| K["⏭️ Skip:<br/>Wrong extension"]
    
    J -->|YES| L["📅 Extract:<br/>Date from filename"]
    
    L --> M{Date<br/>valid?}
    M -->|NO| N["⏭️ Skip:<br/>Invalid format"]
    
    M -->|YES| O{File older<br/>than threshold?}
    
    O -->|NO| P["⏭️ Skip:<br/>File too new"]
    
    O -->|YES| Q["📦 Compress:<br/>Add to ZIP"]
    Q --> R["🗑️ Delete:<br/>Original file"]
    
    K --> S{More<br/>files?}
    N --> S
    P --> S
    R --> S
    
    S -->|YES| I
    S -->|NO| T{More<br/>folders?}
    
    F --> T
    T -->|YES| D
    
    T -->|NO| U["📝 Log:<br/>Write summary"]
    U --> V["🔐 Close:<br/>Logger"]
    V --> END2["✅ END"]
```

---

## ⚙️ Processing Steps

```mermaid
sequenceDiagram
    participant User as 👤 User
    participant App as 🖥️ AutoClearLog
    participant FileSystem as 📁 File System
    participant Logger as 📝 Logger
    
    User->>App: Execute AutoClearLog.exe
    
    App->>FileSystem: Read path.txt
    FileSystem-->>App: List of folders
    
    loop For each folder
        App->>FileSystem: Check folder exists
        FileSystem-->>App: Folder status
        
        App->>FileSystem: Create Zipfile folder
        FileSystem-->>App: Folder created
        
        App->>FileSystem: Find *.log files
        FileSystem-->>App: List of .log files
        
        loop For each .log file
            App->>App: Extract date from filename
            App->>App: Check if date is valid
            App->>App: Compare with cutoff date
            
            alt File is old enough
                App->>FileSystem: Add file to ZIP
                FileSystem-->>App: File added
                
                App->>Logger: Log: File compressed
                Logger-->>App: Logged
                
                App->>FileSystem: Delete original file
                FileSystem-->>App: File deleted
                
                App->>Logger: Log: File deleted
                Logger-->>App: Logged
            else File is too new
                App->>Logger: Log: File skipped
                Logger-->>App: Logged
            end
        end
    end
    
    App->>Logger: Write summary
    Logger-->>App: Summary written
    
    App->>Logger: Close logger
    Logger-->>App: Logger closed
    
    App->>User: Process complete
```

---

## 📊 State Diagram

```mermaid
stateDiagram-v2
    [*] --> ReadConfiguration
    
    ReadConfiguration --> CheckPathFile
    CheckPathFile --> PathFound: File exists
    CheckPathFile --> ErrorExit: File not found
    
    ErrorExit --> [*]
    
    PathFound --> ProcessFolders
    ProcessFolders --> CheckFolder
    
    CheckFolder --> FolderExists: Yes
    CheckFolder --> SkipFolder: No
    
    SkipFolder --> CheckMoreFolders
    FolderExists --> FindLogFiles
    
    FindLogFiles --> ProcessLogFiles
    ProcessLogFiles --> CheckLogFile
    
    CheckLogFile --> ValidateFormat: Is .log
    CheckLogFile --> SkipLog: Not .log
    
    SkipLog --> CheckMoreLogsA
    ValidateFormat --> CheckDateFormat
    
    CheckDateFormat --> ValidDate: Parse OK
    CheckDateFormat --> SkipLog: Parse error
    
    ValidDate --> CompareDates
    CompareDates --> IsOld: Older than threshold
    CompareDates --> SkipLog: Too recent
    
    IsOld --> CompressFile
    CompressFile --> DeleteFile
    DeleteFile --> LogAction
    
    LogAction --> CheckMoreLogsA: More files?
    CheckMoreLogsA --> ProcessLogFiles: Yes
    CheckMoreLogsA --> CheckMoreFolders: No
    
    CheckMoreFolders --> ProcessFolders: Yes
    CheckMoreFolders --> WriteSummary: No
    
    WriteSummary --> CloseLogger
    CloseLogger --> [*]
```

---

## 🎯 Process Summary Map

```mermaid
mindmap
  root((🎯 AutoClearLog))
    INITIALIZATION
      Read config
      Load path.txt
      Set daysThreshold
    FOLDER PROCESSING
      Loop folders
      Validate paths
      Create Zipfile
    FILE DISCOVERY
      Find .log files
      Extract filename
      Parse date
    VALIDATION
      Check format
      Verify date
      Compare age
    COMPRESSION
      Add to ZIP
      Create archive
      Handle existing
    CLEANUP
      Delete original
      Update paths
      Log actions
    ERROR HANDLING
      Catch exceptions
      Write to log
      Continue process
    SUMMARY
      Count files
      Report results
      Close logger
```

---

## 📈 Data Flow Diagram

```mermaid
graph LR
    Input["📥 Input:<br/>path.txt"] --> Parser["🔧 Parser:<br/>Read paths"]
    
    Parser --> FolderList["📋 Folder List"]
    FolderList --> Validator["✅ Validator:<br/>Check exists"]
    
    Validator --> ValidFolders["📂 Valid Folders"]
    Validator --> InvalidFolders["❌ Invalid Folders"]
    
    InvalidFolders --> SkipLog1["⏭️ Skipped"]
    
    ValidFolders --> Scanner["🔍 Scanner:<br/>Find .log files"]
    
    Scanner --> LogFiles["📄 Log Files"]
    LogFiles --> Analyzer["🔎 Analyzer:<br/>Extract date"]
    
    Analyzer --> ValidLogs["✅ Valid Logs"]
    Analyzer --> InvalidLogs["❌ Invalid Logs"]
    
    InvalidLogs --> SkipLog2["⏭️ Skipped"]
    
    ValidLogs --> Filter["🎯 Filter:<br/>Compare dates"]
    
    Filter --> OldLogs["🗂️ Old Logs"]
    Filter --> NewLogs["📌 New Logs"]
    
    NewLogs --> SkipLog3["⏭️ Skipped"]
    
    OldLogs --> Compressor["📦 Compressor:<br/>Create ZIP"]
    
    Compressor --> ZipFiles["📦 ZIP Files"]
    
    ZipFiles --> Deleter["🗑️ Deleter:<br/>Remove original"]
    
    Deleter --> Output["📤 Output:<br/>Zipfile/"]
    
    SkipLog1 --> Logger["📝 Logger"]
    SkipLog2 --> Logger
    SkipLog3 --> Logger
    Deleter --> Logger
    
    Logger --> LogFile["📊 AutoClearLog.log"]
```

---

## ✅ Validation Checklist

```mermaid
graph TD
    A["🎯 Start Validation"] --> B{path.txt<br/>exists?}
    B -->|NO| B1["❌ Create path.txt first"]
    B1 --> END1["⚠️ STOP"]
    
    B -->|YES| C{Folder paths<br/>valid?}
    C -->|NO| C1["❌ Check folder paths"]
    C1 --> END2["⚠️ STOP"]
    
    C -->|YES| D{.log files<br/>found?}
    D -->|NO| D1["⚠️ No .log files"]
    D1 --> END3["✅ OK"]
    
    D -->|YES| E{Filename format<br/>correct?}
    E -->|NO| E1["❌ Rename to EVMSWSS-YYYY-MM-DD.log"]
    E1 --> END4["⚠️ STOP"]
    
    E -->|YES| F{daysThreshold<br/>set?}
    F -->|NO| F1["❌ Set in Program.cs"]
    F1 --> END5["⚠️ STOP"]
    
    F -->|YES| G["✓ All checks passed!"]
    G --> H["🚀 Ready to run"]
    H --> END6["✅ START"]
```

---

## 🔄 Error Handling Flow

```mermaid
flowchart TD
    Error["⚠️ Exception Caught"] --> ErrorType{Error<br/>Type?}
    
    ErrorType -->|File Not Found| E1["❌ path.txt not found"]
    ErrorType -->|Invalid Path| E2["❌ Folder path invalid"]
    ErrorType -->|No Permission| E3["❌ Access denied"]
    ErrorType -->|Parse Error| E4["❌ Invalid filename format"]
    ErrorType -->|ZIP Error| E5["❌ Cannot create ZIP"]
    ErrorType -->|Delete Error| E6["❌ Cannot delete file"]
    
    E1 --> LogError["📝 Write to log"]
    E2 --> LogError
    E3 --> LogError
    E4 --> LogError
    E5 --> LogError
    E6 --> LogError
    
    LogError --> Continue{Continue<br/>process?}
    
    Continue -->|YES| NextFile["➡️ Process next file"]
    Continue -->|NO| Exit["🛑 Exit with error"]
    
    NextFile --> End["✅ Complete"]
    Exit --> End
```

---

**💡 Tip:** These diagrams can be viewed directly in VS Code with the Mermaid extension or on GitHub!
