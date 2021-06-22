using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System.Security.Cryptography;
using System.Text;

namespace BasicSccExample
{
    class Program
    {
        static void Main(string[] args)
        {
            new TfsConnector.App.Service.TfsServiceConnection().Run();
        }
    }
}
