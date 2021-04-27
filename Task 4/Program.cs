using System;
using System.IO;
using System.Globalization;

namespace Task_4
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                if (args.Length < 2)
                {
                    throw new ArgumentException("Invalid argument");
                }

                FileManagementSystem fms = new FileManagementSystem(args[0]);

                if (args[1] == "watch")
                {
                    fms.Watch();
                }
                else if (args[1] == "restore")
                {
                    fms.Restore();
                }
                else
                {
                    throw new ArgumentException("Invalid argument");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
    }
}

class FileManagementSystem
{
    private FileSystemWatcher _watcher;
    private DateTimeFormatInfo _dtfi = new CultureInfo("ru-RU").DateTimeFormat;
    private DirectoryInfo _sourceDirectory;
    private string _backupDir = "C:/backup/";
    private string _path;
    private string _filter = "*.txt";
    private string _timeSeparator = "-";
    private bool _includeSubDirectories = true;
    private bool _enableRaisingEvents = true;

    public FileManagementSystem(string path)
    {
        _path = path;
        _dtfi.TimeSeparator = _timeSeparator;
        _sourceDirectory = new DirectoryInfo(_path);
    }

    public void Watch()
    {
        _watcher = new FileSystemWatcher(_path);

        WatcherInit();

        Console.ReadKey();
    }

    public void SetPath(string newPath)
    {
        _path = newPath;
    }

    private void WatcherInit()
    {
        _watcher.NotifyFilter = NotifyFilters.CreationTime
                                 | NotifyFilters.LastAccess
                                 | NotifyFilters.DirectoryName
                                 | NotifyFilters.FileName
                                 | NotifyFilters.LastWrite;

        _watcher.Changed += OnChanged;
        _watcher.Created += OnFileSystemEvent;
        _watcher.Deleted += OnFileSystemEvent;
        _watcher.Renamed += OnFileSystemEvent;
        _watcher.Error += OnError;

        _watcher.Filter = _filter;
        _watcher.IncludeSubdirectories = _includeSubDirectories;
        _watcher.EnableRaisingEvents = _enableRaisingEvents;
    }

    private void OnChanged(object sender, FileSystemEventArgs e)
    {
        if (e.ChangeType != WatcherChangeTypes.Changed)
        {
            return;
        }

        Copy();
    }

    private void OnFileSystemEvent(object sender, FileSystemEventArgs e)
    {
        Copy();
    }

    private void OnError(object sender, ErrorEventArgs e)
    {
        PrintException(e.GetException());
    }

    private void PrintException(Exception? ex)
    {
        if (ex != null)
        {
            Console.WriteLine($"Message: {ex.Message}");
            Console.WriteLine("Stacktrace:");
            Console.WriteLine(ex.StackTrace);
            Console.WriteLine();
            PrintException(ex.InnerException);
        }
    }

    private void Copy()
    {
        CopyAll(_sourceDirectory, new DirectoryInfo(_backupDir + DateTime.Now.ToString(_dtfi)));
    }

    private void CopyAll(DirectoryInfo source, DirectoryInfo target)
    {
        if (source.FullName.ToLower() == target.FullName.ToLower())
        {
            return;
        }

        if (Directory.Exists(target.FullName) == false)
        {
            Directory.CreateDirectory(target.FullName);
        }

        foreach (FileInfo fi in source.GetFiles())
        {
            fi.CopyTo(Path.Combine(target.ToString(), fi.Name), true);
        }

        foreach (DirectoryInfo diSourceSubDir in source.GetDirectories())
        {
            DirectoryInfo nextTargetSubDir =
                target.CreateSubdirectory(diSourceSubDir.Name);
            CopyAll(diSourceSubDir, nextTargetSubDir);
        }
    }

    public void Restore()
    {
        DirectoryInfo backupDirectory = new DirectoryInfo(_backupDir);
        DirectoryInfo[] backups = backupDirectory.GetDirectories();
        int choosedBackupIndex;

        if (backups.Length == 0)
        {
            Console.WriteLine("No backup!");
            return;
        }

        Console.WriteLine("Choose backup:");

        for (int i = 0; i < backups.Length; i++)
        {
            Console.WriteLine($"{i + 1}: {backups[i].Name}");
        }

        Int32.TryParse(Console.ReadLine(), out choosedBackupIndex);

        if (choosedBackupIndex < 1 || choosedBackupIndex > backups.Length)
        {
            Console.WriteLine("Incorrect index!");
            return;
        }

        RestoreAll(backups[choosedBackupIndex - 1]);
    }

    private void RestoreAll(DirectoryInfo target)
    {
        DeleteAllInDirectory(_sourceDirectory);
        CopyAll(target, _sourceDirectory);
    }

    private void DeleteAllInDirectory(DirectoryInfo source)
    {
        foreach (FileInfo file in source.GetFiles())
        {
            file.Delete();
        }
        foreach (DirectoryInfo directory in source.GetDirectories())
        {
            directory.Delete(true);
        }
    }
}