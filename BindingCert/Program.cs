using System;
using System.Diagnostics;
using System.IO;
using System.Security.Cryptography.X509Certificates;

namespace BindCertToPort
{
    class Program
    {
        static void Main(string[] args)
        {
            int port = 1234;
            string certPath = "C:\\Program Files (x86)\\Microsoft SDKs\\Windows\\v7.1A\\Bin\\MyAdHocTestCert.cer";
            X509Certificate2 certificate = new X509Certificate2(certPath);

            // netsh http add sslcert ipport=0.0.0.0:<port> certhash={<thumbprint>} appid={<some GUID>}
            Process bindPortToCertificate = new Process();
            bindPortToCertificate.StartInfo.FileName = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.SystemX86), "netsh.exe");
            bindPortToCertificate.StartInfo.Arguments = string.Format("http add sslcert ipport=0.0.0.0:{0} certhash={1} appid={{{2}}}", port, certificate.Thumbprint, Guid.NewGuid());
            bindPortToCertificate.Start();
            bindPortToCertificate.WaitForExit();
        }
    }
}