// See https://aka.ms/new-console-template for more information
using midi2event;
using System.Diagnostics;

Console.WriteLine("Hello, World!");
string path =
    "C:\\Users\\mreva\\source\\repos\\midi2event\\midi2event\\testfiles\\testchart(2).mid";
double cap = 10;

Midi2Event eventSys = new Midi2Event(path);
Debug.WriteLine("000");
/*
eventSys.Subscribe(
    () => {Console.WriteLine("C down");},
    Midi2Event.Notes.C,
    2
);

eventSys.Subscribe(
    () => {Console.WriteLine("C up");},
    Midi2Event.Notes.C,
    2,
    Midi2Event.SubType.Stop
);

eventSys.Subscribe(
    () => {Console.WriteLine("D down");},
    Midi2Event.Notes.D,
    2
);

eventSys.Subscribe(
    () => {Console.WriteLine("D up");},
    Midi2Event.Notes.D,
    2,
    Midi2Event.SubType.Stop
);

eventSys.Subscribe(
    () => {Console.WriteLine("E down");},
    Midi2Event.Notes.E,
    2
);

eventSys.Subscribe(
    () => {Console.WriteLine("E up");},
    Midi2Event.Notes.E,
    2,
    Midi2Event.SubType.Stop
);

eventSys.Subscribe(
    () => {Console.WriteLine("F down");},
    Midi2Event.Notes.F,
    2
);

eventSys.Subscribe(
    () => {Console.WriteLine("F up");},
    Midi2Event.Notes.F,
    2,
    Midi2Event.SubType.Stop
);

eventSys.Subscribe(
    () => {Console.WriteLine("G down");},
    Midi2Event.Notes.G,
    2
);

eventSys.Subscribe(
    () => {Console.WriteLine("G up");},
    Midi2Event.Notes.G,
    2,
    Midi2Event.SubType.Stop
);

eventSys.Subscribe(
    () => {Console.WriteLine("all done!");},
    type: Midi2Event.SubType.End
);

double lastElapsed = 0;
double totalElapsed = 0;
eventSys.Play();

while(totalElapsed  < cap){
    eventSys.Update(lastElapsed);
    Thread.Sleep(1);
    lastElapsed = 0.001;
    totalElapsed += lastElapsed;
}
Console.WriteLine("resetting...");
eventSys.Back();
eventSys.Play();
lastElapsed = 0;
totalElapsed = 0;

while(totalElapsed  < cap){
    eventSys.Update(lastElapsed);
    Thread.Sleep(1);
    lastElapsed = 0.001;
    totalElapsed += lastElapsed;
}


Console.WriteLine("End of script");
*/
