using org.apache.zookeeper;
using System;
using System.Threading.Tasks;

namespace Watch
{
    public class MyWatcher : Watcher
    {
        public override Task process(WatchedEvent @event)
        {
            return null;
        }
    }
}
