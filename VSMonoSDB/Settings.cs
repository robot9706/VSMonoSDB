using Microsoft.VisualStudio.Settings;

namespace VSMonoSDB
{
	public class Settings
	{
		private const string CollectionName = "VSMonoSDB";

		private WritableSettingsStore _store;

		public int DebugPort
		{
			get
			{
				return _store.GetInt32(CollectionName, "DefaultDebugPort", 9000);
			}
			set
			{
				_store.SetInt32(CollectionName, "DefaultDebugPort", value);
			}
		}

		public string MonoPath
		{
			get
			{
				return _store.GetString(CollectionName, "MonoPath", string.Empty);
			}
			set
			{
				_store.SetString(CollectionName, "MonoPath", value);
			}
		}

		public string LastRemoteIP
		{
			get
			{
				return _store.GetString(CollectionName, "LastRemoteIP", "127.0.0.1");
			}
			set
			{
				_store.SetString(CollectionName, "LastRemoteIP", value);
			}
		}

		public int LastRemotePort
		{
			get
			{
				return _store.GetInt32(CollectionName, "LatRemotePort", DebugPort);
			}
			set
			{
				_store.SetInt32(CollectionName, "LatRemotePort", value);
			}
		}

		public Settings(WritableSettingsStore store)
		{
			_store = store;

			if (!_store.CollectionExists(CollectionName))
				_store.CreateCollection(CollectionName);
		}
	}
}
