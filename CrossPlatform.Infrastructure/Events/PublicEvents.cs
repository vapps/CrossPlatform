using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CrossPlatform.Infrastructure.Events
{
    /// <summary>
    /// 액션 이벤트
    /// </summary>
    public class ActionEvent : Microsoft.Practices.Prism.PubSubEvents.PubSubEvent<KeyValuePair<string, object>> { }

    /// <summary>
    /// 시스템 이벤트
    /// </summary>
    public class SystemEvent : Microsoft.Practices.Prism.PubSubEvents.PubSubEvent<KeyValuePair<string, object>> { }

    /// <summary>
    /// 변경 이벤트
    /// </summary>
    public class ChangeEvent : Microsoft.Practices.Prism.PubSubEvents.PubSubEvent<KeyValuePair<string, object>> { }
}
