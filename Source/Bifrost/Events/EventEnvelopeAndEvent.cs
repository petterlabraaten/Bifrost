﻿/*---------------------------------------------------------------------------------------------
 *  Copyright (c) 2008-2017 Dolittle. All rights reserved.
 *  Licensed under the MIT License. See LICENSE in the project root for license information.
 *--------------------------------------------------------------------------------------------*/
namespace Bifrost.Events
{
    /// <summary>
    /// Represents an <see cref="EventEnvelope"/> and an <see cref="IEvent"/>
    /// </summary>
    public class EventEnvelopeAndEvent
    {
        /// <summary>
        /// Initializes a new instance of <see cref="EventEnvelopeAndEvent"/>
        /// </summary>
        /// <param name="envelope"><see cref="EventEnvelope">Envelope</see> with metadata for the <see cref="IEvent"/></param>
        /// <param name="theEvent"><see cref="IEvent">Event</see> that is represented</param>
        public EventEnvelopeAndEvent(EventEnvelope envelope, IEvent theEvent)
        {
            Envelope = envelope;
            Event = theEvent;
        }

        /// <summary>
        /// Gets the <see cref="EventEnvelope">envelope</see>
        /// </summary>
        public EventEnvelope    Envelope { get; }

        /// <summary>
        /// Gets the <see cref="IEvent">event</see>
        /// </summary>
        public IEvent Event { get; }
    }
}
