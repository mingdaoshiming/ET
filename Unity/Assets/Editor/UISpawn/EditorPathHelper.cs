using UnityEngine;
using UnityEditor;

namespace ET
{
    public class EditorPathHelper
    {
        public static string GetClientModelViewUIPath()
        {
            string assetpath = Application.dataPath;
            assetpath.Replace("\\", "/");
            string ETPath = assetpath.Replace("Assets", "")+"/Codes/ModelView//Client/UI/";
            return ETPath;
        }
        public static string GetClientHotFixViewUIPath()
        {
            string assetpath = Application.dataPath;
            assetpath.Replace("\\", "/");
            string ETPath = assetpath.Replace("Assets", "")+"/Codes/HotfixView/Client/UI/";
            return ETPath;
        }
    }
}