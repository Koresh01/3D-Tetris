namespace Mycom.Target.Unity.Internal
{
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using Interfaces;
    using UnityEngine;
    using Object = System.Object;

    internal sealed class UnityMainThreadDispatcher : MonoBehaviour, IDispatcher
    {
        private static readonly Object Sync = new Object();
        private static readonly Queue<Action> Queue = new Queue<Action>();

        private static Int32 _mainThreadId;
        private static GameObject GameObject;

        private static Int32 GetThreadId() => Thread.CurrentThread.ManagedThreadId;

        internal static IDispatcher GetInstance()
        {
            try
            {
                if (GameObject == null)
                {
                    lock (Sync)
                    {
                        if (GameObject == null)
                        {
                            var gameObject = new GameObject();

                            gameObject.AddComponent<UnityMainThreadDispatcher>();

                            GameObject = gameObject;

                            DontDestroyOnLoad(GameObject);

                            _mainThreadId = GetThreadId();
                        }
                    }
                }
                return GameObject.GetComponent<UnityMainThreadDispatcher>();
            }
            catch (Exception ex)
            {
                Debug.Log(ex.Message);
            }

            return null;
        }

        void Update()
        {
            lock (Queue)
            {
                if (Queue.Count <= 0)
                {
                    return;
                }

                try
                {
                    Queue.Dequeue()();
                }
                catch (Exception ex)
                {
                    Debug.Log(ex.Message);
                }
            }
        }

        void OnDestroy()
        {
            lock (Sync)
            {
                GameObject = null;
            }
        }

        void IDispatcher.Perform(Action action)
        {
            if (GetThreadId() == _mainThreadId)
            {
                try
                {
                    action();
                }
                catch (Exception ex)
                {
                    Debug.Log(ex.Message);
                }
            }
            else
            {
                lock (Queue)
                {
                    Queue.Enqueue(action);
                }
            }
        }
    }
}
