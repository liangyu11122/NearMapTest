using System.Collections.Generic;

namespace NMTest.DataSource
{
    public interface IDatabaseStore
    {
	    Dictionary<string, string> LoadFromDatabase();
	    string GetValueFromDatabase(string key);
    }
}
