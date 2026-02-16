# 🗑️ AutoClearLog

## Overview

**AutoClearLog** is a Windows Console Application built with .NET Framework 4.8 and C#. It automatically compresses old log files and saves disk space by archiving logs that exceed a specified age threshold.

## ✨ Features

- 📦 **Automatic Log Compression** - Compress old log files into ZIP archives
- 📅 **Date-Based Filtering** - Filter logs based on age (configurable days threshold)
- 🗑️ **Automatic Cleanup** - Delete original log files after compression
- 📝 **Comprehensive Logging** - Track all operations with NLog
- 🔄 **Batch Processing** - Process multiple folders in one run
- 💾 **Efficient Storage** - Reduce disk space usage significantly
- ⚙️ **Flexible Configuration** - Easy to customize paths and settings

## 📋 Requirements

| Item | Requirement |
|------|------------|
| **OS** | Windows 7 or later / Windows Server 2008 R2 or later |
| **.NET Framework** | .NET Framework 4.8 or later |
| **Memory** | Minimum 512 MB |
| **Permissions** | Read/Write access to log folder location |

## 🚀 Quick Start

### Option 1: Direct Execution
```bash
cd D:\เอกสาร\Dev\AutoClearLog\bin\Release
AutoClearLog.exe
```

### Option 2: Batch File
Create `RunAutoClearLog.bat`:
```batch
@echo off
cd /d "D:\เอกสาร\Dev\AutoClearLog\bin\Release"
AutoClearLog.exe
pause
```

### Option 3: Task Scheduler (Recommended)
1. Open Task Scheduler
2. Create Basic Task
3. Set trigger: Daily at 02:00 AM
4. Set action: Run AutoClearLog.exe
5. Configure to run with elevated privileges if needed

## ⚙️ Configuration

### 1. Create path.txt
Create a file at: `D:\My project\Bot\logtest\path.txt`

Content (one folder path per line):
```
D:\My project\Bot\logtest\log\folder1
D:\My project\Bot\logtest\log\folder2
C:\Logs\ApplicationLogs
```

### 2. Set Days Threshold
Edit `Program.cs` (line 26):
```csharp
int daysThreshold = 30; // Change to desired number of days
```

Supported values:
- 7 days
- 14 days
- 30 days (default)
- 60 days
- 90 days
- Custom value

### 3. Customize path.txt Location
Edit `Program.cs` (line 19):
```csharp
string pathFile = "D:\\My project\\Bot\\logtest\\path.txt";
// Change to your desired location
```

### 4. Configure Logging
Edit `NLog.config` to change log output location:
```xml
<target name="logfile" xsi:type="File" 
        fileName="D:\Logs\AutoClearLog.log"
        layout="${longdate}|${level:uppercase=true}|${message}"/>
```

## 📂 Project Structure

```
AutoClearLog/
├── Program.cs                 # Main application logic
├── App.config                 # Application settings
├── NLog.config               # Logging configuration
├── AutoClearLog.csproj       # Project file
├── AutoClearLog.sln          # Solution file
├── packages.config           # NuGet dependencies
├── README.md                 # This file
├── LICENSE.txt               # License information
├── bin/                      # Compiled binaries
├── obj/                      # Build artifacts
└── Properties/               # Project properties
```

## 🔄 How It Works

1. **Read Configuration** - Load path.txt and configuration
2. **Discover Folders** - Scan each folder in path.txt
3. **Find Log Files** - Locate all .log files
4. **Extract Dates** - Parse dates from filenames
5. **Compare Dates** - Check if files exceed age threshold
6. **Compress Old Logs** - Add old files to ZIP archives
7. **Delete Originals** - Remove original log files
8. **Log Operations** - Record all actions in log file
9. **Summary** - Generate summary of operations

## 📝 Log File Naming Convention

**Input Format:** `EVMSWSS-YYYY-MM-DD.log`

**Output Format:** `Files_YYYY-MM-DD.zip`

**Example:**
- Input: `EVMSWSS-2024-01-15.log`
- Output: `Files_2024-01-15.zip`

### Valid Filenames ✅
- EVMSWSS-2024-01-15.log
- EVMSWSS-2025-12-31.log
- EVMSWSS-2023-06-20.log

### Invalid Filenames ❌
- log-20240115.log (wrong date format)
- System.log (no date)
- EVMSWSS20240115.log (missing hyphens)

