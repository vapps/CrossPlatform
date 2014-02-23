using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CrossPlatform.Infrastructure.Events
{
    /// <summary>
    /// Navigate Event
    /// </summary>
    public class NavigatedEvent : Microsoft.Practices.Prism.PubSubEvents.PubSubEvent<CrossPlatform.Infrastructure.Models.NavigationArgs> {}

    /// <summary>
    /// 네비게이션 이벤트
    /// </summary>
    public class NavigatingEvent : Microsoft.Practices.Prism.PubSubEvents.PubSubEvent<CrossPlatform.Infrastructure.Models.NavigationArgs> { }

}
