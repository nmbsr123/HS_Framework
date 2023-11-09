namespace framework
{
    using System;

    /// <summary>
    /// 事件工具
    /// <para>ZhangYu 2019-12-25</para>
    /// </summary>
    public static class EventUtil {

        /// <summary> 事件发送器 </summary>
        private static EventSender<Enum, object> sender = new EventSender<Enum, object>();

        /// <summary> 添加事件监听器 </summary>
        /// <param name="eventType">事件类型</param>
        /// <param name="eventHandler">事件处理器</param>
        public static void AddListener(Enum eventType, Action<object> eventHandler) {
            sender.AddListener(eventType, eventHandler);
        }

        /// <summary> 移除事件监听器 </summary>
        /// <param name="eventType">事件类型</param>
        /// <param name="eventHandler">事件处理器</param>
        public static void RemoveListener(Enum eventType, Action<object> eventHandler) {
            sender.RemoveListener(eventType, eventHandler);
        }

        /// <summary> 是否已经拥有该类型的事件监听器 </summary>
        /// <param name="eventType">事件类型</param>
        public static bool HasListener(Enum eventType) {
            return sender.HasListener(eventType);
        }

        /// <summary> 发送事件 </summary>
        /// <param name="eventType">事件类型</param>
        public static void SendMessage(Enum eventType) {
            sender.SendMessage(eventType, null);
        }

        /// <summary> 发送事件 </summary>
        /// <param name="eventType">事件类型</param>
        public static void SendMessage(Enum eventType, object eventArg) {
            sender.SendMessage(eventType, eventArg);
        }

        /// <summary> 清理所有事件监听器 </summary>
        public static void Clear() {
            sender.Clear();
        }
    }
}