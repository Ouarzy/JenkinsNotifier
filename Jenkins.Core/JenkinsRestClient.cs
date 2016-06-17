// The MIT License (MIT)
// Copyright (c) 2014 Grégory Ghez
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"), to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.

namespace Jenkins.Core
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using Domain;

    internal class JenkinsRestClient : IJenkinsRestClient
    {
        private readonly IRestManager restManager;

        public JenkinsRestClient(IRestManager restManager)
        {
            if (restManager == null)
            {
                throw new ArgumentNullException("restManager");
            }

            this.restManager = restManager;
        }

        public async Task<JenkinsServer> GetServerAsync(string url)
        {
            return await this.GetServerAsync(url, false);
        }

        public async Task<JenkinsServer> GetServerAsync(string url, bool depthLoad)
        {
            if (url == null)
            {
                throw new ArgumentNullException("url");
            }

            var server = new JenkinsServer { Url = url };
            server.Node = await this.restManager.LoadAsync<JenkinsNode>(server.NodeRestUrl);

            if (depthLoad && server.Node != null)
            {
                var users = await this.restManager.LoadAsync<JenkinsServer>(server.UsersRestUrl);
                if (server.Node.Jobs != null)
                {
                    server.Node.Jobs = server.Node.Jobs.Select(j => this.GetJobAsync(j.RestUrl, true).Result).ToArray();
                }

                if (server.Node.JenkinsViews != null)
                {
                    server.Node.JenkinsViews = server.Node.JenkinsViews.Select(v => this.GetViewAsync(v.DetailsUrl, true).Result).ToArray();

                    if (server.Node.PrimaryJenkinsView != null)
                    {
                        server.Node.PrimaryJenkinsView = server.Node.JenkinsViews.FirstOrDefault(v => string.Equals(v.Url, server.Node.PrimaryJenkinsView.Url, StringComparison.OrdinalIgnoreCase));
                    }
                }

                server.Users = users == null ? null : users.Users;
            }

            return server;
        }

        public async Task<JenkinsJob> GetJobAsync(string url)
        {
            return await this.GetJobAsync(url, false);
        }

        public async Task<JenkinsJob> GetJobAsync(string url, bool depthLoad)
        {
            if (url == null)
            {
                throw new ArgumentNullException("url");
            }

            JenkinsJob job = await this.restManager.LoadAsync<JenkinsJob>(url);

            if (job != null)
            {
                if (depthLoad && job.Builds != null)
                {
                    job.Builds = job.Builds.Select(b => this.GetBuildAsync(b.RestUrl, true).Result).ToArray();

                    if (job.FirstBuild != null)
                    {
                        job.FirstBuild = job.Builds.FirstOrDefault(b => string.Equals(b.Url, job.FirstBuild.Url, StringComparison.OrdinalIgnoreCase));
                    }

                    if (job.LastBuild != null)
                    {
                        job.LastBuild = job.Builds.FirstOrDefault(b => string.Equals(b.Url, job.LastBuild.Url, StringComparison.OrdinalIgnoreCase));
                    }

                    if (job.LastCompletedBuild != null)
                    {
                        job.LastCompletedBuild = job.Builds.FirstOrDefault(b => string.Equals(b.Url, job.LastCompletedBuild.Url, StringComparison.OrdinalIgnoreCase));
                    }

                    if (job.LastFailedBuild != null)
                    {
                        job.LastFailedBuild = job.Builds.FirstOrDefault(b => string.Equals(b.Url, job.LastFailedBuild.Url, StringComparison.OrdinalIgnoreCase));
                    }

                    if (job.LastStableBuild != null)
                    {
                        job.LastStableBuild = job.Builds.FirstOrDefault(b => string.Equals(b.Url, job.LastStableBuild.Url, StringComparison.OrdinalIgnoreCase));
                    }

                    if (job.LastSuccessfulBuild != null)
                    {
                        job.LastSuccessfulBuild = job.Builds.FirstOrDefault(b => string.Equals(b.Url, job.LastSuccessfulBuild.Url, StringComparison.OrdinalIgnoreCase));
                    }

                    if (job.LastUnstableBuild != null)
                    {
                        job.LastUnstableBuild = job.Builds.FirstOrDefault(b => string.Equals(b.Url, job.LastUnstableBuild.Url, StringComparison.OrdinalIgnoreCase));
                    }

                    if (job.LastUnsuccessfulBuild != null)
                    {
                        job.LastUnsuccessfulBuild = job.Builds.FirstOrDefault(b => string.Equals(b.Url, job.LastUnsuccessfulBuild.Url, StringComparison.OrdinalIgnoreCase));
                    }
                }
                else
                {
                    if (job.FirstBuild != null)
                    {
                        job.FirstBuild = await GetBuildAsync(job.FirstBuild.RestUrl, depthLoad);
                    }

                    if (job.LastBuild != null)
                    {
                        job.LastBuild = await GetBuildAsync(job.LastBuild.RestUrl, depthLoad);
                    }

                    if (job.LastCompletedBuild != null)
                    {
                        job.LastCompletedBuild = await GetBuildAsync(job.LastCompletedBuild.RestUrl, depthLoad);
                    }

                    if (job.LastFailedBuild != null)
                    {
                        job.LastFailedBuild = await GetBuildAsync(job.LastFailedBuild.RestUrl, depthLoad);
                    }

                    if (job.LastStableBuild != null)
                    {
                        job.LastStableBuild = await GetBuildAsync(job.LastStableBuild.RestUrl, depthLoad);
                    }

                    if (job.LastSuccessfulBuild != null)
                    {
                        job.LastSuccessfulBuild = await GetBuildAsync(job.LastSuccessfulBuild.RestUrl, depthLoad);
                    }

                    if (job.LastUnstableBuild != null)
                    {
                        job.LastUnstableBuild = await GetBuildAsync(job.LastUnstableBuild.RestUrl, depthLoad);
                    }

                    if (job.LastUnsuccessfulBuild != null)
                    {
                        job.LastUnsuccessfulBuild = await GetBuildAsync(job.LastUnsuccessfulBuild.RestUrl, depthLoad);
                    }
                }
            }

            return job;
        }

        public async Task<JenkinsJob> GetJobAsyncWithoutDepthInfos(string url)
        {
            if (url == null)
            {
                throw new ArgumentNullException("url");
            }

            return await this.restManager.LoadAsync<JenkinsJob>(url);
        }

        public async Task<JenkinsView> GetViewAsync(string url)
        {
            return await this.GetViewAsync(url, false);
        }

        public async Task<JenkinsView> GetViewAsync(string url, bool depthLoad)
        {
            if (url == null)
            {
                throw new ArgumentNullException("url");
            }

            JenkinsView jenkinsView = await this.restManager.LoadAsync<JenkinsView>(url);

            if (depthLoad && jenkinsView != null && jenkinsView.Jobs != null)
            {
                jenkinsView.Jobs = jenkinsView.Jobs.Select(j => this.GetJobAsync(j.RestUrl, true).Result).ToArray();
            }

            return jenkinsView;
        }

        public async Task<JenkinsBuild> GetBuildAsync(string url)
        {
            return await this.GetBuildAsync(url, false);
        }

        public async Task<JenkinsBuild> GetBuildAsync(string url, bool depthLoad)
        {
            if (url == null)
            {
                throw new ArgumentNullException("url");
            }

            JenkinsBuild build = await this.restManager.LoadAsync<JenkinsBuild>(url);

            if (depthLoad && build != null)
            {
                build.TestReport = await GetTestReportAsync(build.TestRestUrl);
                // There is actually nothing lazy loaded nested in REST build structure.
            }

            return build;
        }

        public async Task<JenkinsTestReport> GetTestReportAsync(string url)
        {
            if (url == null)
            {
                throw new ArgumentNullException("url");
            }

            JenkinsTestReport report = await this.restManager.LoadAsync<JenkinsTestReport>(url);

            return report;
        }

        public async Task RunJobAsync(string buildRestUrl)
        {
            if (buildRestUrl == null)
            {
                throw new ArgumentNullException("buildRestUrl");
            }

            await this.restManager.PostDataAsync(buildRestUrl, string.Empty);
        }

        public async Task StopJobAsync(string stopRestUrl)
        {
            if (string.IsNullOrEmpty(stopRestUrl))
            {
                throw new ArgumentNullException("stopRestUrl");
            }

            await this.restManager.PostDataAsync(stopRestUrl, string.Empty);
        }
    }
}