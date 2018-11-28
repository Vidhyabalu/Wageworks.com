using System;
using System.Linq;
using Quartz;
using Sitecore.Configuration;
using Sitecore.Data;
using Sitecore.Data.Managers;
using Sitecore.Diagnostics;
using Sitecore.Globalization;
using Sitecore.Publishing;
using Sitecore.Web;

namespace Wageworks.Foundation.SitecoreExtensions.Publishing
{
    public class PublishJob : IJob
    {
        public void Execute(IJobExecutionContext context)
        {
            try
            {


                Log.Info("SitePublishJob Execute - Start", this);
                string targets = WebUtil.ParseUrlParameters(context.JobDetail.JobDataMap.GetString("Parameters"))["Target"];
                string publish = WebUtil.ParseUrlParameters(context.JobDetail.JobDataMap.GetString("Parameters"))["Publish"];
                if (string.IsNullOrEmpty(targets))
                {
                    Log.Warn("SitePublishJob Execute - Target parameter missing", this);
                }
                else
                {
                    Database database = Factory.GetDatabase("master");
                    var dbArray = targets.Split(',');

                    var dbList = dbArray.Select(Factory.GetDatabase).Where(target => target != null).ToList();

                    if (!dbList.Any())
                        return;

                    Language[] langArray = LanguageManager.GetLanguages(database).ToArray();

                    if (publish == "Incremental")
                        PublishManager.PublishIncremental(database, dbList.ToArray(), langArray);
                    else
                        PublishManager.PublishSmart(database, dbList.ToArray(), langArray);
                }
                //temporary fix for bug in Sitecron that causes the job to rerun if it completes in less than 1 second.
                System.Threading.Thread.Sleep(5000);
                Log.Info("SitePublishJob Execute - End", this);
            }
            catch (Exception ex)
            {
                Log.Error("Could not run Auto Publish Job", ex, this);
            }
        }
    }
}