// See https://aka.ms/new-console-template for more information
using MIDI2Event;
using System.Diagnostics;

string path = "testfiles\\midi sys test1.mid";
double cap = 10;

MIDI2EventSystem eventSys = new MIDI2EventSystem(path);

eventSys.Subscribe(
    () =>
    {
        Console.WriteLine("C down");
    },
    MIDI2Event.Notes.C,
    3
);

eventSys.Subscribe(
    () =>
    {
        Console.WriteLine("C up");
    },
    MIDI2Event.Notes.C,
    3,
    MIDI2EventSystem.SubType.Stop
);

eventSys.Subscribe(
    () =>
    {
        Console.WriteLine("D down");
    },
    MIDI2Event.Notes.D,
    3
);

eventSys.Subscribe(
    () =>
    {
        Console.WriteLine("D up");
    },
    MIDI2Event.Notes.D,
    3,
    MIDI2EventSystem.SubType.Stop
);

eventSys.Subscribe(
    () =>
    {
        Console.WriteLine("E down");
    },
    MIDI2Event.Notes.E,
    3
);

eventSys.Subscribe(
    () =>
    {
        Console.WriteLine("E up");
    },
    MIDI2Event.Notes.E,
    3,
    MIDI2EventSystem.SubType.Stop
);

eventSys.Subscribe(
    () =>
    {
        Console.WriteLine("F down");
    },
    MIDI2Event.Notes.F,
    3
);

eventSys.Subscribe(
    () =>
    {
        Console.WriteLine("F up");
    },
    MIDI2Event.Notes.F,
    3,
    MIDI2EventSystem.SubType.Stop
);

eventSys.Subscribe(
    () =>
    {
        Console.WriteLine("G down");
    },
    MIDI2Event.Notes.G,
    3
);

eventSys.Subscribe(
    () =>
    {
        Console.WriteLine("G up");
    },
    MIDI2Event.Notes.G,
    3,
    MIDI2EventSystem.SubType.Stop
);

eventSys.Subscribe(
    () =>
    {
        Console.WriteLine("all done!");
    },
    type: MIDI2EventSystem.SubType.ChartEnd
);

Stopwatch sw = new Stopwatch();
double lastElapsed = 0;
double totalElapsed = 0;

eventSys.Play();

Console.WriteLine("here");

while (totalElapsed < cap)
{
    sw.Restart();

    eventSys.Update(lastElapsed);
    Thread.Sleep(1);

    sw.Stop();

    lastElapsed = sw.ElapsedMilliseconds / 1000.0;
    totalElapsed += lastElapsed;
}
Console.WriteLine("resetting...");
eventSys.Reset();
lastElapsed = 0;
totalElapsed = 0;

while (totalElapsed < cap)
{
    sw.Restart();

    eventSys.Update(lastElapsed);
    Thread.Sleep(1);

    sw.Stop();

    lastElapsed = sw.ElapsedMilliseconds / 1000.0;
    totalElapsed += lastElapsed;
}

Console.WriteLine("End of script");
