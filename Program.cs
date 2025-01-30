using System;
using System.IO;
using System.IO.Compression;
using System.Text.RegularExpressions;
using NLog;
using System.Collections.Generic;

namespace AutoClearLog
{
    internal class Program
    {
        private static readonly ILogger logger = LogManager.GetCurrentClassLogger();

        static void Main(string[] args)
        {
            try
            {
                logger.Info("🎯 Starting ZIP process");

                // Read source folders from path.txt
                string pathFile = "D:\\My project\\Bot\\logtest\\path.txt"; // เปลี่ยนเป็นพาธที่ถูกต้อง
                if (!File.Exists(pathFile))
                {
                    logger.Error($"❌ path.txt not found: {pathFile}");
                    return;
                }

                List<string> sourceFolders = new List<string>(File.ReadAllLines(pathFile));

                int daysThreshold = 30; // ปรับตามต้องการ
                DateTime cutoffDate = DateTime.Now.AddDays(-daysThreshold);
                bool fileAdded = false;

                foreach (string sourceFolder in sourceFolders)
                {
                    if (!Directory.Exists(sourceFolder))
                    {
                        logger.Warn($"⚠ Skipping non-existent folder: {sourceFolder}");
                        continue;
                    }

                    logger.Info($"📂 Processing folder: {sourceFolder}");

                    // Create Zipfile folder inside the sourceFolder
                    string zipFolder = Path.Combine(sourceFolder, "Zipfile");
                    Directory.CreateDirectory(zipFolder);

                    // Get only .log files (excluding folders)
                    string[] files = Directory.GetFiles(sourceFolder, "*.log");

                    foreach (string file in files)
                    {
                        FileInfo fileInfo = new FileInfo(file);

                        if (fileInfo.Extension.ToLower() != ".log")
                        {
                            logger.Warn($"⚠ Skipping file: {fileInfo.Name} (Not a .log file)");
                            continue;
                        }

                        // Extract date from filename
                        Match match = Regex.Match(fileInfo.Name, @"EVMSWSS-(\d{4}-\d{2}-\d{2})\.log");
                        if (!match.Success)
                        {
                            logger.Warn($"⚠ Skipping file: {fileInfo.Name} (Invalid format)");
                            continue;
                        }

                        string datePart = match.Groups[1].Value;
                        if (!DateTime.TryParse(datePart, out DateTime fileDate))
                        {
                            logger.Warn($"⚠ Skipping file: {fileInfo.Name} (Unable to parse date)");
                            continue;
                        }

                        if (fileDate >= cutoffDate)
                        {
                            logger.Info($"⏩ Skipping file: {fileInfo.Name} (Too recent)");
                            continue;
                        }

                        // Define zip file name
                        string zipFileName = $"Files_{datePart}.zip";
                        string zipFilePath = Path.Combine(zipFolder, zipFileName);

                        // Create or append ZIP file
                        using (FileStream zipToCreate = new FileStream(zipFilePath, FileMode.OpenOrCreate))
                        using (ZipArchive archive = new ZipArchive(zipToCreate, ZipArchiveMode.Update))
                        {
                            ZipArchiveEntry entry = archive.CreateEntry(fileInfo.Name);
                            using (FileStream fileStream = new FileStream(file, FileMode.Open, FileAccess.Read))
                            using (Stream entryStream = entry.Open())
                            {
                                fileStream.CopyTo(entryStream);
                            }

                            logger.Info($"✔ Added file: {fileInfo.Name} → {zipFileName}");
                            fileAdded = true;
                        }

                        // Delete the original log file after zipping
                        try
                        {
                            File.Delete(file);
                            logger.Info($"🗑 Deleted file: {fileInfo.Name}");
                        }
                        catch (Exception ex)
                        {
                            logger.Error($"❌ Failed to delete file: {fileInfo.Name}, Error: {ex.Message}");
                        }
                    }
                }

                if (!fileAdded)
                {
                    logger.Warn("⚠ No files matched the compression criteria");
                }
                else
                {
                    logger.Info("✅ ZIP files created successfully.");
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex, "❌ Error: ");
            }
            finally
            {
                LogManager.Shutdown(); // Close logger
            }
        }
    }
}
