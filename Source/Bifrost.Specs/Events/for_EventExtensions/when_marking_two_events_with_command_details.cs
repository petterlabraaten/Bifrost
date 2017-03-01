﻿using System;
using Bifrost.Events;
using Bifrost.Testing.Fakes.Commands;
using Bifrost.Testing.Fakes.Events;
using Machine.Specifications;

namespace Bifrost.Specs.Events.for_EventExtensions
{
    public class when_marking_two_events_with_command_details
    {
        static EventEnvelopeAndEvent[] events = new EventEnvelopeAndEvent[] {
            new EventEnvelopeAndEvent(new EventEnvelope(), new SimpleEvent(Guid.NewGuid())),
            new EventEnvelopeAndEvent(new EventEnvelope(), new SimpleEvent(Guid.NewGuid()))
        };

        static SimpleCommand command = new SimpleCommand(Guid.NewGuid());

        Because of = () => events.MarkEventsWithCommandDetails(command);

        It should_set_name_of_command_on_first_event = () => events[0].Event.CommandName.ShouldEqual(typeof(SimpleCommand).Name);
        It should_set_name_of_command_on_second_event = () => events[1].Event.CommandName.ShouldEqual(typeof(SimpleCommand).Name);
        It should_set_command_context_on_first_event = () => events[0].Event.CommandContext.ShouldEqual(command.Id);
        It should_set_command_context_on_second_event = () => events[1].Event.CommandContext.ShouldEqual(command.Id);
    }
}
