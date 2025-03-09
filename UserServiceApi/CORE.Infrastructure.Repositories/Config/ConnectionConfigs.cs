using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CORE.Infrastructure.Repositories.Config
{
    public class ConnectionConfigs
    {
        public const string ConfigName = "ConnectionStrings";
        public string? PLSQLServerPartnerConnection { get; set; } = string.Empty;
        public string? PLSQLServerDWParConnection { get; set; } = string.Empty;
        public string? MSSQLServerInsideRead { get; set; } = string.Empty;
        public string? MongoDbPartnerDbConnection { get; set; } = string.Empty;
        public string? RedisCache { get; set; } = string.Empty;
    }

    public class JwtSettings
    {
        public const string ConfigName = "JwtSettings";

        public string Key { get; set; } = string.Empty;

        public string Issuer {  get; set; } = string.Empty;

        public string Audience { get; set; } = string.Empty;
    }

    public class RedisConfigs
    {
        public const string ConfigName = "RedisConfigs";
        public int Db { get; set; } = -1;
        public bool Caching { get; set; } = true;
        public string InstanceName { get; set; } = string.Empty;
    }
}
