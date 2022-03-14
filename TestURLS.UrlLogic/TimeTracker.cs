﻿using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using TestURLS.Models;

namespace TestURLS.UrlLogic
{
    public class TimeTracker
    {
        public virtual List<UrlTimeModel> GetLinksWithTime(List<string> linksToGetTime)
        {
            List<UrlTimeModel> urlWithTime = new List<UrlTimeModel>();

            foreach (string url in linksToGetTime)
            {
                //get time of request
                Stopwatch timeToResponse = Stopwatch.StartNew();

                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                var response = (HttpWebResponse)request.GetResponse();

                timeToResponse.Stop();

                var time = timeToResponse.ElapsedMilliseconds;
                UrlTimeModel model = new UrlTimeModel()
                {
                    Link = url,
                    TimeOfResponse = time
                };

                urlWithTime.Add(model);
            }

            return urlWithTime;
        }
    }
}