using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ET
{
    public class UISpawnHelper
    {
        public struct ComInfo
        {
            public string gameObjectName;
            public string gameObjectPath;
            public string CompName;
        }
        public static List<ComInfo> GetComponentName(GameObject gameObject)
        {
            Transform[] trans = gameObject.transform.GetComponentsInChildren<Transform>();
            List<Transform> outPutList = new List<Transform>();
            List<ComInfo> comInfos = new List<ComInfo>();
            foreach (var tran in trans)
            {
                if (tran.gameObject.name.StartsWith("E_"))
                {
                    outPutList.Add(tran);
                }
            }

            foreach (var outPut in outPutList)
            {
                var comName = GetComponentName(outPut);
                if (comName != null)
                {
                    ComInfo comInfo = new ComInfo()
                    {
                        gameObjectName = outPut.gameObject.name,
                        gameObjectPath = GetComPath(outPut),
                        CompName = GetComponentName(outPut),
                    };
                    comInfos.Add(comInfo);
                }
            }

            return comInfos;
        }

        private static string GetComPath(Transform trans)
        {
            string path = trans.gameObject.name;
            if (trans.parent != null && !trans.parent.gameObject.name.EndsWith("Root") && trans.parent.parent != null && !trans.parent.parent.gameObject
            .name.Contains("Canvas"))
            {
                path = trans.parent.gameObject.name+"/"+ path;
            }

            return "\""+path+"\"";
        }

        private static string GetComponentName(Transform trans)
        {
            if (trans.GetComponent<Button>() != null)
                return "Button";
            if (trans.GetComponent<Text>() != null)
                return "Text";
            if (trans.GetComponent<InputField>() != null)
                return "InputField";
            if (trans.GetComponent<Toggle>() != null)
                return "Toggle";
            if (trans.GetComponent<Image>() != null)
                return "Image";
            if (trans.GetComponent<Transform>() != null)
                return "Transform";
            return null;
        }
    }
}