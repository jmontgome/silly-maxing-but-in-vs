using Microsoft.VisualStudio;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using System;
using System.IO;
using System.Media;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Threading;
using Task = System.Threading.Tasks.Task;

namespace ErrorNotifications
{
    /// <summary>
    /// This is the class that implements the package exposed by this assembly.
    /// </summary>
    /// <remarks>
    /// <para>
    /// The minimum requirement for a class to be considered a valid package for Visual Studio
    /// is to implement the IVsPackage interface and register itself with the shell.
    /// This package uses the helper classes defined inside the Managed Package Framework (MPF)
    /// to do it: it derives from the Package class that provides the implementation of the
    /// IVsPackage interface and uses the registration attributes defined in the framework to
    /// register itself and its components with the shell. These attributes tell the pkgdef creation
    /// utility what data to put into .pkgdef file.
    /// </para>
    /// <para>
    /// To get loaded into VS, the package must be referred by &lt;Asset Type="Microsoft.VisualStudio.VsPackage" ...&gt; in .vsixmanifest file.
    /// </para>
    /// </remarks>
    [ProvideAutoLoad(VSConstants.UICONTEXT.SolutionExists_string, PackageAutoLoadFlags.BackgroundLoad)]
    [PackageRegistration(UseManagedResourcesOnly = true, AllowsBackgroundLoading = true)]
    [Guid(ErrorNotificationsPackage.PackageGuidString)]
    public sealed class ErrorNotificationsPackage : AsyncPackage, IVsUpdateSolutionEvents
    {        
        /// <summary>
        /// ErrorNotificationsPackage GUID string.
        /// </summary>
        public const string PackageGuidString = "abc4d583-78ef-480d-87a1-efac9dbc69ee";

        #region Package Members
        private void OnBuildDone(int fSucceeded)
        {
            ThreadHelper.ThrowIfNotOnUIThread();
            if (fSucceeded == 0)
            {
                var assembly = Assembly.GetExecutingAssembly();
                using (Stream stream = assembly.GetManifestResourceStream("ErrorNotifications.Sounds.Error.wav"))
                {
                    new SoundPlayer(stream).Play();
                }
            }
        }

        public int UpdateSolution_Begin(ref int pfCancelUpdate)
        {
            return 0;
        }
        public int UpdateSolution_Done(int fSucceeded, int fModified, int fCancelCommand)
        {
            ThreadHelper.ThrowIfNotOnUIThread();

            if (fCancelCommand == 0)
            {
                OnBuildDone(fSucceeded);
            }

            return 0;
        }
        public int OnActiveProjectCfgChange(IVsHierarchy pIVsHierarchy)
        {
            return 0;
        }

        /// <summary>
        /// Initialization of the package; this method is called right after the package is sited, so this is the place
        /// where you can put all the initialization code that rely on services provided by VisualStudio.
        /// </summary>
        /// <param name="cancellationToken">A cancellation token to monitor for initialization cancellation, which can occur when VS is shutting down.</param>
        /// <param name="progress">A provider for progress updates.</param>
        /// <returns>A task representing the async work of package initialization, or an already completed task if there is none. Do not return null from this method.</returns>
        protected override async Task InitializeAsync(CancellationToken cancellationToken, IProgress<ServiceProgressData> progress)
        {
            // When initialized asynchronously, the current thread may be a background thread at this point.
            // Do any initialization that requires the UI thread after switching to the UI thread.
            await this.JoinableTaskFactory.SwitchToMainThreadAsync(cancellationToken);

            IVsSolutionBuildManager buildManager = await GetServiceAsync(typeof(SVsSolutionBuildManager)) as IVsSolutionBuildManager;
            buildManager.AdviseUpdateSolutionEvents(this, out uint cookie);
        }

        #endregion

        // Unused Interface Methods
        public int UpdateSolution_Cancel() => 0;
        public int UpdateSolution_StartUpdate(ref int pfCancelUpdate) => 0;
    }
}
