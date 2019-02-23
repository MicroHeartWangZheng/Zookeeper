using Newtonsoft.Json;
using org.apache.zookeeper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ZookeeperTest
{
    public class MyWatcher : Watcher
    {
        public override Task process(WatchedEvent @event)
        {
            return null;
        }
    }
}
