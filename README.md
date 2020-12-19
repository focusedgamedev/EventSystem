# EventSystem
Simple Event System

## Example Usage

```
using System;
using SimpleEvents;

namespace EventSystemWorking
{
		public class StringEventData
		{
				private StringEventData() { }
				public StringEventData(string text)
				{
						m_text = text;
				}

				string m_text;

				public string String
				{
						get { return m_text; }
						private set { }
				}
		}

		public class Test
		{
				public void OnStringData(StringEventData data)
				{
						Console.WriteLine("String Event! - " + data.String );
				}

				public void RunTest()
				{
						EventSystem eventSystem = new EventSystem();
						eventSystem.Register<StringEventData>(OnStringData);

						StringEventData data = new StringEventData("This is a string...");

						eventSystem.Dispatch(data);
						eventSystem.Deregister<StringEventData>(OnStringData);
				}
		}

		class Program
		{
				static void Main(string[] args)
				{
						Test test = new Test();
						test.RunTest();
						Console.WriteLine("Done...");
				}
		}
}
```

