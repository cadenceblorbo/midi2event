﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Tracing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MIDI2Event
{
    public class MIDI2EventSystem
    {
        private Dictionary<int, Action> _startEvents;
        private Dictionary<int, Action> _stopEvents;
        private Action _endEvent;
        private Queue<MTrkEvent> _messages;
        private Queue<MTrkEvent> _bin;
        private MidiReader _reader;
        private uint _ticksPerQuarter;

        private double _deltaTimeSinceLastUpdate = 0;

        private double _deltaTimeToNextUpdate = 0;

        //microseconds per quarter note
        private uint _usPerQuarter = 500000;

        //conversion factor from microseconds to seconds
        private readonly double US_TO_S = 1e-6;
        private bool _isPlaying = false;
        private readonly int TET = 12;
        int lowestOctave;

        public double SecPerBeat
        {
            get => _usPerQuarter * US_TO_S;
        }

        public double BeatPerSec
        {
            get => 1 / SecPerBeat;
        }

        //returns true if this system is currently playing
        public bool IsPlaying
        {
            get => _isPlaying;
        }

        /*
         *  Creates a new MIDI2EventSystem using the MIDI data at the specified file path.
         *  The lowestOctave field is present to adjust for any discrepancies in MIDI
         *  creation tools (ex. Logic Pro has a lowest octave of 0 for MIDI data in the editor).
         */
        public MIDI2EventSystem(string filePath, int lowestOctave = -1)
        {
            _startEvents = new Dictionary<int, Action>();
            _stopEvents = new Dictionary<int, Action>();
            _endEvent = () => { };
            _reader = new MidiReader();
            _bin = new();
            this.lowestOctave = lowestOctave;
            //load chart
            (_ticksPerQuarter, _messages) = _reader.Read(filePath);
            if (_messages.Count <= 0)
            {
                Debug.WriteLine("Provided midi generated no events, was this intended?");
            }
        }

        /*
         *  Call this every update frame in whatever engine/tool you're using.
         *  It is advised to calculate this deltaTime in seconds based on the samples of the audio you're using
         *  for maximum accuracy.
         */
        public void Update(double deltaTime)
        {
            //do nothing if the system isn't playing or there are no more events
            if (!(_isPlaying && _messages.Count > 0))
            {
                return;
            }

            //trigger every event that is relevant at this deltaTime
            _deltaTimeSinceLastUpdate += deltaTime;
            while (_deltaTimeToNextUpdate <= _deltaTimeSinceLastUpdate)
            {
                double makeup = _deltaTimeSinceLastUpdate - _deltaTimeToNextUpdate;
                MTrkEvent toProcess = _messages.Dequeue();
                GetEvent(toProcess).Invoke();
                _bin.Enqueue(toProcess);

                _deltaTimeSinceLastUpdate = 0;
                if (_messages.Count <= 0)
                {
                    _isPlaying = false;
                    return;
                }
                _deltaTimeToNextUpdate = DeltaToDeltaTime(_messages.Peek().Delta) - makeup;
            }
        }

        /*
         *  subscribes an action to trigger when the specific event occurs in the midi file
         *  returns a function which unsubscribes the action from the event
         *
         *  params:
         *  action - the action to subscribe to the event system
         *  note - the midi note of the event to subscribe to
         *  octave - the octave of the note in the midi chart
         *  type - the type of subscription to use (start of note, end of note, end of chart)
         */
        public Action Subscribe(
            Action action,
            Notes note = 0,
            int octave = 0,
            SubType type = SubType.NoteStart
        )
        {
            if (type == SubType.ChartEnd)
            {
                _endEvent += action;
                return () =>
                {
                    _endEvent -= action;
                };
            }
            Dictionary<int, Action> events = ToNoteMap(type);
            int noteId = ToNoteId(note, octave);
            if (!events.ContainsKey(noteId))
            {
                events.Add(noteId, () => { });
            }
            events[noteId] += action;
            return () =>
            {
                events[noteId] -= action;
            };
        }

        public enum SubType
        {
            NoteStart,
            NoteStop,
            ChartEnd
        }

        //rewind the system and start it playing again
        public void Reset()
        {
            Back();
            Play();
        }

        //rewind the system and stop it from playing
        public void Stop()
        {
            Back();
            _isPlaying = false;
        }

        //start playing the event system chart
        public void Play()
        {
            if (_messages.Count <= 0)
            {
                return;
            }
            _isPlaying = true;
            _deltaTimeToNextUpdate = DeltaToDeltaTime(_messages.Peek().Delta);
        }

        //pause the event system chart
        public void Pause()
        {
            _isPlaying = false;
        }

        //return the event in the system related to MTrkEvent e
        private Action GetEvent(MTrkEvent e)
        {
            return e switch
            {
                NoteOnEvent on => TryGetEvent(_startEvents, on.Note),
                NoteOffEvent off => TryGetEvent(_stopEvents, off.Note),
                EndTrackMeta => _endEvent,
                SetTempoMeta st
                    => () =>
                    {
                        _usPerQuarter = st.USPerQuarter;
                    },
                _ => () => { }
            };
        }

        private Action TryGetEvent(Dictionary<int, Action> dict, int index)
        {
            if (dict.ContainsKey(index))
            {
                return dict[index];
            }
            return () => { };
        }

        //convert MIDI format 0 delta-time to a delta-time in seconds
        private double DeltaToDeltaTime(uint delta)
        {
            return delta * (_usPerQuarter / _ticksPerQuarter) * US_TO_S;
        }

        //rewind the system to the beginning of the track
        private void Back()
        {
            _deltaTimeSinceLastUpdate = 0;
            _deltaTimeToNextUpdate = 0;
            _usPerQuarter = 500000;
            while (_messages.Count > 0)
            {
                MTrkEvent transfer = _messages.Dequeue();
                _bin.Enqueue(transfer);
            }
            _messages = new(_bin);
            _bin = new();
        }

        //convert a note to its MIDI byte ID
        private int ToNoteId(Notes note, int octave)
        {
            return TET * (octave - lowestOctave) + (int)note;
        }

        //get the event map for the specified subscription type
        private Dictionary<int, Action> ToNoteMap(SubType type) =>
            type switch
            {
                SubType.NoteStart => _startEvents,
                SubType.NoteStop => _stopEvents,
                _ => throw new NotImplementedException("No note map for specified SubType!")
            };
    }
}
