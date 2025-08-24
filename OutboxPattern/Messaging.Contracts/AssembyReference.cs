using System.Reflection;

namespace Messaging.Contracts
{
    public static class AssembyReference
    {
        public static readonly Assembly Assembly = typeof(AssembyReference).Assembly;
    }
}
