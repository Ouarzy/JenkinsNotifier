namespace Jenkins.Notifier.Services
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using Jenkins.Notifier.Model;

    public interface IJenkinService
    {
        Task<IEnumerable<JenkinsViewViewModel>> Views();
    }
}