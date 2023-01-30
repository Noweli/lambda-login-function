using System.Threading.Tasks;

namespace UserLoginFunction.ElastiCache;

public interface ICacheHandler
{
    Task<bool> SaveStringToCache(string key, string input);
}