using System;
using System.Configuration;
using System.Web;
using NHibernate;
using Configuration = NHibernate.Cfg.Configuration;

namespace Automation.Common
{
    public class NHibernateHelper
    {
        ///// <summary>
        ///// 返回 ISession 回话
        ///// </summary>
        //public static ISession GetSession()
        //{
        //    var dir = AppDomain.CurrentDomain.BaseDirectory;
        //    var fileName = System.IO.Path.Combine(dir, "hibernate.cfg.xml");
        //    return (new Configuration()).Configure(fileName).BuildSessionFactory().OpenSession();
        //}
        private static ISessionFactory _sessionFactory;
        private static ISession _session;
        private static object _objLock = new object();
        private NHibernateHelper()
        {

        }
        /// <summary>
        /// 创建ISessionFactory
        /// </summary>
        /// <returns></returns>
        public static ISessionFactory GetSessionFactory()
        {
            if (_sessionFactory == null)
            {
                lock (_objLock)
                {
                    if (_sessionFactory == null)
                    {
                        //配置ISessionFactory
                        _sessionFactory = (new Configuration()).Configure().BuildSessionFactory();
                    }
                }
            }
            return _sessionFactory;

        }
        /// <summary>
        /// 重置Session
        /// </summary>
        /// <returns></returns>
        public static ISession ResetSession()
        {
            if (_session.IsOpen)
                _session.Close();
            _session = _sessionFactory.OpenSession(); ;
            return _session;
        }
        /// <summary>
        /// 打开ISession
        /// </summary>
        /// <returns></returns>
        public static ISession GetSession()
        {
            _sessionFactory = GetSessionFactory();
            if (_session == null)
            {
                lock (_objLock)
                {
                    if (_session == null)
                    {
                        _session = _sessionFactory.OpenSession();
                    }
                }
            }
            return _session;
        }
    }
}

