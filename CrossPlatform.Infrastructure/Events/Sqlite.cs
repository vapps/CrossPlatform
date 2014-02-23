using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CrossPlatform.Infrastructure.Events
{
    /// <summary>
    /// Sqlite KeyValuePair 이벤트
    /// </summary>
    public class SqliteEvent : Microsoft.Practices.Prism.PubSubEvents.PubSubEvent<KeyValuePair<string,object>> { }

    public class SqliteCreateTableEvent : SqliteEvent { }
}
