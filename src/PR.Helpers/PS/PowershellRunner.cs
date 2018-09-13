//using System;
//using System.Collections.Generic;
//using System.Diagnostics;
//using System.Linq;
//using System.Management.Automation;
//using System.Text;
//using PR.Helpers.Contract;

//namespace PR.Helpers.PS
//{
//    public class PowershellRunner : IPowershellRunner
//    {
//        public List<string> RunScript(string script, params Tuple<string,string>[] psParams)
//        {
//            using (var psi = PowerShell.Create())
//            {
//                if (psParams != null)
//                {
//                    foreach (var p in psParams)
//                    {
//                        psi.AddParameter(p.Item1, p.Item2);
//                    }
//                }

//                var output = psi.Invoke();
//                if (psi.HadErrors)
//                {
//                    var all = psi.Streams.Error.ReadAll();
//                    foreach(var aItem in all)
//                    {
//                        Debug.WriteLine(aItem.ErrorDetails);
//                    }
//                }
//                var result = output.Select(_ => _.BaseObject.ToString()).ToList();
                
//                return result;
//            }
//        }
//    }
//}
