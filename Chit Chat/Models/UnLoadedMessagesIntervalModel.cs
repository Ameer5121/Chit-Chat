﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ChitChat.Models
{
    public class UnLoadedMessagesIntervalModel
    {
        public DateTime FirstDate { get; }
        public DateTime LastDate { get; }
        public UserModel To { get; }
        public UserModel From { get; }


        [JsonConstructor]
        public UnLoadedMessagesIntervalModel(DateTime firstDate, DateTime lastDate, UserModel to, UserModel from)
        {
            FirstDate = firstDate;
            LastDate = lastDate;
            To = to;
            From = from;
        }
        public UnLoadedMessagesIntervalModel(DateTime firstDate, DateTime lastDate)
        {
            FirstDate = firstDate;
            LastDate = lastDate;
        }
    }
}