using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MailSendWPF.DesignPattern
{
    public sealed class Singleton<T> where T : class, new()
    {
        static T m_Instance = null;

        /// <summary>
        /// this variable is used for thread safety
        /// </summary>
        static readonly object m_Padlock = new object();

        /// <summary>
        /// returns the reference of the singleton
        /// </summary>
        public static T Instance
        {
            get
            {
                lock (m_Padlock)
                {
                    if (m_Instance == null)
                    {
                        m_Instance = new T();
                    }
                    return m_Instance;
                }
            }
        }
    }


}
