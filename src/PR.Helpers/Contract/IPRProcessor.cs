using System.Threading.Tasks;
using PR.Helpers.Models;

namespace PR.Helpers.Contract
{
    public interface IPRProcessor
    {
        Task HandleARM_PR(VstsRequest req);
    }
}