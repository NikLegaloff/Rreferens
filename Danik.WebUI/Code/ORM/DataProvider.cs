using Newtonsoft.Json;
using System.Collections.Concurrent;

namespace Danik.WebUI.Code.ORM;

public class DataProvider<T> where T : DomainObject
{
    private readonly Config _config;
    private readonly ConcurrentDictionary<Guid, T> _identityMap = new();
    private readonly string _tableName;

    public DataProvider(Config config)
    {
        _tableName = typeof(T).Name;
        _config = config;
        if (!Directory.Exists(BasePath)) Directory.CreateDirectory(BasePath);
    }

    string BasePath => _config.BasePath+"DB\\" + _tableName + "\\";
    

    public T? Find(Guid id)
    {
        if (_identityMap.TryGetValue(id, out var find)) return find;
        if (!File.Exists(BasePath + id)) return null;
        try
        {
            var readAllText = File.ReadAllText(BasePath + id);
            var subj = JsonConvert.DeserializeObject<T>(readAllText);
            subj.Id = id;
            _identityMap.TryAdd(id, subj); 
            return subj;
        }
        catch (FileNotFoundException)
        {
            return null;
        }
    }

    private T[] Load(Guid[] ids)
    {
        var res = new List<T>();
        foreach (var id in ids)
        {
            var item = Find(id);
            if (item != null) res.Add(item);
        }
        return res.ToArray();
    }


    public T[] SelectAll() => Load(Directory.GetFiles(BasePath).Select(file => file.Split('\\').Last().ToGuid()).ToArray());


    public void Save(T subj)
    {
        if (subj.Id == Guid.Empty) Insert(subj); else Update(subj);
    }

    private void Insert(T subj)
    {
        subj.Id = Guid.NewGuid();
        _identityMap[subj.Id] = subj;
        File.WriteAllText(BasePath + subj.Id, subj.ToJsonMax());
    }


    private void Update(T subj)
    {
        File.WriteAllText(BasePath + subj.Id, subj.ToJsonMax());
    }

    public void Delete(Guid id)
    {
        if (_identityMap.ContainsKey(id)) _identityMap.Remove(id,out _);
        if (File.Exists(BasePath + id)) return;
        Find(id)?.OnDelete();
        File.Delete(BasePath + id);
    }
}