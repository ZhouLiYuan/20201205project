//using System.Collections;
//using System.Collections.Generic;
//using System.Reflection;
//using System.Threading.Tasks;
//using UnityEngine;
//using UnityEngine.AddressableAssets;

////YK版本
//namespace BordlessFramework.UI
//{
//    /// <summary>
//    /// 所有窗口（视图）的基类
//    /// </summary>
//    public abstract class Panel
//    {
//        public string Name { get; private set; }
//        public GameObject GameObject { get; private set; }
//        public Transform Transform { get; private set; }
//        public RectTransform RectTransform { get; private set; }
//        internal CanvasGroup CanvasGroup;
//        /// <summary>
//        /// 当前打开的子面板
//        /// </summary>
//        public Panel CurrentSubPanel { get { return subPanels.Count > 0 ? subPanels.Last.Value : null; } }
//        private LinkedList<Panel> subPanels = new LinkedList<Panel>();
//        private Panel parentPanel;
//        internal int Depth;
//        /// <summary>
//        /// 窗口是否可见
//        /// <para>用法：可在窗口不可见时阻止<see cref="OnUpdate"/></para>
//        /// </summary>
//        public bool IsVisible { get; internal set; }

//        internal void Init(string name, GameObject obj, Panel parentPanel)
//        {
//            Name = name;
//            GameObject = obj;
//            Transform = obj.transform;
//            if (!CanvasGroup) CanvasGroup = GameObject.AddComponent<CanvasGroup>();
//            RectTransform = obj.GetComponent<RectTransform>();
//            this.parentPanel = parentPanel;
//        }

//        /// <summary>
//        ///  显示
//        /// </summary>
//        public virtual void Show()
//        {
//            bool isVisiblePrevious = IsVisible;
//            IsVisible = true;
//            CanvasGroup.alpha = 1f;
//            CanvasGroup.interactable = true;
//            CanvasGroup.blocksRaycasts = true;
//            if (!isVisiblePrevious && IsVisible) OnVisible();
//        }

//        /// <summary>
//        /// 隐藏
//        /// </summary>
//        public virtual void Hide()
//        {
//            bool isVisiblePrevious = IsVisible;
//            IsVisible = false;
//            CanvasGroup.alpha = 0f;
//            CanvasGroup.interactable = false;
//            CanvasGroup.blocksRaycasts = false;
//            if (!isVisiblePrevious && IsVisible) OnInvisible();
//        }

//        /// <summary>
//        /// 同步打开窗口
//        /// </summary>
//        /// <typeparam name="TPanel">窗口子类</typeparam>
//        public TPanel OpenSubPanel<TPanel>(bool isVisibleWhenAvailable = true) where TPanel : Panel, new()
//        {
//            TPanel panel = new TPanel();
//            var type = typeof(TPanel);
//            var name = type.Name;
//            var attribute = type.GetCustomAttribute<LoadUIPrefabByPathAttribute>();
//            var path = attribute.PrefabPath;
//            panel.Depth = attribute.Depth;
//            var prefab = AssetModule.LoadAsset<GameObject>(path);
//            var obj = Object.Instantiate(prefab, Transform);
//            AssetModule.Release(prefab);
//            panel.Init(name, obj, this);
//            InsertPanelByDepth(panel);
//            UIManager.Updater.AddUpdateFunction(panel.OnUpdate);
//            if (!isVisibleWhenAvailable) panel.Hide();
//            panel.OnOpen();

//            if (CurrentSubPanel && CurrentSubPanel.IsVisible)
//            {
//                CurrentSubPanel.IsVisible = false;
//                CurrentSubPanel.OnInvisible();
//            }
//            if (isVisibleWhenAvailable && !panel.IsVisible) panel.Show();

//            return panel;
//        }

//        /// <summary>
//        /// 协程方式打开窗口
//        /// </summary>
//        /// <param name="parent">Panel的父级</param>
//        /// <param name="onPanelOpened">Panel完全打开后的回调</param>
//        /// /// <param name="isVisibleWhenAvailable">Panel可用后，是否立即显示</param>
//        /// <typeparam name="T">窗口子类</typeparam>
//        public WaitOpenPanel<T> OpenSubPanelByCoroutine<T>(bool isVisibleWhenAvailable = true) where T : Panel, new()
//        {
//            T panel = new T();
//            var type = typeof(T);
//            var name = type.Name;
//            var attribute = type.GetCustomAttribute<LoadUIPrefabByPathAttribute>();
//            var path = attribute.PrefabPath;
//            panel.Depth = attribute.Depth;
//            var instantiatePrefabHandle = AssetModule.LoadAssetByCoroutine<GameObject>(path);

//            IEnumerator openAction(GameObject prefab)
//            {
//                var obj = Object.Instantiate(prefab, Transform);
//                AssetModule.Release(prefab);
//                panel.Init(name, obj, this);
//                InsertPanelByDepth(panel);
//                UIManager.Updater.AddUpdateFunction(panel.OnUpdate);
//                if (!isVisibleWhenAvailable) panel.Hide();
//                panel.OnOpen();
//                if (panel is IPanelInitialize initializePanel) yield return initializePanel.Initialize();

//                if (CurrentSubPanel && CurrentSubPanel.IsVisible)
//                {
//                    CurrentSubPanel.IsVisible = false;
//                    CurrentSubPanel.OnInvisible();
//                }
//                if (isVisibleWhenAvailable && !panel.IsVisible) panel.Show();
//            }

