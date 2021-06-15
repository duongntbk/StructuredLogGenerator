using System.Threading.Tasks;

namespace StructuredLogGenerator
{
    public interface ILogGenerator
    {
        Task Generate(int logCount);
    }
}
