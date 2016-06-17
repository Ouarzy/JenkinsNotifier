using Jenkins.Notifier.ViewModel.Commands;

namespace Jenkins.Notifier.Services
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Core;
    using Domain;
    using Properties;
    using Model;
    using ViewModel;

    public class JenkinsService : IJenkinService
    {
        private readonly IJenkinsRestClient jenkinsClient;

        private readonly string serverUrl;

        private readonly IList<JenkinsViewModel> jenkinsViewModels;

        public JenkinsService(IJenkinsRestClient jenkinsClient, string serverUrl, IList<JenkinsViewModel> jenkinsViewModels)
        {
            this.jenkinsClient = jenkinsClient;
            this.serverUrl = serverUrl;
            this.jenkinsViewModels = jenkinsViewModels;
        }

        private bool ViewsExisting
        {
            get { return this.jenkinsViewModels != null && this.jenkinsViewModels.Any(); }
        }

        public async Task<IEnumerable<JenkinsViewViewModel>> Views()
        {
            if (this.ViewsExisting)
            {
                return await this.RefreshKnownViews();
            }

            return await this.GetAllViewsFromServer();
        }

        private async Task<IEnumerable<JenkinsViewViewModel>> GetAllViewsFromServer()
        {
            var views = new List<JenkinsViewViewModel>();
            var server = await this.jenkinsClient.GetServerAsync(this.serverUrl, false);

            var jobs = await this.GetJenkinsJobsForView(server.Node.Jobs);
            views.Add(new JenkinsViewViewModel(Resources.All, jobs));
            return views;
        }

        private async Task<IEnumerable<JenkinsViewViewModel>> RefreshKnownViews()
        {
            var views = new List<JenkinsViewViewModel>();
            foreach (var jenkinsViewModel in this.jenkinsViewModels)
            {
                var viewJenkins = await this.jenkinsClient.GetViewAsync(this.serverUrl + "/view/" + jenkinsViewModel.View + "/api/json", false);
                var jenkinsViewModelCopy = jenkinsViewModel;

                var jobsToUpdate = jenkinsViewModel.Jobs.Any() ? viewJenkins.Jobs.Where(o => jenkinsViewModelCopy.Jobs.Contains(o.DisplayName)) : viewJenkins.Jobs;
                var jobs = await this.GetJenkinsJobsForView(jobsToUpdate);
                views.Add(new JenkinsViewViewModel(jenkinsViewModel.View, jobs));
            }
            return views;
        }

        private async Task<IEnumerable<JenkinsJobViewModel>> GetJenkinsJobsForView(IEnumerable<JenkinsJob> jenkinsJobs)
        {
            var jobs = new List<JenkinsJobViewModel>();

            foreach (var job in jenkinsJobs)
            {
                var jobView = await this.CreateJenkinsJobViewModel(job);
                jobs.Add(jobView);
            }

            return jobs;
        }

        private async Task<JenkinsJobViewModel> CreateJenkinsJobViewModel(JenkinsJob job)
        {
            var jobUpdated = await this.jenkinsClient.GetJobAsync(job.RestUrl);
            var lastBuildNumber = this.GetLastRanBuildNumber(jobUpdated);
            var buildRunning = this.GetBuildRunningState(jobUpdated);
            var startBuildUrl = jobUpdated.BuildRestUrl;
            var stopBuildUrl = buildRunning ? this.GetRunningBuild(jobUpdated).StopRestUrl : string.Empty;

            return new JenkinsJobViewModel(job.DisplayName, this.ConvertJenkinsJobToJob(job.Status), job.Url, lastBuildNumber, new RunBuildCommand(this.jenkinsClient, startBuildUrl), new StopBuildCommand(this.jenkinsClient, stopBuildUrl), buildRunning);
        }

        private int GetLastRanBuildNumber(JenkinsJob job)
        {
            var lastBuild = job.Builds
                .Where(b => job.LastCompletedBuild != null && b.Number == job.LastCompletedBuild.Number
                         || job.LastFailedBuild != null && b.Number == job.LastFailedBuild.Number)
                .OrderByDescending(b => b.Number)
                .FirstOrDefault();
            return lastBuild != null ? lastBuild.Number : 0;
        }

        private bool GetBuildRunningState(JenkinsJob job)
        {
            return this.GetRunningBuild(job) != null;
        }

        private JenkinsBuild GetRunningBuild(JenkinsJob job)
        {
            return job.Builds
                .OrderBy(build => build.Number)
                .FirstOrDefault(build => (job.LastCompletedBuild == null || build.Number > job.LastCompletedBuild.Number) && 
                                         (job.LastFailedBuild == null || build.Number > job.LastFailedBuild.Number));
        }

        private JobStatus ConvertJenkinsJobToJob(JenkinsJobStatus status)
        {
            switch (status.State)
            {
                case JenkinsJobState.Aborted:
                    return JobStatus.Aborted;
                case JenkinsJobState.Disabled:
                    return JobStatus.Disabled;
                case JenkinsJobState.Failed:
                    return JobStatus.Failed;
                case JenkinsJobState.Success:
                    return JobStatus.Success;
                case JenkinsJobState.Unstable:
                    return JobStatus.Unstable;
                default:
                    return JobStatus.None;
            }
        }
    }
}