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
    using System.Threading.Tasks;
    using Domain;

    /// <summary>
    /// Defines methods that give access to Jenkins REST API.
    /// </summary>
    public interface IJenkinsRestClient
    {
        /// <summary>
        /// Gets a <see cref="JenkinsServer" /> from given Jenkins web server root url. No depth load executed.
        /// </summary>
        /// <param name="url">The Jenkins web server root URL.</param>
        /// <returns>
        /// A <see cref="JenkinsServer" /> object.
        /// </returns>
        /// <exception cref="ArgumentNullException">If <c>url</c> parameter is null.</exception>
        /// <example>http://jenkins-server/</example>
        Task<JenkinsServer> GetServerAsync(string url);

        /// <summary>
        /// Gets a <see cref="JenkinsServer" /> from given Jenkins web server root url.
        /// </summary>
        /// <param name="url">The Jenkins web server root URL.</param>
        /// <param name="depthLoad">if set to <c>true</c> all nested Jenkins objects will be loaded.</param>
        /// <returns>
        /// A <see cref="JenkinsServer" /> object.
        /// </returns>
        /// <exception cref="ArgumentNullException">If <c>url</c> parameter is null.</exception>
        /// <example>http://jenkins-server/</example>
        Task<JenkinsServer> GetServerAsync(string url, bool depthLoad);

        /// <summary>
        /// Gets a <see cref="JenkinsJob" /> from given job REST url. No depth load executed.
        /// </summary>
        /// <param name="url">The job REST URL.</param>
        /// <returns>
        /// A <see cref="JenkinsJob" /> object.
        /// </returns>
        /// <exception cref="ArgumentNullException">If <c>url</c> parameter is null.</exception>
        /// <example>http://jenkins-server/job/ContinuousIntegration/api/json</example>
        Task<JenkinsJob> GetJobAsync(string url);

        /// <summary>
        /// Gets a <see cref="JenkinsJob" /> from given job REST url.
        /// </summary>
        /// <param name="url">The URL.</param>
        /// <param name="depthLoad">if set to <c>true</c> all nested Jenkins objects will be loaded.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">If <c>url</c> parameter is null.</exception>
        /// <example>http://jenkins-server/job/ContinuousIntegration/api/json</example>
        Task<JenkinsJob> GetJobAsync(string url, bool depthLoad);

        /// <summary>
        /// Gets a <see cref="JenkinsView" /> from given view REST url. No depth load executed.
        /// </summary>
        /// <param name="url">The view REST URL.</param>
        /// <returns>
        /// A <see cref="JenkinsView" /> object.
        /// </returns>
        /// <exception cref="ArgumentNullException">If <c>url</c> parameter is null.</exception>
        /// <example>http://jenkins-server/view/ReleaseJobs/api/json</example>
        Task<JenkinsView> GetViewAsync(string url);

        /// <summary>
        /// Gets a <see cref="JenkinsView" /> from given view REST url.
        /// </summary>
        /// <param name="url">The view REST URL.</param>
        /// <param name="depthLoad">if set to <c>true</c> all nested Jenkins objects will be loaded.</param>
        /// <returns>
        /// A <see cref="JenkinsView" /> object.
        /// </returns>
        /// <exception cref="ArgumentNullException">If <c>url</c> parameter is null.</exception>
        /// <example>http://jenkins-server/view/ReleaseJobs/api/json</example>
        Task<JenkinsView> GetViewAsync(string url, bool depthLoad);

        /// <summary>
        /// Gets a <see cref="JenkinsBuild" /> from given view REST url.
        /// </summary>
        /// <param name="url">The build REST URL.</param>
        /// <returns>
        /// A <see cref="JenkinsBuild" /> object.
        /// </returns>
        /// <exception cref="ArgumentNullException">If <c>url</c> parameter is null.</exception>
        /// <example>http://jenkins-server/job/ContinuousIntegration/18/api/json</example>
        Task<JenkinsBuild> GetBuildAsync(string url);

        /// <summary>
        /// Gets a <see cref="JenkinsBuild" /> from given build REST url.
        /// </summary>
        /// <param name="url">The build REST URL.</param>
        /// <param name="depthLoad">if set to <c>true</c> [depth load].</param>
        /// <returns>
        /// A <see cref="JenkinsBuild" /> object.
        /// </returns>
        /// <exception cref="ArgumentNullException">If <c>url</c> parameter is null.</exception>
        /// <example>http://jenkins-server/job/ContinuousIntegration/18/api/json</example>
        Task<JenkinsBuild> GetBuildAsync(string url, bool depthLoad);

        /// <summary>
        /// Gets a <see cref="JenkinsTestReport" /> from given test report REST url.
        /// </summary>
        /// <param name="url">The test report REST URL.</param>
        /// <returns>
        /// A <see cref="JenkinsTestReport" /> object.
        /// </returns>
        /// <exception cref="ArgumentNullException">If <c>url</c> parameter is null.</exception>
        /// <example>http://jenkins-server/job/ContinuousIntegration/18/testReport/api/json</example>
        Task<JenkinsTestReport> GetTestReportAsync(string url);

        /// <summary>
        /// Runs a <see cref="JenkinsJob" /> on Jenkins build factory.
        /// </summary>
        /// <param name="buildRestUrl">The url to build the job.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">If <c>job</c> parameter is null.</exception>
        Task RunJobAsync(string buildRestUrl);

        Task StopJobAsync(string stopRestUrl);
    }
}