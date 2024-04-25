using Dapper;
using brokenaccesscontrol.Models;
using System.Security.Cryptography;
using System.Text;

namespace brokenaccesscontrol.Services;

public static class UtilService
{
    public static string ReturnMD5(string value){        
        var valueBytes = Encoding.UTF8.GetBytes(value);
        var hashmd = MD5.Create();
        var hash = hashmd.ComputeHash(valueBytes);
        return Convert.ToBase64String(hash);
    }
}
