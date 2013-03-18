﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using System.Xml;
using System.Xml.Serialization;
using odm.player;
using onvif.services;
using utils;

namespace odm.ui.views
{

    public class AbandonedItemAlarmSnapshot : VAEntitySnapshot
    {
        public bool State { get; set; }
        public string Rule { get; set; }
        public string ObjectId { get; set; }

        public override VAEntity Create()
        {
            return new AbandonedItemAlarm(this.Rule, this.ObjectId);
        }
    }

    public class AbandonedItemAlarm : VAAlarm
    {
        public string Rule {get; private set;}
        public string ObjectId { get; private set; }

        public AbandonedItemAlarm(string rule, string objectId)
        {
            this.Rule = rule;
            this.ObjectId = objectId;
        }

        public override string ToString()
        {
            return string.Format("{0}: {1}, {2}", Name, Rule, ObjectId);
        }

        public override bool Fit(VAEntitySnapshot snapshot)
        {
            return snapshot is AbandonedItemAlarmSnapshot 
                && ((AbandonedItemAlarmSnapshot)snapshot).Rule == this.Rule
                && ((AbandonedItemAlarmSnapshot)snapshot).ObjectId == this.ObjectId;
        }

        public override void Update(VAEntitySnapshot snapshot1, Func<double,double> scaleX, Func<double,double> scaleY)
        {
            if (!Fit(snapshot1))
                throw new InvalidOperationException();
 	        
            AbandonedItemAlarmSnapshot snapshot = (AbandonedItemAlarmSnapshot)snapshot1;

            this.State = snapshot.State;
            FireUpdated();
        }
    }

    public class AbandonedItemAlarmMetadataProcessor : BaseNotificationMessageProcessor<AbandonedItemAlarmSnapshot>
    {
        public AbandonedItemAlarmMetadataProcessor(string videoSourceConfToken, string videoAnalyticsConfToken, Action<AbandonedItemAlarmSnapshot> initialized, Action<AbandonedItemAlarmSnapshot> changed, Action<AbandonedItemAlarmSnapshot> deleted)
            : base(null, videoSourceConfToken, videoAnalyticsConfToken, initialized, changed, deleted)
        {
        }

        protected override bool VerifyTopic(TopicExpressionType topic)
        {
            if (@"http://docs.oasis-open.org/wsn/t-1/TopicExpression/Concrete".Equals(topic.Dialect))
            {
                var innerTopic = topic.Any;
                //TODO : resolve namespace prefixes!
                if (innerTopic != null && innerTopic.Length == 1 && "tns1:RuleEngine/FieldDetector/tnsx:AbandonedAlarm".Equals(innerTopic[0].InnerText))
                    return true;
            }
            return false;
        }

        protected override AbandonedItemAlarmSnapshot Parse(TopicExpressionType topic, Message message)
        {
            if (message.Source == null || message.Source.simpleItem == null)
                throw new InvalidOperationException();
            string rule = null;
            foreach (var s in message.Source.simpleItem)
                if (s.name == "Rule") rule = s.value;
            if (string.IsNullOrEmpty(rule))
                throw new InvalidOperationException();

            if (message.Key == null || message.Key.simpleItem == null || message.Key.simpleItem.Length != 1)
                throw new InvalidOperationException();
            var key = message.Key.simpleItem[0];

            if (key == null || key.name != "ObjectId")
                throw new InvalidOperationException();
            string objectId = key.value;

            bool state = false;
            
            var data = message.Data;
            if (data.simpleItem != null)
            {
                foreach (var si in data.simpleItem)
                {
                    if (si.name == "State")
                    {
                        si.value.TryParseInvariant(out state);
                    }
                }
            }
            
            var snapshot = new AbandonedItemAlarmSnapshot
                {
                    State = state,
                    Rule = rule,
                    ObjectId = objectId
                };
            return snapshot;
        }
    }
}
