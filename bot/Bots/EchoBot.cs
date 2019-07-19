// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Schema;

namespace Microsoft.BotBuilderSamples.Bots
{
    public class EchoBot : ActivityHandler
    {
        protected override async Task OnMessageActivityAsync(ITurnContext<IMessageActivity> turnContext, CancellationToken cancellationToken)
        {
            if(turnContext.Activity.Text.Contains("human"))
            {
                var a1 = MessageFactory.Text($"msg1");
                var a2 = MessageFactory.Text($"msg2");
                var transcript = new Activity[] { a1, a2 };
                var context = new { Skill = "credit cards ", CallerId = "CCI" };
                IHandoffRequest request = await turnContext.InitiateHandoffAsync(transcript, context, cancellationToken);

                if(await request.IsCompletedAsync())
                {
                    await turnContext.SendActivityAsync("Handoff request has been completed");
                }
                else
                {
                    await turnContext.SendActivityAsync("Handoff request has NOT been completed");
                }

                return;
            }
            await turnContext.SendActivityAsync(MessageFactory.Text($"Echo: {turnContext.Activity.Text}"), cancellationToken);
        }

        protected override async Task OnMembersAddedAsync(IList<ChannelAccount> membersAdded, ITurnContext<IConversationUpdateActivity> turnContext, CancellationToken cancellationToken)
        {
            foreach (var member in membersAdded)
            {
                if (member.Id != turnContext.Activity.Recipient.Id)
                {
                    await turnContext.SendActivityAsync(MessageFactory.Text($"Hello and welcome5!"), cancellationToken);
                }
            }
        }
    }
}
