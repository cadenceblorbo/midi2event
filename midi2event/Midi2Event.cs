using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Tracing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace midi2event
{
    internal class Midi2Event
    {
        private Dictionary<int, Action> _startEvents;
        private Dictionary<int, Action> _stopEvents;
        private Dictionary<int, Action> _duringEvents;
        private Queue<MTrkEvent> _messages;

        private readonly int TET = 12;

        public Midi2Event()
        {
            _startEvents = new Dictionary<int, Action>();
            _stopEvents = new Dictionary<int, Action>();
            _duringEvents = new Dictionary<int, Action>();
        }

        public void Update(double deltaTime) { }

        public void Reset()
        {
            //TODO
        }

        private int ToNoteId(Notes note, int octave)
        {
            return TET * (octave + 1) + (int)note;
        }

        public void Subscribe(Action action, Notes note, int octave, SubType type = SubType.Start)
        {
            Dictionary<int, Action> events = ToNoteMap(type);
            int noteId = ToNoteId(note, octave);
            if (!events.ContainsKey(noteId))
            {
                events.Add(noteId, () => { });
            }
            events[noteId] += action;
        }

        private Dictionary<int, Action> ToNoteMap(SubType type) => type switch{
            SubType.Start => _startEvents,
            SubType.During => _duringEvents,
            SubType.Stop => _stopEvents,
            _ => throw new NotImplementedException("No note map for specified SubType!")
        };

        public enum SubType
        {
            Start,
            Stop,
            During
        }

        public enum Notes
        {
            C = 0,
            Cs = 1,
            D = 2,
            Ds = 3,
            E = 4,
            F = 5,
            Fs = 6,
            G = 7,
            Gs = 8,
            A = 9,
            As = 10,
            B = 11
        }


    }
}
