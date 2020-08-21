using Dapper;
using Demo.ApiGateway.Core.Models;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Demo.ApiGateway.Core.DbConfiguration
{
    public class MySqlOcelotDbConfiguration : OcelotDbConfigurationBase
    {
        public override async Task<(GlobalConfiguration, List<Route>)> GetConfiguration(string connectionString)
        {
            string globalSql = @"
SELECT
	*
FROM
	GlobalConfiguration
WHERE
	IsDefault = 1
	AND InfoStatus = 1
";
            using (var connection = new MySqlConnection(connectionString))
            {
                //提取全局配置信息
                var globalResult = await connection.QueryFirstOrDefaultAsync<GlobalConfiguration>(globalSql);
                if (globalResult != null)
                {
                    //提取所有路由信息
                    string routeSql = @"
SELECT
	T2.*
FROM
	RouteMapping T1
INNER JOIN Route T2 ON
	T1.RouteId = T2.RouteId
WHERE
	GlobalId = @GlobalId
	AND InfoStatus = 1
";
                    var routeResult = (await connection.QueryAsync<Route>(routeSql, new { globalResult.GlobalId }))?.AsList();
                    if (routeResult != null && routeResult.Count > 0)
                    {
                        return (globalResult, routeResult);
                    }
                }

                throw new Exception("未监测到任何可用的配置信息");
            }
        }
    }
}
