using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Application.Services.Accounts.Helpers;
public static class IBANGenerator
{
    internal static string GenerateAccountNumber()
    {
        StringBuilder sb = new StringBuilder();
        Random random  = new Random();

        sb.Append("GE");
        sb.Append(random.Next(10,99));
        sb.Append("TR");
        for (int i = 0; i < 14; i++)
        {
            sb.Append(random.Next(0, 10));
        }
        return sb.ToString();
    }
}
