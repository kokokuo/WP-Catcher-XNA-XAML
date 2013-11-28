using System;
using System.Collections.Generic;

namespace WP_CatcherGame_XNA_XAML
{
    /// <summary>
    /// 實作應用程式的 IServiceProvider。這個型別透過 App.Services
    /// 屬性公開，並且可以用於 ContentManagers 或其他需要存取 IServiceProvider 的型別。
    /// </summary>
    public class AppServiceProvider : IServiceProvider
    {
        // 服務本身與服務型別的對應
        private readonly Dictionary<Type, object> services = new Dictionary<Type, object>();

        /// <summary>
        /// 將新的服務加入服務提供者。
        /// </summary>
        /// <param name="serviceType">要加入的服務類型。</param>
        /// <param name="service">服務物件本身。</param>
        public void AddService(Type serviceType, object service)
        {
            // 驗證輸入
            if (serviceType == null)
                throw new ArgumentNullException("serviceType");
            if (service == null)
                throw new ArgumentNullException("service");
            if (!serviceType.IsAssignableFrom(service.GetType()))
                throw new ArgumentException("service does not match the specified serviceType");

            // 將服務加入字典
            services.Add(serviceType, service);
        }

        /// <summary>
        /// 從服務提供者取得服務。
        /// </summary>
        /// <param name="serviceType">要擷取的服務類型。</param>
        /// <returns>註冊為指定之型別的服務物件..</returns>
        public object GetService(Type serviceType)
        {
            // 驗證輸入
            if (serviceType == null)
                throw new ArgumentNullException("serviceType");

            // 從字典擷取服務
            return services[serviceType];
        }

        /// <summary>
        /// 從服務提供者移除服務。
        /// </summary>
        /// <param name="serviceType">要移除的服務類型。</param>
        public void RemoveService(Type serviceType)
        {
            // 驗證輸入
            if (serviceType == null)
                throw new ArgumentNullException("serviceType");

            // 從字典移除服務
            services.Remove(serviceType);
        }
    }
}