## 🎯 Output Structure

After processing, files are organized as follows:

```
folder1/
├── EVMSWSS-2025-02-10.log (current - not processed)
└── Zipfile/
    ├── Files_2024-01-15.zip (old files compressed)
    └── Files_2024-06-20.zip (old files compressed)

folder2/
├── EVMSWSS-2025-02-15.log (current - not processed)
└── Zipfile/
    └── Files_2023-12-10.zip (old files compressed)
```

## 🛠️ Build from Source

### Prerequisites
- Visual Studio 2019 or later
- .NET Framework 4.8 SDK
- Git

### Steps
```bash
# Clone the repository
git clone https://github.com/NPKman/AutoClearLog.git

# Navigate to project
cd AutoClearLog

# Build the project
# Using Visual Studio: Ctrl + Shift + B
# Or using MSBuild:
msbuild AutoClearLog.sln /p:Configuration=Release

# Run the application
.\bin\Release\AutoClearLog.exe
```

## 🐛 Troubleshooting

### Issue: "path.txt not found"
**Solution:**
1. Create path.txt at the correct location
2. Verify the path in Program.cs line 19
3. Ensure file is saved as UTF-8

### Issue: "Skipping non-existent folder"
**Solution:**
1. Check folder paths in path.txt are correct
2. Verify folder exists and is accessible
3. Check Read/Write permissions

### Issue: "Skipping file: Invalid format"
**Solution:**
1. Verify filenames match format: `EVMSWSS-YYYY-MM-DD.log`
2. Rename files if necessary
3. Check date parsing in Program.cs

### Issue: "Failed to delete file"
**Solution:**
1. Close any programs using the file
2. Check folder Write permissions
3. Run AutoClearLog with Admin privileges

### Issue: "No files matched the compression criteria"
**Solution:**
1. Verify log files exist in configured folders
2. Check daysThreshold value in Program.cs
3. Ensure files are older than threshold

## 📊 Logging

Application logs are written to:
```
D:\เอกสาร\Dev\AutoClearLog\logs\AutoClearLog.log
```

### View Logs
```bash
# Using Notepad
notepad D:\เอกสาร\Dev\AutoClearLog\logs\AutoClearLog.log

# Using PowerShell (live tail)
Get-Content D:\เอกสาร\Dev\AutoClearLog\logs\AutoClearLog.log -Tail 50 -Wait
```

## 💾 Dependencies

- **NLog** - Comprehensive logging framework
- **.NET Framework** - Built-in compression and IO libraries

## 📜 License

See [LICENSE.txt](LICENSE.txt) for details.

## 👨‍💻 Development

This project is maintained at: https://github.com/NPKman/AutoClearLog

### Report Issues
Found a bug? Please open an issue on GitHub with:
- System information (OS, .NET version)
- Error message from log file
- Steps to reproduce
- Expected vs actual behavior

### Feature Requests
Have an idea? Submit a feature request on GitHub Issues.

## 📞 Support

For help and documentation:
- Check the [SOP Documentation](../SOP/) in the parent directory
- View detailed guides and flowcharts
- Review configuration examples

## 🎯 Use Cases

1. **Application Log Management** - Automatic cleanup of application logs
2. **Server Log Archival** - Archive old server logs to save space
3. **Backup Integration** - Compress logs before backup
4. **System Maintenance** - Automated disk space management
5. **Compliance** - Maintain log retention policies

## ⏰ Recommended Schedule

For daily runs at 2:00 AM (when system load is minimal):
- **Frequency:** Daily
- **Time:** 02:00 AM
- **Retention:** Keep recent logs (adjust daysThreshold as needed)

## 🔐 Security Notes

- Compressed files retain original permissions
- Ensure path.txt is not accessible to unauthorized users
- Run Task Scheduler tasks with appropriate privileges
- Regularly verify compressed file integrity

## 📈 Performance Tips

1. Schedule execution during off-peak hours
2. Start with small test folders first
3. Monitor system resources during processing
4. Archive processed ZIP files periodically
5. Keep recent logs uncompressed for quick access

---

**Version:** 1.0  
**Last Updated:** February 16, 2026  
**Author:** Development Team  
**License:** See LICENSE.txt

**For detailed setup and usage instructions, see the SOP documentation in the parent directory.**