using PayPalCheckoutSdk.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace API.PayPal
{
    public static class CaptureOrderSample
    {
        static string clientId = "AR9bAQYaLIrC4_2DH_UC3DOb1i3Vo08RPG89hq8FvxhBq9e37_UlEd3uVffMoqe9Aa6rT-ceCI3ukIC4";
        static string secret = "EOS1z5bwMaQZyJynYp-DqSpyRlgKnJ0EmFe08HdbrjCdIdvy6ESrmCnlaXTbXg3ge1yDVI3H3o7x_FzG";

        public static PayPalHttpClient client()
        {
            // Creating a sandbox environment
            PayPalEnvironment environment = new SandboxEnvironment(clientId, secret);
            // Creating a client for the environment
            PayPalHttpClient client = new PayPalHttpClient(environment);
            return client;
        }
    }
}
