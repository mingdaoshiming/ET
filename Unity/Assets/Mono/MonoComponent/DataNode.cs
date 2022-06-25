using System.Collections.Generic;
using Sirenix.OdinInspector;

namespace ET
{
    public class DataNode : SerializedMonoBehaviour
    {
        public Dictionary<string, dynamic> dataList = new Dictionary<string, dynamic>();

        public void Add(string key, dynamic value)
        {
            if (this.dataList.ContainsKey(key))
                this.dataList[key] = value;
            //else
                // this.dataList.Add(key, value);
        }

        public dynamic Get(string key)
        {
            this.dataList.TryGetValue(key, out dynamic value);
            return value;
        }

        public bool HasKey(string key)
        {
            return this.dataList.ContainsKey(key);
        }

        public void Clear()
        {
            this.dataList.Clear();
        }

        private void OnDestroy()
        {
            this.dataList.Clear();
        }
    }
}