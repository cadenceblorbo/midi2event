using System;
using System.Buffers.Binary;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.XPath;

namespace midi2event
{
    internal class MidiReader
    {
        private string fileName;
        private ushort _ticksPerQuarter;

        private readonly uint MTHD_LENGTH = 6;

        public MidiReader(string fileName)
        {
            this.fileName = fileName;
        }

        public Queue<MTrkEvent> Read(){
            Stream fileStream = File.Open(fileName, FileMode.Open, FileAccess.Read);
            ReadHeader(fileStream);
            Queue<MTrkEvent> result = ReadTrack(fileStream);
            return result;
        }

        private void ReadHeader(Stream fileStream)
        {
            byte[] buffer = new byte[4];
            //MThd header
            fileStream.Read(buffer, 0, buffer.Length);
            if (BinaryPrimitives.ReadUInt32BigEndian(buffer) != (int)ChunkTypes.MThd)
            {
                throw new InvalidDataException("MThd chunk type expected!");
            }
            //MThd length
            fileStream.Read(buffer, 0, buffer.Length);
            if (BinaryPrimitives.ReadUInt32BigEndian(buffer) != MTHD_LENGTH)
            {
                throw new InvalidDataException("MThd needs length of " + MTHD_LENGTH + " !");
            }
            //MIDI file format
            //Currently only supports 0, which is fine for this system, but 1 + 2 should be impemented if expanding code for general MIDI functionality.
            fileStream.Read(buffer, 2, 2);
            if (BinaryPrimitives.ReadUInt32BigEndian(buffer) != 0)
            {
                throw new InvalidDataException(
                    "Only MIDI format 0 currently supported, sorry :<\nTry using a file with a single multi-channel track!"
                );
            }
            //Number of tracks (should be 1)
            fileStream.Read(buffer, 2, 2);
            if (BinaryPrimitives.ReadUInt32BigEndian(buffer) != 1)
            {
                throw new InvalidDataException("Only one track should be present!");
            }
            //delta time meaning (only supports ticks per quarter-note)
            fileStream.Read(buffer, 2, 2);
            ushort division = BinaryPrimitives.ReadUInt16BigEndian(buffer);
            if ((division & 0x8000) != 0)
            {
                throw new InvalidDataException("Only supports ticks per quarter note for now :<");
            }
            _ticksPerQuarter = division;

            Debug.WriteLine("header successfully read!");
        }

        private Queue<MTrkEvent> ReadTrack(Stream fileStream){
            Queue<MTrkEvent> result = new();

            //track header parsing
            byte[] buffer = new byte[4];
            fileStream.Read(buffer, 0, buffer.Length);
            if (BinaryPrimitives.ReadUInt32BigEndian(buffer) != (int)ChunkTypes.MTrk)
            {
                throw new InvalidDataException("MTrk chunk type expected!");
            }
            fileStream.Read(buffer, 0, buffer.Length);
            uint trackLength = BinaryPrimitives.ReadUInt32BigEndian(buffer);

            //parse every event in the track
            bool trackOver = false;
            while(!trackOver)
            {
                MTrkEvent? nextEvent = ReadNextMTrkEvent(fileStream);
                if (nextEvent is not null){
                    result.Enqueue(nextEvent);
                }
            }
            return result;
        }

        private MTrkEvent? ReadNextMTrkEvent(Stream fileStream){
            uint delta = ParseVarLen(fileStream);
            
            byte status = (byte)fileStream.ReadByte();
            switch(status){
                case (byte)StatusTypes.NoteOn:
                    
                    break;
                case (byte)StatusTypes.NoteOff:

                    break;
                case (byte)StatusTypes.MetaEvent:

                    break;
                default:
                    return null;
            }

            return new MTrkEvent();
        }

        private MTrkEvent? ReadMetaEvent(Stream fileStream){
            return new MTrkEvent();
        }

        private uint ParseVarLen(Stream fileStream){
            //Parse variable length quantity value from stream
            uint result = 0;
            byte next;
            do
            {
                next = (byte)fileStream.ReadByte();
                result = (result << 7) | (next & 0x7Fu);
            }
            while((next & 0x80) != 0);
            return result;
        }

        private enum ChunkTypes
        {
            MThd = 1297377380,
            MTrk = 1297379947
        }

        private enum StatusTypes
        {
            NoteOn = 0x90,
            NoteOff = 0x80,
            MetaEvent = 0xFF,
        }

        private enum MetaTypes
        {
            SetTempo = 0x51,
            EndOfTrack = 0x2F
        }
    }
}
