using Sitecore.Analytics;
using Sitecore.Analytics.Tracking;
using Sitecore.Configuration;
using Sitecore.Diagnostics;
using Sitecore.Marketing.Definitions;
using Sitecore.Marketing.Definitions.Goals;
using Sitecore.Marketing.Definitions.Outcomes.Model;
using Sitecore.Marketing.Definitions.PageEvents;
using System;
using System.Globalization;
using Wageworks.Foundation.DependencyInjection;

namespace Wageworks.Foundation.SitecoreExtensions.Services
{
    [Service(typeof(ITrackerService), Lifetime = Lifetime.Transient)]
    public class TrackerService : ITrackerService
    {
        public TrackerService(IDefinitionManager<IPageEventDefinition> pageEventDefinitionManager, IDefinitionManager<IOutcomeDefinition> outcomeDefinitionManager, IDefinitionManager<IGoalDefinition> goalDefinitionManager)
        {
            this.PageEventDefinitionManager = pageEventDefinitionManager;
            this.OutcomeDefinitionManager = outcomeDefinitionManager;
            this.GoalDefinitionManager = goalDefinitionManager;
            this.ContactManager = Factory.CreateObject("tracking/contactManager", true) as ContactManager;
        }

        public IDefinitionManager<IGoalDefinition> GoalDefinitionManager { get; }
        public ContactManager ContactManager { get; }
        public IDefinitionManager<IPageEventDefinition> PageEventDefinitionManager { get; }
        public IDefinitionManager<IOutcomeDefinition> OutcomeDefinitionManager { get; }


        public bool IsActive
        {
            get
            {
                if (Tracker.Enabled == false || Tracker.Current == null)
                {
                    return false;
                }
                if (!Tracker.Current.IsActive)
                {
                    Tracker.StartTracking();
                }
                return true;
            }
        }

        public virtual void TrackPageEvent(Guid pageEventId, string text = null, string data = null, string dataKey = null, int? value = null)
        {
            Assert.ArgumentNotNull(pageEventId, nameof(pageEventId));
            if (!this.IsActive)
            {
                return;
            }

            var pageEventDefinition = this.PageEventDefinitionManager.Get(pageEventId, CultureInfo.InvariantCulture);
            if (pageEventDefinition == null)
            {
                Log.Warn($"Cannot find page event: {pageEventId}", this);
                return;
            }

            var eventData = Tracker.Current?.CurrentPage?.RegisterPageEvent(pageEventDefinition);
            if (eventData == null) return;

            if (data != null)
            {
                eventData.Data = data;
            }
            if (dataKey != null)
            {
                eventData.DataKey = dataKey;
            }
            if (text != null)
            {
                eventData.Text = text;
            }
            if (value != null)
            {
                eventData.Value = value.Value;
            }
        }

        public void TrackGoal(Guid goalId, string text = null, string data = null, string dataKey = null, int? value = null)
        {
            Assert.ArgumentNotNull(goalId, nameof(goalId));
            if (!this.IsActive)
            {
                return;
            }

            var goalDefinition = this.GoalDefinitionManager.Get(goalId, CultureInfo.InvariantCulture);
            if (goalDefinition == null)
            {
                Log.Warn($"Cannot find goal: {goalId}", this);
                return;
            }

            var eventData = Tracker.Current?.CurrentPage?.RegisterGoal(goalDefinition);
            if (eventData == null) return;

            if (data != null)
            {
                eventData.Data = data;
            }
            if (dataKey != null)
            {
                eventData.DataKey = dataKey;
            }
            if (text != null)
            {
                eventData.Text = text;
            }
            if (value != null)
            {
                eventData.Value = value.Value;
            }
        }

        public void TrackOutcome(Guid outComeDefinitionId)
        {
            Assert.ArgumentNotNull(outComeDefinitionId, nameof(outComeDefinitionId));

            if (!this.IsActive || Tracker.Current.Contact == null)
            {
                return;
            }

            var outcomeDefinition = this.OutcomeDefinitionManager.Get(outComeDefinitionId, CultureInfo.InvariantCulture);
            if (outcomeDefinition == null)
            {
                Log.Warn($"Cannot find outcome: {outComeDefinitionId}", this);
                return;
            }
            Tracker.Current.CurrentPage.RegisterOutcome(outcomeDefinition, "USD", 0);
        }


        public void IdentifyContact(string source, string identifier)
        {
            try
            {
                Log.Info($"Begin Identify Contact. identifier: {identifier}, source: {source}", this);
                if (!this.IsActive)
                {
                    Log.Warn("Identify Contact. Tracker not Active", this);
                    return;
                }

                Tracker.Current.Session.IdentifyAs(source, identifier);
                Log.Info($"End Identify Contact. identifier: {identifier}", this);
            }
            catch (Exception ex)
            {
                Log.Error("Could not identify contact", ex, this);
            }
        }
    }
}