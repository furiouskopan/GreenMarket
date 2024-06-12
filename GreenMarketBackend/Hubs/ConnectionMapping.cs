//namespace GreenMarketBackend.Hubs
//{
//    public class ConnectionMapping
//    {
//        private readonly Dictionary<string, string> _connections =
//            new Dictionary<string, string>();

//        public void Add(string userName, string connectionId)
//        {
//            lock (_connections)
//            {
//                _connections[userName] = connectionId;
//            }
//        }

//        public string GetConnectionId(string userName)
//        {
//            lock (_connections)
//            {
//                return _connections.TryGetValue(userName, out var connectionId)
//                    ? connectionId
//                    : null;
//            }
//        }

//        public void Remove(string userName)
//        {
//            lock (_connections)
//            {
//                if (_connections.ContainsKey(userName))
//                {
//                    _connections.Remove(userName);
//                }
//            }
//        }
//    }
//}
