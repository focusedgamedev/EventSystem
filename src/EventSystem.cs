//#define VERBOSE_LOGGING

using System;
using System.Collections.Generic;

namespace SimpleEvents
{
    public interface IEvent { }

    public class BaseEvent<T> : IEvent
    {
        public BaseEvent()
        {
            m_listeners = new List<T>();
        }

        public void AddListener( T listener )
        {
            m_listeners.Add( listener );
        }

        public void RemoveListener( T listener )
        {
            m_listeners.Remove( listener );
        }

        protected List<T> m_listeners;
    }

    public class Event<T> : BaseEvent<Action<T>>
    {
        public void Dispatch( T arg )
        {
            for ( int i = 0; i < m_listeners.Count; i++ )
            {
                m_listeners[i].Invoke( arg );
            }
        }
    }

    public class EventSystem
    {
        static EventSystem m_instance;
        public static EventSystem Instance
        {
            get
            {
                return m_instance;
            }
            private set { }
        }

				public EventSystem()
				{
						m_instance = this;
				}

        Dictionary<Type, IEvent> m_eventHandlers = new Dictionary<Type, IEvent>();

        public void Register<T>( Action<T> evt )
        {
            IEvent ievent;
            Event<T> _event;
            if ( m_eventHandlers.TryGetValue( typeof(T), out ievent ) )
            {
								_event = ievent as Event<T>;
                if (_event == null)
                {
                    throw new InvalidOperationException();
                }
            }
            else
            {
								_event = new Event<T>();
                m_eventHandlers.Add( typeof(T), _event);
            }

						_event.AddListener(evt);
        }

        public void Deregister<T>(Action<T> evt)
        {
            IEvent ievent;
            if ( m_eventHandlers.TryGetValue( typeof(T), out ievent ) )
            {
                Event<T> _event = ievent as Event<T>;
                if (_event == null)
                {
                    throw new InvalidOperationException();
                }

                _event.RemoveListener(evt);
            }
        }

        public void Dispatch<T>( T arg )
        {
            Dispatch<T>( typeof(T), arg );
        }

        public void Dispatch<T>( Type signalKey, T arg )
        {
            IEvent _evt;
            if ( m_eventHandlers.TryGetValue( signalKey, out _evt ) )
            {
                Event<T> _event = _evt as Event<T>;
                if (_event == null)
                {
                    throw new InvalidOperationException();
                }

                _event.Dispatch(arg);
            }
        }
    }
}