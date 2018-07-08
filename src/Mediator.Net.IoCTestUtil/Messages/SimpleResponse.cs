﻿using System;
using Mediator.Net.Contracts;

namespace Mediator.Net.IoCTestUtil.Messages
{
    public class SimpleResponse : IResponse
    {
        public string EchoMessage { get; }

        public SimpleResponse(string echoMessage)
        {
            EchoMessage = echoMessage;
        }
    }
}
