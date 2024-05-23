using System;
using System.Buffers.Binary;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace midi2event
{
    internal class MidiReader
    {
        private Stream _fileStream;
        private ushort _ticksPerQuarter;

        private readonly uint MTHD_LENGTH = 6;

        public MidiReader(string fileName)
        {
            _fileStream = File.Open(fileName, FileMode.Open, FileAccess.Read);
            ReadHeader();
        }

        private void ReadHeader()
        {
            byte[] buffer = new byte[4];
            //MThd header
            _fileStream.Read(buffer, 0, buffer.Length);
            if (BinaryPrimitives.ReadUInt32BigEndian(buffer) != (int)ChunkTypes.MThd)
            {
                throw new InvalidDataException("MThd chunk type expected!");
            }
            //MThd length
            _fileStream.Read(buffer, 0, buffer.Length);
            if (BinaryPrimitives.ReadUInt32BigEndian(buffer) != MTHD_LENGTH)
            {
                throw new InvalidDataException("MThd needs length of " + MTHD_LENGTH + " !");
            }
            //MIDI file format
            //Currently only supports 0, which is fine for this system, but 1 + 2 should be impemented if expanding code for general MIDI functionality.
            _fileStream.Read(buffer, 2, 2);
            if (BinaryPrimitives.ReadUInt32BigEndian(buffer) != 0)
            {
                throw new InvalidDataException(
                    "Only MIDI format 0 currently supported, sorry :<\nTry using a file with a single multi-channel track!"
                );
            }
            //Number of tracks (should be 1)
            _fileStream.Read(buffer, 2, 2);
            if (BinaryPrimitives.ReadUInt32BigEndian(buffer) != 1)
            {
                throw new InvalidDataException("Only one track should be present!");
            }
            //delta time meaning (only supports ticks per quarter-note)
            _fileStream.Read(buffer, 2, 2);
            ushort division = BinaryPrimitives.ReadUInt16BigEndian(buffer);
            if ((division & 0x8000) != 0)
            {
                throw new InvalidDataException("Only one track should be present!");
            }
            _ticksPerQuarter = division;

            Debug.WriteLine("yippee!!!");
        }

        private enum ChunkTypes
        {
            MThd = 1297377380,
            MTrk = 1297379947
        }
    }
}
