using System;
namespace CrossPlatform.Infrastructure.Interfaces
{
    public interface ICommonILC
    {
        event System.Collections.Specialized.NotifyCollectionChangedEventHandler CollectionChanged;
        void Dispose();
        bool HasMoreItems { get; set; }
        bool IsBusy { get; set; }
        System.Collections.IList Items { get; set; }
        int MaxCount { get; set; }
    }
}
