using StackExchange.Redis;
using System.Reflection;
using Newtonsoft.Json;
using GameLobbySignalRTemplate.Server.Models.Redis;

namespace GameLobbySignalRTemplate.Server.Utils
{
    public static class RedisUtils
    {
        //Serialize in Redis format:
        public static string SerializeJson(this object obj)
        {
            string json = JsonConvert.SerializeObject(obj);
            return json;
        }
        public static T DeserializeJson<T>(this string json)
        {
            T obj = JsonConvert.DeserializeObject<T>(json);
            return obj;
        }

    }
}
