using System;
using System.Collections.Generic;
using System.Text;

namespace PR.Helpers.Contract
{
    public interface IPowershellRunner
    {
        List<string> RunScript(string script, params Tuple<string,string>[] psParams);
    }
}
