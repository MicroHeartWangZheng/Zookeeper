using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using org.apache.zookeeper;
using static org.apache.zookeeper.ZooDefs;

namespace ZookeeperTest.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        [HttpGet]
        public ActionResult<IEnumerable<string>> Get()
        {

            MyWatcher myWatcher = new MyWatcher();

            ZooKeeper zk = new ZooKeeper("127.0.0.1:2181", 10000, myWatcher);


            var stat = zk.existsAsync("/root", true);
            //创建一个节点root，数据是mydata,不进行ACL权限控制，节点为永久性的(即客户端shutdown了也不会消失) 
            //zk.Create("/root", "mydata".GetBytes(), Ids.OPEN_ACL_UNSAFE, CreateMode.Persistent);

            //在root下面创建一个childone znode,数据为childone,不进行ACL权限控制，节点为永久性的 
            zk.createAsync("/root/childone", Encoding.UTF8.GetBytes("childone"), Ids.OPEN_ACL_UNSAFE, CreateMode.PERSISTENT);
            //取得/root节点下的子节点名称,返回List<String> 
            zk.getChildrenAsync("/root", true);
            //取得/root/childone节点下的数据,返回byte[] 
            zk.getDataAsync("/root/childone", true);

            //修改节点/root/childone下的数据，第三个参数为版本，如果是-1，那会无视被修改的数据版本，直接改掉
            zk.setDataAsync("/root/childone", Encoding.UTF8.GetBytes("childonemodify"), -1);
            //删除/root/childone这个节点，第二个参数为版本，－1的话直接删除，无视版本 
            zk.deleteAsync("/root/childone", -1);

            zk.existsAsync("/root", true);
            return null;
        }
    }
}
