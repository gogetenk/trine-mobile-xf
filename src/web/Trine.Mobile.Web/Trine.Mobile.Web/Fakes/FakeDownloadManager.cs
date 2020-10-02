using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using Plugin.DownloadManager.Abstractions;

namespace Trine.Mobile.Web.Fakes
{
    public class FakeDownloadManager : IDownloadManager
    {
        public IEnumerable<IDownloadFile> Queue => throw new NotImplementedException();

        private readonly Func<IDownloadFile, string> _fileName;
        public Func<IDownloadFile, string> PathNameForDownloadedFile { get => _fileName; set { } }

        public event NotifyCollectionChangedEventHandler CollectionChanged;

        public void Abort(IDownloadFile file)
        {
        }

        public void AbortAll()
        {
        }

        public IDownloadFile CreateDownloadFile(string url)
        {
            return null;
        }

        public IDownloadFile CreateDownloadFile(string url, IDictionary<string, string> headers)
        {
            return null;
        }

        public void Start(IDownloadFile file, bool mobileNetworkAllowed = true)
        {
        }
    }
}
