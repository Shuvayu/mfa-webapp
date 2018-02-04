using System.Threading.Tasks;

namespace MFA.IInfrastructure
{
    public interface IWaitCall
    {
        /// <summary>
        /// Adds the current time to the static timestamp queue.
        /// Call this before a call.
        /// </summary>
        void AddCallTimeToQueue();

        /// <summary>
        /// Adds a delay if the calls have reached above the call per second threshold
        /// </summary>
        /// <returns></returns>
        Task WaitCallLimitPerMinAsync();
    }
}
