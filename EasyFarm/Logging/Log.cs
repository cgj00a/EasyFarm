/*///////////////////////////////////////////////////////////////////
<EasyFarm, general farming utility for FFXI>
Copyright (C) Mykezero

This program is free software: you can redistribute it and/or modify
it under the terms of the GNU General Public License as published by
the Free Software Foundation, either version 3 of the License, or
(at your option) any later version.

This program is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
GNU General Public License for more details.

You should have received a copy of the GNU General Public License
///////////////////////////////////////////////////////////////////*/

using System;
using System.Collections.ObjectModel;
using System.Linq;
using NLog;
using NLog.Config;
using System.Windows;

namespace EasyFarm.Logging
{
    public static class Log
    {
        private static Logger _logger;

        public static ObservableCollection<string> LoggedItems = new ObservableCollection<string>();

        public static void Initialize()
        {
            LogManager.ThrowExceptions = true;
            var config = new LoggingConfiguration();
            var target = new LogSink(PublishLogItem) { Layout = @"${date:format=HH\:mm\:ss} ${message}" };
            config.AddTarget("LogSink", target);
            config.LoggingRules.Add(new LoggingRule("*", LogLevel.Info, target));
            LogManager.Configuration = config;
            _logger = LogManager.GetLogger("LogSink");
        }

        public static void Write(string message)
        {
            _logger.Info(message);
        }

        /// <summary>
        ///     Publish log item under the right thread context.
        /// </summary>
        /// <param name="message"></param>
        private static void PublishLogItem(string message)
        {
            Application.Current.Dispatcher.Invoke(delegate
            {
                AddLogItem(message);
            });
        }

        /// <summary>
        /// Add message to log without while preventing a <see cref="OutOfMemoryException"/>. 
        /// </summary>
        /// <param name="message"></param>
        private static void AddLogItem(string message)
        {
            LoggedItems.Add(message);

            // Limit list to only 1000 items: prevent system out of memory exception. 
            if (LoggedItems.Count > 1000)
            {
                LoggedItems.Remove(LoggedItems.Last());
            }
        }
    }
}
