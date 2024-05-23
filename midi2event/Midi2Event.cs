using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace midi2event
{
    internal class Midi2Event
    {
        private Dictionary<int, Action> _events;
        private Queue<MTrkEvent> _messages;

        private readonly int TET = 12;

        public Midi2Event()
        {
            _events = new Dictionary<int, Action>();
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

        public void Subscribe(Action action, Notes note, int octave)
        {
            int noteId = ToNoteId(note, octave);
            if (!_events.ContainsKey(noteId))
            {
                _events.Add(noteId, () => { });
            }
            _events[noteId] += action;
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
