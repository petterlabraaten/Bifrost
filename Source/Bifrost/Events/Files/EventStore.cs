﻿/*---------------------------------------------------------------------------------------------
 *  Copyright (c) 2008-2017 Dolittle. All rights reserved.
 *  Licensed under the MIT License. See LICENSE in the project root for license information.
 *--------------------------------------------------------------------------------------------*/
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Bifrost.Serialization;

namespace Bifrost.Events.Files
{
    /// <summary>
    /// Represents a simple file based <see cref="IEventStore"/>
    /// </summary>
    public class EventStore : IEventStore
    {
        class EventHolder
        {
            public Type Type { get; set; }
            public EventSourceVersion Version { get; set; }
            public string Event { get; set; }
        }


        EventStoreConfiguration _configuration;
        IEventEnvelopes _eventEnvelopes;
        ISerializer _serializer;

        /// <summary>
        /// Initializes a new instance of <see cref="EventStore"/>
        /// </summary>
        /// <param name="configuration"><see cref="EventStoreConfiguration"/> to use as configuration</param>
        /// <param name="eventEnvelopes"><see cref="IEventEnvelopes"/> for working with <see cref="EventEnvelope"/></param>
        /// <param name="serializer"><see cref="ISerializer"/> to use for serialization</param>
        public EventStore(EventStoreConfiguration configuration, IEventEnvelopes eventEnvelopes, ISerializer serializer)
        {
            _configuration = configuration;
            _eventEnvelopes = eventEnvelopes;
            _serializer = serializer;
        }

#pragma warning disable 1591 // Xml Comments
        public CommittedEventStream GetForEventSource(IEventSource eventSource, EventSourceId eventSourceId)
        {
            var eventPath = GetPathFor(eventSource.GetType().Name, eventSourceId);
            var files = Directory.GetFiles(eventPath).OrderBy(f => f);

            var events = new List<EventEnvelopeAndEvent>();

            var target = new EventHolder
            {
                Type = typeof(string),
                Version = EventSourceVersion.Zero,
                Event = string.Empty
            };

            foreach (var file in files)
            {
                var json = File.ReadAllText(file);
                _serializer.FromJson(target, json);

                var @event = _serializer.FromJson(target.Type, json) as IEvent;
                events.Add(new EventEnvelopeAndEvent(_eventEnvelopes.CreateFrom(eventSource, @event), @event));
            }
            return new CommittedEventStream(eventSourceId, events);
        }

        public CommittedEventStream Commit(UncommittedEventStream uncommittedEventStream)
        {
            foreach (var eventAndEnvelope in uncommittedEventStream)
            {
                var @event = eventAndEnvelope.Event;
                var eventSourceName = Type.GetType(@event.EventSource).Name;
                var eventPath = GetPathFor(eventSourceName, @event.EventSourceId);

                @event.Id = GetNextEventId();

                var json = _serializer.ToJson(new EventHolder
                {
                    Type = @event.GetType(),
                    Version = @event.Version,
                    Event = _serializer.ToJson(@event)
                });

                var path = Path.Combine(eventPath,$"{@event.Version.Commit}.{@event.Version.Sequence}");
                File.WriteAllText(path, json);
            }

            var committedEventStream = new CommittedEventStream(uncommittedEventStream.EventSourceId, uncommittedEventStream);
            return committedEventStream;
        }


        public EventSourceVersion GetLastCommittedVersion(IEventSource eventSource, EventSourceId eventSourceId)
        {
            var eventPath = GetPathFor(eventSource.GetType().Name, eventSourceId);
            var first = Directory.GetFiles(eventPath).OrderByDescending(f => f).FirstOrDefault();
            if (first == null) return EventSourceVersion.Zero;

            var json = File.ReadAllText(first);
            var target = new EventHolder
            {
                Type = typeof(string),
                Version = EventSourceVersion.Zero,
                Event = string.Empty
            };

            _serializer.FromJson(target, json);

            return target.Version;
        }

        string GetPathFor(string eventSource)
        {
            var fullPath = Path.Combine(_configuration.Path, "EventStore", eventSource);
            if (!Directory.Exists(fullPath))
            {
                Directory.CreateDirectory(fullPath);
            }
            return fullPath;
        }

        string GetPathFor(string eventSource, EventSourceId eventSourceId)
        {
            var fullPath = Path.Combine(GetPathFor(eventSource), eventSourceId.ToString());
            if (!Directory.Exists(fullPath))
            {
                Directory.CreateDirectory(fullPath);
            }
            return fullPath;
        }

        int GetNextEventId()
        {
            var id = 0;

            var idFile = Path.Combine(_configuration.Path, "LastEventID");

            if (File.Exists(idFile))
            {
                var idAsString = File.ReadAllText(idFile);
                int.TryParse(idAsString, out id);
            }

            id++;
            File.WriteAllText(idFile, id.ToString());

            return id;
        }
#pragma warning restore 1591 // Xml Comments
    }
}
