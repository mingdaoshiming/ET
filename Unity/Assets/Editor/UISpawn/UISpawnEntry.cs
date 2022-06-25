using System.IO;
using System.Text;
using UnityEditor;
using UnityEditor.UI;
using UnityEngine;

namespace ET
{
    public class UISpawnEntry : Editor
    {
        [MenuItem("Assets/Tools/SpawnUICode")]
        public static void SpawnUICode()
        {
            GameObject curSelect = Selection.activeGameObject;
            if (!curSelect.gameObject.name.StartsWith("UI"))
            {
                Debug.Log("预制体名称需要以UI为开头");
                return;
            }
            var modelViewPath = EditorPathHelper.GetClientModelViewUIPath();
            if (!Directory.Exists(modelViewPath + curSelect.name))
            {
                Directory.CreateDirectory(modelViewPath + curSelect.name);
            }

            string fileName = curSelect.name + "Component.cs";
            // 创建 Component
            // if (!File.Exists(modelViewPath + curSelect.name +"/"+fileName))
            // {
                File.Create(modelViewPath + curSelect.name +"/"+fileName).Close();
                StringBuilder sb = new StringBuilder();
                sb.Clear();
                sb.Append("using UnityEngine;");
                sb.AppendLine("using UnityEngine.UI;");
                sb.AppendLine();
                sb.AppendLine("namespace ET.Client");
                sb.AppendLine("{");
                sb.AppendLine("\t[ComponentOf(typeof(UI))]");
                sb.AppendLine(string.Format("\tpublic class {0}Component : Entity, IAwake, IDestroy",curSelect.name));
                sb.AppendLine("\t{");
                var comInfos = UISpawnHelper.GetComponentName(curSelect);
                foreach (var comInfo in comInfos)
                {
                    sb.AppendLine(string.Format("\t\tpublic {0} {1};", comInfo.CompName, comInfo.gameObjectName));
                }

                sb.AppendLine("\t};");
                sb.AppendLine("};");
                string componentFile = sb.ToString();
                File.WriteAllText(modelViewPath + curSelect.name +"/"+fileName, componentFile, Encoding.UTF8);
            //}
            
            // 创建Event
            sb.Clear();
            sb.Append("using UnityEngine;");
            sb.AppendLine();
            sb.AppendLine("namespace ET.Client");
            sb.AppendLine("{");
            sb.AppendLine(string.Format("\t[UIEvent(UIType.{0})]", curSelect.gameObject.name));
            sb.AppendLine(string.Format("\tpublic class {0}Event: AUIEvent", curSelect.gameObject.name));
            sb.AppendLine("\t{");
            sb.AppendLine("\t\tpublic override async ETTask<UI> OnShow(UIComponent uiComponent, UILayer uiLayer)");
            sb.AppendLine("\t\t{");
            sb.AppendLine(string.Format("\t\t\tUI ui = uiComponent.Get(UIType.{0});", curSelect.gameObject.name));
            sb.AppendLine("\t\t\tif (ui == null)");
            sb.AppendLine("\t\t\t{");
            sb.AppendLine(string.Format("\t\t\t\tGameObject gameObject = await uiComponent.Domain.GetComponent<YooAssetComponent>().InstantiateASync(UIType.{0});", curSelect.gameObject.name));
            sb.AppendLine(string.Format("\t\t\t\tui = uiComponent.AddChild<UI, string, GameObject>(UIType.{0}, gameObject);", curSelect.gameObject.name));
            sb.AppendLine(string.Format("\t\t\t\tui.AddComponent<{0}Component>();", curSelect.gameObject.name));
            sb.AppendLine("\t\t\t}");
            sb.AppendLine("\t\t\telse");
            sb.AppendLine("\t\t\t{");
            sb.AppendLine("\t\t\t\tui.GameObject.SetActive(true);");
            sb.AppendLine("\t\t\t}");
            sb.AppendLine("\t\t\treturn ui;");
            sb.AppendLine("\t\t}");
            sb.AppendLine();
            sb.AppendLine("\t\tpublic override void OnHide(UIComponent uiComponent)");
            sb.AppendLine("\t\t{");
            sb.AppendLine(string.Format("\t\t\tUI ui = uiComponent.Get(UIType.{0});", curSelect.gameObject.name));
            sb.AppendLine("\t\t\tif (ui == null)");
            sb.AppendLine("\t\t\t\treturn;");
            sb.AppendLine("\t\t\tui.GameObject.SetActive(false);");
            sb.AppendLine(string.Format("\t\t\tvar component = ui.GetComponent<{0}Component>();", curSelect.gameObject.name));
            sb.AppendLine("\t\t\tcomponent.OnHide();");
            sb.AppendLine("\t\t}");
            sb.AppendLine();
            sb.AppendLine("\t\tpublic override void OnRemove(UIComponent uiComponent)");
            sb.AppendLine("\t\t{");
            sb.AppendLine(string.Format("\t\t\tUI ui = uiComponent.Get(UIType.{0});", curSelect.gameObject.name));
            sb.AppendLine("\t\t\tif (ui == null)");
            sb.AppendLine("\t\t\t\treturn;");
            sb.AppendLine(string.Format("\t\t\tvar component = ui.GetComponent<{0}Component>();", curSelect.gameObject.name));
            sb.AppendLine("\t\t\tcomponent.OnRemove();");
            sb.AppendLine("\t\t}");
            sb.AppendLine("\t}");
            sb.AppendLine("}");
            
            var eventPath = EditorPathHelper.GetClientHotFixViewUIPath();
            if (!Directory.Exists(eventPath + curSelect.gameObject.name))
            {
                Directory.CreateDirectory(eventPath + curSelect.gameObject.name);
            }
            string eventFileName = curSelect.gameObject.name + "Event.cs";
            string eventInfo = sb.ToString();
            File.WriteAllText(eventPath + curSelect.gameObject.name+"/"+eventFileName, eventInfo, Encoding.UTF8);

            // 创建System
            sb.Clear();
            var hotFixViewPath = EditorPathHelper.GetClientHotFixViewUIPath();
            if (!Directory.Exists(hotFixViewPath + curSelect.gameObject.name))
            {
                Directory.CreateDirectory(hotFixViewPath + curSelect.gameObject.name);
            }
            string systemFileName = curSelect.name + "ComponentSystem.cs";
            if (!File.Exists(hotFixViewPath + curSelect.gameObject.name+"/"+systemFileName))
            {
                File.Create(hotFixViewPath + curSelect.gameObject.name+"/"+systemFileName).Close();
                sb.AppendLine("using UnityEngine;");
                sb.AppendLine("using UnityEngine.UI;");
                sb.AppendLine("namespace ET.Client");
                sb.AppendLine("{");
                // awake system
                sb.AppendLine("\t[ObjectSystem]");
                sb.AppendLine(string.Format("\tpublic class {0}ComponentAwakeSystem : AwakeSystem<{1}Component>", curSelect.gameObject.name,curSelect.gameObject.name));
                sb.AppendLine("\t{");
                sb.AppendLine(string.Format("\t\tpublic override void Awake({0}Component self)", curSelect.gameObject.name));
                sb.AppendLine("\t\t{");
                foreach (var comInfo in comInfos)
                {
                    sb.AppendLine(string.Format("\t\t\tself.{0} = self.GetParent<UI>().GameObject.transform.Find({1}).GetComponent<{2}>();", 
                        comInfo.gameObjectName, comInfo.gameObjectPath, comInfo.CompName));
                }
                sb.AppendLine("\t\t\tself.OnShow();");
                sb.AppendLine("\t\t}");
                sb.AppendLine("\t}");
                sb.AppendLine();
                // destroy System
                sb.AppendLine("\t[ObjectSystem]");
                sb.AppendLine(string.Format("\tpublic class {0}ComponentDestroySystem : DestroySystem<{1}Component>", curSelect.gameObject.name, curSelect.gameObject.name));
                sb.AppendLine("\t{");
                sb.AppendLine(string.Format("\t\tpublic override void Destroy({0}Component self)", curSelect.gameObject.name));
                sb.AppendLine("\t\t{");
                foreach (var comInfo in comInfos)
                {
                    sb.AppendLine(string.Format("\t\t\tself.{0} = null;", comInfo.gameObjectName));
                }
                sb.AppendLine("\t\t}");
                sb.AppendLine("\t}");
                sb.AppendLine();
                sb.AppendLine(string.Format("\t[FriendClass(typeof({0}Component))]", curSelect.gameObject.name));
                sb.AppendLine(string.Format("\tpublic static class {0}ComponentSystem", curSelect.gameObject.name));
                sb.AppendLine("\t{");
                sb.AppendLine(string.Format("\t\tpublic static void OnShow(this {0}Component self)", curSelect.gameObject.name));
                sb.AppendLine("\t\t{");
                sb.AppendLine("\t\t}");
                sb.AppendLine();
                sb.AppendLine(string.Format("\t\tpublic static void OnHide(this {0}Component self)", curSelect.gameObject.name));
                sb.AppendLine("\t\t{");
                sb.AppendLine("\t\t}");
                sb.AppendLine();
                sb.AppendLine(string.Format("\t\tpublic static void OnRemove(this {0}Component self)", curSelect.gameObject.name));
                sb.AppendLine("\t\t{");
                sb.AppendLine("\t\t}");
                sb.AppendLine("\t}");
                sb.AppendLine("}");

                string systemInfo = sb.ToString();
                File.WriteAllText(hotFixViewPath + curSelect.gameObject.name+"/"+systemFileName,systemInfo, Encoding.UTF8);
                string pathInfo = AssetDatabase.GetAssetPath(curSelect);
                UnityEngine.GUIUtility.systemCopyBuffer = pathInfo;
                Debug.Log("生成 UICode 结束 ------ >>>");
            }
            

        }
    }
}