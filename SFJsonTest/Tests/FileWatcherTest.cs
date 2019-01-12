using System;
using System.Collections.Generic;
using Assets.Editor.FileWatcher;
using NUnit.Framework;
using SFJson;
using SFJson.Conversion;
using SFJson.Conversion.Settings;
using SFJson.Utils;

namespace Assets.Editor.FileWatcher
{
    public class FileMover : FileWatchDataExtension
    {
        public string FileDestination;
    }
    public class NullExtension : FileWatchDataExtension
    {
        public string FileDestination;
    }
    public abstract class FileWatchDataExtension
    {
        public Type SourceType;
    }
    
    
    public class DirectoryWatchDataList
    {
        public List<DirectoryWatchData> DirectoryWatchData;

        public DirectoryWatchDataList()
        {
            DirectoryWatchData = new List<DirectoryWatchData>();
        }
    }
    public class WatcherData
    {
        public string Type = "BaseFileMover";
        public bool SelectFiles;
        public List<FileWatchData> Files;
        public FileWatchDataExtension FileWatchDataExtension;

        public WatcherData()
        {
            Files = new List<FileWatchData>();
        }
    }
    public class DirectoryWatchData
    {
        public bool HasDirectoryInfoChanged;
        public string DirectoryPath;
        public List<WatcherData> Watchers;
//        public DateTime LastSyncTime;

        public DirectoryWatchData()
        {
            Watchers = new List<WatcherData>();
        }
    }
    public class FileWatchData
    {
        public string RelativePath;
        public string Filename;
//        public DateTime LastSyncTime;
    }
}

namespace SFJsonTest
{
    [TestFixture]
    public class FileWatcherTests
    {
        private Serializer _serializer;
        private Deserializer _deserializer;

        [OneTimeSetUp]
        public void Init()
        {
            _deserializer = new Deserializer();
            _serializer = new Serializer();
        }

        [Test]
        public void Test()
        {
            var str = "{\"$type\":\"Assets.Editor.FileWatcher.DirectoryWatchDataList, SFJsonTest\",\"DirectoryWatchData\":{\"$type\":\"System.Collections.Generic.List`1[[Assets.Editor.FileWatcher.DirectoryWatchData, SFJsonTest]], mscorlib\",\"$values\":[{\"$type\":\"Assets.Editor.FileWatcher.DirectoryWatchData, SFJsonTest\",\"HasDirectoryInfoChanged\":false,\"DirectoryPath\":\"/Users/Chris/UnityProjects/GraveDefense/Test\",\"Watchers\":{\"$type\":\"System.Collections.Generic.List`1[[Assets.Editor.FileWatcher.WatcherData, SFJsonTest]], mscorlib\",\"$values\":[{\"$type\":\"Assets.Editor.FileWatcher.WatcherData, SFJsonTest\",\"Type\":\"BaseFileMover\",\"SelectFiles\":true,\"Files\":{\"$type\":\"System.Collections.Generic.List`1[[Assets.Editor.FileWatcher.FileWatchData, SFJsonTest]], mscorlib\",\"$values\":[{\"$type\":\"Assets.Editor.FileWatcher.FileWatchData, SFJsonTest\",\"RelativePath\":null,\"Filename\":\"WatchDataNew.json\"}]},\"FileWatchDataExtension\":{\"$type\":\"Assets.Editor.FileWatcher.FileMover, SFJsonTest\",\"FileDestination\":\"/Users/Chris/UnityProjects/GraveDefense/GraveDefense/Assets/_Libraries\",\"SourceType\":null}},{\"$type\":\"Assets.Editor.FileWatcher.WatcherData, SFJsonTest\",\"Type\":\"BaseFileMover\",\"SelectFiles\":true,\"Files\":{\"$type\":\"System.Collections.Generic.List`1[[Assets.Editor.FileWatcher.FileWatchData, SFJsonTest]], mscorlib\",\"$values\":[{\"$type\":\"Assets.Editor.FileWatcher.FileWatchData, SFJsonTest\",\"RelativePath\":null,\"Filename\":\"WatchDataNew copy 2.json\"}]},\"FileWatchDataExtension\":{\"$type\":\"Assets.Editor.FileWatcher.FileMover, SFJsonTest\",\"FileDestination\":\"/Users/Chris/UnityProjects/GraveDefense/GraveDefense/Assets/_Libraries/Editor\",\"SourceType\":null}},{\"$type\":\"Assets.Editor.FileWatcher.WatcherData, SFJsonTest\",\"Type\":\"BaseFileMover\",\"SelectFiles\":true,\"Files\":{\"$type\":\"System.Collections.Generic.List`1[[Assets.Editor.FileWatcher.FileWatchData, SFJsonTest]], mscorlib\",\"$values\":[{\"$type\":\"Assets.Editor.FileWatcher.FileWatchData, SFJsonTest\",\"RelativePath\":null,\"Filename\":\"WatchDataNew copy.json\"}]},\"FileWatchDataExtension\":{\"$type\":\"Assets.Editor.FileWatcher.FileMover, SFJsonTest\",\"FileDestination\":\"/Users/Chris/UnityProjects/GraveDefense/GraveDefense/Assets/_MyEffects\",\"SourceType\":null}},{\"$type\":\"Assets.Editor.FileWatcher.WatcherData, SFJsonTest\",\"Type\":\"Test\",\"SelectFiles\":false,\"Files\":{\"$type\":\"System.Collections.Generic.List`1[[Assets.Editor.FileWatcher.FileWatchData, SFJsonTest]], mscorlib\",\"$values\":[{\"$type\":\"Assets.Editor.FileWatcher.FileWatchData, SFJsonTest\",\"RelativePath\":null,\"Filename\":null}]},\"FileWatchDataExtension\":null}]}}]}}";
            var data = _deserializer.Deserialize<DirectoryWatchDataList>(str);
            var redo = _serializer.Serialize(data, new SerializerSettings {SerializationTypeHandle = SerializationTypeHandle.All});
            Console.WriteLine(redo);
            Assert.NotNull(data);
            Assert.AreEqual(str, redo);
        }
    }
}