//            return new WaitOpenPanel<T>(panel, instantiatePrefabHandle, openAction);
//        }

//        /// <summary>
//        /// 异步打开窗口
//        /// </summary>
//        /// <typeparam name="T">窗口子类</typeparam>
//        public async Task<T> OpenSubPanelAsync<T>(bool isVisibleWhenAvailable = true) where T : Panel, new()
//        {
//            T panel = new T();
//            var type = typeof(T);
//            var name = type.Name;
//            var attribute = type.GetCustomAttribute<LoadUIPrefabByPathAttribute>();
//            var path = attribute.PrefabPath;
//            panel.Depth = attribute.Depth;
//            var prefab = await AssetModule.LoadAssetAsync<GameObject>(path);
//            var obj = Object.Instantiate(prefab, Transform);
//            AssetModule.Release(prefab);
//            panel.Init(name, obj, this);
//            InsertPanelByDepth(panel);
//            UIManager.Updater.AddUpdateFunction(panel.OnUpdate);
//            if (!isVisibleWhenAvailable) panel.Hide();
//            panel.OnOpen();

//            if (CurrentSubPanel && CurrentSubPanel.IsVisible)
//            {
//                CurrentSubPanel.IsVisible = false;
//                CurrentSubPanel.OnInvisible();
//            }
//            if (isVisibleWhenAvailable && !panel.IsVisible) panel.Show();

//            return panel;
//        }

//        /// <summary>
//        /// 协程方式切换窗口
//        /// </summary>
//        /// <typeparam name="T"></typeparam>
//        /// <param name="self"></param>
//        /// <param name="parent"></param>
//        /// <param name="beforeLoad">在加载新Panel之前先做些什么，比如打开Loading界面</param>
//        /// <param name="afterLoad">在加载新Panel之后先做些什么，比如关闭Loading界面</param>
//        /// <returns></returns>
//        public IEnumerator SwitchPanelByCoroutine<T>(bool isVisibleWhenAvailable = true, IEnumerator beforeLoad = null, IEnumerator afterLoad = null) where T : Panel, new()
//        {
//            if (beforeLoad != null) yield return beforeLoad;
//            foreach (var openedPanel in subPanels) if (openedPanel is T) goto Close;
//            var openCoroutine = parentPanel.OpenSubPanelByCoroutine<T>(isVisibleWhenAvailable);
//            yield return openCoroutine;
//            var panel = openCoroutine.Panel;
//            if (!isVisibleWhenAvailable && !panel.IsVisible) panel.Show();
//            Close:
//            Close();
//            if (afterLoad != null) yield return afterLoad;
//        }

//        /// <summary>
//        /// 插入panel，根据depth设置siblingIndex
//        /// </summary>
//        /// <param name="insertedPanel"></param>
//        internal void InsertPanelByDepth(Panel insertedPanel)
//        {
//            var currentPanel = subPanels.Last;
//            for (int i = subPanels.Count - 1; i > -1; i--)
//            {
//                if (currentPanel.Value.Depth < insertedPanel.Depth)
//                {
//                    subPanels.AddAfter(currentPanel, insertedPanel);
//                    insertedPanel.Transform.SetSiblingIndex(i + 1);
//                    return;
//                }
//                currentPanel = currentPanel.Previous;
//            }

//            subPanels.AddFirst(insertedPanel);
//            insertedPanel.Transform.SetSiblingIndex(0);
//        }

//        /// <summary>
//        /// 关闭自身
//        /// </summary>
//        public void Close()
//        {
//            parentPanel.CloseSubPanel(this);
//        }

//        /// <summary>
//        /// 关闭子面板
//        /// </summary>
//        /// <param name="subPanel"></param>
//        public void CloseSubPanel(Panel subPanel)
//        {
//            if (!subPanels.Remove(subPanel))
//            {
//                Debug.LogError($"no subPanel {subPanel.Name} in {Name}");
//                return;
//            }

//            subPanel.CloseSelf();
//        }

//        private void CloseSelf()
//        {
//            // 关闭面板前，先关闭所有子面板，否则看起来虽然子面板关闭了，但不会触发子面板的OnClose
//            foreach (var panel in subPanels) panel.CloseSelf();
//            subPanels.Clear();

//            // 关闭自身
//            UIManager.Updater.RemoveUpdateFunction(OnUpdate);
//            OnClose();

//            IsVisible = false;
//            OnInvisible();
//            if (parentPanel.CurrentSubPanel && !parentPanel.CurrentSubPanel.IsVisible)
//            {
//                parentPanel.CurrentSubPanel.IsVisible = true;
//                parentPanel.CurrentSubPanel.OnVisible();
//            }

//            Object.Destroy(GameObject);
//        }

//        public static implicit operator bool(Panel self) { return self != null; }

//        public virtual void OnUpdate(float deltaTime) { }
//        public virtual void OnOpen() { }
//        public virtual void OnClose() { }
//        public virtual void OnVisible() { }
//        public virtual void OnInvisible() { }

//        public T Find<T>(string childPath) where T : Object
//        {
//            Transform transform = Transform.Find(childPath);
//            if (!transform) { Debug.LogError($"can not find child in {childPath}"); return null; }
//            if (typeof(T) == typeof(GameObject)) return transform.gameObject as T;
//            if (typeof(T) == typeof(Transform)) return transform as T;
//            return transform.GetComponent<T>();
//        }

//    }
//}